using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class RulesBookController : MonoBehaviour
{
    
    [SerializeField] private InputActionReference JoystickAction;

    private void OnEnable()
    {
        JoystickAction.action.performed += TurnBookPage;
    }

    private void TurnBookPage(InputAction.CallbackContext obj)
    {
        Vector2 direction = obj.action.ReadValue<Vector2>();
        float x = direction.x;
        if(x<0)  Debug.Log("Turn Left");
        if(x>0)  Debug.Log("Turn Right");
       
    }
}
