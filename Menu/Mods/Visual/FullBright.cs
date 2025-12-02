﻿using Colossal.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

namespace Colossal.Mods
{
    public class FullBright : MonoBehaviour
    {
        public void Start()
        {
            LightmapSettings.lightmapsMode = LightmapsMode.NonDirectional;

            if (SceneManager.GetActiveScene().name.ToLower().Contains("ghost"))
            {
                string path = "GhostReactorRoot/GhostReactorZone";
                if (GameObject.Find(path) != null)
                {
                    if (GameObject.Find(path).activeSelf)
                    {
                        GameLightingManager.instance.SetDesaturateAndTintEnabled(false, Color.white);
                        GameLightingManager.instance.SetAmbientLightDynamic(Color.white);
                    }
                }
            }
        }
        public void Update()
        {
            if (!PluginConfig.fullbright)
            {
                if (LightmapSettings.lightmapsMode != LightmapsMode.CombinedDirectional)
                    LightmapSettings.lightmapsMode = LightmapsMode.CombinedDirectional;

                string path = "GhostReactorRoot/GhostReactorZone";
                if (GameObject.Find(path) != null)
                {
                    if (GameObject.Find(path).activeSelf)
                    {
                        GameLightingManager.instance.SetDesaturateAndTintEnabled(false, Color.black);
                        GameLightingManager.instance.SetAmbientLightDynamic(Color.black);
                    }
                }

                Destroy(this.GetComponent<FullBright>());
            }
        }
    }
}
