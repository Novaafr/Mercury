﻿using HarmonyLib;
using System;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(LegalAgreements), "Update")]
    public class TOSPatch
    {
        private static bool Prefix(LegalAgreements __instance)
        {
            var controllerBehaviourField = typeof(LegalAgreements).GetField("controllerBehaviour", BindingFlags.NonPublic | BindingFlags.Instance);
            if (controllerBehaviourField == null)
            {
                return true;
            }
            var controllerBehaviour = controllerBehaviourField.GetValue(__instance);
            var isDownStickField = controllerBehaviour.GetType().GetField("isDownStick", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (isDownStickField == null)
            {
                return true;
            }
            isDownStickField.SetValue(controllerBehaviour, true);

            var scrollSpeedField = typeof(LegalAgreements).GetField("scrollSpeed", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (scrollSpeedField == null)
            {
                return true;
            }
            scrollSpeedField.SetValue(__instance, 10f);

            var maxScrollSpeedField = typeof(LegalAgreements).GetField("_maxScrollSpeed", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)
                                   ?? typeof(LegalAgreements).GetField("maxScrollSpeed", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (maxScrollSpeedField != null)
            {
                maxScrollSpeedField.SetValue(__instance, 10f);
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(ModIOTermsOfUse_v1), "PostUpdate")]
    public class TOSPatch2
    {
        private static bool Prefix(ModIOTermsOfUse_v1 __instance)
        {
            var turnPageMethod = typeof(ModIOTermsOfUse_v1).GetMethod("TurnPage", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (turnPageMethod == null)
            {
                return true;
            }
            turnPageMethod.Invoke(__instance, new object[] { 999 });

            var controllerBehaviourField = typeof(ModIOTermsOfUse_v1).GetField("controllerBehaviour", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (controllerBehaviourField == null)
            {
                return true;
            }
            var controllerBehaviour = controllerBehaviourField.GetValue(__instance);
            var isDownStickField = controllerBehaviour.GetType().GetField("isDownStick", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (isDownStickField == null)
            {
                return true;
            }
            isDownStickField.SetValue(controllerBehaviour, true);

            var holdTimeField = typeof(ModIOTermsOfUse_v1).GetField("holdTime", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (holdTimeField == null)
            {
                return true;
            }
            holdTimeField.SetValue(__instance, 0.1f);

            return false;
        }
    }

    [HarmonyPatch(typeof(AgeSlider), "PostUpdate")]
    public class TOSPatch3
    {
        private static bool Prefix(AgeSlider __instance)
        {
            var controllerBehaviourField = typeof(AgeSlider).GetField("controllerBehaviour", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (controllerBehaviourField == null)
            {
                return true;
            }
            var controllerBehaviour = controllerBehaviourField.GetValue(__instance);
            var buttonDownField = controllerBehaviour.GetType().GetField("buttonDown", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (buttonDownField == null)
            {
                return true;
            }
            buttonDownField.SetValue(controllerBehaviour, true);

            var holdTimeField = typeof(AgeSlider).GetField("holdTime", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (holdTimeField == null)
            {
                return true;
            }
            holdTimeField.SetValue(__instance, 0.1f);

            return false;
        }
    }

    [HarmonyPatch(typeof(PrivateUIRoom), "StartOverlay")]
    public class TOSPatch4
    {
        private static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(KIDManager), "UseKID")]
    public class TOSPatch5
    {
        private static bool Prefix(ref Task<bool> __result)
        {
            __result = Task.FromResult(false);
            return false;
        }
    }
}