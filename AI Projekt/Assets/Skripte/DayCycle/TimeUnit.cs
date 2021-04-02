using UnityEngine;
using System;
using System.Dynamic;

[Serializable]
public class TimeUnit
{
    [Header("Start")]
    [SerializeField] private int hour;
    [SerializeField] private Color32 color;
    [Range(0, 1.2f)] public float light;

    public int GetHour()
    {
        return hour;
    }

    public float GetLight()
    {
        return light;
    }

    public Color32 GetColor()
    {
        return color;
    }
}
