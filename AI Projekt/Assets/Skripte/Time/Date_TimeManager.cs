using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Date_TimeManager : MonoBehaviour
{
    private DaysInSeason days;
    private Season season;
    public Date Date { private set; get; }
    [Inject] private IDateEvents dateEvents;

    private void Awake()
    {
        days = new DaysInSeason(0);
        season = new Season(0);
        if (Date == null)
        {
            Date = new Date();
        }
    }

    public void IncrementDate()
    {
        Date.mDay = days.UpdateDay();
        if (days.IsNextSeason())
        {
            Date.mSeason = season.UpdateSeason();
        }
        dateEvents.Event_UpdateDate(Date);
    }

    public void SetDate(Date newDate)
    {
        Date = newDate;
        days._currentDay = Date.mDay;
        season._currentSeason = Date.mSeason;
    }
}
