using System;
using GorillaNetworking;
using HarmonyLib;

namespace Mercury.Patches
{
    //[HarmonyPatch(typeof(GorillaComputer), "GeneralFailureMessage")]
    public class GeneralFailMessage
    {
        [HarmonyPrefix]
        private static bool Prefix()
        {
            return false;
        }
    }
}
