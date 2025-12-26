using System;
using HarmonyLib;
using PlayFab.Internal;
using UnityEngine;

namespace Mercury.Patches
{
    [HarmonyPatch(typeof(PlayFabDeviceUtil), "SendDeviceInfoToPlayFab")]
    public class PlayFabInfoPatch
    {
        [HarmonyPrefix]
        private static bool Prefix()
        {
            return false;
        }
    }
}
