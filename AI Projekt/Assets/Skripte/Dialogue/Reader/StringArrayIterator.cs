using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringArrayIterator
{
    private int currentSpot;
    private string[] currentStringArray;
    public string GetStringAndIterate()
    {
        if (!IsEndOfStringArray())
            return currentStringArray[currentSpot++];
        return "";
    }

    public void SetNewStringArray(string[] value)
    {

        currentStringArray = value;
        currentSpot = 0;
    }

    public bool IsEndOfStringArray()
    {
        return currentStringArray==null || currentSpot >= currentStringArray.Length;
    }
}
