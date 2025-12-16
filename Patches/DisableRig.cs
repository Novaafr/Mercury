﻿using Colossal;
using HarmonyLib;
using Photon.Pun;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(VRRig), "OnDisable")]
    public class DisableRig
    {
        public static bool Prefix(VRRig __instance) =>
            !__instance.isLocal;
    }

    /* [HarmonyPatch(typeof(VRRigJobManager), "DeregisterVRRig")]
     public static class DisableRigBypass
     {
         public static bool Prefix(VRRigJobManager __instance, VRRig rig)
         {
             return !(__instance == VRRig.LocalRig);
         }
     }*/

    [HarmonyPatch(typeof(GRPlayer), "Awake")]
    public class GRPlayerPatch
    {
        public static bool Prefix(VRRig __instance) =>
            __instance.gameObject.name != "Local Gorilla Player(Clone)";
    }

    [HarmonyPatch(typeof(VRRig), "Awake")]
    public class RigPatch2
    {
        public static bool Prefix(VRRig __instance) =>
            __instance.gameObject.name != "Local Gorilla Player(Clone)";
    }

    [HarmonyPatch(typeof(VRRig), "PostTick")]
    public class RigPatch3
    {
        public static bool Prefix(VRRig __instance) =>
            !__instance.isLocal || __instance.enabled;
    }

    [HarmonyPatch(typeof(GRPlayer), "Tick")]
    public static class GRPlayerTP
    {
        public static bool Prefix(GRPlayer __instance)
        {
            if (__instance == null)
                return false;
            if (__instance.MyRig == null || __instance.bodyCenter == null || __instance.transform == null)
            {
                return false; 
            }
            return true; 
        }
    }

    [HarmonyPatch(typeof(PostVRRigPhysicsSynch), "LateUpdate")]
    public static class PostVRRigPhysicsSynchLUP
    {
        public static bool Prefix(PostVRRigPhysicsSynch __instance)
        {
            if (__instance == null)
                return false;
            if (__instance.GetComponent<Rigidbody>() == null)
                return false;
            if (__instance.transform == null)
                return false;
            return true;
        }
    }

    [HarmonyPatch(typeof(GTPosRotConstraintManager), "LateUpdate")]
    public static class GTPosRotConstraintManagerLUP
    {
        public static bool Prefix(GTPosRotConstraintManager __instance)
        {
            if (__instance == null)
                return false;
            if (__instance.transform == null)
                return false;
            if (__instance.transform.parent == null)
                return false;
            return true;
        }
    }
}