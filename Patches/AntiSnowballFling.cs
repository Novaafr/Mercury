using Mercury.Menu;
using HarmonyLib;
using UnityEngine;

namespace Mercury.Patches
{
    [HarmonyPatch(typeof(SlingshotProjectile), "CheckForAOEKnockback")]
    public class AntiSnowballFling
    {
        public static bool Prefix()
        {
            if (PluginConfig.antisnowballfling)
            {
                return false;
            }
            return true;
        }
    }
}
