using Colossal.Menu;
using Colossal.Patches;
using HarmonyLib;
using Oculus.Interaction.Input;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(VRMapIndex), "MapMyFinger", 0)]
    internal class FingerIndex
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            return !PluginConfig.nofinger;
        }
    }
    [HarmonyPatch(typeof(VRMapMiddle), "MapMyFinger", 0)]
    internal class MiddleIndex
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            return !PluginConfig.nofinger;
        }
    }
    [HarmonyPatch(typeof(VRMapThumb), "MapMyFinger", 0)]
    internal class ThumbIndex
    {
        [HarmonyPrefix]
        public static bool Prefix()
        {
            return !PluginConfig.nofinger;
        }
    }

    //public class NoFinger : MonoBehaviour
    //{
    //    public void Update()
    //    {
    //        if (!PluginConfig.nofinger)
    //        {
    //            Destroy(this);
    //            return;
    //        }

    //        if (ControllerInputPoller.instance.leftControllerGripFloat != 0f)
    //            ControllerInputPoller.instance.leftControllerGripFloat = 0f;
    //        if (ControllerInputPoller.instance.rightControllerGripFloat != 0f)
    //            ControllerInputPoller.instance.rightControllerGripFloat = 0f;

    //        if (ControllerInputPoller.instance.leftControllerIndexFloat != 0f)
    //            ControllerInputPoller.instance.leftControllerIndexFloat = 0f;
    //        if (ControllerInputPoller.instance.rightControllerIndexFloat != 0f)
    //            ControllerInputPoller.instance.rightControllerIndexFloat = 0f;
    //    }
    //}
}