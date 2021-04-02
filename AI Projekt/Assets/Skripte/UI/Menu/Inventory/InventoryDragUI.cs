using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InventoryDragUI : MonoBehaviour
{
    private MouseEventHelper mouseEventHelper;
    public ItemCursorObject itemCursorObject;
    public Action OnEndDrag;
    private IStorageSwap storageSwap;

    private void Start()
    {
        mouseEventHelper = new MouseEventHelper();
    }

    private void Update()
    {
        if (mouseEventHelper.MouseDragItemUI != null)
            itemCursorObject.itemUICursor.transform.position = Input.mousePosition;
    }

    private void OnDisable()
    {
        OnEndDragDelegate();
    }

    public void SetActionListener(ItemUI[] itemUIs, IStorageSwap _storageSwap)
    {
        storageSwap = _storageSwap;
        UIActionEventConstructor eventConstructor = new UIActionEventConstructor();
        foreach (ItemUI itemUI in itemUIs)
        {
            eventConstructor.ConstructEvent(itemUI.gameObject, EventTriggerType.BeginDrag, delegate { OnDragDelegate(itemUI); });
            eventConstructor.ConstructEvent(itemUI.gameObject, EventTriggerType.EndDrag, delegate { OnEndDragDelegate(); });
            eventConstructor.ConstructEvent(itemUI.gameObject, EventTriggerType.PointerEnter, delegate { OnEnterDelegate(itemUI); });
            eventConstructor.ConstructEvent(itemUI.gameObject, EventTriggerType.PointerExit, delegate { OnExitDelegate(); });
        }
    }

    private void OnDragDelegate(ItemUI itemUI)
    {
        if (itemUI != null && itemUI.Properties != null && itemUI.Properties.Item != null)
        {
            itemCursorObject.SetAndTurnOn(itemUI.Properties);
            mouseEventHelper.DragObject(itemUI);
            mouseEventHelper.MouseDragItemUI.UISwitch(false);
        }
    }

    private void OnEndDragDelegate()
    {
        if (mouseEventHelper.MouseDragItemUI != null && mouseEventHelper.MouseEnterItemUI!=null)
        {
            storageSwap.Swap(mouseEventHelper.MouseEnterItemUI.Properties.PosInInventory, mouseEventHelper.MouseDragItemUI.Properties.PosInInventory);
        }
        itemCursorObject.Switch(false);
        mouseEventHelper.EndDragObject();
        OnEndDrag?.Invoke();
    }

    private void OnEnterDelegate(ItemUI itemUI)
    {
        if (itemUI != null && mouseEventHelper.MouseDragItemUI != null)
            mouseEventHelper.EnterObject(itemUI);
    }

    private void OnExitDelegate()
    {
        mouseEventHelper.ExitObject();
    }
}