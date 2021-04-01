using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;

public class TimeOfDayUI : MonoBehaviour
{
    public TextMeshProUGUI clockText;

    void Start()
    {
        Events_TimeManager.OnTimeOfDayUpdated += UpdateUI_OnTimeOfDayUpdated;
    }


    private void UpdateUI_OnTimeOfDayUpdated(object sender, Clock clock)
    {
        clockText.SetText(clock.hour.ToString("D2") + " : " + clock.minute.ToString("D2"));
    }
}
