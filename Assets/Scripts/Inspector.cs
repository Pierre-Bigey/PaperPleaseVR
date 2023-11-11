using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inspector : MonoBehaviour
{
    private static Inspector _instance;
    public static Inspector Instance
    {
        get { return _instance;  }
    }

    [SerializeField] private Color activatedColor;
    [SerializeField] private Color incoherentColor;
    [SerializeField] private Color correctColor;
    [SerializeField] private Color discrepancyColor;

    [SerializeField] private float checkingDelay = 2;
    
    private DocumentButton dButton0;
    private DocumentButton dButton1;

    private bool isProcessing = false;


    private void Awake()
    {
        //Singleton Instantiation
        if (_instance != null && _instance != this) Destroy(this);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        dButton0 = null;
        dButton1 = null;
        
        DocumentButton.ResetNumberOfButton();
    }

    public void ButtonPressed(DocumentButton dButton)
    {
        Debug.Log("You have clicked on dButton n" + dButton.index + " member of " + dButton.documentType.ToString() +
                  " which contains a " + dButton.information.ToString() + " of value " + dButton.displayedValue);
        if (!isProcessing) ProcessDButton(dButton);
    }

    private void ProcessDButton(DocumentButton dButton)
    {
        if(dButton1) Debug.Log("ERROR, there is a dButton1 in processButton... Index = "+dButton1.index);
        if (dButton0) //If a button is already selected
        {
            Debug.Log("Inspector There is already a button selected : "+dButton0.index);
            //We first check if the button clicked is already selected
            if (dButton0.index == dButton.index)
            {
                Debug.Log("Inspector The button pressed is the same, we disable it");
                ResetInternalButton(0);
            }
            //If not, check if it's the same document
            else if (dButton0.documentType == dButton.documentType) 
            {
                Debug.Log("Inspector This is a button from the same document");
                SetButton0(dButton); //We change the button
            }
            //If not check if it is the same type of information
            else if ( dButton0.information != dButton.information)
            {
                Debug.Log("Inspector Buttons selected aren't of the same type");
                IncoherentButton(dButton); //if not it's incoherent
            }
            else if (dButton0.displayedValue.Equals(dButton.displayedValue))
            {
                Debug.Log("Inspector The buttons share the same information");
                CorrectButton(dButton);
            }
            else
            {
                Debug.Log("Inspector There is a discrepancy !!!");
                DiscrepancyButton(dButton);
            }
        }
        else
        {
            Debug.Log("Inspector This is the first button pressed");
            SetButton0(dButton);
        }
    }

    private void IncoherentButton(DocumentButton dButton)
    {
        dButton1 = dButton;
        StartCoroutine(ColorButtons(incoherentColor));
    }

    private void CorrectButton(DocumentButton dButton)
    {
        dButton1 = dButton;
        StartCoroutine(ColorButtons(correctColor));
    }

    private void DiscrepancyButton(DocumentButton dButton)
    {
        dButton1 = dButton;
        StartCoroutine(ColorButtons(discrepancyColor));
    }

    private IEnumerator ColorButtons(Color color)
    {
        isProcessing = true;
        Debug.Log("ColorButton Called to color : \nbutton 0 : "+dButton0.index+"\nbutton 1 : "+dButton1.index);
        
        dButton0.SetBackgroundColor(color);
        dButton1.SetBackgroundColor(color);
        yield return new WaitForSeconds(checkingDelay);
        ResetInternalButton(0);
        ResetInternalButton(1);
        Debug.Log("ColorButton Color Finished");
        isProcessing = false;
    }

    private void SetButton0(DocumentButton dButton)
    {
        if(dButton0) dButton0.ResetBackground();
        dButton0 = dButton;
        dButton.SetBackgroundColor(activatedColor);
    }

    private void ResetInternalButton(int index)
    {
        if (index == 0)
        {
            dButton0.ResetBackground();
            dButton0 = null;
        }
        else if (index == 1)
        {
            dButton1.ResetBackground();
            dButton1 = null;
        }
    }
    

}
