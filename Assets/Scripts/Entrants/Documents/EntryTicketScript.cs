using System.Collections;
using System.Collections.Generic;
using Entrants;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EntryTicketScript : MonoBehaviour
{
    [SerializeField] private DocumentButton ValidOnDateButton;


    //(string name, string id, string purpose, string duration, string enterByDate)
    public void SetData(string data)
    {
        EntrantManager.DocumentType docType = EntrantManager.DocumentType.ENTRY_TICKET;
        ValidOnDateButton.Initialize(docType,EntrantManager.InfoType.VALID_ON_DATE, data);
    }

}
