using GorillaLocomotion;
using HarmonyLib;

namespace Mercury.Patches
{
    [HarmonyPatch(typeof(Playspace), "Update")]
    public class PlayspacePatch
    {
        public static bool Prefix()
        {
            return false;
        }
    }
}