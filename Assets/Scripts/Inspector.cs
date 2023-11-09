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
    
    private Button button1;
    private Button button2;


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

    public void ActivateButton(DocumentButton button)
    {
        Debug.Log("You have clicked on button n" + button.index + " member of " + button.documentType +
                  " which contains a " + button.information + " of value " + button.value);
    }
    

}
