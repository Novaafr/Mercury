using Colossal.Menu;
using HarmonyLib;
using UnityEngine.XR.Interaction.Toolkit;

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(VRRig), "PackCompetitiveData")]
    public class FPSSpoof
    {
        public static void Postfix(ref short __result)
        {
            if (PluginConfig.fpsspoof)
            {
                __result = (short)(UnityEngine.Random.Range(0, 255));
            }
        }
    }
}