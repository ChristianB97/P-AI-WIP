using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(TimeManager))]
public class TimeOfDay_TimeManager : MonoBehaviour
{
    public static int MINUTE_STEP { private set; get; } = 5;
    public static float STEP_LENGTH_IN_SECONDS { private set; get; } = 0.1f;

    public static int MAX_MINUTE { private set; get; } = 60;
    public static int MIN_MINUTE { private set; get; } = 0;
    public static int MAX_HOUR { private set; get; } = 24;
    public static int MIN_HOUR { private set; get; } = 0;
    private int WAKE_UP_HOUR = 8;

    private float secondsCounter;
    private Clock clock;

    [Inject] private ITimeOfDayEvents timeOfDay;
    private TimeManager timeManager;

    private void Start()
    {
        secondsCounter = 0;
        clock = new Clock();
        clock.minute = 0;
        clock.hour = WAKE_UP_HOUR;
        TriggerTimeOfDayEvent();
        timeManager = GetComponent<TimeManager>();
    }

    void Update()
    {
        UpdateRealTime();
        CheckStepReached();
    }

    private void UpdateRealTime()
    {
        secondsCounter += Time.deltaTime;
    }

    private void CheckStepReached()
    {
        if ((secondsCounter / STEP_LENGTH_IN_SECONDS) > 1)
        {
            secondsCounter = 0;
            UpdateGameTimeOfDay();
        }
    }

    private void UpdateGameTimeOfDay()
    {

        clock.minute += MINUTE_STEP;
        clock.AdjustTime();
        TriggerTimeOfDayEvent();
        CheckNewDay();
    }

    private void CheckNewDay()
    {
        if (clock.hour == 0 && clock.minute == 0)
        {
            timeManager.NextDay();
        }
    }

    private void TriggerTimeOfDayEvent()
    {
        timeOfDay.Event_UpdateTimeOfDay(clock);
    }
}
