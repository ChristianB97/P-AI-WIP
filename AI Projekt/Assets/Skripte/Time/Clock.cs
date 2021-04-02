using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Clock : EventArgs
{
    public int minute;
    public int hour;

    public void AdjustTime()
    {
        if (minute >= TimeOfDay_TimeManager.MAX_MINUTE)
        {
            minute = TimeOfDay_TimeManager.MIN_MINUTE;
            hour++;
        }
        if (hour >= TimeOfDay_TimeManager.MAX_HOUR)
        {
            hour = TimeOfDay_TimeManager.MIN_HOUR;
        }
    }
}
