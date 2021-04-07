using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tag
{
    [SerializeField] private string tagName;

    public Tag(string _tagName)
    {
        tagName = _tagName;
    }

    private bool CompareTagByString(string _tagName)
    {
        return tagName == _tagName;
    }

    private bool CompareTagByObject(Tag tag)
    {
        return tag == this;
    }

    public string GetTagAsString()
    {
        return tagName;
    }
}
