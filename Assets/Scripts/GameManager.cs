using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance;  }
    }
    
    public DateTime date { get; private set; }
    public readonly DateTime startDate = new DateTime(1982, 11, 23);
    public static event Action DayAdded ;
    
    public (int, int) inGameTime { get; private set; }
    private (int, int) startHour = (6, 0);
    private (int, int) endHour = (18, 0);
    private float inGameTimeRatio = 2; //Ho many minutes in game for IRL second
    public static event Action MinuteAdded;


    public bool addDay = false;
    
    private void Awake()
    {
        //Singleton instantiation
        if (_instance != null && _instance != this) Destroy(gameObject);
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        
        //date initialization

        date = startDate;
        CalendarController.Instance.UpdateCalendar();
    }

    private void Start()
    {
        inGameTime = startHour;
        StartCoroutine(InGameMinuter());
    }

    private void Update()
    {
        if (addDay)
        {
            addDay = false;
            AddDay();
        }
    }

    public void AddDay()
    {
        date = date.AddDays(1);
        DayAdded.Invoke();
    }

    private IEnumerator InGameMinuter()
    {
        while (inGameTime.Item1 < endHour.Item1 )
        {
            AddMinute();
            yield return new WaitForSeconds(1 / inGameTimeRatio);
        }
    }
    
    public void AddMinute()
    {
        (int hour, int minute) = inGameTime;
        minute++;
        if (minute == 60)
        {
            minute = 0;
            hour++;
        }

        inGameTime = (hour, minute);
        MinuteAdded.Invoke();
    }
    
}
