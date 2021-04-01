using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Default Object", menuName = "Inventory/Items/Default")]
public class DefaultObject : ItemObject
{
    public void Awake()
    {
        Type = ItemType.Default;
    }
}
