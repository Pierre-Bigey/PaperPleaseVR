using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarController : MonoBehaviour
{
    private static CalendarController _instance;
    public static CalendarController Instance
    {
        get { return _instance;  }
    }    
    
    
    private GameObject NovemberMonth;
    private GameObject DecemberMonth;
    private GameObject RedCircle;

    void Awake()
    {
        //Singleton Instantiation
        if (_instance != null && _instance != this) Destroy(this);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        
        NovemberMonth = transform.Find("November").gameObject;
        DecemberMonth = transform.Find("December").gameObject;
        RedCircle = transform.Find("RedCircle").gameObject;

        
    }

    public void UpdateCalendar()
    {
        DateTime date = GameManager.Instance.date;
        int day = date.Day;
        int month = date.Month;
        
        if (month == 11)
        {
            NovemberMonth.SetActive(true);
            DecemberMonth.SetActive(false);
        }
        else if (month == 12)
        {
            NovemberMonth.SetActive(false);
            DecemberMonth.SetActive(true);
        }
        else throw new Exception("Invalid month");

        RedCircle.transform.localPosition = GetCirclePosition(day, month);
        
        CalendarScript CS = GetComponent<CalendarScript>();
        CS.SetData(GameManager.Instance.date);
    }
    
    private Vector3 GetCirclePosition(int day, int month)
    {
        int offset = 0;
        if (month == 12) offset = 2;

        int row = (day + offset) / 7 ;
        int column = (day + offset) % 7;
        
        Debug.Log("Ask to place on row"+row+" and column"+column);

        return GetCalendarPosition(row, column);
    }

    private Vector3 GetCalendarPosition(int row, int column)
    {
        float x = Mathf.Lerp(0.024f, 0.176f, column / 6f);
        float y = Mathf.Lerp(0.189f, 0.0488f, row / 4f);
        return new Vector3(x, y, -0.0001f);
    }
}
