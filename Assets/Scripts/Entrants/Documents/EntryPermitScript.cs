using System.Collections;
using System.Collections.Generic;
using Entrants;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryPermitScript : MonoBehaviour
{
    [SerializeField] private DocumentButton nameButton;
    [SerializeField] private DocumentButton idButton;
    [SerializeField] private DocumentButton purposeButton;
    [SerializeField] private DocumentButton durationButton;
    [SerializeField] private DocumentButton enterByDateButton;


    //(string name, string id, string purpose, string duration, string enterByDate)
    public void SetData((string, string, string, string, string) data)
    {
        EntrantManager.DocumentType docType = EntrantManager.DocumentType.ENTRY_PERMIT;
        nameButton.Initialize(docType, EntrantManager.InfoType.NAME,data.Item1);
        string id = data.Item2;
        string id_ = id.Substring(0,4)+"-"+id.Substring(4);
        idButton.Initialize(docType,EntrantManager.InfoType.ID,data.Item2);
        purposeButton.Initialize(docType,EntrantManager.InfoType.ENTRYPURPOSE,data.Item3); //Useless...
        durationButton.Initialize(docType,EntrantManager.InfoType.ACCESS_DURATION,data.Item4);
        enterByDateButton.Initialize(docType,EntrantManager.InfoType.ENTER_BY_DATE,data.Item5);
    }

}
