using Colossal.Menu;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Colossal.Mods
{
    public class NoWind
    {
        [HarmonyPatch(typeof(ForceVolume), nameof(ForceVolume.OnTriggerEnter), MethodType.Normal)]
        internal class OnTriggerEnter
        {
            [HarmonyPrefix]
            static bool Patch()
            {
                if (PluginConfig.nowind)
                {
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(ForceVolume), nameof(ForceVolume.OnTriggerExit), MethodType.Normal)]
        internal class OnTriggerExit
        {
            [HarmonyPrefix]
            static bool Patch()
            {
                if(PluginConfig.nowind)
                {
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(ForceVolume), nameof(ForceVolume.OnTriggerStay), MethodType.Normal)]
        internal class OnTriggerStay
        {
            [HarmonyPrefix]
            static bool Patch()
            {
                if (PluginConfig.nowind)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
