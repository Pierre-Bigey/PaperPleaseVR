using System;
using System.Collections;
using System.Collections.Generic;
using Documents;
using Entrants;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryPermitScript : DocumentScript
{
    [SerializeField] private DocumentButton nameButton;
    [SerializeField] private DocumentButton idButton;
    [SerializeField] private DocumentButton purposeButton;
    [SerializeField] private DocumentButton durationButton;
    [SerializeField] private DocumentButton enterByDateButton;

    //(string name, string id, EntrantType purpose, string duration, DateTime entryByDate)
    public void SetData((string, string, EntrantType, string, DateTime) data)
    {
        docType = DocumentType.ENTRY_PERMIT;
        nameButton.Initialize(docType, InfoType.NAME,data.Item1);
        string id = data.Item2;
        string id_ = id.Substring(0,4)+"-"+id.Substring(4);
        idButton.Initialize(docType,InfoType.ID,id_);
        purposeButton.Initialize(docType,InfoType.ENTRYPURPOSE,data.Item3.ToString()); //Useless...
        durationButton.Initialize(docType,InfoType.ACCESS_DURATION,data.Item4);
        enterByDateButton.Initialize(docType,InfoType.ENTER_BY_DATE,data.Item5.ToString("dd.MM.yyyy"));
    }
}
