using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Season
{
    public static int MIN_SEASON = 0;
    public static int MAX_SEASON = 3;
    public int _currentSeason;

    public Season(int currentSeason)
    {
        _currentSeason = currentSeason;
    }

    public int UpdateSeason()
    {
        _currentSeason++;
        CheckSeasonOutOfBoundary();
        return _currentSeason;
    }

    private void CheckSeasonOutOfBoundary()
    {
        if (_currentSeason > MAX_SEASON)
        {
            _currentSeason = MIN_SEASON;
        }
    }
}
