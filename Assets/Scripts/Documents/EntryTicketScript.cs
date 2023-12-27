using System;
using System.Collections;
using System.Collections.Generic;
using Documents;
using Entrants;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryTicketScript : DocumentScript
{
    [SerializeField] private DocumentButton ValidOnDateButton;


    //(string name, string id, string purpose, string duration, string enterByDate)
    public void SetData(DateTime data)
    {
        docType = DocumentType.ENTRY_TICKET;
        ValidOnDateButton.Initialize(docType,InfoType.VALID_ON_DATE, data.ToString("yyyy.MM.dd"));
    }

}
