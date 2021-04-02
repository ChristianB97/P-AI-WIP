using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(SeasonMapping))]
public class DateUI : MonoBehaviour
{
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI seasonText;

    private SeasonMapping mapper;

    void Start()
    {
        Events_TimeManager.OnDateUpdated += UpdateUI_OnDayPassed;
        mapper = GetComponent<SeasonMapping>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateUI_OnDayPassed(object sender, Date date)
    {
        dayText.SetText(date.mDay + ".");
        seasonText.SetText(mapper.GetSeasonName(date.mSeason));
    }
}
