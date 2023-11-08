using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassportScript : MonoBehaviour
{
    [SerializeField] private GameObject passportOpened;
    [SerializeField] private GameObject passportClosed;

    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private TMP_Text issText;
    [SerializeField] private TMP_Text sexText;
    [SerializeField] private TMP_Text dobText;
    [SerializeField] private TMP_Text idText;
    [SerializeField] private Image photoImage;

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
        nameText.SetText(data.Item1);
        expText.SetText(data.Item2);
        issText.SetText(data.Item3);
        sexText.SetText(data.Item4);
        dobText.SetText(data.Item5);
        string id = data.Item6;
        string id_ = id.Substring(0,4)+"-"+id.Substring(4);
        idText.SetText(id_);
    }

    public void SwitchIsOpened()
    {
        isOpened = !isOpened;
        CorrectIsOpened();
    }
}
