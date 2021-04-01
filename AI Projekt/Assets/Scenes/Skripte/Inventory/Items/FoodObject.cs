using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food Object", menuName = "Inventory/Items/Food")]
public class FoodObject : ItemObject
{
    public void Awake()
    {
        Type = ItemType.Food;
    }
}
