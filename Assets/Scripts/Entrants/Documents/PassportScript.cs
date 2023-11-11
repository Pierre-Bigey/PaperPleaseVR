using System;
using System.Collections;
using System.Collections.Generic;
using Entrants;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PassportScript : MonoBehaviour
{
    [SerializeField] private GameObject passportOpened;
    [SerializeField] private GameObject passportClosed;

    [SerializeField] private DocumentButton nameButton;
    [SerializeField] private DocumentButton expButton;
    [SerializeField] private DocumentButton issButton;
    [SerializeField] private DocumentButton sexButton;
    [SerializeField] private DocumentButton dobButton;
    [SerializeField] private DocumentButton idButton;
    [SerializeField] private DocumentButton photoButton;

    public bool isOpened;
    // Start is called before the first frame update
    void Start()
    {
        //isOpened = false;
        CorrectIsOpened();
        
    }

    private void CorrectIsOpened()
    {
        passportOpened.SetActive(isOpened);
        passportClosed.SetActive(!isOpened);
    }

    //(string name, DateTime exp, string iss, Sex sex, DateTime dob, string id)
    public void SetData((string, DateTime, string, EntrantManager.Sex, DateTime, string) data)
    {
        EntrantManager.DocumentType docType = EntrantManager.DocumentType.PASSPORT;
        nameButton.Initialize(docType,EntrantManager.InfoType.NAME,data.Item1);
        expButton.Initialize(docType,EntrantManager.InfoType.EXP_DATE,data.Item2.ToString("dd.MM.yyyy"));
        issButton.Initialize(docType,EntrantManager.InfoType.PASS_ISS,data.Item3);
        sexButton.Initialize(docType,EntrantManager.InfoType.SEX,data.Item4.ToString()[0].ToString());
        dobButton.Initialize(docType,EntrantManager.InfoType.DOB,data.Item5.ToString("dd.MM.yyyy"));
        string id = data.Item6;
        string id_ = id.Substring(0,4)+"-"+id.Substring(4);
        idButton.Initialize(docType,EntrantManager.InfoType.ID,id_);
    }

    public void SwitchIsOpened()
    {
        isOpened = !isOpened;
        CorrectIsOpened();
    }
}
