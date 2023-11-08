using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance;  }
    }
    
    public DateTime date { get; private set; }
    
    private void Awake()
    {
        //Singleton instantiation
        if (_instance != null && _instance != this) Destroy(this);
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        
        //date initialization
        
        date = new DateTime(1982, 11, 23);
    }

    public void AddDay()
    {
        date = date.AddDays(1);
    }
    
}
