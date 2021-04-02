using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DaysInSeason
{
    private static int MIN_DAY = 1;
    private static int MAX_DAY = 30;

    private bool wasOutOfBoundary;

    public int _currentDay;

    public DaysInSeason(int currentDay)
    {
        _currentDay = currentDay;
    }

    public int UpdateDay()
    {
        _currentDay++;
        CheckDayOutOfBoundary();
        return _currentDay;
    }

    public bool IsNextSeason()
    {
        if (wasOutOfBoundary)
        {
            wasOutOfBoundary = false;
            return true;
        }
        return false;
    }

    private void CheckDayOutOfBoundary()
    {
        if (_currentDay > MAX_DAY)
        {
            _currentDay = MIN_DAY;
            wasOutOfBoundary = true;
        }
    }
}
