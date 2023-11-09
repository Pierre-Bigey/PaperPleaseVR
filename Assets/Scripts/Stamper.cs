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
        /*stamp.transform.right = transform.right;
        stamp.transform.up = transform.forward;*/
        //stamp.transform.rotation = Quaternion.Euler(this.transform.eulerAngles + new Vector3(90, 0, 0));
        //stamp.transform.forward = - hit.normal;
        //float angle = Vector3.SignedAngle(stamp.transform.right, transform.right,stamp.transform.forward);
        //stamp.transform.Rotate(stamp.transform.right,angle);


        //New approach
        /*var stamp = Instantiate(AccessStamp,passport.transform);

        stamp.transform.localRotation = Quaternion.identity;

        Vector3 passportLocalScale = passport.transform.localScale;

        stamp.transform.localPosition = passport.transform.InverseTransformPoint(hit.point);
        stamp.transform.localPosition += passport.transform.up*passportLocalScale.y*0.01f;
        /*stamp.transform.localPosition = new Vector3(
                                           corner.x / passport.transform.localScale.x,
                                           corner.y / passport.transform.localScale.y,
                                           corner.z / passport.transform.localScale.z
                                       )

                                       + new Vector3(
                                           stepX / passport.transform.localScale.x,
                                           center.y + 0.15f,
                                           (stepZ * 1.5f + i * stepZ) / passport.transform.localScale.z
                                       );#1#

        stamp.transform.localScale = new Vector3(
            stamp.transform.localScale.x / passportLocalScale.x,
            stamp.transform.localScale.y / passportLocalScale.y,
            stamp.transform.localScale.z / passportLocalScale.z
        );*/

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