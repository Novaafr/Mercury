﻿using Colossal.Menu;
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

namespace Colossal.Mods
{
    public class NoLeaves : MonoBehaviour
    {
        public void Start()
        {
            if (PluginConfig.NoLeaves)
            {
                //GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest").transform.GetChild(18).gameObject.SetActive(false);
                //GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest").transform.GetChild(19).gameObject.SetActive(false);
                //GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest").transform.GetChild(20).gameObject.SetActive(false);
                //GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest").transform.GetChild(21).gameObject.SetActive(false);
                foreach (GameObject GOs in Resources.FindObjectsOfTypeAll<GameObject>())
                    if (GOs.name.Contains("leaves") && GOs != null)
                        GOs.SetActive(false);
            }
        }
        public void Update()
        {
            if (!PluginConfig.NoLeaves)
            {
                Destroy(this.GetComponent<NoLeaves>());

                foreach (GameObject GOs in Resources.FindObjectsOfTypeAll<GameObject>())
                    if (GOs.name.Contains("leaves") && GOs != null)
                        GOs.SetActive(true);
            }
        }
    }
}
