using Colossal.Notifacation;
using Colossal.Patches;
using ExitGames.Client.Photon;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using JoinType = GorillaNetworking.JoinType;

namespace Colossal.Menu
{
    public class MenuOption
    {
        public string DisplayName;
        public string _type;
        public bool AssociatedBool;
        public string AssociatedString;
        public float AssociatedFloat;
        public int AssociatedInt;
        public string[] StringArray;
        public int stringsliderind;
        public string AssociatedBind;
        public string extra;
    }
    public class Menu : MonoBehaviour
    {
        public static bool GUIToggled = true;


        public static GameObject MenuHub;
        public static Text MenuHubText;


        public static GameObject AgreementHub;
        public static Text AgreementHubText;


        public static GameObject comictext;


        public static string MenuColour = "magenta";
        public static float menurgb = 0;


        private static GameObject pointerObj;
        private static PanelElement activePanel; // Track the currently active panel
        private static PanelElement grabbedPanel = null; // Ensure this is a field
        private static Vector3 grabOffset;
        private static bool isVRModeActive = false;
        private static bool vrInputDetected = false;
        private static bool hasReceivedMouseInput = false;

        public static string MenuState = "MainMenu";
        public static int SelectedOptionIndex = 0;

        public static MenuOption[] CurrentViewingMenu = null;
        public static MenuOption[] MainMenu;
        public static MenuOption[] Movement;
        public static MenuOption[] Movement2;
        public static MenuOption[] Visual;
        public static MenuOption[] Visual2;
        public static MenuOption[] Player;
        public static MenuOption[] Player2;
        public static MenuOption[] Computer;
        public static MenuOption[] Gamemodes;
        public static MenuOption[] Exploits;
        public static MenuOption[] Exploits2;
        public static MenuOption[] Safety;
        public static MenuOption[] Settings;
        public static MenuOption[] Info;
        public static MenuOption[] MusicPlayer;
        public static MenuOption[] Dev;
        private static bool devMenuAdded = false;

        public static MenuOption[] Speed;
        public static MenuOption[] Strafe;
        public static MenuOption[] Tracers;
        public static MenuOption[] NameTags;
        public static MenuOption[] CosmeticsSpoofer;

        public static MenuOption[] ColourSettings;


        private static bool isGrabbing = false;
        public static bool inputcooldown = false;
        public static bool menutogglecooldown = false;
        public static bool agreement = false;

        private static string roomtojoin = "";

        public static void LoadOnce()
        {
            try
            {
                if (!agreement)
                {
                    // AssetBundleLoader.SpawnVoidBubbles();

                    (AgreementHub, AgreementHubText) = GUICreator.CreateTextGUI("<color=magenta><VR CONTROLS></color>\nLeft Joystick Click (Hold):\nRight Grip: Select\nRight Trigger: Scroll\nLeft Trigger: Custom Bind\nLeft Grip: Remove Custom Bind\nBoth Joysticks: Toggle UI\n\n<color=magenta><PC CONTROLS></color>\nEnterKey: Select\nArrowKey (Up): Move Up\nArrowKey (Down): Move Down\n\n<color=red>Be Patient While Loading</color>\n<color=cyan>Press Both Joysticks Or Enter...</color>", "AgreementHub", TextAnchor.MiddleCenter, new Vector3(0, 0f, 2), true);
                }
                else
                {
                    // Adding once the menu has been made or like whatever because it causes errors
                    //if (Plugin.holder.GetComponent<SpeedMod>() == null)
                    //    Plugin.holder.AddComponent<SpeedMod>();
                    //if (Plugin.holder.GetComponent<SkyColour>() == null)
                    //    Plugin.holder.AddComponent<SkyColour>();
                    //if (Plugin.holder.GetComponent<AntiReport>() == null)
                    //    Plugin.holder.AddComponent<AntiReport>();

                    // Adding here so you dont see them before you accepted the aggreement
                    if (PluginConfig.legacyUi)
                    {
                        if (Plugin.holder.GetComponent<Overlay>() == null)
                            Plugin.holder.AddComponent<Overlay>();

                        if (Plugin.holder.GetComponent<Notifacations>() == null)
                            Plugin.holder.AddComponent<Notifacations>();
                    }

                    if (Plugin.holder.GetComponent<ToolTips>() == null)
                        Plugin.holder.AddComponent<ToolTips>();

                    if (Plugin.holder.GetComponent<Boards>() == null)
                        Plugin.holder.AddComponent<Boards>();

                    CustomConsole.Debug("Added all menu components");


                    //if (BepInPatcher.buttonthingy.IsNullOrEmpty().ToString().IsNullOrWhiteSpace() || BepInPatcher.backthingy.IsNullOrEmpty().ToString().IsNullOrWhiteSpace() || BepInPatcher.submenuthingy.IsNullOrEmpty().ToString().IsNullOrWhiteSpace() || BepInPatcher.sliderthingy.IsNullOrEmpty().ToString().IsNullOrWhiteSpace() || BepInPatcher.togglethingy.IsNullOrEmpty().ToString().IsNullOrWhiteSpace())
                    if (!BepInPatcher.buttonthingy.IsNullOrEmpty() || !BepInPatcher.backthingy.IsNullOrEmpty() || !BepInPatcher.submenuthingy.IsNullOrEmpty() || !BepInPatcher.sliderthingy.IsNullOrEmpty() || !BepInPatcher.togglethingy.IsNullOrEmpty())
                    {
                        (MenuHub, MenuHubText) = GUICreator.CreateTextGUI("", "MenuHub", TextAnchor.UpperLeft, new Vector3(0, 0.4f, 3.6f), true);

                        MainMenu = new MenuOption[12];
                        MainMenu[0] = new MenuOption { DisplayName = "Movement", _type = "submenuthingy", AssociatedString = "Movement" };
                        MainMenu[1] = new MenuOption { DisplayName = "Visual", _type = "submenuthingy", AssociatedString = "Visual" };
                        MainMenu[2] = new MenuOption { DisplayName = "Player", _type = "submenuthingy", AssociatedString = "Player" };
                        MainMenu[3] = new MenuOption { DisplayName = "Computer", _type = "submenuthingy", AssociatedString = "Computer" };
                        MainMenu[4] = new MenuOption { DisplayName = "Exploits", _type = "submenuthingy", AssociatedString = "Exploits" };
                        MainMenu[5] = new MenuOption { DisplayName = "Safety", _type = "submenuthingy", AssociatedString = "Safety" };
                        MainMenu[6] = new MenuOption { DisplayName = "MusicPlayer", _type = "submenuthingy", AssociatedString = "MusicPlayer" };
                        MainMenu[7] = new MenuOption { DisplayName = "Settings", _type = "submenuthingy", AssociatedString = "Settings" };
                        MainMenu[8] = new MenuOption { DisplayName = "Info", _type = "submenuthingy", AssociatedString = "Info" };
                        MainMenu[9] = new MenuOption { DisplayName = "Notifications", _type = "togglethingy", AssociatedBool = PluginConfig.Notifications };
                        MainMenu[10] = new MenuOption { DisplayName = "Overlay", _type = "togglethingy", AssociatedBool = PluginConfig.overlay };
                        MainMenu[11] = new MenuOption { DisplayName = "Tool Tips", _type = "togglethingy", AssociatedBool = PluginConfig.tooltips };

                        Movement = new MenuOption[12];
                        Movement[0] = new MenuOption { DisplayName = "ExcelFly", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "Super Slow", "Slow", "Medium", "Fast", "Super Fast" } };
                        Movement[1] = new MenuOption { DisplayName = "TFly", _type = "togglethingy", AssociatedBool = PluginConfig.tfly };
                        Movement[2] = new MenuOption { DisplayName = "WallWalk", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "6.8", "7", "7.5", "7.8", "8", "8.5", "8.8", "9", "9.5", "9.8" } };
                        Movement[3] = new MenuOption { DisplayName = "Speed Options", _type = "submenuthingy", AssociatedString = "Speed Options" };
                        Movement[4] = new MenuOption { DisplayName = "Platforms", _type = "togglethingy", AssociatedBool = PluginConfig.platforms };
                        Movement[5] = new MenuOption { DisplayName = "UpsideDown Monkey", _type = "togglethingy", AssociatedBool = PluginConfig.upsidedownmonkey };
                        Movement[6] = new MenuOption { DisplayName = "WateryAir", _type = "togglethingy", AssociatedBool = PluginConfig.wateryair };
                        Movement[7] = new MenuOption { DisplayName = "LongArms", _type = "togglethingy", AssociatedBool = PluginConfig.longarms };
                        Movement[8] = new MenuOption { DisplayName = "SpinBot", _type = "togglethingy", AssociatedBool = PluginConfig.SpinBot };
                        Movement[9] = new MenuOption { DisplayName = "WASDFly", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "5", "7", "10", "13", "16" } };
                        Movement[10] = new MenuOption { DisplayName = "Next", _type = "submenuthingy", AssociatedString = "Movement2" };
                        Movement[11] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Movement2 = new MenuOption[14];
                        Movement2[0] = new MenuOption { DisplayName = "Timer", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "1.03x", "1.06x", "1.09x", "1.1x", "1.13x", "1.16x", "1.19x", "1.2x", "1.23x", "1.26", "1.29", "1.3x", "2x", "3x", "4x", "5x" } };
                        Movement2[1] = new MenuOption { DisplayName = "FloatyMonkey", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "1.1", "1.2", "1.4", "1.6", "1.8", "2", "2.2", "2.4", "2.6", "2.8", "3", "3.2", "3.4", "3.6", "3.8", "4", "Anti Grav" } };
                        Movement2[2] = new MenuOption { DisplayName = "Climbable Gorillas", _type = "togglethingy", AssociatedBool = PluginConfig.ClimbableGorillas };
                        Movement2[3] = new MenuOption { DisplayName = "Near Pulse", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" } };
                        Movement2[4] = new MenuOption { DisplayName = "Near Pulse Distance", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" } };
                        Movement2[5] = new MenuOption { DisplayName = "Player Scale", _type = "togglethingy", AssociatedBool = PluginConfig.PlayerScale };
                        Movement2[6] = new MenuOption { DisplayName = "No Clip", _type = "togglethingy", AssociatedBool = PluginConfig.NoClip };
                        Movement2[7] = new MenuOption { DisplayName = "Force Tag Freeze", _type = "togglethingy", AssociatedBool = PluginConfig.forcetagfreeze };
                        Movement2[8] = new MenuOption { DisplayName = "Teleport To Random", _type = "buttonthingy", AssociatedString = "teleporttorandom" };
                        Movement2[9] = new MenuOption { DisplayName = "HZ Hands", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" } };
                        Movement2[10] = new MenuOption { DisplayName = "Throw", _type = "togglethingy", AssociatedBool = PluginConfig.Throw };
                        Movement2[11] = new MenuOption { DisplayName = "Strafe Options", _type = "submenuthingy", AssociatedString = "Strafe Options" };
                        Movement2[12] = new MenuOption { DisplayName = "Joystick Fly", _type = "togglethingy", AssociatedBool = PluginConfig.joystickfly };
                        Movement2[13] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Speed = new MenuOption[5];
                        Speed[0] = new MenuOption { DisplayName = "Speed", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "7", "7.2", "7.4", "7.6", "7.8", "8", "8.2", "8.4", "8.6" } };
                        Speed[1] = new MenuOption { DisplayName = "Speed Toggle", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "7", "7.2", "7.4", "7.6", "7.8", "8", "8.2", "8.4", "8.6" } };
                        Speed[2] = new MenuOption { DisplayName = "Near Speed", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "7", "7.2", "7.4", "7.6", "7.8", "8", "8.2", "8.4", "8.6" } };
                        Speed[3] = new MenuOption { DisplayName = "Near Speed Distance", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25" } };
                        Speed[4] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Strafe = new MenuOption[4];
                        Strafe[0] = new MenuOption { DisplayName = "Strafe", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "Look", "Target", "Target [TEAM]", "L Joystick" } };
                        Strafe[1] = new MenuOption { DisplayName = "Strafe Speed", _type = "sliderthingy", StringArray = new string[] { "6", "8", "10", "12", "14", "16", "18", "20" } };
                        Strafe[2] = new MenuOption { DisplayName = "Strafe Jump Amount", _type = "sliderthingy", StringArray = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" } };
                        Strafe[3] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Visual = new MenuOption[12];
                        Visual[0] = new MenuOption { DisplayName = "Chams", _type = "togglethingy", AssociatedBool = PluginConfig.chams };
                        Visual[1] = new MenuOption { DisplayName = "BoxESP", _type = "togglethingy", AssociatedBool = PluginConfig.boxesp };
                        Visual[2] = new MenuOption { DisplayName = "HollowBoxESP", _type = "togglethingy", AssociatedBool = PluginConfig.hollowboxesp };
                        Visual[3] = new MenuOption { DisplayName = "BoneESP", _type = "togglethingy", AssociatedBool = PluginConfig.boneesp };
                        Visual[4] = new MenuOption { DisplayName = "Tracers", _type = "submenuthingy", AssociatedString = "Tracers" };
                        Visual[5] = new MenuOption { DisplayName = "NameTags", _type = "submenuthingy", AssociatedString = "NameTags" };
                        Visual[6] = new MenuOption { DisplayName = "Proximity Alert", _type = "togglethingy", AssociatedBool = PluginConfig.ProximityAlert };
                        Visual[7] = new MenuOption { DisplayName = "Full Bright", _type = "togglethingy", AssociatedBool = PluginConfig.fullbright };
                        Visual[8] = new MenuOption { DisplayName = "Sky Colour", _type = "sliderthingy", StringArray = new string[] { "Default", "Purple", "Red", "Cyan", "Green", "Black" } };
                        Visual[9] = new MenuOption { DisplayName = "WhyIsEveryoneLookingAtMe", _type = "togglethingy", AssociatedBool = PluginConfig.whyiseveryonelookingatme };
                        Visual[10] = new MenuOption { DisplayName = "Next", _type = "submenuthingy", AssociatedString = "Visual2" };
                        Visual[11] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Visual2 = new MenuOption[7];
                        Visual2[0] = new MenuOption { DisplayName = "SplashMonkey", _type = "togglethingy", AssociatedBool = PluginConfig.SplashMonkey };
                        Visual2[1] = new MenuOption { DisplayName = "NoLeaves", _type = "togglethingy", AssociatedBool = PluginConfig.NoLeaves };
                        Visual2[2] = new MenuOption { DisplayName = "ComicTags [DISABLED]", _type = "togglethingy", AssociatedBool = PluginConfig.ComicTags };
                        Visual2[3] = new MenuOption { DisplayName = "Anti Screen Share", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "VR View", "PC View" } };
                        Visual2[4] = new MenuOption { DisplayName = "CCM Sight", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "Self & Others", "Others", "Self" } };
                        Visual2[5] = new MenuOption { DisplayName = "Show Boards", _type = "togglethingy", AssociatedBool = PluginConfig.ShowBoards };
                        Visual2[6] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Tracers = new MenuOption[3];
                        Tracers[0] = new MenuOption { DisplayName = "Tracers", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "RHand", "LHand", "Head", "Screen" } };
                        Tracers[1] = new MenuOption { DisplayName = "Tracer Size", _type = "sliderthingy", StringArray = new string[] { "Extremely Small", "Super Small", "Small", "Medium", "Large", "Giant", "Huge" } };
                        Tracers[2] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        NameTags = new MenuOption[10];
                        NameTags[0] = new MenuOption { DisplayName = "NameTags", _type = "togglethingy", AssociatedBool = PluginConfig.NameTags };
                        NameTags[1] = new MenuOption { DisplayName = "Show Creation Date", _type = "togglethingy", AssociatedBool = PluginConfig.ShowCreationDate };
                        NameTags[2] = new MenuOption { DisplayName = "Show Colour Code", _type = "togglethingy", AssociatedBool = PluginConfig.ShowColourCode };
                        NameTags[3] = new MenuOption { DisplayName = "Show Distance", _type = "togglethingy", AssociatedBool = PluginConfig.ShowDistance };
                        NameTags[4] = new MenuOption { DisplayName = "Always Visible", _type = "togglethingy", AssociatedBool = PluginConfig.AlwaysVisible };
                        NameTags[5] = new MenuOption { DisplayName = "Show FPS", _type = "togglethingy", AssociatedBool = PluginConfig.ShowFPS };
                        NameTags[6] = new MenuOption { DisplayName = "NameTag Height", _type = "sliderthingy", StringArray = new string[] { "Chest", "Above Head" } };
                        NameTags[7] = new MenuOption { DisplayName = "NameTag Size", _type = "sliderthingy", StringArray = new string[] { "Chest Size", "Small", "Medium", "Large" } };
                        NameTags[8] = new MenuOption { DisplayName = "NameTag Colour", _type = "sliderthingy", StringArray = new string[] { "White", "Yellow", "Green", "Blue", "Red", "Cyan", "Black" } };
                        NameTags[9] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Player = new MenuOption[14];
                        Player[0] = new MenuOption { DisplayName = "NoFinger", _type = "togglethingy", AssociatedBool = PluginConfig.nofinger };
                        Player[1] = new MenuOption { DisplayName = "TagGun", _type = "togglethingy", AssociatedBool = PluginConfig.taggun };
                        Player[2] = new MenuOption { DisplayName = "CreeperMonkey", _type = "togglethingy", AssociatedBool = PluginConfig.creepermonkey };
                        Player[3] = new MenuOption { DisplayName = "GhostMonkey", _type = "togglethingy", AssociatedBool = PluginConfig.ghostmonkey };
                        Player[4] = new MenuOption { DisplayName = "InvisMonkey", _type = "togglethingy", AssociatedBool = PluginConfig.invismonkey };
                        Player[5] = new MenuOption { DisplayName = "TagAura", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "Really Close", "Close", "Legit", "Semi Legit", "Semi Blatant", "Blatant", "Rage" } };
                        Player[6] = new MenuOption { DisplayName = "TagAll", _type = "togglethingy", AssociatedBool = PluginConfig.tagall };
                        Player[7] = new MenuOption { DisplayName = "Desync [DISABLED]", _type = "togglethingy", AssociatedBool = PluginConfig.desync };
                        Player[8] = new MenuOption { DisplayName = "HitBoxes", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "Really Close", "Close", "Legit", "Semi Legit", "Semi Blatant", "Blatant", "Rage" } };
                        Player[9] = new MenuOption { DisplayName = "No Wind", _type = "togglethingy", AssociatedBool = PluginConfig.nowind };
                        Player[10] = new MenuOption { DisplayName = "Anti Grab", _type = "togglethingy", AssociatedBool = PluginConfig.antigrab };
                        Player[11] = new MenuOption { DisplayName = "Name Changer", _type = "togglethingy", AssociatedBool = PluginConfig.namechanger, extra = "[STUMP]" };
                        Player[12] = new MenuOption { DisplayName = "Next", _type = "submenuthingy", AssociatedString = "Player2" };
                        Player[13] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Player2 = new MenuOption[12];
                        Player2[0] = new MenuOption { DisplayName = "Decapitation", _type = "togglethingy", AssociatedBool = PluginConfig.decapitation };
                        Player2[1] = new MenuOption { DisplayName = "Rainbow Monkey", _type = "togglethingy", AssociatedBool = PluginConfig.rainbowmonkey, extra = "[STUMP]" };
                        Player2[2] = new MenuOption { DisplayName = "Bad Apple Monkey", _type = "togglethingy", AssociatedBool = PluginConfig.badapplemonkey, extra = "[STUMP]" };
                        Player2[3] = new MenuOption { DisplayName = "Aimbot", _type = "sliderthingy", extra = "[PAINTBRAWL]", StringArray = new string[] { "[OFF]", "Silent Aim", "Slilent Aim (Preds)" } };
                        Player2[4] = new MenuOption { DisplayName = "Anti Tag", _type = "togglethingy", AssociatedBool = PluginConfig.antitag };
                        Player2[5] = new MenuOption { DisplayName = "Fake Lag", _type = "togglethingy", AssociatedBool = PluginConfig.fakelag };
                        Player2[6] = new MenuOption { DisplayName = "Disable Ghost Doors", _type = "togglethingy", AssociatedBool = PluginConfig.disableghostdoors };
                        Player2[7] = new MenuOption { DisplayName = "Coloured Braclet", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "Rainbow", "Purple", "Black", "White", "Red", "Green", "Blue", "Yellow" } };
                        Player2[8] = new MenuOption { DisplayName = "Ghost Self", _type = "buttonthingy", AssociatedString = "ghostself", extra = "[GR]" };
                        Player2[9] = new MenuOption { DisplayName = "Ghost Revive Self", _type = "buttonthingy", AssociatedString = "ghostreviveself", extra = "[M] [GR]" };
                        Player2[10] = new MenuOption { DisplayName = "FPS Spoof", _type = "togglethingy", AssociatedBool = PluginConfig.fpsspoof };
                        Player2[11] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Exploits = new MenuOption[10];
                        Exploits[0] = new MenuOption { DisplayName = "Break NameTags", _type = "togglethingy", AssociatedBool = PluginConfig.breaknametags };
                        Exploits[1] = new MenuOption { DisplayName = "SS Platforms", _type = "togglethingy", AssociatedBool = PluginConfig.SSPlatforms, extra = "[M] [BASEMENT]" };
                        Exploits[2] = new MenuOption { DisplayName = "Audio Crash", _type = "togglethingy", AssociatedBool = PluginConfig.audiocrash };
                        Exploits[3] = new MenuOption { DisplayName = "Cosmetics Spoofer", _type = "submenuthingy", AssociatedString = "Cosmetics Spoofer" };
                        Exploits[4] = new MenuOption { DisplayName = "Freeze All", _type = "togglethingy", AssociatedBool = PluginConfig.freezeall };
                        Exploits[5] = new MenuOption { DisplayName = "Become Guardian", _type = "buttonthingy", AssociatedString = "Become Guardian", extra = "[M]" };
                        Exploits[6] = new MenuOption { DisplayName = "Always Guardian", _type = "togglethingy", AssociatedBool = PluginConfig.alwaysguardian };
                        Exploits[7] = new MenuOption { DisplayName = "Assend All", _type = "togglethingy", AssociatedBool = PluginConfig.assendall, extra = "[GUARDIAN]" };
                        Exploits[8] = new MenuOption { DisplayName = "Next", _type = "submenuthingy", AssociatedString = "Exploits2" };
                        Exploits[9] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Exploits2 = new MenuOption[5];
                        Exploits2[0] = new MenuOption { DisplayName = "App Quit All", _type = "togglethingy", AssociatedBool = PluginConfig.appquitall, extra = "[GUARDIAN]" };
                        Exploits2[1] = new MenuOption { DisplayName = "Snowball Gun", _type = "togglethingy", AssociatedBool = PluginConfig.snowballgun };
                        Exploits2[2] = new MenuOption { DisplayName = "Max Quest Score", _type = "buttonthingy", AssociatedString = "Max Quest Score" };
                        Exploits2[3] = new MenuOption { DisplayName = "Kick All", _type = "togglethingy", AssociatedBool = PluginConfig.kickall, extra = "[GUARDIAN]" };
                        Exploits2[4] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        CosmeticsSpoofer = new MenuOption[2];
                        CosmeticsSpoofer[0] = new MenuOption { DisplayName = "Spaz All Cosmetics", _type = "togglethingy", AssociatedBool = PluginConfig.spazallcosmetics };
                        CosmeticsSpoofer[1] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Computer = new MenuOption[10];
                        Computer[0] = new MenuOption { DisplayName = "Disconnect", _type = "buttonthingy", AssociatedString = "disconnect" };
                        Computer[1] = new MenuOption { DisplayName = "Join GTC", _type = "buttonthingy", AssociatedString = "join GTC" };
                        Computer[2] = new MenuOption { DisplayName = "Join TTT", _type = "buttonthingy", AssociatedString = "join TTT" };
                        Computer[3] = new MenuOption { DisplayName = "Join YTTV", _type = "buttonthingy", AssociatedString = "join YTTV" };
                        Computer[4] = new MenuOption { DisplayName = "Join MODS", _type = "buttonthingy", AssociatedString = "join MODS" };
                        Computer[5] = new MenuOption { DisplayName = "Join MOD", _type = "buttonthingy", AssociatedString = "join MOD" };
                        Computer[6] = new MenuOption { DisplayName = "Join:", _type =   "buttonthingy", AssociatedString = "join", extra = "roomtojoin" };
                        Computer[7] = new MenuOption { DisplayName = "Join CCMV2 Only", _type = "buttonthingy", AssociatedString = "join CCMV2 Only" };
                        Computer[8] = new MenuOption { DisplayName = "Gamemodes", _type = "submenuthingy", AssociatedString = "Gamemodes" };
                        Computer[9] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Gamemodes = new MenuOption[10];
                        Gamemodes[0] = new MenuOption { DisplayName = "Modded Gamemode", _type = "togglethingy", AssociatedBool = PluginConfig.moddedgamemode };
                        Gamemodes[1] = new MenuOption { DisplayName = "Competitive Gamemode", _type = "togglethingy", AssociatedBool = PluginConfig.competitivegamemode };
                        Gamemodes[2] = new MenuOption { DisplayName = "Infection", _type = "buttonthingy", AssociatedString = "cgamemode Infection" };
                        Gamemodes[3] = new MenuOption { DisplayName = "Casual", _type = "buttonthingy", AssociatedString = "cgamemode Casual" };
                        Gamemodes[4] = new MenuOption { DisplayName = "Hunt", _type = "buttonthingy", AssociatedString = "cgamemode Hunt" };
                        Gamemodes[5] = new MenuOption { DisplayName = "PaintBrawl", _type = "buttonthingy", AssociatedString = "cgamemode Paintbrawl" };
                        Gamemodes[6] = new MenuOption { DisplayName = "Guardian", _type = "buttonthingy", AssociatedString = "cgamemode Guardian" };
                        Gamemodes[7] = new MenuOption { DisplayName = "Ambush", _type = "buttonthingy", AssociatedString = "cgamemode Ambush" };
                        Gamemodes[8] = new MenuOption { DisplayName = "Freeze Tag", _type = "buttonthingy", AssociatedString = "cgamemode FreezeTag" };
                        Gamemodes[9] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Safety = new MenuOption[7];
                        Safety[0] = new MenuOption { DisplayName = "Panic", _type = "togglethingy", AssociatedBool = PluginConfig.Panic };
                        Safety[1] = new MenuOption { DisplayName = "AntiReport", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "Disconnect", "Reconnect", "Join Random" } };
                        Safety[2] = new MenuOption { DisplayName = "RandomIdentity", _type = "buttonthingy", AssociatedString = "randomidentity" };
                        Safety[3] = new MenuOption { DisplayName = "Pc Check Bypass", _type = "togglethingy", AssociatedBool = PluginConfig.pccheckbypass };
                        Safety[4] = new MenuOption { DisplayName = "Fake Quest Menu", _type = "togglethingy", AssociatedBool = PluginConfig.fakequestmenu };
                        Safety[5] = new MenuOption { DisplayName = "Fake Report Menu", _type = "togglethingy", AssociatedBool = PluginConfig.fakereportmenu };
                        Safety[6] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Settings = new MenuOption[11];
                        Settings[0] = new MenuOption { DisplayName = "Colour Settings", _type = "submenuthingy", AssociatedString = "ColourSettings" };
                        Settings[1] = new MenuOption { DisplayName = "MenuPosition", _type = "sliderthingy", StringArray = new string[] { "Top Left", "Middle", "Top Right" } };
                        Settings[2] = new MenuOption { DisplayName = "Config", _type = "sliderthingy", StringArray = new string[] { } };
                        Settings[3] = new MenuOption { DisplayName = "Load Config", _type = "buttonthingy", AssociatedString = "loadconfig" };
                        Settings[4] = new MenuOption { DisplayName = "Save Config", _type = "buttonthingy", AssociatedString = "saveconfig" };
                        Settings[5] = new MenuOption { DisplayName = "Player Logging", _type = "togglethingy", AssociatedBool = PluginConfig.PlayerLogging };
                        Settings[6] = new MenuOption { DisplayName = "Inverted Controls", _type = "togglethingy", AssociatedBool = PluginConfig.invertedControls };
                        Settings[7] = new MenuOption { DisplayName = "Legacy UI", _type = "togglethingy", AssociatedBool = PluginConfig.legacyUi };
                        Settings[8] = new MenuOption { DisplayName = "Log Out", _type = "buttonthingy", AssociatedString = "logout" };
                        Settings[9] = new MenuOption { DisplayName = "Dev", _type = "submenuthingy", AssociatedString = "Dev" };
                        Settings[10] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        Info = new MenuOption[5];
                        Info[0] = new MenuOption { DisplayName = "PlayerList", _type = "buttonthingy" };
                        Info[1] = new MenuOption { DisplayName = "GTC Ranked Codes", _type = "buttonthingy" };
                        Info[2] = new MenuOption { DisplayName = "IIDK Menu Users", _type = "buttonthingy" };
                        Info[3] = new MenuOption { DisplayName = "CCMV2 Menu Users", _type = "buttonthingy" };
                        Info[4] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        MusicPlayer = new MenuOption[8];
                        MusicPlayer[0] = new MenuOption { DisplayName = "Music", _type = "sliderthingy", StringArray = new string[] { } };
                        MusicPlayer[1] = new MenuOption { DisplayName = "Play Music", _type = "buttonthingy", AssociatedString = "playmusic" };
                        MusicPlayer[2] = new MenuOption { DisplayName = "Stop Music", _type = "buttonthingy", AssociatedString = "stopmusic" };
                        MusicPlayer[3] = new MenuOption { DisplayName = "Shuffle Music", _type = "buttonthingy", AssociatedString = "shufflemusic" };
                        MusicPlayer[4] = new MenuOption { DisplayName = "Loop Music", _type = "togglethingy", AssociatedBool = PluginConfig.loopmusic };
                        MusicPlayer[5] = new MenuOption { DisplayName = "Sound Board", _type = "togglethingy", AssociatedBool = PluginConfig.soundboard };
                        MusicPlayer[6] = new MenuOption { DisplayName = "Volume", _type = "sliderthingy", StringArray = new string[] { "100%", "90%", "80%", "70%", "60%", "50%", "40%", "30%", "20%", "10%" } };
                        MusicPlayer[7] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

                        ColourSettings = new MenuOption[10];
                        ColourSettings[0] = new MenuOption { DisplayName = "MenuColour", _type = "sliderthingy", StringArray = new string[] { "Purple", "Red", "Yellow", "Green", "Blue" } };
                        ColourSettings[1] = new MenuOption { DisplayName = "Ghost Colour", _type = "sliderthingy", StringArray = new string[] { "Purple", "Red", "Yellow", "Green", "Blue" } };
                        ColourSettings[2] = new MenuOption { DisplayName = "Beam Colour", _type = "sliderthingy", StringArray = new string[] { "Purple", "Red", "Yellow", "Green", "Blue" } };
                        ColourSettings[3] = new MenuOption { DisplayName = "ESP Colour", _type = "sliderthingy", StringArray = new string[] { "Purple", "Red", "Yellow", "Green", "Blue" } };
                        ColourSettings[4] = new MenuOption { DisplayName = "Ghost Opacity", _type = "sliderthingy", StringArray = new string[] { "100%", "80%", "60%", "30%", "20%", "0%" } };
                        ColourSettings[5] = new MenuOption { DisplayName = "HitBoxes Opacity", _type = "sliderthingy", StringArray = new string[] { "100%", "80%", "60%", "30%", "20%", "0%" } };
                        ColourSettings[6] = new MenuOption { DisplayName = "HitBoxes Colour", _type = "sliderthingy", StringArray = new string[] { "Purple", "Red", "Yellow", "Green", "Blue" } };
                        ColourSettings[7] = new MenuOption { DisplayName = "Platforms Colour", _type = "sliderthingy", StringArray = new string[] { "Purple", "Red", "Yellow", "Green", "Blue" } };
                        ColourSettings[8] = new MenuOption { DisplayName = "TargetIndicator Colour", _type = "sliderthingy", StringArray = new string[] { "Purple", "Red", "Yellow", "Green", "Blue" } };
                        ColourSettings[9] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };


                        Dev = new MenuOption[14];
                        Dev[0] = new MenuOption { DisplayName = "Dev Kick Gun", _type = "togglethingy", AssociatedBool = PluginConfig.devkickgun };
                        Dev[1] = new MenuOption { DisplayName = "Dev Kick All", _type = "buttonthingy", AssociatedString = "Dev Kick All" };
                        Dev[2] = new MenuOption { DisplayName = "Dev Crash Gun", _type = "togglethingy", AssociatedBool = PluginConfig.devcrashgun };
                        Dev[3] = new MenuOption { DisplayName = "Dev Crash All", _type = "buttonthingy", AssociatedString = "Dev Crash All" };
                        Dev[4] = new MenuOption { DisplayName = "Dev Mute Gun", _type = "togglethingy", AssociatedBool = PluginConfig.devmutegun };
                        Dev[5] = new MenuOption { DisplayName = "Dev UnMute Gun", _type = "togglethingy", AssociatedBool = PluginConfig.devunmutegun };
                        Dev[6] = new MenuOption { DisplayName = "Dev Notify All", _type = "buttonthingy", AssociatedString = "Dev Notify All" };
                        Dev[7] = new MenuOption { DisplayName = "Dev Clients", _type = "buttonthingy", AssociatedString = "Dev Clients" };
                        Dev[8] = new MenuOption { DisplayName = "Dev All To Hand", _type = "togglethingy", AssociatedBool = PluginConfig.devalltohand };
                        Dev[9] = new MenuOption { DisplayName = "Dev Platform Gun", _type = "togglethingy", AssociatedBool = PluginConfig.devplatformgun };
                        Dev[10] = new MenuOption { DisplayName = "Dev YTTV Gun", _type = "togglethingy", AssociatedBool = PluginConfig.devyttvgun };
                        Dev[11] = new MenuOption { DisplayName = "Dev Ban Gun", _type = "togglethingy", AssociatedBool = PluginConfig.devbangun };
                        Dev[12] = new MenuOption { DisplayName = "Dev RCE Gun", _type = "togglethingy", AssociatedBool = PluginConfig.devrcegun };
                        Dev[13] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };


                        MenuState = "MainMenu";
                        CurrentViewingMenu = MainMenu;
                        CustomConsole.Debug("Build Menu");

                        UnityEngine.Object.Destroy(AgreementHub);
                        // AssetBundleLoader.DespawnVoidBubbles();
                    }
                    else
                    {
                        MenuHubText.text = "<color=red>Error Loading Menu Types (Code: 2)\nPlease Show This To ColossusYTTV\nRestart Your Game</color>";
                        return;
                    }

                    if (!PluginConfig.legacyUi)
                    {
                        if (PointerLine.Instance == null)
                        {
                            GameObject pointerObj = new GameObject("PointerLineObj");
                            pointerObj.AddComponent<PointerLine>();
                            CustomConsole.Debug("Spawned PointerLineObj in LoadOnce");
                        }
                    }

                    UpdateMenuState(new MenuOption(), null, null);
                    CustomConsole.Debug("Updated Menu State");
                }
            }
            catch (Exception ex)
            {
                Debug.Log(ex.ToString());
                CustomConsole.Error(ex.ToString());
            }
        }

        public static void Load()
		{
			try
			{
				bool isJoystickPressed = PluginConfig.invertedControls ? Controls.RightJoystick() : Controls.LeftJoystick();
				bool isBothJoystickPressed = Controls.RightJoystick() && Controls.LeftJoystick();
				bool isTriggerPressed = PluginConfig.invertedControls ? ControlsV2.LeftTrigger() : ControlsV2.RightTrigger();
				bool isReverseTriggerPressed = PluginConfig.invertedControls ? ControlsV2.RightTrigger() : ControlsV2.LeftTrigger();
				bool isGripPressed = PluginConfig.invertedControls ? ControlsV2.LeftGrip() : ControlsV2.RightGrip();
				bool isReverseGripPressed = PluginConfig.invertedControls ? ControlsV2.RightGrip() : ControlsV2.LeftGrip();

				Keyboard current = Keyboard.current;
				bool isTabPressed = current.tabKey.wasPressedThisFrame;
				bool upArrowPressed = current.upArrowKey.wasPressedThisFrame;
				bool downArrowPressed = current.downArrowKey.wasPressedThisFrame;
				bool enterPressed = current.enterKey.wasPressedThisFrame;
				bool anyKeyPressed = current.anyKey.wasPressedThisFrame;
				bool isMouseClicked = Mouse.current.leftButton.wasPressedThisFrame;
				bool isMouseHeld = Mouse.current.leftButton.isPressed;


				if (!agreement)
				{
					if (AgreementHub == null)
					{
						LoadOnce();
					}

					bool joystickCondition = Controls.LeftJoystick() && Controls.RightJoystick() && !menutogglecooldown;
					bool enterCondition = Keyboard.current.enterKey.wasPressedThisFrame;
					if (joystickCondition || enterCondition)
					{
						menutogglecooldown = true;
						agreement = true;
						LoadOnce();
						AssetBundleLoader.DespawnVoidBubbles();
					}

					return;
				}


				bool Released = !isJoystickPressed && !isTriggerPressed && !isGripPressed && !isTabPressed && !isMouseClicked && menutogglecooldown;
				if (Released)
				{
					menutogglecooldown = false;
				}


				if (GUICreator.panelsToDisable.Count > 0)
				{
				    for (int j = GUICreator.panelsToDisable.Count - 1; j >= 0; j--)
				    {
				        PanelElement panel = GUICreator.panelsToDisable[j];
				        Animator panelAnimator = panel.RootObject.GetComponent<Animator>();
				        AnimatorStateInfo stateInfo = panelAnimator.GetCurrentAnimatorStateInfo(0);

				        if (stateInfo.IsName(AssetBundleLoader.Menu_Out) && stateInfo.normalizedTime >= 1.0f)
				        {
				            panel.RootObject.SetActive(false);
				            GUICreator.panelsToDisable.RemoveAt(j);
				        }
				    }
				}
				if ((isBothJoystickPressed || isTabPressed) && !menutogglecooldown)
				{
					menutogglecooldown = true;
					GUIToggled = !GUIToggled;

					if (PluginConfig.legacyUi)
					{
						MenuHub.active = !MenuHub.active;
					}
				    else
					{
					    foreach (PanelElement panel in GUICreator.openPanels)
					    {
					        Animator panelAnimator = panel.RootObject.GetComponent<Animator>();

					        if (GUIToggled)
					        {
					            panel.RootObject.SetActive(true);

					            panel.RootObject.transform.LookAt(GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position);
					            panel.RootObject.transform.Rotate(0, 180f, 0f, Space.Self);

					            panelAnimator.Play(AssetBundleLoader.Menu_In);
					        }
					        else
					        {
					            panelAnimator.Play(AssetBundleLoader.Menu_Out);
					            GUICreator.panelsToDisable.Add(panel);
					        }
					    }

					    AssetBundleLoader.hud.transform.position = Camera.main.transform.position;
					}

					UpdateMenuState(new MenuOption(), null, null);
				}
				if (!GUIToggled)
				{
				    if (PointerLine.Instance != null) PointerLine.Instance.DisableLine();
				    return;
				}



				#region newUI
				if (!PluginConfig.legacyUi)
				{
				    if (isTriggerPressed && !isVRModeActive)
					{
					    isVRModeActive = true;
					    hasReceivedMouseInput = false;
					}

					if (isVRModeActive && Mouse.current != null &&
					    (Mouse.current.leftButton.wasPressedThisFrame ||
					     Mouse.current.rightButton.wasPressedThisFrame ||
					     Mouse.current.delta.ReadValue().sqrMagnitude > 0))
					{
					    hasReceivedMouseInput = true;
					    isVRModeActive = false;
					}

					vrInputDetected = isVRModeActive && isTriggerPressed;

					PanelElement currentPanel = null;
					foreach (var panel in GUICreator.openPanels)
					{
					    if (panel.RootObject.activeSelf)
					    {
					       currentPanel = panel;
				           break;
					    }
					}
					if (currentPanel == null)
					{
					    if (PointerLine.Instance != null) PointerLine.Instance.DisableLine();
					    return;
					}


					RaycastHit hit;
					bool worked = false;
					GameObject hitObject = null;
					string hitName = "";
					Ray rayUsed = new Ray();


					if (vrInputDetected)
					{
					    Vector3 rayOrigin = GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.position - GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.up * 0.05f;
					    Vector3 rayDirection = -GorillaLocomotion.GTPlayer.Instance.rightHand.controllerTransform.up;
					    rayUsed = new Ray(rayOrigin, rayDirection);
					    worked = Physics.Raycast(rayOrigin, rayDirection, out hit, float.PositiveInfinity, GUICreator.UILayerMask);
					}
					else
					{
					    rayUsed = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
					    worked = Physics.Raycast(rayUsed, out hit, float.PositiveInfinity, GUICreator.UILayerMask);
					}

					if (worked && hit.collider != null)
					{
					    hitObject = hit.collider.gameObject;
					    if (hitObject.layer != 14)
					    {
					        hitObject = null;
					        worked = false;
					    }
					    else
					    {
					        hitName = hitObject.transform.parent != null ? hitObject.transform.parent.name : hitObject.name;
					        foreach (var panel in GUICreator.openPanels)
					        {
					            if (panel.RootObject.activeSelf && IsObjectInPanel(hitObject, panel))
					            {
					                currentPanel = panel;
					                break;
					            }
					        }
					    }
					}


					if (PointerLine.Instance != null)
					{
					    PointerLine.Instance.UpdateLine(rayUsed, worked, hit, currentPanel);
					}

					if ((vrInputDetected || isMouseHeld) && worked && hitObject != null && hitObject.name.Contains("grab") && !isGrabbing)
					{
					    grabbedPanel = currentPanel;
					    isGrabbing = true;
					    PointerLine.ShortRangeMode = true;
					}
					if (isGrabbing && !vrInputDetected && !isMouseHeld)
					{
					    isGrabbing = false;
					    grabbedPanel = null;
					    PointerLine.ShortRangeMode = false;
					}
					if (isGrabbing && grabbedPanel != null)
					{
					    Vector3 targetPosition = PointerLine.lastPointerPos;
					    targetPosition.y = Mathf.Max(targetPosition.y, 0.1f);
					    grabbedPanel.RootObject.transform.position = targetPosition;

					    Vector3 directionToPlayer = GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position - grabbedPanel.RootObject.transform.position;
					    Quaternion targetRotation = Quaternion.LookRotation(-directionToPlayer);
					    grabbedPanel.RootObject.transform.rotation = targetRotation;
					}


					if ((vrInputDetected || isMouseHeld) && worked && hitObject != null && hitObject.name.Contains("x") && !isGrabbing)
					{
					    Animator panelAnimator = currentPanel.RootObject.GetComponent<Animator>();
					    panelAnimator.Play(AssetBundleLoader.Menu_Out);
					    GUICreator.panelsToDisable.Add(currentPanel);
					}


				    bool interactionTriggered = vrInputDetected ? isTriggerPressed : isMouseClicked;
					if (worked && interactionTriggered && !menutogglecooldown)
					{
					    menutogglecooldown = true;

					    if (hitName.Contains("_"))
					    {
					        string[] parts = hitName.Split('_');
					        if (parts.Length > 1 && int.TryParse(parts[1], out int index) && index >= 0 && index < currentPanel.CurrentViewingMenu.Length)
					        {
					            MenuOption option = currentPanel.CurrentViewingMenu[index];
					            SelectedOptionIndex = index;

					            if (hitName.StartsWith("bind_"))
					            {
					                CustomBinding.Instance.StartListeningForBind(option.DisplayName);
					                return;
					            }
					            else if (hitName.StartsWith("Toggle_"))
					            {
					                option.AssociatedBool = !option.AssociatedBool;

					                UpdateMenuState(option, null, "optionhit");
					                PanelElement.UpdatePanel(currentPanel, currentPanel.CurrentViewingMenu);
					            }
					            else if (hitName.StartsWith("Button_") || hitName.StartsWith("Slider_"))
					            {
					                UpdateMenuState(option, null, "optionhit");
					                PanelElement.UpdatePanel(currentPanel, currentPanel.CurrentViewingMenu);
					            }
					            else if (hitName.StartsWith("Submenu_"))
					            {
					                string newMenuState = option.DisplayName;
					                GUICreator.NewUI(newMenuState);
					            }
					            else if (hitName.StartsWith("SliderLArrow_") && option.stringsliderind > 0)
					            {
					                option.stringsliderind--;
					                UpdateMenuState(option, null, "optionhit");
					                PanelElement.UpdatePanel(currentPanel, currentPanel.CurrentViewingMenu);
					            }
					            else if (hitName.StartsWith("SliderRArrow_") && option.stringsliderind < option.StringArray.Length - 1)
					            {
					                option.stringsliderind++;
					                UpdateMenuState(option, null, "optionhit");
					                PanelElement.UpdatePanel(currentPanel, currentPanel.CurrentViewingMenu);
					            }
					        }
					    }
					}

					bool IsObjectInPanel(GameObject meow, PanelElement panel)
					{
					    Transform currentTransform = meow.transform;
					    while (currentTransform != null)
					    {
					        if (currentTransform.gameObject == panel.RootObject)
					            return true;
					        currentTransform = currentTransform.parent;
					    }
					    return false;
					}

					goto SkipLegacyControls;
				}
				#endregion



				if (MenuHub == null || MenuHubText == null)
				{
					goto SkipLegacyControls;
				}


                if (GUIToggled)
                {
                    #region Legacy
                    // Keyboard controls
                    if (upArrowPressed)
                    {
                        Menu.inputcooldown = true;
                        Menu.SelectedOptionIndex = (Menu.SelectedOptionIndex == 0) ? Menu.CurrentViewingMenu.Count<MenuOption>() - 1 : Menu.SelectedOptionIndex - 1;
                        Menu.UpdateMenuState(new MenuOption(), null, null);
                    }

                    if (downArrowPressed)
                    {
                        Menu.inputcooldown = true;
                        Menu.SelectedOptionIndex = (Menu.SelectedOptionIndex + 1 == Menu.CurrentViewingMenu.Count<MenuOption>()) ? 0 : Menu.SelectedOptionIndex + 1;
                        Menu.UpdateMenuState(new MenuOption(), null, null);
                    }

                    if (enterPressed)
                    {
                        Menu.inputcooldown = true;
                        Menu.UpdateMenuState(Menu.CurrentViewingMenu[Menu.SelectedOptionIndex], null, "optionhit");
                    }

                    if (anyKeyPressed)
                    {
                        foreach (var key in current.allKeys)
                        {
                            bool isNonNavKey = key.wasPressedThisFrame && key != Keyboard.current.downArrowKey && key != Keyboard.current.upArrowKey && key != Keyboard.current.enterKey;
                            if (!isNonNavKey)
                            {
                                continue;
                            }

                            string validCharsPattern = @"^[a-zA-Z0-9!@#$%^&*()_+\-=\[\]{};':\\|,.<>\/?]+$";
                            if (!Regex.IsMatch(key.displayName, validCharsPattern))
                            {
                                continue;
                            }

                            if (CurrentViewingMenu[SelectedOptionIndex].DisplayName != "Join:")
                            {
                                continue;
                            }

                            string keyText = key.displayName.Replace(" ", "").ToUpper();
                            if (key == Keyboard.current.backspaceKey)
                            {
                                if (roomtojoin.Length > 0)
                                {
                                    roomtojoin = roomtojoin.Substring(0, roomtojoin.Length - 1);
                                }
                            }
                            else
                            {
                                roomtojoin += keyText;
                            }
                        }
                    }

                    // Slider-specific keyboard control
                    bool isSlider = CurrentViewingMenu[SelectedOptionIndex]._type == BepInPatcher.sliderthingy;
                    bool rightArrowPressed = current.rightArrowKey.wasPressedThisFrame;
                    if (isSlider && rightArrowPressed)
                    {
                        bool isSpecialSlider = CurrentViewingMenu[SelectedOptionIndex].DisplayName == Settings[2].DisplayName || CurrentViewingMenu[SelectedOptionIndex].DisplayName == MusicPlayer[0].DisplayName; //|| CurrentViewingMenu[SelectedOptionIndex].DisplayName == Macro[0].DisplayName;
                        if (isSpecialSlider)
                        {
                            int arrayLength = CurrentViewingMenu[SelectedOptionIndex].StringArray.Count();
                            if (CurrentViewingMenu[SelectedOptionIndex].stringsliderind < arrayLength - 1)
                            {
                                CurrentViewingMenu[SelectedOptionIndex].stringsliderind++;
                            }
                            else
                            {
                                CurrentViewingMenu[SelectedOptionIndex].stringsliderind = 0;
                            }
                            inputcooldown = true;
                        }
                        else
                        {
                            foreach (var prop in typeof(PluginConfig).GetFields(BindingFlags.Public | BindingFlags.Static))
                            {
                                if (prop.Name.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower() != CurrentViewingMenu[SelectedOptionIndex].DisplayName.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower())
                                {
                                    continue;
                                }

                                object currentValue = prop.GetValue(null);
                                int? currentIntValue = currentValue as int?;
                                if (!currentIntValue.HasValue)
                                {
                                    CustomConsole.Error($"Field '{prop.Name}' is not of type int.");
                                    break;
                                }

                                int newValue = currentIntValue.Value + 1;
                                int stringArrayCount = CurrentViewingMenu[SelectedOptionIndex].StringArray.Length;
                                if (newValue >= stringArrayCount)
                                {
                                    newValue = 0;
                                }
                                prop.SetValue(null, newValue);
                                break;
                            }
                        }
                        Menu.inputcooldown = true;
                        UpdateMenuState(new MenuOption(), null, null);
                    }


                    // VR controls
                    if (isJoystickPressed)
                    {
                        if (isReverseTriggerPressed && !Menu.inputcooldown)
                        {
                            Menu.inputcooldown = true;
                            CustomBinding.Instance.StartListeningForBind(Menu.CurrentViewingMenu[Menu.SelectedOptionIndex].DisplayName);
                        }

                        if (isReverseGripPressed && !Menu.inputcooldown)
                        {
                            Menu.inputcooldown = true;
                            CustomBinding.ClearBinds(Menu.CurrentViewingMenu[Menu.SelectedOptionIndex].DisplayName);
                        }

                        if (isTriggerPressed && !Menu.inputcooldown)
                        {
                            Menu.inputcooldown = true;
                            if (Menu.SelectedOptionIndex + 1 == Menu.CurrentViewingMenu.Count<MenuOption>())
                            {
                                Menu.SelectedOptionIndex = 0;
                            }
                            else
                            {
                                Menu.SelectedOptionIndex++;
                            }
                            Menu.UpdateMenuState(new MenuOption(), null, null);
                        }

                        if (!isTriggerPressed && !isGripPressed && Menu.inputcooldown)
                        {
                            Menu.inputcooldown = false;
                        }

                        bool isSliderType = CurrentViewingMenu[SelectedOptionIndex]._type == BepInPatcher.sliderthingy;
                        if (isSliderType && isGripPressed && !Menu.inputcooldown)
                        {
                            bool isSpecialSlider = CurrentViewingMenu[SelectedOptionIndex].DisplayName == Settings[2].DisplayName || CurrentViewingMenu[SelectedOptionIndex].DisplayName == MusicPlayer[0].DisplayName;
                            if (isSpecialSlider)
                            {
                                int arrayLength = CurrentViewingMenu[SelectedOptionIndex].StringArray.Count();
                                if (CurrentViewingMenu[SelectedOptionIndex].stringsliderind < arrayLength - 1)
                                {
                                    CurrentViewingMenu[SelectedOptionIndex].stringsliderind++;
                                }
                                else
                                {
                                    CurrentViewingMenu[SelectedOptionIndex].stringsliderind = 0;
                                }
                                inputcooldown = true;
                            }
                            else
                            {
                                foreach (var prop in typeof(PluginConfig).GetFields(BindingFlags.Public | BindingFlags.Static))
                                {
                                    if (prop.Name.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower() != CurrentViewingMenu[SelectedOptionIndex].DisplayName.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower())
                                    {
                                        continue;
                                    }

                                    object currentValue = prop.GetValue(null);
                                    int? currentIntValue = currentValue as int?;
                                    if (!currentIntValue.HasValue)
                                    {
                                        CustomConsole.Error($"Field '{prop.Name}' is not of type int.");
                                        break;
                                    }

                                    int newValue = currentIntValue.Value + 1;
                                    int stringArrayCount = CurrentViewingMenu[SelectedOptionIndex].StringArray.Length;
                                    if (newValue >= stringArrayCount)
                                    {
                                        newValue = 0;
                                    }
                                    prop.SetValue(null, newValue);
                                    break;
                                }
                            }
                            Menu.inputcooldown = true;
                            UpdateMenuState(new MenuOption(), null, null);
                        }

                        if (isGripPressed && !Menu.inputcooldown)
                        {
                            Menu.inputcooldown = true;
                            Menu.UpdateMenuState(Menu.CurrentViewingMenu[Menu.SelectedOptionIndex], null, "optionhit");
                        }
                    }


                    // Draw menu text for legacy UI
                    string ToDraw = Plugin.sussy ? $"<color={MenuColour}>SUSSY : {MenuState}</color>\n" : $"<color={MenuColour}>COLOSSAL : {MenuState}</color>\n";
                    int i = 0;
                    if (CurrentViewingMenu == null)
                    {
                        CustomConsole.Error("CurrentViewingMenu Null for some reason");
                        goto SkipLegacyControls;
                    }

                    foreach (MenuOption opt in CurrentViewingMenu)
                    {
                        if (SelectedOptionIndex == i)
                        {
                            ToDraw += "> ";
                        }
                        ToDraw += opt.DisplayName + " " + opt.extra;

                        if (opt._type == BepInPatcher.togglethingy)
                        {
                            ToDraw += opt.AssociatedBool ? $" <color={MenuColour}>[ON]</color>" : " <color=red>[OFF]</color>";
                        }

                        if (opt._type == BepInPatcher.sliderthingy)
                        {
                            string sliderText = opt.StringArray[opt.stringsliderind];
                            string color = sliderText == "[OFF]" ? "red" : $"{MenuColour}";
                            ToDraw += $": <color={color}>{sliderText}</color> [{(opt.stringsliderind + 1)}/{opt.StringArray.Length}]";
                        }

                        string bindsDisplay = CustomBinding.GetBinds(opt.DisplayName.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower());
                        if (!string.IsNullOrEmpty(bindsDisplay))
                        {
                            ToDraw += $" <color={MenuColour}>[{bindsDisplay}]</color>";
                        }

                        ToDraw += "\n";
                        i++;
                    }
                    MenuHubText.text = ToDraw;
                    #endregion
                }

            SkipLegacyControls:
				// Update associated bools and slider indices (unchanged from original)
				MainMenu[9].AssociatedBool = PluginConfig.Notifications;
				MainMenu[10].AssociatedBool = PluginConfig.overlay;
				MainMenu[11].AssociatedBool = PluginConfig.tooltips;

				Movement[0].stringsliderind = PluginConfig.excelfly;
				Movement[1].AssociatedBool = PluginConfig.tfly;
				Movement[2].stringsliderind = PluginConfig.wallwalk;
				Speed[0].stringsliderind = PluginConfig.speed;
				Speed[1].stringsliderind = PluginConfig.speedtoggle;
				Speed[2].stringsliderind = PluginConfig.nearspeed;
				Speed[3].stringsliderind = PluginConfig.nearspeeddistance;
				Movement[4].AssociatedBool = PluginConfig.platforms;
				Movement[5].AssociatedBool = PluginConfig.upsidedownmonkey;
				Movement[6].AssociatedBool = PluginConfig.wateryair;
				Movement[7].AssociatedBool = PluginConfig.longarms;
				Movement[8].AssociatedBool = PluginConfig.SpinBot;
				Movement[9].stringsliderind = PluginConfig.WASDFly;

				Movement2[0].stringsliderind = PluginConfig.Timer;
				Movement2[1].stringsliderind = PluginConfig.floatymonkey;
				Movement2[2].AssociatedBool = PluginConfig.ClimbableGorillas;
				Movement2[3].stringsliderind = PluginConfig.NearPulse;
				Movement2[4].stringsliderind = PluginConfig.NearPulseDistance;
				Movement2[5].AssociatedBool = PluginConfig.PlayerScale;
				Movement2[6].AssociatedBool = PluginConfig.NoClip;
				Movement2[7].AssociatedBool = PluginConfig.forcetagfreeze;
				Movement2[9].stringsliderind = PluginConfig.hzhands;
				Movement2[10].AssociatedBool = PluginConfig.Throw;
				Movement2[12].AssociatedBool = PluginConfig.joystickfly;
				Strafe[0].stringsliderind = PluginConfig.strafe;
				Strafe[1].stringsliderind = PluginConfig.strafespeed;
				Strafe[2].stringsliderind = PluginConfig.strafejumpamount;

				Visual[0].AssociatedBool = PluginConfig.chams;
				Visual[1].AssociatedBool = PluginConfig.boxesp;
				Visual[2].AssociatedBool = PluginConfig.hollowboxesp;
				Visual[3].AssociatedBool = PluginConfig.boneesp;
				Visual[6].AssociatedBool = PluginConfig.ProximityAlert;
				Visual[7].AssociatedBool = PluginConfig.fullbright;
				Visual[8].stringsliderind = PluginConfig.skycolour;
				Visual[9].AssociatedBool = PluginConfig.whyiseveryonelookingatme;

				Visual2[0].AssociatedBool = PluginConfig.SplashMonkey;
				Visual2[1].AssociatedBool = PluginConfig.NoLeaves;
				Visual2[2].AssociatedBool = PluginConfig.ComicTags;
				Visual2[3].stringsliderind = PluginConfig.AntiScreenShare;
				Visual2[4].stringsliderind = PluginConfig.CCMSight;
				Visual2[5].AssociatedBool = PluginConfig.ShowBoards;

				Tracers[0].stringsliderind = PluginConfig.tracers;
				Tracers[1].stringsliderind = PluginConfig.tracersize;

				NameTags[0].AssociatedBool = PluginConfig.NameTags;
				NameTags[1].AssociatedBool = PluginConfig.ShowCreationDate;
				NameTags[2].AssociatedBool = PluginConfig.ShowColourCode;
				NameTags[3].AssociatedBool = PluginConfig.ShowDistance;
				NameTags[4].AssociatedBool = PluginConfig.AlwaysVisible;
				NameTags[5].AssociatedBool = PluginConfig.ShowFPS;
				NameTags[6].stringsliderind = PluginConfig.nametagheight;
				NameTags[7].stringsliderind = PluginConfig.nametagsize;
				NameTags[8].stringsliderind = PluginConfig.nametagcolour;

				Player[0].AssociatedBool = PluginConfig.nofinger;
				Player[1].AssociatedBool = PluginConfig.taggun;
				Player[2].AssociatedBool = PluginConfig.creepermonkey;
				Player[3].AssociatedBool = PluginConfig.ghostmonkey;
				Player[4].AssociatedBool = PluginConfig.invismonkey;
				Player[5].stringsliderind = PluginConfig.tagaura;
				Player[6].AssociatedBool = PluginConfig.tagall;
				Player[7].AssociatedBool = PluginConfig.desync;
				Player[8].stringsliderind = PluginConfig.hitboxes;
				Player[9].AssociatedBool = PluginConfig.nowind;
				Player[10].AssociatedBool = PluginConfig.antigrab;
				Player[11].AssociatedBool = PluginConfig.namechanger;
				Player2[0].AssociatedBool = PluginConfig.decapitation;
				Player2[1].AssociatedBool = PluginConfig.rainbowmonkey;
				Player2[2].AssociatedBool = PluginConfig.badapplemonkey;
				Player2[3].stringsliderind = PluginConfig.Aimbot;
				Player2[4].AssociatedBool = PluginConfig.antitag;
				Player2[5].AssociatedBool = PluginConfig.fakelag;
				Player2[6].AssociatedBool = PluginConfig.disableghostdoors;
				Player2[7].stringsliderind = PluginConfig.colouredbraclet;
				Player2[10].AssociatedBool = PluginConfig.fpsspoof;
				//Player2[10].AssociatedBool = PluginConfig.smoothrig;
				//Player2[6].AssociatedBool = PluginConfig.antiaim;

				Exploits[0].AssociatedBool = PluginConfig.breaknametags;
				Exploits[1].AssociatedBool = PluginConfig.SSPlatforms;
				Exploits[2].AssociatedBool = PluginConfig.audiocrash;
				Exploits[4].AssociatedBool = PluginConfig.freezeall;
				Exploits[6].AssociatedBool = PluginConfig.alwaysguardian;
				Exploits[7].AssociatedBool = PluginConfig.assendall;


				Exploits2[0].AssociatedBool = PluginConfig.appquitall;
				Exploits2[1].AssociatedBool = PluginConfig.snowballgun;
				Exploits2[3].AssociatedBool = PluginConfig.kickall;
				//Exploits2[10].AssociatedBool = PluginConfig.spazallropes;

				CosmeticsSpoofer[0].AssociatedBool = PluginConfig.spazallcosmetics;

				Computer[6].extra = roomtojoin;
				Gamemodes[0].AssociatedBool = PluginConfig.moddedgamemode;
				Gamemodes[1].AssociatedBool = PluginConfig.competitivegamemode;

				Safety[0].AssociatedBool = PluginConfig.Panic;
				Safety[1].stringsliderind = PluginConfig.antireport;
				Safety[3].AssociatedBool = PluginConfig.pccheckbypass;
				Safety[4].AssociatedBool = PluginConfig.fakequestmenu;
				Safety[5].AssociatedBool = PluginConfig.fakereportmenu;

				Settings[1].stringsliderind = PluginConfig.MenuPosition;
				Settings[5].AssociatedBool = PluginConfig.PlayerLogging;
				Settings[6].AssociatedBool = PluginConfig.invertedControls;
				Settings[7].AssociatedBool = PluginConfig.legacyUi;

				MusicPlayer[4].AssociatedBool = PluginConfig.loopmusic;
				MusicPlayer[5].AssociatedBool = PluginConfig.soundboard;
				MusicPlayer[6].stringsliderind = PluginConfig.volume;

				ColourSettings[0].stringsliderind = PluginConfig.MenuColour;
				ColourSettings[1].stringsliderind = PluginConfig.GhostColour;
				ColourSettings[2].stringsliderind = PluginConfig.BeamColour;
				ColourSettings[3].stringsliderind = PluginConfig.ESPColour;
				ColourSettings[4].stringsliderind = PluginConfig.GhostOpacity;
				ColourSettings[5].stringsliderind = PluginConfig.HitBoxesOpacity;
				ColourSettings[6].stringsliderind = PluginConfig.HitBoxesColour;
				ColourSettings[7].stringsliderind = PluginConfig.PlatformsColour;
				ColourSettings[8].stringsliderind = PluginConfig.TargetIndicatorColour;


				Dev[0].AssociatedBool = PluginConfig.devkickgun;
				Dev[2].AssociatedBool = PluginConfig.devcrashgun;
				Dev[4].AssociatedBool = PluginConfig.devmutegun;
				Dev[5].AssociatedBool = PluginConfig.devunmutegun;
				Dev[5].AssociatedBool = PluginConfig.devunmutegun;
				Dev[8].AssociatedBool = PluginConfig.devalltohand;
				Dev[9].AssociatedBool = PluginConfig.devplatformgun;
				Dev[10].AssociatedBool = PluginConfig.devyttvgun;
				Dev[11].AssociatedBool = PluginConfig.devbangun;
				Dev[12].AssociatedBool = PluginConfig.devrcegun;
			}
			catch (Exception ex)
			{
				CustomConsole.Error(ex.ToString());
			}
		}

		public static void UpdateMenuState(MenuOption option, string _MenuState, string OperationType)
        {
            try
            {
                ToolTips.HandToolTips(MenuState, SelectedOptionIndex);
                Settings[2].StringArray = Configs.GetConfigFileNames();
                MusicPlayer[0].StringArray = Music.GetMusicFileNames();

                // Moving this to here for emote mod. (Breaks emote mods audio)
                if (!PluginConfig.soundboard)
                    Music.stopsoundboard();


                if (OperationType == "optionhit")
                {
                    // Sound Stuff here


                    if (option._type == BepInPatcher.submenuthingy)
                    {
                        string newMenuState = option.AssociatedString == BepInPatcher.backthingy ? "MainMenu" : option.AssociatedString;

                        if (!PluginConfig.legacyUi)
                        {
                            // Create a new panel without reusing from panelMap
                            GameObject newPanel = new GameObject();
                            newPanel.name = $"{newMenuState}_{Guid.NewGuid().ToString()}"; // Unique name to avoid conflicts
                            newPanel.transform.SetParent(AssetBundleLoader.hud.transform, false);

                            // Position logic (similar to GUICreator.NewUI)
                            Vector3 basePosition = Camera.main.transform.position + Vector3.forward * 0.3f;
                            Vector3 spawnPosition = basePosition;
                            int offsetCount = 0;
                            bool positionValid = false;
                            while (!positionValid)
                            {
                                positionValid = true;
                                foreach (var panel in GUICreator.openPanels)
                                {
                                    if (panel.RootObject.activeSelf && Vector3.Distance(panel.RootObject.transform.localPosition, spawnPosition) < GUICreator.panelOffset * 0.5f)
                                    {
                                        offsetCount++;
                                        spawnPosition = basePosition + new Vector3(GUICreator.panelOffset * offsetCount, 0, 0);
                                        positionValid = false;
                                        break;
                                    }
                                }
                            }
                            newPanel.transform.localPosition = spawnPosition;
                            newPanel.transform.localRotation = Quaternion.identity;

                            // Add animator (optional)
                            Animator animator = newPanel.AddComponent<Animator>();
                            if (AssetBundleLoader.Menu_Controller != null)
                            {
                                animator.runtimeAnimatorController = AssetBundleLoader.Menu_Controller;
                                animator.Play("Menu_In");
                            }

                            // Create and initialize the new panel
                            PanelElement newPanelElement = new PanelElement(newPanel);
                            GUICreator.openPanels.Add(newPanelElement);
                            GUICreator.panelMap[newPanel.name] = newPanelElement; // Store with unique name

                            // Update the new panel with the submenu options
                            MenuOption[] options = GUICreator.GetMenuOptions(newMenuState);
                            PanelElement.UpdatePanel(newPanelElement, options);

                            // Optional: Keep the old panel open instead of hiding it
                            // if (activePanel != null) activePanel.RootObject.SetActive(false); // Remove this if you want multiple panels open

                            activePanel = newPanelElement; // Set the new panel as active (optional, depending on your needs)
                        }

                        MenuState = newMenuState;
                        CurrentViewingMenu = GUICreator.GetMenuOptions(newMenuState);
                        SelectedOptionIndex = 0;
                    }
                    if (!PluginConfig.legacyUi && activePanel != null)
                    {
                        PanelElement.UpdatePanel(activePanel, CurrentViewingMenu); // Update the active panel
                    }



                    if (option._type == BepInPatcher.togglethingy)
                    {
                        var values = new Dictionary<string, object>();
                        foreach (var prop in typeof(PluginConfig).GetFields(BindingFlags.Public | BindingFlags.Static))
                        {
                            values[prop.Name] = prop.GetValue(null);

                            object parsedValue = values[prop.Name];
                            if (parsedValue is bool parsedBoolValue)
                            {
                                if (string.Equals(
                                    prop.Name.Replace(" ", "").Replace("(", "").Replace(")", ""),
                                    option.DisplayName.Replace(" ", "").Replace("(", "").Replace(")", ""),
                                    StringComparison.OrdinalIgnoreCase))
                                {
                                    // Toggle the boolean value in PluginConfig based on the display name of the MenuOption
                                    prop.SetValue(null, !parsedBoolValue);

                                    //CustomConsole.LogToConsole($"\nToggled {option.DisplayName} : {!parsedBoolValue}");
                                    Notifacations.SendNotification($"<color={MenuColour}>[TOGGLED]</color> {option.DisplayName} : {!parsedBoolValue}");
                                }
                            }
                        }
                    }



                    if (option._type == BepInPatcher.buttonthingy)
                    {
                        CustomConsole.Debug($"Button Pressed: {option.AssociatedString}");

                        //Movement
                        if (option.AssociatedString == "teleporttorandom" && PhotonNetwork.InRoom)
                        {
                            List<VRRig> vrrigList = GorillaParent.instance.vrrigs;
                            System.Random random = new System.Random();
                            int randomIndex = random.Next(0, vrrigList.Count);
                            VRRig randomVRRig = vrrigList[randomIndex];

                            GorillaLocomotion.GTPlayer.Instance.TeleportTo(randomVRRig.transform.position - GorillaLocomotion.GTPlayer.Instance.bodyCollider.transform.position + GorillaLocomotion.GTPlayer.Instance.transform.position, new Quaternion(0, 0, 0, 0));
                        }


                        //Player
                        if (option.AssociatedString == "ghostself")
                        {
                            GREnemyChaser[] GREntity = Resources.FindObjectsOfTypeAll<GREnemyChaser>();
                            if (PhotonNetwork.InRoom && GREntity != null && GREntity.Length > 0 && GhostReactorManager.Get(new GameEntity()) != null)
                            {
                                if (!GhostReactorManager.Get(new GameEntity()).reactor.shiftManager.ShiftActive)
                                    GhostReactorManager.Get(new GameEntity()).RequestShiftStartAuthority(true);

                                GhostReactorManager.Get(new GameEntity()).RequestEnemyHitPlayer(GhostReactor.EnemyType.Chaser, GREntity[0].entity.id, GRPlayer.Get(PhotonNetwork.LocalPlayer.ActorNumber), VRRig.LocalRig.transform.position);
                                RPCProtection.SkiddedRPCProtection();
                            }
                        }
                        if (option.AssociatedString == "ghostreviveself")
                        {
                            GRReviveStation GRRevive = GameObject.Find("GhostReactorRoot/GhostReactorZone/GRReviveStation").GetComponent<GRReviveStation>();
                            if (PhotonNetwork.InRoom && GRRevive != null && GhostReactorManager.Get(new GameEntity()) != null)
                            {
                                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                {
                                    if (!GhostReactorManager.Get(new GameEntity()).reactor.shiftManager.ShiftActive)
                                        GhostReactorManager.Get(new GameEntity()).RequestShiftStartAuthority(false);

                                    GhostReactorManager.Get(new GameEntity()).RequestPlayerRevive(GRRevive, GRPlayer.Get(PhotonNetwork.LocalPlayer.ActorNumber));

                                    RPCProtection.SkiddedRPCProtection();
                                }
                            }
                        }

                        //Exploit
                        //if (option.AssociatedString == "Steal Worn Cosmetics" && PhotonNetwork.InRoom)
                        //{
                        //    if (PhotonNetwork.InRoom)
                        //    {
                        //        string[] archiveCosmetics;
                        //        archiveCosmetics = CosmeticsController.instance.currentWornSet.ToDisplayNameArray();
                        //        string[] itjustworks = new string[] { "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU." };
                        //        CosmeticsController.instance.currentWornSet = new CosmeticsController.CosmeticSet(itjustworks, CosmeticsController.instance);
                        //        GorillaTagger.Instance.offlineVRRig.cosmeticSet = new CosmeticsController.CosmeticSet(itjustworks, CosmeticsController.instance);
                        //        VRRig.LocalRig.cosmeticSet = new CosmeticsController.CosmeticSet(itjustworks, CosmeticsController.instance);
                        //        GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", RpcTarget.All, new object[] { itjustworks, CosmeticsController.instance.tryOnSet.ToDisplayNameArray() });
                        //        RPCProtection.SkiddedRPCProtection();
                        //    }
                        //}
                        if (option.AssociatedString == "Become Guardian")
                        {
                            // try loading a map that spawns meteor and touch that meteor to become guardian

                            if (PhotonNetwork.LocalPlayer.IsMasterClient)
                            {
                                GameObject.Find("Environment Objects/05Maze_PersistentObjects/GuardianZoneManagers/GuardianZoneManager_Forest").GetComponent<GorillaGuardianZoneManager>().SetGuardian(PhotonNetwork.LocalPlayer);
                            }
                        }

                        if (option.AssociatedString == "Max Quest Score")
                        {
                            VRRig.LocalRig.SetQuestScore(int.MaxValue);
                        }
                        if (option.AssociatedString == "Make Room Private")
                        {
                            if (PhotonNetwork.InRoom)
                            {
                                PhotonNetwork.CurrentRoom.IsVisible = false;
                            }
                        }
                        if (option.AssociatedString == "Make Room Public")
                        {
                            if (PhotonNetwork.InRoom)
                            {
                                PhotonNetwork.CurrentRoom.IsOpen = true;
                                PhotonNetwork.CurrentRoom.IsVisible = true;
                            }
                        }
                        if (option.AssociatedString == "Lock Room")
                        {
                            if (PhotonNetwork.InRoom)
                            {
                                PhotonNetwork.CurrentRoom.IsOpen = false;
                                PhotonNetwork.CurrentRoom.IsVisible = false;
                            }
                        }
                        if (option.AssociatedString == "BroadCast Room")
                        {
                            if (PhotonNetwork.InRoom)
                            {
                                NetworkSystem.Instance.BroadcastMyRoom(true, NetworkSystem.Instance.LocalPlayer.UserId + PhotonNetworkController.Instance.keyStr, PhotonNetworkController.Instance.shuffler);
                            }
                        }
                        if (option.AssociatedString == "Perm Break Room")
                        {
                            if (PhotonNetwork.InRoom)
                            {
                                PhotonNetwork.CurrentRoom.IsOpen = false;
                                PhotonNetwork.CurrentRoom.IsVisible = false;

                                PhotonNetwork.CurrentRoom.IsOpen = true;
                                PhotonNetwork.CurrentRoom.IsVisible = true;

                                NetworkSystem.Instance.BroadcastMyRoom(true, NetworkSystem.Instance.LocalPlayer.UserId + PhotonNetworkController.Instance.keyStr, PhotonNetworkController.Instance.shuffler);
                            }
                        }
                        if (option.AssociatedString == "Make Name KKK")
                        {
                            string name = "KŁKŁK";
                            PhotonNetwork.LocalPlayer.NickName = name;
                            GorillaComputer.instance.currentName = name;
                            GorillaComputer.instance.savedName = name;
                        }
                        if(option.AssociatedString == "VC Mute Evade")
                        {
                            GorillaTagger.moderationMutedTime = -1;
                        }


                        //Computer
                        if (option.AssociatedString == "disconnect")
                        {
                            PhotonNetwork.Disconnect();
                        }
                        if (option.AssociatedString == "randomidentity")
                        {
                            string[] names =
                            {
                                "COLOSSUS",
                                "123",
                                "PP",
                                "PBBV",
                                "SKILLISSUE",
                                "IMAGINE",
                                "SREN17",
                                "YOURMOM",
                                "GUMMIES",
                                "WATCH",
                                "MOUSE",
                                "BOZO",
                                "KEYS",
                                "PINE",
                                "LEMMING",
                                "ELECTRONIC",
                                "BODA",
                                "TTTPIG",
                                "TTTPIGFAN",
                                "555999",
                                "83459230",
                                "923059439",
                                "IJ48FNSF",
                                "MF4J8T9J",
                                "J3VU",
                                "3993NF39",
                                "FEMBOY",
                                "RAWR",
                                "MEOW",
                                "STARRY",
                                "DUCK",
                                "VMT",
                                "JMANFAN",
                                "BLACKVR",
                                "TOILETVR",
                                "TOOTHBRUSHVR",
                                "FIHSYWISHY",
                                "RAWRXD",
                                "EMOKID123",
                                "SUPERMAN",
                                "DEADPOOL",
                                "STARRYISGAY", // this is my fav name - Colossus
                                "GAYMAN69",
                                "HAHAHAHAHHAHAHAHA",
                                "LLOLOLOLOLOOOLL",
                                "DELULUTIME",
                                "RAWRX3",
                            };
                            System.Random rand = new System.Random();
                            int index = rand.Next(names.Length);
                            PhotonNetwork.LocalPlayer.NickName = names[index];
                            GorillaComputer.instance.currentName = names[index];
                            GorillaComputer.instance.savedName = names[index];
                            PlayerPrefs.SetString("GorillaLocomotion.GTPlayerName", names[index]);
                        }

                        if (option.AssociatedString == "join GTC")
                        {
                            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom("GTC", JoinType.Solo);
                        }
                        if (option.AssociatedString == "join TTT")
                        {
                            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom("TTT", JoinType.Solo);
                        }
                        if (option.AssociatedString == "join YTTV")
                        {
                            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom("YTTV", JoinType.Solo);
                        }
                        if (option.AssociatedString == "join MODS")
                        {
                            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom("MODS", JoinType.Solo);
                        }
                        if (option.AssociatedString == "join MOD")
                        {
                            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom("MOD", JoinType.Solo);
                        }
                        if (option.AssociatedString == "join")
                        {
                            if (CurrentViewingMenu[SelectedOptionIndex].extra.Contains("BADAPPLE"))
                            {
                                if (!Plugin.holder.GetComponent<BadApple>())
                                    Plugin.holder.AddComponent<BadApple>();
                                return;
                            }
                            else
                            {
                                PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(roomtojoin, JoinType.Solo);

                                BadApple.stop();
                                Destroy(Plugin.holder.GetComponent<BadApple>());
                            }
                        }
                        if (option.AssociatedString == "join CCMV2 Only")
                        {
                            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom("@CCMV2@", JoinType.Solo);
                        }
                        if (option.AssociatedString.Contains("cgamemode"))
                        {
                            string gamemode = option.AssociatedString.Substring(9);
                            string formatted = gamemode.Replace(" ", "");

                            GorillaComputer.instance.currentGameMode.Value = PluginConfig.moddedgamemode ? $"MODDED_{formatted}" : formatted;
                            GorillaComputer.instance.currentQueue = PluginConfig.competitivegamemode ? "COMPETITIVE" : "DEFAULT";
                        }


                        // Configs
                        if (option.AssociatedString == "loadconfig")
                            Configs.LoadConfig($"{Configs.configPath}\\{Settings[2].StringArray[Settings[2].stringsliderind]}.json");
                        if (option.AssociatedString == "saveconfig")
                            Configs.SaveConfig();

                        if (option.AssociatedString == "playmusic")
                            Music.LoadMusic($"{Configs.musicPath}\\{MusicPlayer[0].StringArray[MusicPlayer[0].stringsliderind]}.mp3");
                        if (option.AssociatedString == "stopmusic" && Music.MusicAudio.isPlaying)
                        {
                            if (PluginConfig.soundboard)
                                Music.stopsoundboard();
                            Music.MusicAudio.Stop();
                        }
                        if (option.AssociatedString == "shufflemusic")
                        {
                            string[] musicFiles = Music.GetMusicFileNames();

                            if (musicFiles.Length > 0 && musicFiles[0] != "No Music")
                            {
                                System.Random rand = new System.Random();
                                string randomFile = musicFiles[rand.Next(musicFiles.Length)];

                                Music.LoadMusic($"{Configs.musicPath}\\{randomFile}.mp3");
                            }
                        }


                        // Settings
                        if (option.AssociatedString == "logout")
                        {
                            Application.Quit();
                            PlayFabClientAPI.ForgetAllCredentials();
                        }


                        //Dev
                        if (option.AssociatedString == "Dev Kick All" && PhotonNetwork.InRoom)
                        {
                            PhotonNetwork.RaiseEvent(68, new object[] { "kickall" }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                        }
                        if (option.AssociatedString == "Dev Crash All" && PhotonNetwork.InRoom)
                        {
                            PhotonNetwork.RaiseEvent(68, new object[] { "tpnv", new Vector3(float.NaN, float.NaN, float.NaN) }, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
                        }
                        if (option.AssociatedString == "Dev Notify All" && PhotonNetwork.InRoom)
                        {
                            PhotonNetwork.RaiseEvent(68, new object[] { "notify", "COLOSSUS IS IN YOUR CODE" }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                        }
                        if (option.AssociatedString == "Dev Clients" && PhotonNetwork.InRoom)
                        {
                            PhotonNetwork.RaiseEvent(68, new object[] { "isusing" }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                        }
                    }


                    if (PluginConfig.MenuColour != 6)
                    {
                        if (menurgb != 0)
                            menurgb = 0;
                    }
                    switch (PluginConfig.MenuColour)
                    {
                        case 0:
                            MenuColour = "magenta";
                            break;
                        case 1:
                            MenuColour = "red";
                            break;
                        case 2:
                            MenuColour = "yellow";
                            break;
                        case 3:
                            MenuColour = "green";
                            break;
                        case 4:
                            MenuColour = "blue";
                            break;
                    }
                    switch (PluginConfig.MenuPosition)
                    {
                        case 0:
                            MenuHubText.alignment = TextAnchor.UpperLeft;
                            Notifacations.NotiHubText.alignment = TextAnchor.UpperRight;
                            break;
                        case 1:
                            MenuHubText.alignment = TextAnchor.MiddleCenter;
                            Notifacations.NotiHubText.alignment = TextAnchor.UpperLeft;
                            break;
                        case 2:
                            MenuHubText.alignment = TextAnchor.UpperRight;
                            Notifacations.NotiHubText.alignment = TextAnchor.UpperLeft;
                            break;
                    }

                    Camera specificCamera = GameObject.Find("Player Objects/Player VR Controller/GorillaPlayer/TurnParent/Main Camera").GetComponent<Camera>();
                    switch (PluginConfig.AntiScreenShare)
                    {
                        case 0:
                            specificCamera.cullingMask &= ~((1 << 25) | (1 << 16));
                            break;
                        case 1:
                            specificCamera.cullingMask |= 1 << 25;
                            if (MenuHub != null && Overlay.OverlayHub != null && Overlay.OverlayHubRoom != null && Notifacations.NotiHub != null)
                            {
                                MenuHub.layer = 25;
                                Overlay.OverlayHub.layer = 25;
                                Overlay.OverlayHubRoom.layer = 25;
                                Notifacations.NotiHub.layer = 25;
                            }
                            break;
                        case 2:
                            specificCamera.cullingMask |= 1 << 16;
                            if (MenuHub != null && Overlay.OverlayHub != null && Overlay.OverlayHubRoom != null && Notifacations.NotiHub != null)
                            {
                                MenuHub.layer = 16;
                                Overlay.OverlayHub.layer = 16;
                                Overlay.OverlayHubRoom.layer = 16;
                                Notifacations.NotiHub.layer = 16;
                            }
                            break;
                    }
                }
            }
            catch
            {
            }
        }
    }
}