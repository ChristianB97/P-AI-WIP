using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemCursorObject
{
    [SerializeField] private ItemUI itemUI;
    public GameObject itemUICursor;

    public ItemCursorObject()
    {
    }

    public void SetAndTurnOn(ItemUIProperties copyProperties)
    {
        Switch(true);
        itemUI.InitiateItemUI(-1);
        itemUI.SetProperties(copyProperties.Item, copyProperties.Amount);
    }

    public void Switch(bool boolean)
    {
        itemUICursor.gameObject.SetActive(boolean);
    }
}
