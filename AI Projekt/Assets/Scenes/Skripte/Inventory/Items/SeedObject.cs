using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Seed Object", menuName = "Inventory/Items/Seed")]
public class SeedObject : ItemObject
{
    public PlantStageObject[] plantStages;
    public bool isProductRegrowthable;

    public void Awake()
    {
        Type = ItemType.Seed;
    }
}

