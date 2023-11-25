using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class EntrantPhotographer : MonoBehaviour
{
    [SerializeField] private GameObject entrantBody;
    [SerializeField] private bool summon;
    [SerializeField] private Camera entrantCamera;
    [SerializeField] private RenderTexture _renderTexture;
    [SerializeField] private Material destination;
    private GameObject entrant;
   

    // Update is called once per frame
    void Update()
    {
        if (summon)
        {
            
            summon = false;
            entrant = Instantiate(entrantBody, transform);
            entrant.SetLayerRecursively(11);
            entrantCamera.gameObject.SetActive(true);
        }
    }

    private void LateUpdate()
    {
        if (entrantCamera.gameObject.activeInHierarchy)
        {
            Texture2D snapshot = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RG32, false);
            entrantCamera.Render();
            RenderTexture.active = entrantCamera.targetTexture;
            snapshot.ReadPixels(new Rect(0,0,_renderTexture.width,_renderTexture.height),0,0);
            entrantCamera.gameObject.SetActive(false);
            Destroy(entrant);
        }
    }
    
    /*void Update()
    {
        if (summon)
        {
            
            summon = false;
            GameObject entrant = Instantiate(entrantBody, transform);
            entrant.SetLayerRecursively(11);
            StartCoroutine(Snapshot());
        }
    }

    private IEnumerator Snapshot()
    {
        yield return new WaitForSeconds(1);
        entrantCamera.gameObject.SetActive(true);
        Texture2D snapshot = new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RG32, false);
        entrantCamera.Render();
        RenderTexture.active = entrantCamera.targetTexture;
        snapshot.ReadPixels(new Rect(0,0,_renderTexture.width,_renderTexture.height),0,0);
        entrantCamera.gameObject.SetActive(false);
        
    }*/
}
