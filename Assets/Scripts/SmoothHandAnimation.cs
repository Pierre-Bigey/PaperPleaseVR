using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class SmoothHandAnimation : MonoBehaviour
{
    [SerializeField] private Animator handAnimator;
    [SerializeField] private InputActionReference triggerActionRef;
    [SerializeField] private InputActionReference gripActionRef;
    //[SerializeField] private InputActionReference gripTouchActionRef;
    [SerializeField] private InputActionReference triggerTouchActionRef;

    [SerializeField] private float triggerTouchSetter = 0.2f;

    private static readonly int TriggerAnimation = Animator.StringToHash("Trigger");
    private static readonly int GripAnimation = Animator.StringToHash("Grip");
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log("For "+this.gameObject.name+" triggerTOuch = "+triggerTouchActionRef.action.ReadValue<float>());
        float triggerValue = triggerActionRef.action.ReadValue<float>();
        if (triggerValue == 0 && triggerTouchActionRef.action.ReadValue<float>() >0.9f) triggerValue = triggerTouchSetter;
        else if (triggerValue >0) triggerValue =  triggerTouchSetter + (triggerActionRef.action.ReadValue<float>()*(1-triggerTouchSetter));
        handAnimator.SetFloat(TriggerAnimation, triggerValue);

        float gripValue = gripActionRef.action.ReadValue<float>();
        handAnimator.SetFloat(GripAnimation,gripValue);
    }
}
