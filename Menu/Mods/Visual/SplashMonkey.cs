using Mercury.Menu;
using Photon.Pun;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace Mercury.Mods
{
    public class SplashMonkey : MonoBehaviour
    {
        private GameObject waterbox;
        public void Update()
        {
            if (PluginConfig.SplashMonkey)
            {
                if (waterbox == null)
                {
                    GameObject gameObject = GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach/ForestToBeach_Prefab_V4/CaveWaterVolume");

                    if (!gameObject.activeSelf)
                        gameObject.SetActive(true);
                    waterbox = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    if (waterbox.GetComponent<Renderer>() != null)
                        GameObject.Destroy(waterbox.GetComponent<Renderer>());
                }
                if (GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform)
                    waterbox.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                if (GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform)
                    waterbox.transform.position = GorillaTagger.Instance.leftHandTransform.position;
            }
            else
            {
                Destroy(this.GetComponent<SplashMonkey>());
                if (waterbox != null)
                    GameObject.Destroy(waterbox);
            }
        }
    }
}
