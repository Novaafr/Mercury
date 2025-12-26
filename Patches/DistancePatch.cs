using HarmonyLib;
using UnityEngine;

namespace Mercury.Patches
{
    [HarmonyPatch(typeof(VRRig), "IsPositionInRange")]
    public class DistancePatch
    {
        public static bool enabled;

        public static void Postfix(VRRig __instance, ref bool __result, Vector3 position, float range)
        {
            NetPlayer player = GetPlayerFromVRRig(__instance) ?? null;
            if ((enabled && __instance.isLocal) || (player != null && ShouldBypassChecks(player)))
                __result = true;
        }
        public static NetPlayer GetPlayerFromVRRig(VRRig p) =>
            p.Creator;
        public static bool ShouldBypassChecks(NetPlayer Player) =>
             Player == (NetworkSystem.Instance.LocalPlayer);
    }
}