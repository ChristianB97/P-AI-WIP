using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StorageSlot : IGetStorageSlot
{
    [SerializeField]private ItemObject item;
    [SerializeField]private int amount;

    public StorageSlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }

    public int GetAmount()
    {
        return amount;
    }

    public ItemObject GetItem()
    {
        return item;
    }

    public void RemoveItem()
    {
        item = null;
        amount = 0;
    }

    public void SetItem(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void RemoveAmountOfItem(int _amount)
    {
        if (item != null)
        {
            if (amount >= _amount)
            {
                RemoveItem();
            }
            else
            {
                amount -= _amount;
            }
        }
    }
}
