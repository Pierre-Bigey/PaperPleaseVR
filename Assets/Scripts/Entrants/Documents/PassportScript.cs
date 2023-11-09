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

    //(string name, string exp, string iss, string sex, string dob, string id)
    public void SetData((string, string, string, string, string, string) data)
    {
        EntrantManager.DocumentType docType = EntrantManager.DocumentType.PASSPORT;
        nameButton.Initialize(docType,"name",data.Item1);
        expButton.Initialize(docType,"exp",data.Item2);
        issButton.Initialize(docType,"iss",data.Item3);
        sexButton.Initialize(docType,"sex",data.Item4);
        dobButton.Initialize(docType,"dob",data.Item5);
        string id = data.Item6;
        string id_ = id.Substring(0,4)+"-"+id.Substring(4);
        idButton.Initialize(docType,"id",id_);
    }

    public void SwitchIsOpened()
    {
        isOpened = !isOpened;
        CorrectIsOpened();
    }
}
