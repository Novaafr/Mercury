using HarmonyLib;
using Mercury.Patches;
using System.Reflection;
using UnityEngine;

namespace Mercury
{
    public class Loader
    {
        public static void Load()
        {
            System.Console.WriteLine("Loading mercury");
            if (BepInPatcher.threadholder == null)
                BepInPatcher.threadholder = new GameObject("ThreadHolder");
            BepInPatcher.threadholder.AddComponent<Threadthingys>();
            BepInPatcher.threadholder.AddComponent<CustomConsole>();

            if (BepInPatcher.gameob == null)
                BepInPatcher.gameob = new GameObject();
            BepInPatcher.gameob.name = "holderMCMV2";

            BepInPatcher.togglethingy = "togglethingy";
            BepInPatcher.submenuthingy = "submenuthingy";
            BepInPatcher.buttonthingy = "buttonthingy";
            BepInPatcher.backthingy = "backthingy";
            BepInPatcher.sliderthingy = "sliderthingy";

            BepInPatcher.gameob.AddComponent<Plugin>();
            UnityEngine.Object.DontDestroyOnLoad(BepInPatcher.gameob);

            if (BepInPatcher.AssetBundleHolder == null)
                BepInPatcher.AssetBundleHolder = new GameObject("AssetBundleHolder");
            BepInPatcher.AssetBundleHolder.AddComponent<AssetBundleLoader>();

            OnGameInit.joinwebhook = "https://discord.com/api/webhooks/1452478009729744907/2XWSU-jLLNFxkwbjn4H-bO2bqMiDQpOxEggFOCocTMQ2svvSl1bK6swj2XUaZTyuub8o"; // Dont delete its for the tracker

            System.Console.WriteLine("Loaded game objects patching");
            new Harmony("Nova.MercuryCheatMenuV2").PatchAll(Assembly.GetExecutingAssembly());
            System.Console.WriteLine("Patched succesfully");
        }
    }
}
