using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUIProperties
{
    public ItemObject Item { private set; get; }
    public int Amount { private set; get; }
    public int PosInInventory { private set; get; }
    public Sprite Sprite
    {
        private set { }
        get
        {
            return Item.picture;
        }
    }

    public ItemUIProperties(int _posInInventory)
    {
        PosInInventory = _posInInventory;
    }
    public void SetAttributes(ItemObject _item, int _amount)
    {
        Item = _item;
        Amount = _amount;
    }
}
