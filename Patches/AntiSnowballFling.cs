using Mercury.Menu;
using HarmonyLib;

namespace Mercury.Patches
{
    // Disables the snowball throwing on your client
    [HarmonyPatch(typeof(SnowballThrowable), "PerformSnowballThrowAuthority")]
    //[HarmonyPatch(typeof(GrowingSnowballThrowable), "PerformSnowballThrowAuthority")]
    public class AntiSnowballFling
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
