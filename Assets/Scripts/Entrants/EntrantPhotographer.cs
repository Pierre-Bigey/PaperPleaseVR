using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entrants
{
    public class EntrantPhotographer : MonoBehaviour
    {
        [FormerlySerializedAs("entrantBody")] [SerializeField]
        private GameObject defaultEntrantBody;

        [FormerlySerializedAs("summon")] [SerializeField]
        private bool Test;

        [SerializeField] private Camera entrantCamera;
        [SerializeField] private RenderTexture _renderTexture;
        private GameObject entrant;


        // Update is called once per frame
        void Update()
        {
            if (Test)
            {
                Test = false;
                PhotoEntrant(defaultEntrantBody);
            }
        }

        public void PhotoEntrant(GameObject entrantBody)
        {
            entrant = Instantiate(entrantBody, transform);
            entrant.SetLayerRecursively(11);
            entrantCamera.gameObject.SetActive(true);
        }

        private void LateUpdate()
        {
            if (entrantCamera.gameObject.activeInHierarchy)
            {
                Texture2D snapshot =
                    new Texture2D(_renderTexture.width, _renderTexture.height, TextureFormat.RG32, false);
                entrantCamera.Render();
                RenderTexture.active = entrantCamera.targetTexture;
                snapshot.ReadPixels(new Rect(0, 0, _renderTexture.width, _renderTexture.height), 0, 0);
                entrantCamera.gameObject.SetActive(false);
                Destroy(entrant);
            }
        }

    }
}