using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using HarmonyLib;
using System.Threading;
using System.Net;
using Photon.Pun;

namespace Colossal.Patches
{
    //[HarmonyPatch(typeof(VRRig), "OnDisable")]
    internal class SIWniwm__ : MonoBehaviour
    {
        public static bool Prefix(VRRig __instance)
        {
            Traverse.Create(__instance).Field("initialized").SetValue(false);
            __instance.muted = false;
            Traverse.Create(__instance).Field("voiceAudio").SetValue(null);
            Traverse.Create(__instance).Field("tempRig").SetValue(null);
            Traverse.Create(__instance).Field("timeSpawned").SetValue(0f);
            Traverse.Create(__instance).Field("tempMatIndex").SetValue(0);
            __instance.setMatIndex = 0;
            __instance.ChangeMaterialLocal(__instance.setMatIndex);
            Traverse.Create(__instance).Field("creator").SetValue(null);
            return false;
        }
    }
}