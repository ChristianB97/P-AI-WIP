using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Zenject;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(InventoryDragUI))]
public class InventoryUI : MonoBehaviour
{
    [Inject(Id = "Player")] private PlayerStorage playerInventory;
    public ItemUI[] itemUIs;
    private InventoryDragUI inventoryActionsUI;

    private void Start()
    {
        inventoryActionsUI = GetComponent<InventoryDragUI>();
        inventoryActionsUI.SetActionListener(itemUIs, (IStorageSwap)playerInventory);
        inventoryActionsUI.OnEndDrag += Redraw;
    }

    private void OnEnable()
    {
        Redraw();
    }

    private void Redraw()
    {
        List<IGetStorageSlot> items = playerInventory.GetAllItemSlotValues();
        for (int i = 0; i < itemUIs.Length; i++)
        {
            itemUIs[i].InitiateItemUI(i);
            if (items.ElementAtOrDefault(i) != null)
                itemUIs[i].SetProperties(items[i].GetItem(), items[i].GetAmount());
        }
    }
}
