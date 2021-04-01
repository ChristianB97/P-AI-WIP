using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Save;

[RequireComponent(typeof(TimeOfDay_TimeManager), typeof(Date_TimeManager))]
public class TimeManager : MonoBehaviour
{
    private Date_TimeManager dateManager;
    private string DATE_SAVE_KEY = "DATE";

    private void Awake()
    {
        dateManager = GetComponent<Date_TimeManager>();
        SaveLoadEvent.OnLoad += Load;
        SaveLoadEvent.OnSave += Save;
    }

    private void Start()
    {
        Load();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            NextDay();
    }

    public void NextDay()
    {
        dateManager.IncrementDate();
        Save();
    }

    private void Save()
    {
        SaveLoadStation save = new SaveLoadStation();
        save.SaveObject(dateManager.Date, DATE_SAVE_KEY);
    }

    private void Load()
    {
        SaveLoadStation save = new SaveLoadStation();
        Date date = save.LoadObject<Date>(DATE_SAVE_KEY);
        dateManager.SetDate(date);
        Events_TimeManager.OnDateUpdated?.Invoke(this, dateManager.Date);
    }
}
