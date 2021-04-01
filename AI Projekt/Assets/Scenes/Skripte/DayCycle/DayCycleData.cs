using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DayCycleData")]
public class DayCycleData : ScriptableObject
{
    [SerializeField]private TimeUnit[] timeUnits;
    private int lastIndex = -1;

    public TimeUnit GetDataByHour(int hour)
    {
        int maxSearchLength = Mathf.CeilToInt(Mathf.Log(timeUnits.Length, 2f));
        int max = timeUnits.Length;
        int min = 0;
        int current = Mathf.CeilToInt(max/2);
        for (int i = 0; i < maxSearchLength; i++)
        {
            if (hour == timeUnits[current].GetHour())
            {
                //Debug.Log("Gefunden! " + hour.ToString());
                lastIndex = current;
                return timeUnits[current];
            }
            else
            {
                if (hour > timeUnits[current].GetHour())
                    min = current;
                else
                    max = current;
                int difference = max - min;
                current = Mathf.CeilToInt(difference / 2) + min;
            }
        }
        return null;
    }

    public TimeUnit GetCurrentUnit()
    {
        if (IsInIndexBoundaries(lastIndex))
            return timeUnits[lastIndex];
        return null;
    }

    public TimeUnit GetNextUnit()
    {
        if (IsInIndexBoundaries(lastIndex)) return timeUnits[GetNextIndex()];
        return null;
    }

    private int GetNextIndex()
    {
        int nextIndex = lastIndex + 1;
        if (!IsInUpperBoundary(nextIndex))
            nextIndex = 0;
        return nextIndex;
    }
    private bool IsInIndexBoundaries(int index) { return IsInUpperBoundary(index) && IsInLowerBoundary(index); }
    private bool IsInUpperBoundary(int index) { return index < timeUnits.Length; }
    private bool IsInLowerBoundary(int index) { return index > -1; }
}



