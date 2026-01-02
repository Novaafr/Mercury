using Mercury.Menu;
using HarmonyLib;

namespace Mercury.Patches
{
    [HarmonyPatch(typeof(SnowballThrowable), "PerformSnowballThrowAuthority")]
    public class DisableSnowballThrow
    {
        public static bool Prefix()
        {
            if (PluginConfig.disablesnowballthrow)
            {
                return false;
            }
            return true;
        }
    }
}
