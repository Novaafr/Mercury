using System.Diagnostics;
using System.Reflection;
using Colossal.Patches;
using HarmonyLib;
using UnityEngine;

namespace Colossal
{
    public class HarmonyLoader
    {
        public static bool IsPatched { get; private set; }
        private static Harmony instance;
        public const string InstanceId = "org.Colossal";

        internal static void ApplyHarmonyPatches()
        {
            if (!IsPatched)
            {
                if (instance == null)
                {
                    instance = new Harmony("org.Colossal");
                }
                instance.PatchAll(Assembly.GetExecutingAssembly());
                IsPatched = true;
            }
        }

        internal static void RemoveHarmonyPatches()
        {
            if (instance != null && IsPatched)
            {
                instance.UnpatchSelf();
                IsPatched = false;
            }
        }
    }
}
