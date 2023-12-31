using System.Collections;
using System.Collections.Generic;
using Entrants;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DocumentButton : MonoBehaviour
{
    public DocumentType documentType { get; private set; }
    public InfoType information { get; private set; }
    public string displayedValue { get; private set; }
    public string realValue { get; private set; }
    public RawImage image { get; private set; }
    private Image backgroundImage;

    public int index { get; private set; } = -1;

    private static int numberOfButtons;

    public static void ResetNumberOfButton()
    {
        numberOfButtons = 0;}
    public void Initialize(DocumentType _documentType, InfoType _information, string _displayerValue, bool isImage = false)
    {
        this.index = numberOfButtons;
        numberOfButtons++;
        
        documentType = _documentType;
        
        information = _information;
        
        displayedValue = _displayerValue;
        backgroundImage = transform.GetChild(1).GetComponent<Image>(); //The background Image has to be the second child
        if(!isImage) GetComponentInChildren<TMP_Text>().SetText(_displayerValue);
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

    private void SetRealValue()
    {
        
    }
    
    
}
