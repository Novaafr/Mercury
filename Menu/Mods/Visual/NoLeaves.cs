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
                GameObject Forest = GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest");
                foreach (Transform child in Forest.GetComponentsInChildren<Transform>())
                {
                    GameObject unityTempFile = child.gameObject;
                    if (unityTempFile.name.Contains("UnityTempFile-1a1350753b2f46f438d1b5f2c3b9f9db (combined by EdMeshCombiner)") ||
                        unityTempFile.name == "UnityTempFile-1a1350753b2f46f438d1b5f2c3b9f9db (combined by EdMeshCombiner)")
                    {
                        unityTempFile.SetActive(false);
                    }
                }
            }
        }
        public void Update()
        {
            if (!PluginConfig.NoLeaves)
            {
                GameObject Forest = GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest");
                foreach (Transform child in Forest.GetComponentsInChildren<Transform>())
                {
                    GameObject unityTempFile = child.gameObject;
                    if (unityTempFile.name.Contains("UnityTempFile-1a1350753b2f46f438d1b5f2c3b9f9db (combined by EdMeshCombiner)") ||
                        unityTempFile.name == "UnityTempFile-1a1350753b2f46f438d1b5f2c3b9f9db (combined by EdMeshCombiner)")
                    {
                        unityTempFile.SetActive(true);
                    }
                }
                Destroy(this.GetComponent<NoLeaves>());
            }
        }
    }
}
