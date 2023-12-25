using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance;  }
    }
    
    public readonly DateTime startDate = new DateTime(1982, 11, 23);
    
    public DateTime date { get; private set; }

    public bool addDay = false;
    
    private void Awake()
    {
        //Singleton instantiation
        if (_instance != null && _instance != this) Destroy(this);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        
        //date initialization

        date = startDate;
        CalendarController.Instance.UpdateCalendar();
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
        CalendarController.Instance.UpdateCalendar();
    }
    
}
