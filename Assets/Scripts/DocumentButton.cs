using System.Collections;
using System.Collections.Generic;
using Entrants;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DocumentButton : MonoBehaviour
{
    public EntrantManager.DocumentType documentType { get; private set; }
    public string information { get; private set; }
    public string value { get; private set; }
    public RawImage image { get; private set; }
    private Image backgroundImage;

    public int index { get; private set; } = -1;

    private static int numberOfButtons;

    public static void ResetNumberOfButton()
    {
        numberOfButtons = 0;}
    public void Initialize(EntrantManager.DocumentType _documentType, string _information, string _value, bool isImage = false)
    {
        this.index = numberOfButtons;
        numberOfButtons++;
        documentType = _documentType;
        information = _information;
        value = _value;
        backgroundImage = transform.GetChild(1).GetComponent<Image>();
        if(!isImage) GetComponentInChildren<TMP_Text>().SetText(_value);
    }
    
    public void isClicked()
    {
        Inspector.Instance.ActivateButton(this);
    }
}
