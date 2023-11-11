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

    //(string districtName, string name, string dob, string height, string weight)
    public void SetData((string, string, string, string, string) data)
    {
        EntrantManager.DocumentType docType = EntrantManager.DocumentType.ID_CARD;
        districtButton.Initialize(docType, EntrantManager.InfoType.DISTRICT,data.Item1+" DISTRICT");
        nameButton.Initialize(docType, EntrantManager.InfoType.NAME,data.Item2);
        dobButton.Initialize(docType, EntrantManager.InfoType.DOB,data.Item3);
        heightButton.Initialize(docType, EntrantManager.InfoType.HEIGHT,data.Item4+"cm");
        weightButton.Initialize(docType, EntrantManager.InfoType.WEIGHT,data.Item5+"Kg");
    }

}
