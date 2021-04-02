using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory_Storage : IStorageSwap
{
    public Action OnInventoryUpdated;
    public StorageSlot[] slots;
    
    public Inventory_Storage(int size)
    {
        slots = new StorageSlot[size];
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
    }

    protected int FindEmptySpace()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public IGetStorageSlot GetItemSlotValues(int _i)
    {
        if (_i < slots.Length && slots.ElementAtOrDefault(_i) != null)
        {
            return slots[_i];
        }
        else
        {
            return null;
        }
    }

    public List<IGetStorageSlot> GetAllItemSlotValues()
    {
        return slots.Cast<IGetStorageSlot>().ToList();
    }

    public void Swap(int a, int b)
    {
        StorageSlot storageSlotA = slots[a];
        slots[a] = slots[b];
        slots[b] = storageSlotA;
        OnInventoryUpdated?.Invoke();
    }




}
