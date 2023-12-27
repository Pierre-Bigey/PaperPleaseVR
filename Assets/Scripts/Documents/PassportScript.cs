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

    private bool isOpened;
    // Start is called before the first frame update
    void Start()
    {
        //isOpened = false;
        SetIsOpen(false);
    }

    //(string name, DateTime exp, string iss, Sex sex, DateTime dob, string id)
    public void SetData((string, DateTime, string, Sex, DateTime, string) data)
    {
        DocumentType docType = DocumentType.PASSPORT;
        nameButton.Initialize(docType,InfoType.NAME,data.Item1);
        expButton.Initialize(docType,InfoType.EXP_DATE,data.Item2.ToString("dd.MM.yyyy"));
        issButton.Initialize(docType,InfoType.PASS_ISS,data.Item3);
        sexButton.Initialize(docType,InfoType.SEX,data.Item4.ToString()[0].ToString());
        dobButton.Initialize(docType,InfoType.DOB,data.Item5.ToString("dd.MM.yyyy"));
        string id = data.Item6;
        string id_ = id.Substring(0,4)+"-"+id.Substring(4);
        idButton.Initialize(docType,InfoType.ID,id_);
    }

    public void SwitchIsOpened()
    {
        isOpened = !isOpened;
        SetIsOpen(isOpened);
    }

    public void SetIsOpen(bool value)
    {
        isOpened = value;
        foreach (Transform child in transform)
        {
            if (child.name.Equals(passportClosed.name)) child.gameObject.SetActive(!value);
            else child.gameObject.SetActive(value);
        }
    }

}
