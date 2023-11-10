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
    public EntrantManager.InfoType information { get; private set; }
    public string value { get; private set; }
    public RawImage image { get; private set; }
    private Image backgroundImage;

    public int index { get; private set; } = -1;

    private static int numberOfButtons;

    public static void ResetNumberOfButton()
    {
        numberOfButtons = 0;}
    public void Initialize(EntrantManager.DocumentType _documentType, EntrantManager.InfoType _information, string _value, bool isImage = false)
    {
        this.index = numberOfButtons;
        numberOfButtons++;
        documentType = _documentType;
        information = _information;
        value = _value;
        backgroundImage = transform.GetChild(1).GetComponent<Image>(); //The background Image has to be the second child
        if(!isImage) GetComponentInChildren<TMP_Text>().SetText(_value);
    }
    
    public void isClicked()
    {
        Inspector.Instance.ButtonPressed(this);
    }

    public void SetBackgroundColor(Color color)
    {
        backgroundImage.color = color;
    }

    public void ResetBackground()
    {
        Color color = Color.white;
        color.a = 0;
        backgroundImage.color = color;
    }
    
    
}
