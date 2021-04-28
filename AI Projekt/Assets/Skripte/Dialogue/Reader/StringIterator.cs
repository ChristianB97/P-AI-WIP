using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StringIterator
{
    private int currentSpot;
    private string currentString;
    public char GetCharAndIterate()
    {
        if (!IsEndOfString())
            return currentString[currentSpot++];
        return '\0';
    }

    public string EndIterationAndGetString()
    {
        currentSpot = currentString.Length;
        return currentString;
    }

    public void SetNewString(string value)
    {
        currentString = value;
        currentSpot = 0;
        Debug.Log(value);
    }

    public bool IsEndOfString()
    {
        return string.IsNullOrWhiteSpace(currentString) || currentSpot >= currentString.Length;
    }
}
