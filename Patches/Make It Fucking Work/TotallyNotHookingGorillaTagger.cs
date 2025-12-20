using HarmonyLib;
using UnityEngine;
using static Colossal.Patches.BepInPatcher;

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(GorillaTagger), "Awake")]
    internal class OnGameInit
    {
        public static string anti1;
        public static string anti2;
        public static string hash;
        public static string betahash;

        public static string localversion = "8.3";
        public static string serverversion = "8.3";

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
            gameob.name = "holderCCMV3";

           // BepInPatcher.gtagfont = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/UI/debugtext/debugtext").GetComponent<UnityEngine.UI.Text>().font;

            gameob.AddComponent<Plugin>();
            UnityEngine.Object.DontDestroyOnLoad(gameob);

            if (AssetBundleHolder == null)
                AssetBundleHolder = new GameObject("AssetBundleHolder");
            AssetBundleHolder.AddComponent<AssetBundleLoader>();



            loggedin = false;

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
