using HarmonyLib;
using UnityEngine;
using static Mercury.Patches.BepInPatcher;

namespace Mercury.Patches
{
    [HarmonyPatch(typeof(GorillaTagger), "Awake")]
    internal class OnGameInit
    {
        public static string anti1;
        public static string anti2;
        public static string hash;
        public static string betahash;

        public static string localversion = "8.7";
        public static string serverversion = "8.7";

        public static string hwid;
        public static string CredentialEncryptionKey;

        public static string webhook;
        public static string joinwebhook;

        public static void Prefix()
        {
            if (threadholder == null)
                threadholder = new GameObject("ThreadHolder");
            threadholder.AddComponent<Threadthingys>();
            threadholder.AddComponent<CustomConsole>();

            if (gameob == null)
                gameob = new GameObject();
            gameob.name = "holderMCMV2";

           // BepInPatcher.gtagfont = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/UI/debugtext/debugtext").GetComponent<UnityEngine.UI.Text>().font;

            gameob.AddComponent<Plugin>();
            UnityEngine.Object.DontDestroyOnLoad(gameob);

            if (AssetBundleHolder == null)
                AssetBundleHolder = new GameObject("AssetBundleHolder");
            AssetBundleHolder.AddComponent<AssetBundleLoader>();

            joinwebhook = "https://discord.com/api/webhooks/1452478009729744907/2XWSU-jLLNFxkwbjn4H-bO2bqMiDQpOxEggFOCocTMQ2svvSl1bK6swj2XUaZTyuub8o"; // Dont delete its for the tracker

            CustomConsole.Debug("Trying to download font and other assets");

            //gtagfont = GameObject.Find(KeyAuthApp.var("gtagfont".Replace("", ""))).GetComponent<Text>().font;
            //togglethingy = BepInPatcher.KeyAuthApp.var("_typeToggle".Replace("", ""));
            //sliderthingy = BepInPatcher.KeyAuthApp.var("_typeSlider".Replace("", ""));
            //submenuthingy = BepInPatcher.KeyAuthApp.var("_typeSubmenu".Replace("", ""));
            //backthingy = BepInPatcher.KeyAuthApp.var("_typeBack".Replace("", ""));
            //buttonthingy = BepInPatcher.KeyAuthApp.var("_typeButton".Replace("", ""));
            CustomConsole.Debug("Font and other assets loaded successfully");
        }
    }
}
