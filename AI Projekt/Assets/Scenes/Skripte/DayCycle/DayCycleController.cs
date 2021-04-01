using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Experimental.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DayCycleController : MonoBehaviour
{
    public DayCycleData dayCycleData;
    private TimeUnit currentTimeUnit;
    private TimeUnit nextTimeUnit;
    private float timeUnitDuration;
    private float currentTime;
    private Light2D sun;
    private bool allowCycle;

    private int timeUnitLengthInHours;

    
    private void Start()
    {
        timeUnitLengthInHours = 4;
        Events_TimeManager.OnHourUpdated += TryAdvancingDayCycle;
        timeUnitDuration = (TimeOfDay_TimeManager.MAX_MINUTE * timeUnitLengthInHours / TimeOfDay_TimeManager.MINUTE_STEP) * TimeOfDay_TimeManager.STEP_LENGTH_IN_SECONDS;
        sun = GetComponent<Light2D>();
    }

    private void LateUpdate()
    {
        if (allowCycle && currentTime < timeUnitDuration)
        {
            currentTime += Time.deltaTime;
            float lightDifference = currentTimeUnit.GetLight() - nextTimeUnit.GetLight();
            //Debug.Log(nextTimeUnit.GetLight().ToString() + " - " + currentTimeUnit.GetLight().ToString() + " = " + lightDifference);
            float lightPerSecond = -lightDifference / timeUnitDuration;
            //Debug.Log("l/s: " + lightPerSecond);
            float sunStep =  lightPerSecond * Time.deltaTime;
            sun.intensity += sunStep;
            //Debug.Log("intens: " + sun.intensity);
        }
    }


    public void TryAdvancingDayCycle(object sender, int hour) {
        TimeUnit newUnit = dayCycleData.GetDataByHour(hour);
        if (newUnit != null && hour == newUnit.GetHour())
            GetCurrentUnit(newUnit);
    }

    private void GetCurrentUnit(TimeUnit newUnit)
    {
        currentTimeUnit = newUnit;
        currentTime = 0;
        nextTimeUnit = dayCycleData.GetNextUnit();
        CheckIsCycleAllowed();
        if (allowCycle)
        {
            sun.intensity = currentTimeUnit.GetLight();
        }
    }

    private void CheckIsCycleAllowed()
    {
        if (sun != null)
        {
            GetSun();
        }
        if (currentTimeUnit != null && sun != null && nextTimeUnit != null)
        {
            allowCycle = true;
        }
        else
            allowCycle = false;
    }



    //später in Manager auslagern, der notfalls die Sonne auch spawnen lässt, falls diese verschwunden ist
    private void GetSun()
    {
        sun = GetComponent<Light2D>();
        sun.lightType = Light2D.LightType.Global;
    }
}
