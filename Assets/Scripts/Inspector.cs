using System;
using System.Collections;
using System.Collections.Generic;
using Entrants;
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
                dButton1 = dButton;
                IncoherentButton(); //if not it's incoherent
            }
            else
            {
                Debug.Log("Inspector The buttons share the same type of information, we are going to process them");
                dButton1 = dButton;
                
                if (CompareButtons())
                {
                    CorrectButton();
                }
                else
                {
                    DiscrepancyButton();
                }
            }
        }
        else
        {
            Debug.Log("Inspector This is the first button pressed");
            SetButton0(dButton);
        }
    }


    private bool CompareCalendar(int dButtonCalendar)
    {
        DocumentButton calendarButton = dButton0;
        if (dButtonCalendar == 1) calendarButton = dButton1;
        
        switch (dButton0.information)
        {
            
            case EntrantManager.InfoType.DOB:
                DateTime dob0 = DateTime.ParseExact(dButton0.displayedValue, EntrantManager.dateFormat[dButton0.documentType], null);
                DateTime dob1 = DateTime.ParseExact(dButton1.displayedValue, EntrantManager.dateFormat[dButton1.documentType], null);
                return dob0.Equals(dob1);
            
            
            case EntrantManager.InfoType.EXP_DATE:
                DateTime exp0 = DateTime.ParseExact(dButton0.displayedValue, EntrantManager.dateFormat[dButton0.documentType], null);
                DateTime exp1 = DateTime.ParseExact(dButton1.displayedValue, EntrantManager.dateFormat[dButton1.documentType], null);
                
                //Calendar comparison
                return true;
            
            
            case EntrantManager.InfoType.ACCESS_DURATION:
                //Comparison with Purpose
                return true;
            
            case EntrantManager.InfoType.WORK_END_DATE:
                //Comparison with acces duration and date
                return true;
            
            case EntrantManager.InfoType.ENTER_BY_DATE:
                //Comparison with calendar
                return true;
            
            case EntrantManager.InfoType.VALID_ON_DATE:
                //Comparison with calendar
                return true;
            
            default:
                return true;
        }
    }

    private bool CompareButtons()
    {
        switch (dButton0.information)
        {
            case EntrantManager.InfoType.NAME:
                //String =
                return dButton0.displayedValue.Equals(dButton1.displayedValue);
            
            case EntrantManager.InfoType.DOB:
                DateTime dob0 = DateTime.ParseExact(dButton0.displayedValue, EntrantManager.dateFormat[dButton0.documentType], null);
                DateTime dob1 = DateTime.ParseExact(dButton1.displayedValue, EntrantManager.dateFormat[dButton1.documentType], null);
                return dob0.Equals(dob1);
            
            case EntrantManager.InfoType.PASS_ISS:
                //Rule book comparison
                return true;
            
            case EntrantManager.InfoType.EXP_DATE:
                DateTime exp0 = DateTime.ParseExact(dButton0.displayedValue, EntrantManager.dateFormat[dButton0.documentType], null);
                DateTime exp1 = DateTime.ParseExact(dButton1.displayedValue, EntrantManager.dateFormat[dButton1.documentType], null);
                
                //Calendar comparison
                return true;
            
            case EntrantManager.InfoType.PHOTO:
                //EntrantBody Comparison
                return true;
            
            case EntrantManager.InfoType.SEX:
                //Comparison with name
                return true;
            
            case EntrantManager.InfoType.ID:
                //String =
                return dButton0.displayedValue.Equals(dButton1.displayedValue);
            
            case EntrantManager.InfoType.DISTRICT:
                //RuleBook Comparison
                return true;
            
            case EntrantManager.InfoType.ENTRYPURPOSE:
                //Doesn't exist
                return true;
            
            case EntrantManager.InfoType.ACCESS_DURATION:
                //Comparison with Purpose
                return true;
            
            case EntrantManager.InfoType.HEIGHT:
                //Comparison with entrantBody
                return true;
            
            case EntrantManager.InfoType.WEIGHT:
                //Comparison with entrantBody
                return true;
            
            case EntrantManager.InfoType.DESCRIPTION:
                //Comparison with entrantBody
                return true;
            
            case EntrantManager.InfoType.WORK_SEAL:
                //Comparison with RuleBook
                return true;
            
            case EntrantManager.InfoType.WORK_END_DATE:
                //Comparison with acces duration and date
                return true;
            
            case EntrantManager.InfoType.ENTER_BY_DATE:
                //Comparison with calendar
                return true;
            
            case EntrantManager.InfoType.VALID_ON_DATE:
                //Comparison with calendar
                return true;
            
            case EntrantManager.InfoType.COUNTRY:
                return true;
            
            case EntrantManager.InfoType.ALIAS:
                //Comparison with discourt/Audio transcript
                return true;
            
            case EntrantManager.InfoType.COUNTRY_ACCESS:
                //Comparison with ruleBook
                return true;
            
            case EntrantManager.InfoType.VACCINE:
                //Comparison with RuleBook
                return true;
            
            default:
                return true;
        }
    }

    private void IncoherentButton()
    {
        StartCoroutine(ColorButtons(incoherentColor));
    }

    private void CorrectButton()
    {
        StartCoroutine(ColorButtons(correctColor));
    }

    private void DiscrepancyButton()
    {
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
