using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Stamper : MonoBehaviour
{
    [SerializeField] private GameObject AccessStamp;
    [SerializeField] private GameObject StampContact;
    [SerializeField] private float rayRange = 0.005f;
    private XRGrabInteractable XRGI;

    private bool stamping = false;

    private void Start()
    {
        XRGI = this.GetComponent<XRGrabInteractable>();
       
    }

    /*private void Update()
    {
        if (XRGI.isSelected)
        {
            RaycastHit hit;
            //Debug.Log("GRABBBBED");
            Debug.DrawLine(StampContact.transform.position ,StampContact.transform.position -StampContact.transform.up ,Color.green,Time.deltaTime);
            if (Physics.Raycast(StampContact.transform.position, -StampContact.transform.up, out hit, 0.01f))
            {
                if (hit.collider.gameObject.layer == 10 && !stamping)
                {
                    stamping = true;
                    Stamp(hit);
                }
                
            }
            else stamping = false;
        }
    }*/

    private void Stamp(RaycastHit hit )
    {
        GameObject passport = hit.collider.gameObject;
        Debug.Log("[Stamper] Stamping acces denied on " + passport.name);
        GameObject stamp = Instantiate(AccessStamp);
        stamp.transform.position = hit.point;
        stamp.transform.position += hit.normal * 0.001f;
        stamp.transform.forward = - hit.normal;
        stamp.transform.parent = passport.transform;
    }

    private void OnCollisionEnter(Collision other)
    {
        GameObject passport = other.gameObject;
        if (passport.tag == "Passport")
        {
            Debug.Log("On collision Enter for "+this.name);
            if (XRGI.isSelected && !stamping)
            {
                RaycastHit hit;
                //Debug.Log("GRABBBBED");
                //Debug.DrawLine(StampContact.transform.position ,StampContact.transform.position,Color.green,Time.deltaTime);
                if (Physics.Raycast(this.transform.position -this.transform.up * (this.transform.localScale.y /2- rayRange ) , -this.transform.up, out hit, rayRange *2))
                {
                    stamping = true;
                    Stamp(hit);
                }
                
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        stamping = false;
    }
}