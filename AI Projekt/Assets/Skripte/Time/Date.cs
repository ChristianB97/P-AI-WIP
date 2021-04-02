using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class Date
{
    public int mDay;
    public int mSeason;

    public Date()
    {

    }
    public Date(int day, int season)
    {
        mDay = day;
        mSeason = season;
    }
}
