using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Events_TimeManager : ITimeOfDayEvents, IDateEvents
{
    public static EventHandler<Date> OnDateUpdated;
    public static Action OnDayUpdated;
    public static EventHandler OnSeasonUpdated;
    public static EventHandler<Clock> OnTimeOfDayUpdated;
    public static EventHandler<int> OnHourUpdated;

    public Events_TimeManager() { }

    public void Event_DayUpdated(Date date)
    {
        OnDayUpdated?.Invoke();
        Event_UpdateDate(date);
    }

    public void Event_UpdateDate(Date date)
    {
        OnDateUpdated?.Invoke(this, date);
        //SaveManager.instanz.Save();
        OnDayUpdated?.Invoke();
    }

    public void Event_UpdateTimeOfDay(Clock clock)
    {
        OnTimeOfDayUpdated?.Invoke(this, clock);
        if (clock.minute == 0)
        {
            Event_HourUpdated(clock.hour);
        }
    }

    private void Event_HourUpdated(int hour)
    {
        OnHourUpdated?.Invoke(this, hour);
    }
}
