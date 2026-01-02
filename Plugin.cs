using GorillaNetworking;
using Mercury.Menu;
using Mercury.Mods;
using Mercury.Notifacation;
using Mercury.Patches;
using Photon.Pun;
using PlayFab;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Mercury
{
    public class Plugin : MonoBehaviour
    {
        public static Plugin test;

        public static GameObject holder;
        public static float version = 8.6f;

        public static bool sussy = false;
        public static bool oculus = false;

        public static float runtime = 0;
        public static float playtime = 0;
        public static string rutimestring;
        public static string playtimestring;

        public static bool devOnGUI = false; // for debugging shit
        private Rect devRect = new Rect(125, 125, 250, 250);
        public void OnGUI()
        {
            if (!devOnGUI)
                return;
            devRect = GUILayout.Window(6969, devRect, dvGUI, "Mercury Dev UI");
        }
        public void dvGUI(int devint)
        {
            if (devOnGUI)
            {
                GUILayout.Label($"Photon Conncet: {PhotonNetwork.IsConnected}");
                GUILayout.Label($"PlayFab Conncet: {PlayFabSettings.staticPlayer.IsClientLoggedIn()}");
                GUILayout.Label($"RoomCount: {PhotonNetwork.CountOfRooms}");
                GUILayout.Label($"PlayerCount: {PhotonNetwork.CountOfPlayers}");
                if (GUILayout.Button("Dump RPC's"))
                {
                    foreach (string rpc in PhotonNetwork.PhotonServerSettings.RpcList)
                    {
                        Debug.Log(rpc);
                    }
                }
                if (GUILayout.Button("Dump RoomInfo"))
                {
                    foreach (Photon.Realtime.Player plr in PhotonNetwork.PlayerListOthers)
                    {
                        Debug.Log($"NickName: {plr.NickName} : UserId: {plr.UserId} : Mater: {plr.IsMasterClient}\nRoomName: {PhotonNetwork.CurrentRoom.Name} : PlayreCount: {PhotonNetwork.CurrentRoom.PlayerCount}");
                    }
                }
            }
        }


        private static bool boughtcosmetics = false;

        static AssetBundle assetBundle;
        private static void LoadAssetBundle()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"Mercury.AssetBundles.utopium");
            if (stream != null)
                assetBundle = AssetBundle.LoadFromStream(stream);
            else
                Debug.LogError("Failed to load assetbundle");
        }

        public static T LoadAsset<T>(string assetName) where T : UnityEngine.Object
        {
            if (assetBundle == null)
                LoadAssetBundle();

            T gameObject = assetBundle.LoadAsset(assetName) as T;
            return gameObject;
        }

        public void Start()
        {
            LoadAssetBundle();
            GameObject go = LoadAsset<GameObject>("utopium");
            Text t = go.GetComponent<Text>();
            Debug.Log("[Mercury] LOADED " + t.name);
            BepInPatcher.gtagfont = t.font;

            test = this;

            // Environment Objects/MonkeBlocksRoomPersistent/MonkeBlocksComputer/UI FOR ATTIC COMPUTER/Text/downtext
            //if (BepInPatcher.gtagfont == null)
            // BepInPatcher.gtagfont = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/UI/debugtext/debugtext").GetComponent<Text>().font;

            HarmonyLoader.ApplyHarmonyPatches();

            //PhotonNetwork.LogLevel = PunLogLevel.Full;
            CustomConsole.Debug("Plugin Start Call");

            CustomConsole.Debug("Spawned Holder");
            holder = new GameObject();
            holder.name = "HolderMCMV2";
            holder.AddComponent<EventNotifacation>();
            holder.AddComponent<JoinNotifacation>();
            holder.AddComponent<LeaveNotifacation>();
            holder.AddComponent<MasterChangeNotifacation>();
            holder.AddComponent<JoinRoom>();
            holder.AddComponent<Configs>();
            //holder.AddComponent<AssetBundleLoader>();
            holder.AddComponent<GUICreator>();
            holder.AddComponent<Mercury.Menu.CustomBinding>();
            holder.AddComponent<Mercury.Console.CoroutineManager>();
            Music.MusicAudio = holder.AddComponent<AudioSource>();


            WhatAmI.OculusCheck();

            if (PhotonNetworkController.Instance.disableAFKKick == false)
            {
                PhotonNetworkController.Instance.disableAFKKick = true;
            }

            //quit box disable
            if (GameObject.Find("Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/QuitBox").activeSelf)
            {
                GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest/Super Infection Zone - Forest Variant/ForestDome_Prefab").SetActive(false);
                GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToHoverboard/TurnOnInForestAndHoverboard").SetActive(false);
                GameObject.Find("Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/QuitBox").SetActive(false);
            }


            if (BepInPatcher.gtagfont != null && holder != null) // Me after writing semi good code 😭 -Colossus
            {
                Menu.Menu.LoadOnce();
                CustomConsole.Debug("Loaded menu start");

                Overlay.SpawnOverlay();
                CustomConsole.Debug("Loaded overlay");

                Notifacations.SpawnNoti();
                CustomConsole.Debug("Loaded noti");

                // removed dev stuff
                //PhotonNetwork.NetworkingClient.EventReceived += Console.Console.Receiver; // Some problem with a null ref, will fix later
            }
        }

        public void Update()
        {
            if (BepInPatcher.gtagfont != null)
            {
                Menu.Menu.Load();
                if (Menu.Menu.agreement)
                {
                    Dictionary<Type, bool> ToggleConditions = new Dictionary<Type, bool>
                    {
                        { typeof(ThisGuyIsUsingColossal), true },
                        { typeof(LongArm), PluginConfig.longarms },
                        { typeof(WhyIsEveryoneLookingAtMe), PluginConfig.whyiseveryonelookingatme },
                        { typeof(WateryAir), PluginConfig.wateryair },
                        { typeof(FreezeMonkey), PluginConfig.freezemonkey },
                        { typeof(Platforms), PluginConfig.platforms },
                        { typeof(TFly), PluginConfig.tfly },
                        { typeof(UpsideDownMonkey), PluginConfig.upsidedownmonkey },
                        { typeof(Chams), PluginConfig.chams },
                        { typeof(HollowBoxEsp), PluginConfig.hollowboxesp },
                        { typeof(BoxEsp), PluginConfig.boxesp },
                        { typeof(CreeperMonkey), PluginConfig.creepermonkey },
                        { typeof(GhostMonkey), PluginConfig.ghostmonkey },
                        { typeof(InvisMonkey), PluginConfig.invismonkey },
                        { typeof(LegMod), PluginConfig.legmod },
                        { typeof(TagGun), PluginConfig.taggun },
                        { typeof(TagAll), PluginConfig.tagall },
                        { typeof(BreakNameTags), PluginConfig.breaknametags },
                        { typeof(SpinBot), PluginConfig.SpinBot },
                        { typeof(Desync), PluginConfig.desync },
                        { typeof(FakeQuestMenu), PluginConfig.fakequestmenu },
                        { typeof(BoneESP), PluginConfig.boneesp },
                        { typeof(ClimbableGorillas), PluginConfig.ClimbableGorillas },
                        { typeof(PlayerScale), PluginConfig.PlayerScale },
                        { typeof(FullBright), PluginConfig.fullbright },
                        { typeof(Panic), PluginConfig.Panic },
                        { typeof(NoClip), PluginConfig.NoClip },
                        { typeof(ForceTagFreeze), PluginConfig.forcetagfreeze },
                        { typeof(NameTags), PluginConfig.NameTags },
                        { typeof(PlayerLog), PluginConfig.PlayerLogging },
                        { typeof(ProximityAlert), PluginConfig.ProximityAlert },
                        { typeof(SplashMonkey), PluginConfig.SplashMonkey },
                        { typeof(SSPlatforms), PluginConfig.SSPlatforms },
                        { typeof(NoLeaves), PluginConfig.NoLeaves },
                        //{ typeof(AudioCrash), PluginConfig.audiocrash },
                        //{ typeof(SpazAllCosmeicsTryOn), PluginConfig.spazallcosmeticstryon },
                        { typeof(SpazAllCosmeics), PluginConfig.spazallcosmetics },
                        { typeof(FreezeAll), PluginConfig.freezeall },
                        //{ typeof(AlwaysGuardian), PluginConfig.alwaysguardian },
                        //{ typeof(GrabAll), PluginConfig.graball },
                        //{ typeof(AssendAll), PluginConfig.assendall },
                        //{ typeof(AppQuitAll), PluginConfig.appquitall },
                        { typeof(SnowBallGun), PluginConfig.snowballgun },
                        { typeof(Throw), PluginConfig.Throw },
                        //{ typeof(DevKickGun), PluginConfig.devkickgun },
                        //{ typeof(DevCrashGun), PluginConfig.devcrashgun },
                        //{ typeof(DevMuteGun), PluginConfig.devmutegun },
                        //{ typeof(DevUnmuteGun), PluginConfig.devunmutegun },
                        //{ typeof(DevAllToHand), PluginConfig.devalltohand },
                        //{ typeof(DevPlatformGun), PluginConfig.devplatformgun },
                        //{ typeof(DevYTTVGun), PluginConfig.devyttvgun },
                        //{ typeof(DevBanGun), PluginConfig.devbangun },
                        //{ typeof(DevRCEGun), PluginConfig.devrcegun },
                        //{ typeof(CrashAll), PluginConfig.crashall },
                        { typeof(FakeReportMenu), PluginConfig.fakereportmenu },
                        { typeof(NameChanger), PluginConfig.namechanger },
                        //{ typeof(CrashAllFirework), PluginConfig.crashallfirework },
                        //{ typeof(CrashGun), PluginConfig.crashgun },
                        //{ typeof(KickGun), PluginConfig.kickgun },
                        //{ typeof(SSSizeChanger), PluginConfig.sssizechanger },
                        //{ typeof(KickAll), PluginConfig.kickall },
                        //{ typeof(LagAll), PluginConfig.lagall },
                        //{ typeof(SSPenis), PluginConfig.sspenisgun },
                        { typeof(Decapitation), PluginConfig.decapitation },
                        { typeof(RainbowMonkey), PluginConfig.rainbowmonkey },
                        { typeof(BadAppleMonkey), PluginConfig.badapplemonkey },
                        { typeof(AntiTag), PluginConfig.antitag },
                        //{ typeof(AntiAim), PluginConfig.antiaim },
                        { typeof(JoystickFly), PluginConfig.joystickfly },
                        { typeof(DisableGhostDoors), PluginConfig.disableghostdoors },
                        //{ typeof(ParticleSpam), PluginConfig.particlespam },
                        { typeof(FakeLag), PluginConfig.fakelag },
                        { typeof(PullMod), PluginConfig.pullmod },
                        { typeof(ElfLauncherSpam), PluginConfig.ElfSpammer },
                        { typeof(WaterSplash), PluginConfig.WaterSplash },
                        { typeof(SpazAllRopes), PluginConfig.spazallropes },
                        //{ typeof(SmoothRig), PluginConfig.smoothrig },
                    };
                    if (ToggleConditions != null)
                    {
                        foreach (var kvp in ToggleConditions)
                        {
                            if (holder != null)
                            {
                                if (kvp.Value && holder.GetComponent(kvp.Key) == null)
                                    holder.AddComponent(kvp.Key);
                            }
                            else
                            {
                                CustomConsole.Error("Holder is null");
                                holder = new GameObject();
                            }
                        }
                    }

                    Dictionary<Type, int> IntConditions = new Dictionary<Type, int>
                    {
                        { typeof(ExcelFly), PluginConfig.excelfly },
                        { typeof(WASDFly), PluginConfig.WASDFly },
                        { typeof(FloatyMonkey), PluginConfig.floatymonkey },
                        { typeof(TagAura), PluginConfig.tagaura },
                        { typeof(WallWalk), PluginConfig.wallwalk },
                        { typeof(SpeedMod), PluginConfig.nearspeed },
                        { typeof(Timer), PluginConfig.Timer },
                        { typeof(Tracers), PluginConfig.tracers },
                        { typeof(SkyColour), PluginConfig.skycolour },
                        { typeof(AntiReport), PluginConfig.antireport },
                        { typeof(FirstPerson), PluginConfig.firstperson },
                        { typeof(HitBoxes), PluginConfig.hitboxes },
                        { typeof(NearPulse), PluginConfig.NearPulse },
                        { typeof(HzHands), PluginConfig.hzhands },
                        //{ typeof(SSGiantEmojis), PluginConfig.ssgiantemojis },
                        { typeof(Aimbot), PluginConfig.Aimbot },
                        { typeof(Strafe), PluginConfig.strafe },
                        { typeof(ColouredBraclet), PluginConfig.colouredbraclet },
                    };
                    if (IntConditions != null)
                    {
                        foreach (var kvp in IntConditions)
                        {
                            if (holder != null)
                            {
                                if (kvp.Value != 0 && holder.GetComponent(kvp.Key) == null)
                                    holder.AddComponent(kvp.Key);
                            }
                            else
                            {
                                CustomConsole.Error("Holder is null");
                                holder = new GameObject();
                            }
                        }
                    }
                }


                // Removed for now, they temp removed kid and pretty sure this bans you
                // Kid Bypass
                //if (KIDManager.KidEnabled)
                //    KIDManager.DisableKid();


                // Playtime counter
                playtime += Time.deltaTime;

                int hours = (int)(playtime / 3600);
                int minutes = (int)((playtime % 3600) / 60);
                int seconds = (int)(playtime % 60);

                playtimestring = "";
                if (hours > 0)
                    playtimestring += hours.ToString("00") + ":";
                if (minutes > 0 || hours > 0)
                    playtimestring += minutes.ToString("00") + ":";
                playtimestring += seconds.ToString("00");


                // Music Player
                switch (PluginConfig.volume)
                {
                    case 0:
                        Music.volume = 1;
                        break;
                    case 1:
                        Music.volume = 0.9f;
                        break;
                    case 2:
                        Music.volume = 0.8f;
                        break;
                    case 3:
                        Music.volume = 0.7f;
                        break;
                    case 4:
                        Music.volume = 0.6f;
                        break;
                    case 5:
                        Music.volume = 0.5f;
                        break;
                    case 6:
                        Music.volume = 0.4f;
                        break;
                    case 7:
                        Music.volume = 0.3f;
                        break;
                    case 8:
                        Music.volume = 0.2f;
                        break;
                    case 9:
                        Music.volume = 0.1f;
                        break;
                }
                if (Music.MusicAudio.loop != PluginConfig.loopmusic)
                    Music.MusicAudio.loop = PluginConfig.loopmusic;
                if (Music.MusicAudio.volume != PluginConfig.volume)
                    Music.MusicAudio.volume = Music.volume;

                string bind = Mercury.Menu.CustomBinding.GetBinds("playmusic");
                if (!string.IsNullOrEmpty(bind) || bind != "UNBOUND")
                {
                    if (ControlsV2.GetControl(bind))
                    {
                        test.StartCoroutine(Music.LoadMusic($"{Configs.musicPath}\\{Menu.Menu.MusicPlayer[0].StringArray[Menu.Menu.MusicPlayer[0].stringsliderind]}.mp3"));
                    }
                }

                // Auto Buy any free cosmetic
                if (PhotonNetwork.IsConnectedAndReady && !boughtcosmetics)
                {
                    foreach (CosmeticsController.CosmeticItem item in CosmeticsController.instance.allCosmetics)
                    {
                        if (item.canTryOn && item.cost == 0 && !CosmeticsController.instance.unlockedCosmetics.Contains(item))
                        {
                            CustomConsole.Debug($"Auto Buying: {item.displayName}");

                            CosmeticsController.instance.itemToBuy = item;
                            CosmeticsController.instance.PurchaseItem();

                            boughtcosmetics = true;
                        }
                    }
                }
            }
        }
    }
}