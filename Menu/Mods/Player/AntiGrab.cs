﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Colossal.Menu;
using HarmonyLib;
using UnityEngine;

namespace Colossal.Patches
{
    internal class AntiGrab
    {
        [HarmonyPatch(typeof(VRRig), "GrabbedByPlayer")]
        public class GrabPatch
        {
            public static bool Prefix(VRRig __instance, VRRig grabbedByRig, bool grabbedBody, bool grabbedLeftHand, bool grabbedWithLeftHand)
            {
                if (PluginConfig.antigrab && __instance == VRRig.LocalRig)
                    return false;

                return true;
            }
        }

        [HarmonyPatch(typeof(VRRig), "DroppedByPlayer")]
        public class DropPatch
        {
            public static bool Prefix(VRRig __instance, VRRig grabbedByRig, Vector3 throwVelocity)
            {
                if (PluginConfig.antigrab && __instance == VRRig.LocalRig)
                    return false;

                return true;
            }
        }

        [HarmonyPatch(typeof(GorillaLocomotion.GTPlayer), "ApplyKnockback")]
        public class KnockbackPatch
        {
            public static bool Prefix(Vector3 direction, float speed)
            {
                if (PluginConfig.antigrab)
                {
                    return false;
                }
                return true;
            }
        }
    }
}
