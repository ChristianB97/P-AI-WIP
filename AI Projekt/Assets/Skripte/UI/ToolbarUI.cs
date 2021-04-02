using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ToolbarUI : MonoBehaviour
{
    [Inject(Id ="Player")]PlayerStorage playerStorage;
    public ItemUI[] itemUIs;
    private ItemUI currentFocus;

    private void Start()
    {
        UIActionEventConstructor constructor = new UIActionEventConstructor();
        playerStorage.OnInventoryUpdated += SetToolbar;
        for (int i = 0; i < itemUIs.Length; i++)
        {
            ItemUI currentItemUI = itemUIs[i];
            currentItemUI.InitiateItemUI(i);
            currentItemUI.SwitchFocus(false);
            constructor.ConstructEvent(itemUIs[i].gameObject, EventTriggerType.PointerClick, delegate { DoFocus(currentItemUI); });
        }
        if (itemUIs.Length >= 1)
        {
            DoFocus(itemUIs[0]);
        }
        SetToolbar();
    }

    public void SetToolbar()
    {
        for (int i = 0; i < itemUIs.Length; i++)
        {
            IGetStorageSlot currentStorageGetter = playerStorage.GetItemSlotValues(i);
            ItemUI currentItemUI = itemUIs[i];
            if (currentStorageGetter != null)
            {
                currentItemUI.SetProperties(currentStorageGetter.GetItem(), currentStorageGetter.GetAmount());
            }
            else
            {
                currentItemUI.SetProperties(null, 0);
            }
            
        }
    }

    private void DoFocus(ItemUI newFocus)
    {
        if (currentFocus != null)
            currentFocus.SwitchFocus(false);
        currentFocus = newFocus;
        playerStorage.SetItemFocus(newFocus.Properties.PosInInventory);
        newFocus.SwitchFocus(true);
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            int nextID = GetIncrementedFocusID();
            DoFocus(itemUIs[nextID]);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            int nextID = GetDecrementedFocusID();
            DoFocus(itemUIs[nextID]);
        }

        for (int i = 1; i <= 9; ++i)
        {
            if (Input.GetKeyDown("" + i) && i < itemUIs.Length)
            {
                DoFocus(itemUIs[i-1]);
            }
        }
        if (Input.GetKeyDown("0") && itemUIs.Length == 10)
        {
            DoFocus(itemUIs[itemUIs.Length-1]);
        }
    }

    private int GetIncrementedFocusID()
    {
        int nextID;
        if (currentFocus != null)
        {
            nextID = currentFocus.Properties.PosInInventory;
            nextID++;
            if (nextID >= itemUIs.Length)
                nextID = 0;
        }
        else
            nextID = 0;
        return nextID;
    }

    private int GetDecrementedFocusID()
    {
        int nextID;
        if (currentFocus != null)
        {
            nextID = currentFocus.Properties.PosInInventory;
            nextID--;
            if (nextID < 0)
                nextID = itemUIs.Length-1;
        }
        else
            nextID = nextID = itemUIs.Length - 1;
        return nextID;
    }
}
