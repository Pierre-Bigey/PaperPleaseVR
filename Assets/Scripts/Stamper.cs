using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Stamper : MonoBehaviour
{
    [SerializeField] private GameObject AccessStamp;
    [SerializeField] private float rayRange = 0.005f;
    private XRGrabInteractable XRGI;

    private bool stamping = false;

    private void Start()
    {
        XRGI = this.GetComponent<XRGrabInteractable>();
       
    }

    private void Stamp(RaycastHit hit )
    {
        GameObject passport = hit.collider.gameObject;
        Debug.Log("[Stamper] Stamping  on " + passport.name);
        GameObject stamp = Instantiate(AccessStamp);
        stamp.transform.position = hit.point;
        stamp.transform.position += hit.normal * 0.001f;
        //stamp.transform.rotation = quaternion.identity;
        //stamp.transform.SetParent(passport.transform);
        Vector3 stampUp = Vector3.ProjectOnPlane(this.transform.forward, passport.transform.up);
        //stamp.transform.rotation.SetLookRotation(-passport.transform.up,stampUp);
        
        stamp.transform.SetParent(passport.transform.parent,true);

        stamp.transform.localEulerAngles = new Vector3(90, 0, 0);
        float angle = Vector3.SignedAngle(stampUp,stamp.transform.up,transform.up );
        stamp.transform.localEulerAngles = new Vector3(90, 0, angle);

    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject passport = collision.gameObject;

        if (passport.CompareTag("Passport"))
        {
            Debug.Log("On collision Enter for "+this.name+" with : "+passport.name);
            if (XRGI.isSelected && !stamping)
            {
                RaycastHit hit;
                LayerMask passportOpened = LayerMask.GetMask("PassportStampPage");
                
                //Debug.Log("GRABBBBED");
                Debug.Log("Local scale = " + transform.localScale.y);
                //Vector3 origin = transform.position - transform.up * (transform.localScale.y / 2 - rayRange);
                Vector3 origin = transform.position;
                Vector3 direction = -transform.up;
                Debug.Log("Sending raycast from :"+origin.ToString("F3")+ " with as direction :"+direction.ToString());
                Debug.DrawLine(origin,origin + direction * rayRange*2,Color.magenta,1);
                if (Physics.Raycast(origin, direction, out hit, rayRange *2,passportOpened))
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