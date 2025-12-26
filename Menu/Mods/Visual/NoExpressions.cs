using System;
using Mercury.Menu;
using HarmonyLib;
using UnityEngine;

namespace Mercury.Patches
{
    //[HarmonyPatch(typeof(GorillaMouthFlap), "UpdateMouthFlapFlipbook")]
    //[HarmonyPatch(typeof(GorillaMouthFlap), "CheckMouthflapChange")]
    //[HarmonyPatch(typeof(GorillaMouthFlap), "InvokeUpdate")]
    internal class NoExpressions : MonoBehaviour
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            if(PluginConfig.noexpressions)
                return false;
            return true;
        }
    }
}
