﻿using BepInEx;
using Mercury.Menu;
using Mercury.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace Mercury.Mods
{
    public class PlayerScale : MonoBehaviour
    {
        public static float scale = 1f;
        NativeSizeChangerSettings settings = new NativeSizeChangerSettings();
        public void Update()
        {
            if (PluginConfig.PlayerScale)
            {
                if (Controls.LeftTrigger() && Controls.RightJoystick())
                {
                    scale -= 0.01f;
                    settings.playerSizeScale = scale;
                }
                if (Controls.RightTrigger() && Controls.RightJoystick())
                {
                    scale += 0.01f;
                    settings.playerSizeScale = scale;
                }
                if (Controls.RightTrigger() && Controls.LeftTrigger() && Controls.RightJoystick())
                {
                    scale = 1f;
                    settings.playerSizeScale = scale;
                    return;
                }


                if (GorillaLocomotion.GTPlayer.Instance.NativeScale != scale)
                    GorillaLocomotion.GTPlayer.Instance.SetNativeScale(settings);

                // stole this from longarms!!!!!!!!
            }
            else
            {
                Destroy(this.GetComponent<PlayerScale>());

                scale = 1f;
                settings.playerSizeScale = scale;
                GorillaLocomotion.GTPlayer.Instance.SetNativeScale(settings);
            }
        }
    }
}