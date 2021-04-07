using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagDefiner : MonoBehaviour
{
    [SerializeField] private Tag[] tags;
    public Action onUpdate;

    public void OnValidate()
    {
        onUpdate?.Invoke();
    }
    public string[] GetTagAsStringArray()
    {
        string[] stringTagArray = new string[tags.Length];
        for (int i = 0; i < tags.Length; i++)
        {
            stringTagArray[i] = tags[i].GetTagAsString();
        }
        return stringTagArray;
    }

    public Tag GetTagByString(string tagName)
    {
        foreach (Tag tag in tags)
        {
            if (tag.GetTagAsString() == tagName)
                return tag;
        }
        return null;
    }
}
