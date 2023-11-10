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


    private void Awake()
    {
        //Singleton Instantiation
        if (_instance != null && _instance != this) Destroy(this);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        DocumentButton.ResetNumberOfButton();
    }

    public void ButtonPressed(DocumentButton dButton)
    {
        Debug.Log("You have clicked on dButton n" + dButton.index + " member of " + dButton.documentType.ToString() +
                  " which contains a " + dButton.information.ToString() + " of value " + dButton.value);
        ProcessDButton(dButton);
    }

    private void ProcessDButton(DocumentButton dButton)
    {
        if (dButton0) //If a button is already selected
        {
            //We first check if the button clicked is already selected
            if (dButton0.index == dButton.index)
            {
                ResetInternalButton(0);
            }
            //If not, check if it's the same document
            else if (dButton0.documentType == dButton.documentType) 
            {
                SetButton0(dButton); //We change the button
            }
            //If not check if it is the same type of information
            else if ( dButton0.information != dButton.information)
            {
                IncoherentButton(dButton0); //if not it's incoherent
            }
            else if (dButton0.value.Equals(dButton.value))
            {
                CorrectButton(dButton);
            }
            else
            {
                DiscrepancyButton(dButton);
            }
        }
        else
        {
            SetButton0(dButton);
        }
    }

    private void IncoherentButton(DocumentButton dButton)
    {
        dButton1 = dButton;
        StartCoroutine(BlinkButtons(incoherentColor));
    }

    private void CorrectButton(DocumentButton dButton)
    {
        dButton1 = dButton;
        StartCoroutine(BlinkButtons(correctColor));
    }

    private void DiscrepancyButton(DocumentButton dButton)
    {
        dButton1 = dButton;
        StartCoroutine(BlinkButtons(discrepancyColor));
    }

    private IEnumerator BlinkButtons(Color color)
    {
        dButton0.SetBackgroundColor(color);
        dButton1.SetBackgroundColor(color);
        yield return new WaitForSeconds(checkingDelay);
        ResetInternalButton(0);
        ResetInternalButton(1);
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
