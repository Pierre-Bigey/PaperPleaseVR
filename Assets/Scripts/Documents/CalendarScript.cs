using System;
using System.Collections;
using System.Collections.Generic;
using Entrants;
using UnityEngine;

public class CalendarScript : MonoBehaviour
{
    [SerializeField] private DocumentButton CalendarButton;
    
    public void SetData(DateTime data)
    {
        EntrantManager.DocumentType docType = EntrantManager.DocumentType.CALENDAR;
        CalendarButton.Initialize(docType,EntrantManager.InfoType.CALENDAR_DATE, data.ToString("dd.MM.yyyy"));
    }
}
