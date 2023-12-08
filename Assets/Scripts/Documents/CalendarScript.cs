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
        DocumentType docType = DocumentType.CALENDAR;
        CalendarButton.Initialize(docType,InfoType.CALENDAR_DATE, data.ToString("dd.MM.yyyy"));
    }
}
