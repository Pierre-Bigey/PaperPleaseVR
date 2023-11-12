using System;
using System.Collections;
using System.Collections.Generic;
using Entrants;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IDCardScript : MonoBehaviour
{
    [SerializeField] private DocumentButton districtButton;
    [SerializeField] private DocumentButton nameButton;
    [SerializeField] private DocumentButton dobButton;
    [SerializeField] private DocumentButton heightButton;
    [SerializeField] private DocumentButton weightButton;
    [SerializeField] private DocumentButton photoButton;

    //(District districtName, string name, DateTime dob, int height, int weight)
    public void SetData((EntrantManager.District, string, DateTime, int, int) data)
    {
        EntrantManager.DocumentType docType = EntrantManager.DocumentType.ID_CARD;
        districtButton.Initialize(docType, EntrantManager.InfoType.DISTRICT,data.Item1.ToString()+" DISTRICT");
        nameButton.Initialize(docType, EntrantManager.InfoType.NAME,data.Item2);
        dobButton.Initialize(docType, EntrantManager.InfoType.DOB,data.Item3.ToString("yyyy.MM.dd"));
        heightButton.Initialize(docType, EntrantManager.InfoType.HEIGHT,data.Item4.ToString()+"cm");
        weightButton.Initialize(docType, EntrantManager.InfoType.WEIGHT,data.Item5.ToString()+"Kg");
    }

}
