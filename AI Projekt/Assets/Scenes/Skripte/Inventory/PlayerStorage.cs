using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStorage : Inventory_Storage
{
    public Action OnItemFocusUpdated;
    private int focusedSlotNumber;

    public PlayerStorage(int size) : base(size)
    {
    }

    public void AddItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].GetItem() == _item)
            {
                slots[i].AddAmount(_amount);
                hasItem = true;
                break;
            }
        }
        if (!hasItem)
        {
            Debug.Log(_item.itemName + " added");
            slots[FindEmptySpace()] = new StorageSlot(_item, _amount);
        }
        OnInventoryUpdated?.Invoke();
        OnItemFocusUpdated?.Invoke();
    }

    public void SetItemFocus(int slotNumber)
    {
        focusedSlotNumber = slotNumber;
        OnItemFocusUpdated?.Invoke();
    }

    public int GetFocusedSlotNumber()
    {
        return focusedSlotNumber;
    }
}
