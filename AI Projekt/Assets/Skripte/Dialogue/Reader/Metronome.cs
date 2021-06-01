using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Metronome : MonoBehaviour
{
    private float durationWithoutTick;
    private float currentTimeWithoutTick;

    public Action onTick;

    private void Start()
    {
        currentTimeWithoutTick = 1;
    }

    private void Update()
    {
        currentTimeWithoutTick += Time.deltaTime;
        if (currentTimeWithoutTick > durationWithoutTick)
        {
            onTick?.Invoke();
            currentTimeWithoutTick = 0;
        }
    }

    public void SetDurationWithoutTick(float time)
    {
        durationWithoutTick = time;
    }
}
