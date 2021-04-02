using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipment,
    QuestItem,
    Food,
    Tool,
    Seed,
    Default
}

public abstract class ItemObject : ScriptableObject
{
    public string itemName;
    public Sprite picture;
    public ItemType Type { protected set; get; }
    [TextArea(15,20)] public string description;
}
