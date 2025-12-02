using System;
using Colossal.Menu;
using HarmonyLib;
using UnityEngine;

namespace Colossal.Patches
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
