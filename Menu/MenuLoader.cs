using Mercury.Menu;
using UnityEngine;

namespace Mercury
{
    public static class MenuLoader
    {
        public static MenuOption[] LoadMenu()
        {
            Menu.Menu.MainMenu = new MenuOption[13];
            Menu.Menu.MainMenu[0] = new MenuOption { DisplayName = "Movement", _type = "submenuthingy", AssociatedString = "Movement" };
            Menu.Menu.MainMenu[1] = new MenuOption { DisplayName = "Visual", _type = "submenuthingy", AssociatedString = "Visual" };
            Menu.Menu.MainMenu[2] = new MenuOption { DisplayName = "Player", _type = "submenuthingy", AssociatedString = "Player" };
            Menu.Menu.MainMenu[3] = new MenuOption { DisplayName = "Computer", _type = "submenuthingy", AssociatedString = "Computer" };
            Menu.Menu.MainMenu[4] = new MenuOption { DisplayName = "Exploits", _type = "submenuthingy", AssociatedString = "Exploits" };
            Menu.Menu.MainMenu[5] = new MenuOption { DisplayName = "Safety", _type = "submenuthingy", AssociatedString = "Safety" };
            Menu.Menu.MainMenu[6] = new MenuOption { DisplayName = "MusicPlayer", _type = "submenuthingy", AssociatedString = "MusicPlayer" };
            Menu.Menu.MainMenu[7] = new MenuOption { DisplayName = "Settings", _type = "submenuthingy", AssociatedString = "Settings" };
            Menu.Menu.MainMenu[8] = new MenuOption { DisplayName = "Info", _type = "submenuthingy", AssociatedString = "Info" };
            Menu.Menu.MainMenu[9] = new MenuOption { DisplayName = "Macro", _type = "submenuthingy", AssociatedString = "Macro" };
            Menu.Menu.MainMenu[10] = new MenuOption { DisplayName = "Notifications", _type = "togglethingy", AssociatedBool = PluginConfig.Notifications };
            Menu.Menu.MainMenu[11] = new MenuOption { DisplayName = "Overlay", _type = "togglethingy", AssociatedBool = PluginConfig.overlay };
            Menu.Menu.MainMenu[12] = new MenuOption { DisplayName = "Tool Tips", _type = "togglethingy", AssociatedBool = PluginConfig.tooltips };

            Menu.Menu.Movement = new MenuOption[13];
            Menu.Menu.Movement[0] = new MenuOption { DisplayName = "ExcelFly", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "Super Slow", "Slow", "Medium", "Fast", "Super Fast" } };
            Menu.Menu.Movement[1] = new MenuOption { DisplayName = "TFly", _type = "togglethingy", AssociatedBool = PluginConfig.tfly };
            Menu.Menu.Movement[2] = new MenuOption { DisplayName = "WallWalk", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "6.8", "7", "7.5", "7.8", "8", "8.5", "8.8", "9", "9.5", "9.8" } };
            Menu.Menu.Movement[3] = new MenuOption { DisplayName = "Speed Options", _type = "submenuthingy", AssociatedString = "Speed Options" };
            Menu.Menu.Movement[4] = new MenuOption { DisplayName = "Platforms", _type = "togglethingy", AssociatedBool = PluginConfig.platforms };
            Menu.Menu.Movement[5] = new MenuOption { DisplayName = "UpsideDown Monkey", _type = "togglethingy", AssociatedBool = PluginConfig.upsidedownmonkey };
            Menu.Menu.Movement[6] = new MenuOption { DisplayName = "WateryAir", _type = "togglethingy", AssociatedBool = PluginConfig.wateryair };
            Menu.Menu.Movement[7] = new MenuOption { DisplayName = "LongArms", _type = "togglethingy", AssociatedBool = PluginConfig.longarms };
            Menu.Menu.Movement[8] = new MenuOption { DisplayName = "SpinBot [DISABLED]", _type = "togglethingy", AssociatedBool = PluginConfig.SpinBot };
            Menu.Menu.Movement[9] = new MenuOption { DisplayName = "WASDFly", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "5", "7", "10", "13", "16" } };
            Menu.Menu.Movement[10] = new MenuOption { DisplayName = "Joystick Fly", _type = "togglethingy", AssociatedBool = PluginConfig.joystickfly };
            Menu.Menu.Movement[11] = new MenuOption { DisplayName = "Next", _type = "submenuthingy", AssociatedString = "Movement2" };
            Menu.Menu.Movement[12] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Movement2 = new MenuOption[14];
            Menu.Menu.Movement2[0] = new MenuOption { DisplayName = "Timer", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "1.03x", "1.06x", "1.09x", "1.1x", "1.13x", "1.16x", "1.19x", "1.2x", "1.23x", "1.26", "1.29", "1.3x", "2x", "3x", "4x", "5x" } };
            Menu.Menu.Movement2[1] = new MenuOption { DisplayName = "FloatyMonkey", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "1.1", "1.2", "1.4", "1.6", "1.8", "2", "2.2", "2.4", "2.6", "2.8", "3", "3.2", "3.4", "3.6", "3.8", "4", "Anti Grav" } };
            Menu.Menu.Movement2[2] = new MenuOption { DisplayName = "Climbable Gorillas", _type = "togglethingy", AssociatedBool = PluginConfig.ClimbableGorillas };
            Menu.Menu.Movement2[3] = new MenuOption { DisplayName = "Near Pulse", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" } };
            Menu.Menu.Movement2[4] = new MenuOption { DisplayName = "Near Pulse Distance", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" } };
            Menu.Menu.Movement2[5] = new MenuOption { DisplayName = "Player Scale", _type = "togglethingy", AssociatedBool = PluginConfig.PlayerScale };
            Menu.Menu.Movement2[6] = new MenuOption { DisplayName = "No Clip", _type = "togglethingy", AssociatedBool = PluginConfig.NoClip };
            Menu.Menu.Movement2[7] = new MenuOption { DisplayName = "Force Tag Freeze", _type = "togglethingy", AssociatedBool = PluginConfig.forcetagfreeze };
            Menu.Menu.Movement2[8] = new MenuOption { DisplayName = "Teleport To Random", _type = "buttonthingy", AssociatedString = "teleporttorandom" };
            Menu.Menu.Movement2[9] = new MenuOption { DisplayName = "HZ Hands", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" } };
            Menu.Menu.Movement2[10] = new MenuOption { DisplayName = "Throw", _type = "togglethingy", AssociatedBool = PluginConfig.Throw };
            Menu.Menu.Movement2[11] = new MenuOption { DisplayName = "Strafe Options", _type = "submenuthingy", AssociatedString = "Strafe Options" };
            Menu.Menu.Movement2[12] = new MenuOption { DisplayName = "PullMod", _type = "togglethingy", AssociatedBool = PluginConfig.pullmod };
            Menu.Menu.Movement2[13] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Speed = new MenuOption[5];
            Menu.Menu.Speed[0] = new MenuOption { DisplayName = "Speed", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "7", "7.2", "7.4", "7.6", "7.8", "8", "8.2", "8.4", "8.6" } };
            Menu.Menu.Speed[1] = new MenuOption { DisplayName = "Speed Toggle", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "7", "7.2", "7.4", "7.6", "7.8", "8", "8.2", "8.4", "8.6" } };
            Menu.Menu.Speed[2] = new MenuOption { DisplayName = "Near Speed", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "7", "7.2", "7.4", "7.6", "7.8", "8", "8.2", "8.4", "8.6" } };
            Menu.Menu.Speed[3] = new MenuOption { DisplayName = "Near Speed Distance", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25" } };
            Menu.Menu.Speed[4] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Strafe = new MenuOption[4];
            Menu.Menu.Strafe[0] = new MenuOption { DisplayName = "Strafe", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "Look", "Target", "Target [TEAM]", "L Joystick" } };
            Menu.Menu.Strafe[1] = new MenuOption { DisplayName = "Strafe Speed", _type = "sliderthingy", StringArray = new string[] { "6", "8", "10", "12", "14", "16", "18", "20" } };
            Menu.Menu.Strafe[2] = new MenuOption { DisplayName = "Strafe Jump Amount", _type = "sliderthingy", StringArray = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" } };
            Menu.Menu.Strafe[3] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Visual = new MenuOption[13];
            Menu.Menu.Visual[0] = new MenuOption { DisplayName = "Chams", _type = "togglethingy", AssociatedBool = PluginConfig.chams };
            Menu.Menu.Visual[1] = new MenuOption { DisplayName = "BoxESP", _type = "togglethingy", AssociatedBool = PluginConfig.boxesp };
            Menu.Menu.Visual[2] = new MenuOption { DisplayName = "HollowBoxESP", _type = "togglethingy", AssociatedBool = PluginConfig.hollowboxesp };
            Menu.Menu.Visual[3] = new MenuOption { DisplayName = "BoneESP", _type = "togglethingy", AssociatedBool = PluginConfig.boneesp };
            Menu.Menu.Visual[4] = new MenuOption { DisplayName = "Tracers", _type = "submenuthingy", AssociatedString = "Tracers" };
            Menu.Menu.Visual[5] = new MenuOption { DisplayName = "NameTags", _type = "submenuthingy", AssociatedString = "NameTags" };
            Menu.Menu.Visual[6] = new MenuOption { DisplayName = "Proximity Alert", _type = "togglethingy", AssociatedBool = PluginConfig.ProximityAlert };
            Menu.Menu.Visual[7] = new MenuOption { DisplayName = "Full Bright", _type = "togglethingy", AssociatedBool = PluginConfig.fullbright };
            Menu.Menu.Visual[8] = new MenuOption { DisplayName = "Sky Colour", _type = "sliderthingy", StringArray = new string[] { "Default", "Purple", "Red", "Cyan", "Green", "Black" } };
            Menu.Menu.Visual[9] = new MenuOption { DisplayName = "WhyIsEveryoneLookingAtMe", _type = "togglethingy", AssociatedBool = PluginConfig.whyiseveryonelookingatme };
            Menu.Menu.Visual[10] = new MenuOption { DisplayName = "FirstPerson", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "60", "70", "80", "90", "100", "110", "120", "130", "140" } };
            Menu.Menu.Visual[11] = new MenuOption { DisplayName = "Next", _type = "submenuthingy", AssociatedString = "Visual2" };
            Menu.Menu.Visual[12] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Visual2 = new MenuOption[7];
            Menu.Menu.Visual2[0] = new MenuOption { DisplayName = "SplashMonkey", _type = "togglethingy", AssociatedBool = PluginConfig.SplashMonkey };
            Menu.Menu.Visual2[1] = new MenuOption { DisplayName = "NoLeaves", _type = "togglethingy", AssociatedBool = PluginConfig.NoLeaves };
            Menu.Menu.Visual2[2] = new MenuOption { DisplayName = "ComicTags [DISABLED]", _type = "togglethingy", AssociatedBool = PluginConfig.ComicTags };
            Menu.Menu.Visual2[3] = new MenuOption { DisplayName = "Anti Screen Share", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "VR View", "PC View" } };
            Menu.Menu.Visual2[4] = new MenuOption { DisplayName = "MCM Sight", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "Self & Others", "Others", "Self" } };
            Menu.Menu.Visual2[5] = new MenuOption { DisplayName = "Show Boards", _type = "togglethingy", AssociatedBool = PluginConfig.ShowBoards };
            Menu.Menu.Visual2[6] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Tracers = new MenuOption[3];
            Menu.Menu.Tracers[0] = new MenuOption { DisplayName = "Tracers", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "RHand", "LHand", "Head", "Screen" } };
            Menu.Menu.Tracers[1] = new MenuOption { DisplayName = "Tracer Size", _type = "sliderthingy", StringArray = new string[] { "Extremely Small", "Super Small", "Small", "Medium", "Large", "Giant", "Huge" } };
            Menu.Menu.Tracers[2] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.NameTags = new MenuOption[7];
            Menu.Menu.NameTags[0] = new MenuOption { DisplayName = "NameTags", _type = "togglethingy", AssociatedBool = PluginConfig.NameTags };
            Menu.Menu.NameTags[1] = new MenuOption { DisplayName = "Show Colour Code", _type = "togglethingy", AssociatedBool = PluginConfig.ShowColourCode };
            Menu.Menu.NameTags[2] = new MenuOption { DisplayName = "Show Distance", _type = "togglethingy", AssociatedBool = PluginConfig.ShowDistance };
            Menu.Menu.NameTags[3] = new MenuOption { DisplayName = "Show FPS", _type = "togglethingy", AssociatedBool = PluginConfig.ShowFPS };
            Menu.Menu.NameTags[4] = new MenuOption { DisplayName = "Show Platform", _type = "togglethingy", AssociatedBool = PluginConfig.showplatform };
            Menu.Menu.NameTags[5] = new MenuOption { DisplayName = "NameTag Colour", _type = "sliderthingy", StringArray = new string[] { "White", "Yellow", "Green", "Blue", "Red", "Cyan", "Black" } };
            Menu.Menu.NameTags[6] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Player = new MenuOption[14];
            Menu.Menu.Player[0] = new MenuOption { DisplayName = "NoFinger", _type = "togglethingy", AssociatedBool = PluginConfig.nofinger };
            Menu.Menu.Player[1] = new MenuOption { DisplayName = "TagGun", _type = "togglethingy", AssociatedBool = PluginConfig.taggun };
            Menu.Menu.Player[2] = new MenuOption { DisplayName = "CreeperMonkey", _type = "togglethingy", AssociatedBool = PluginConfig.creepermonkey };
            Menu.Menu.Player[3] = new MenuOption { DisplayName = "GhostMonkey", _type = "togglethingy", AssociatedBool = PluginConfig.ghostmonkey };
            Menu.Menu.Player[4] = new MenuOption { DisplayName = "InvisMonkey", _type = "togglethingy", AssociatedBool = PluginConfig.invismonkey };
            Menu.Menu.Player[5] = new MenuOption { DisplayName = "TagAura", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "Really Close", "Close", "Legit", "Semi Legit", "Semi Blatant", "Blatant", "Rage" } };
            Menu.Menu.Player[6] = new MenuOption { DisplayName = "TagAll", _type = "togglethingy", AssociatedBool = PluginConfig.tagall };
            Menu.Menu.Player[7] = new MenuOption { DisplayName = "Desync [DISABLED]", _type = "togglethingy", AssociatedBool = PluginConfig.desync };
            Menu.Menu.Player[8] = new MenuOption { DisplayName = "HitBoxes", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "Really Close", "Close", "Legit", "Semi Legit", "Semi Blatant", "Blatant", "Rage" } };
            Menu.Menu.Player[9] = new MenuOption { DisplayName = "No Wind", _type = "togglethingy", AssociatedBool = PluginConfig.nowind };
            Menu.Menu.Player[10] = new MenuOption { DisplayName = "Anti Grab", _type = "togglethingy", AssociatedBool = PluginConfig.antigrab };
            Menu.Menu.Player[11] = new MenuOption { DisplayName = "Name Changer", _type = "togglethingy", AssociatedBool = PluginConfig.namechanger, extra = "[STUMP]" };
            Menu.Menu.Player[12] = new MenuOption { DisplayName = "Next", _type = "submenuthingy", AssociatedString = "Player2" };
            Menu.Menu.Player[13] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Player2 = new MenuOption[12];
            Menu.Menu.Player2[0] = new MenuOption { DisplayName = "Decapitation", _type = "togglethingy", AssociatedBool = PluginConfig.decapitation };
            Menu.Menu.Player2[1] = new MenuOption { DisplayName = "Rainbow Monkey", _type = "togglethingy", AssociatedBool = PluginConfig.rainbowmonkey, extra = "[STUMP]" };
            Menu.Menu.Player2[2] = new MenuOption { DisplayName = "Bad Apple Monkey", _type = "togglethingy", AssociatedBool = PluginConfig.badapplemonkey, extra = "[STUMP]" };
            Menu.Menu.Player2[3] = new MenuOption { DisplayName = "Aimbot", _type = "sliderthingy", extra = "[PAINTBRAWL]", StringArray = new string[] { "[OFF]", "Silent Aim", "Slilent Aim (Preds)" } };
            Menu.Menu.Player2[4] = new MenuOption { DisplayName = "Anti Tag", _type = "togglethingy", AssociatedBool = PluginConfig.antitag };
            Menu.Menu.Player2[5] = new MenuOption { DisplayName = "Fake Lag", _type = "togglethingy", AssociatedBool = PluginConfig.fakelag };
            Menu.Menu.Player2[6] = new MenuOption { DisplayName = "Disable Ghost Doors", _type = "togglethingy", AssociatedBool = PluginConfig.disableghostdoors };
            Menu.Menu.Player2[7] = new MenuOption { DisplayName = "Coloured Braclet", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "Rainbow", "Purple", "Black", "White", "Red", "Green", "Blue", "Yellow" } };
            Menu.Menu.Player2[8] = new MenuOption { DisplayName = "Ghost Self", _type = "buttonthingy", AssociatedString = "ghostself", extra = "[GR]" };
            Menu.Menu.Player2[9] = new MenuOption { DisplayName = "Ghost Revive Self", _type = "buttonthingy", AssociatedString = "ghostreviveself", extra = "[M] [GR]" };
            Menu.Menu.Player2[10] = new MenuOption { DisplayName = "FPS Spoof", _type = "togglethingy", AssociatedBool = PluginConfig.fpsspoof };
            Menu.Menu.Player2[11] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Exploits = new MenuOption[8];
            Menu.Menu.Exploits[0] = new MenuOption { DisplayName = "Break NameTags", _type = "togglethingy", AssociatedBool = PluginConfig.breaknametags };
            Menu.Menu.Exploits[1] = new MenuOption { DisplayName = "SS Platforms", _type = "togglethingy", AssociatedBool = PluginConfig.SSPlatforms, extra = "[M] [BASEMENT]" };
            Menu.Menu.Exploits[2] = new MenuOption { DisplayName = "Cosmetics Spoofer", _type = "submenuthingy", AssociatedString = "Cosmetics Spoofer" };
            Menu.Menu.Exploits[3] = new MenuOption { DisplayName = "Freeze All", _type = "togglethingy", AssociatedBool = PluginConfig.freezeall };
            Menu.Menu.Exploits[4] = new MenuOption { DisplayName = "Snowball Gun", _type = "togglethingy", AssociatedBool = PluginConfig.snowballgun };
            Menu.Menu.Exploits[5] = new MenuOption { DisplayName = "Max Quest Score", _type = "buttonthingy", AssociatedString = "Max Quest Score" };
            Menu.Menu.Exploits[6] = new MenuOption { DisplayName = "Next", _type = "submenuthingy", AssociatedString = "Exploits2" };
            Menu.Menu.Exploits[7] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Exploits2 = new MenuOption[8];
            Menu.Menu.Exploits2[0] = new MenuOption { DisplayName = "Disable Snowball Throw", _type = "togglethingy", AssociatedBool = PluginConfig.disablesnowballthrow };
            Menu.Menu.Exploits2[1] = new MenuOption { DisplayName = "Anti Snowball Fling", _type = "togglethingy", AssociatedBool = PluginConfig.antisnowballfling };
            Menu.Menu.Exploits2[2] = new MenuOption { DisplayName = "ElfSpammer", _type = "togglethingy", AssociatedBool = PluginConfig.ElfSpammer, extra = "[CITY] [TRY ON]" }; // once this coosmetic is removed make anti snowball fling | slingshot projectile aoeknockback
            Menu.Menu.Exploits2[3] = new MenuOption { DisplayName = "WaterSplash", _type = "togglethingy", AssociatedBool = PluginConfig.WaterSplash };
            Menu.Menu.Exploits2[4] = new MenuOption { DisplayName = "SpazAllRopes", _type = "togglethingy", AssociatedBool = PluginConfig.spazallropes };
            Menu.Menu.Exploits2[5] = new MenuOption { DisplayName = "Unlock All Gadgets", _type = "buttonthingy", AssociatedString = "UnlockAllGadgets", extra = "[SUPERINFECTION]" };
            Menu.Menu.Exploits2[6] = new MenuOption { DisplayName = "Complete All Quests", _type = "buttonthingy", AssociatedString = "CompleteAllQuests", extra = "[SUPERINFECTION]" };
            Menu.Menu.Exploits2[7] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.CosmeticsSpoofer = new MenuOption[2];
            Menu.Menu.CosmeticsSpoofer[0] = new MenuOption { DisplayName = "Spaz All Cosmetics", _type = "togglethingy", AssociatedBool = PluginConfig.spazallcosmetics };
            Menu.Menu.CosmeticsSpoofer[1] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Computer = new MenuOption[10];
            Menu.Menu.Computer[0] = new MenuOption { DisplayName = "Disconnect", _type = "buttonthingy", AssociatedString = "disconnect" };
            Menu.Menu.Computer[1] = new MenuOption { DisplayName = "Join GTC", _type = "buttonthingy", AssociatedString = "join GTC" };
            Menu.Menu.Computer[2] = new MenuOption { DisplayName = "Join TTT", _type = "buttonthingy", AssociatedString = "join TTT" };
            Menu.Menu.Computer[3] = new MenuOption { DisplayName = "Join YTTV", _type = "buttonthingy", AssociatedString = "join YTTV" };
            Menu.Menu.Computer[4] = new MenuOption { DisplayName = "Join MODS", _type = "buttonthingy", AssociatedString = "join MODS" };
            Menu.Menu.Computer[5] = new MenuOption { DisplayName = "Join MOD", _type = "buttonthingy", AssociatedString = "join MOD" };
            Menu.Menu.Computer[6] = new MenuOption { DisplayName = "Join:", _type = "buttonthingy", AssociatedString = "join", extra = "roomtojoin" };
            Menu.Menu.Computer[7] = new MenuOption { DisplayName = "Join MCMV2 Only", _type = "buttonthingy", AssociatedString = "join MCMV2 Only" };
            Menu.Menu.Computer[8] = new MenuOption { DisplayName = "Gamemodes", _type = "submenuthingy", AssociatedString = "Gamemodes" };
            Menu.Menu.Computer[9] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Gamemodes = new MenuOption[10];
            Menu.Menu.Gamemodes[0] = new MenuOption { DisplayName = "Modded Gamemode", _type = "togglethingy", AssociatedBool = PluginConfig.moddedgamemode };
            Menu.Menu.Gamemodes[1] = new MenuOption { DisplayName = "Competitive Gamemode", _type = "togglethingy", AssociatedBool = PluginConfig.competitivegamemode };
            Menu.Menu.Gamemodes[2] = new MenuOption { DisplayName = "Infection", _type = "buttonthingy", AssociatedString = "cgamemode Infection" };
            Menu.Menu.Gamemodes[3] = new MenuOption { DisplayName = "Casual", _type = "buttonthingy", AssociatedString = "cgamemode Casual" };
            Menu.Menu.Gamemodes[4] = new MenuOption { DisplayName = "Hunt", _type = "buttonthingy", AssociatedString = "cgamemode HuntDown" };
            Menu.Menu.Gamemodes[5] = new MenuOption { DisplayName = "PaintBrawl", _type = "buttonthingy", AssociatedString = "cgamemode Paintbrawl" };
            Menu.Menu.Gamemodes[6] = new MenuOption { DisplayName = "Guardian", _type = "buttonthingy", AssociatedString = "cgamemode Guardian" };
            Menu.Menu.Gamemodes[7] = new MenuOption { DisplayName = "Ambush", _type = "buttonthingy", AssociatedString = "cgamemode Ambush" };
            Menu.Menu.Gamemodes[8] = new MenuOption { DisplayName = "Freeze Tag", _type = "buttonthingy", AssociatedString = "cgamemode FreezeTag" };
            Menu.Menu.Gamemodes[9] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Safety = new MenuOption[7];
            Menu.Menu.Safety[0] = new MenuOption { DisplayName = "Panic", _type = "togglethingy", AssociatedBool = PluginConfig.Panic };
            Menu.Menu.Safety[1] = new MenuOption { DisplayName = "AntiReport", _type = "sliderthingy", StringArray = new string[] { "[OFF]", "Disconnect", "Reconnect", "Join Random" } };
            Menu.Menu.Safety[2] = new MenuOption { DisplayName = "RandomIdentity", _type = "buttonthingy", AssociatedString = "randomidentity" };
            Menu.Menu.Safety[3] = new MenuOption { DisplayName = "Pc Check Bypass", _type = "togglethingy", AssociatedBool = PluginConfig.pccheckbypass };
            Menu.Menu.Safety[4] = new MenuOption { DisplayName = "Fake Quest Menu", _type = "togglethingy", AssociatedBool = PluginConfig.fakequestmenu };
            Menu.Menu.Safety[5] = new MenuOption { DisplayName = "Fake Report Menu", _type = "togglethingy", AssociatedBool = PluginConfig.fakereportmenu };
            Menu.Menu.Safety[6] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Settings = new MenuOption[8];
            Menu.Menu.Settings[0] = new MenuOption { DisplayName = "Colour Settings", _type = "submenuthingy", AssociatedString = "ColourSettings" };
            Menu.Menu.Settings[1] = new MenuOption { DisplayName = "MenuPosition", _type = "sliderthingy", StringArray = new string[] { "Top Left", "Middle", "Top Right" } };
            Menu.Menu.Settings[2] = new MenuOption { DisplayName = "Config", _type = "sliderthingy", StringArray = new string[] { } };
            Menu.Menu.Settings[3] = new MenuOption { DisplayName = "Load Config", _type = "buttonthingy", AssociatedString = "loadconfig" };
            Menu.Menu.Settings[4] = new MenuOption { DisplayName = "Save Config", _type = "buttonthingy", AssociatedString = "saveconfig" };
            Menu.Menu.Settings[5] = new MenuOption { DisplayName = "Player Logging", _type = "togglethingy", AssociatedBool = PluginConfig.PlayerLogging };
            Menu.Menu.Settings[6] = new MenuOption { DisplayName = "Inverted Controls", _type = "togglethingy", AssociatedBool = PluginConfig.invertedControls };
            Menu.Menu.Settings[7] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Info = new MenuOption[5];
            Menu.Menu.Info[0] = new MenuOption { DisplayName = "PlayerList", _type = "buttonthingy" };
            Menu.Menu.Info[1] = new MenuOption { DisplayName = "GTC Ranked Codes", _type = "buttonthingy" };
            Menu.Menu.Info[2] = new MenuOption { DisplayName = "IIDK Menu Users", _type = "buttonthingy" };
            Menu.Menu.Info[3] = new MenuOption { DisplayName = "MCMV2 Menu Users", _type = "buttonthingy" };
            Menu.Menu.Info[4] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.MusicPlayer = new MenuOption[8];
            Menu.Menu.MusicPlayer[0] = new MenuOption { DisplayName = "Music", _type = "sliderthingy", StringArray = new string[] { } };
            Menu.Menu.MusicPlayer[1] = new MenuOption { DisplayName = "Play Music", _type = "buttonthingy", AssociatedString = "playmusic" };
            Menu.Menu.MusicPlayer[2] = new MenuOption { DisplayName = "Stop Music", _type = "buttonthingy", AssociatedString = "stopmusic" };
            Menu.Menu.MusicPlayer[3] = new MenuOption { DisplayName = "Shuffle Music", _type = "buttonthingy", AssociatedString = "shufflemusic" };
            Menu.Menu.MusicPlayer[4] = new MenuOption { DisplayName = "Loop Music", _type = "togglethingy", AssociatedBool = PluginConfig.loopmusic };
            Menu.Menu.MusicPlayer[5] = new MenuOption { DisplayName = "Sound Board", _type = "togglethingy", AssociatedBool = PluginConfig.soundboard };
            Menu.Menu.MusicPlayer[6] = new MenuOption { DisplayName = "Volume", _type = "sliderthingy", StringArray = new string[] { "100%", "90%", "80%", "70%", "60%", "50%", "40%", "30%", "20%", "10%" } };
            Menu.Menu.MusicPlayer[7] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.ColourSettings = new MenuOption[10];
            Menu.Menu.ColourSettings[0] = new MenuOption { DisplayName = "MenuColour", _type = "sliderthingy", StringArray = new string[] { "Purple", "Red", "Yellow", "Green", "Blue", "Black", "RGB", "Custom" } };
            Menu.Menu.ColourSettings[1] = new MenuOption { DisplayName = "Ghost Colour", _type = "sliderthingy", StringArray = new string[] { "Purple", "Red", "Yellow", "Green", "Blue", "Black" } };
            Menu.Menu.ColourSettings[2] = new MenuOption { DisplayName = "Beam Colour", _type = "sliderthingy", StringArray = new string[] { "Purple", "Red", "Yellow", "Green", "Blue", "Black" } };
            Menu.Menu.ColourSettings[3] = new MenuOption { DisplayName = "ESP Colour", _type = "sliderthingy", StringArray = new string[] { "Purple", "Red", "Yellow", "Green", "Blue", "Black" } };
            Menu.Menu.ColourSettings[4] = new MenuOption { DisplayName = "Ghost Opacity", _type = "sliderthingy", StringArray = new string[] { "100%", "80%", "60%", "30%", "20%", "0%" } };
            Menu.Menu.ColourSettings[5] = new MenuOption { DisplayName = "HitBoxes Opacity", _type = "sliderthingy", StringArray = new string[] { "100%", "80%", "60%", "30%", "20%", "0%" } };
            Menu.Menu.ColourSettings[6] = new MenuOption { DisplayName = "HitBoxes Colour", _type = "sliderthingy", StringArray = new string[] { "Purple", "Red", "Yellow", "Green", "Blue", "Black" } };
            Menu.Menu.ColourSettings[7] = new MenuOption { DisplayName = "Platforms Colour", _type = "sliderthingy", StringArray = new string[] { "Purple", "Red", "Yellow", "Green", "Blue", "Black" } };
            Menu.Menu.ColourSettings[8] = new MenuOption { DisplayName = "TargetIndicator Colour", _type = "sliderthingy", StringArray = new string[] { "Purple", "Red", "Yellow", "Green", "Blue", "Black" } };
            Menu.Menu.ColourSettings[9] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            Menu.Menu.Macro = new MenuOption[9];
            Menu.Menu.Macro[0] = new MenuOption { DisplayName = "Macro", _type = "sliderthingy", StringArray = new string[] { } };
            Menu.Menu.Macro[1] = new MenuOption { DisplayName = "Load Macro", _type = "buttonthingy", AssociatedString = "loadmacro" };
            Menu.Menu.Macro[2] = new MenuOption { DisplayName = "Stop Macro", _type = "buttonthingy", AssociatedString = "stopmacro" };
            Menu.Menu.Macro[3] = new MenuOption { DisplayName = "Record Macro", _type = "togglethingy", AssociatedBool = PluginConfig.recordmacro };
            Menu.Menu.Macro[4] = new MenuOption { DisplayName = "Delete Macro", _type = "buttonthingy", AssociatedString = "deletemacro" };
            Menu.Menu.Macro[5] = new MenuOption { DisplayName = "Auto Play Proximity", _type = "togglethingy", AssociatedBool = PluginConfig.autoplayproximity };
            Menu.Menu.Macro[6] = new MenuOption { DisplayName = "Auto Play Distance", _type = "sliderthingy", StringArray = new string[] { "Really Close", "Close", "Legit", "Semi Legit", "Semi Blatant", "Blatant", "Rage" } };
            Menu.Menu.Macro[7] = new MenuOption { DisplayName = "Macro Lerp Speed", _type = "sliderthingy", StringArray = new string[] { "0.1", "0.2", "0.3", "0.4", "0.5", "0.6" } };
            Menu.Menu.Macro[8] = new MenuOption { DisplayName = "Back", _type = "submenuthingy", AssociatedString = "backthingy" };

            // Only uncomment for dev builds or testing new ui dont uncomment unless you want to see the new ui through stuff while your playing
            // This shit buggy asf when on legacy ui it doesnt disable to show new ui at your camera needs fixing
            // Ill fix this one day like im lazy but ill fix it 
            //RegisterAllPanels();

            return Menu.Menu.MainMenu;
        }

        public static void RegisterAllPanels()
        {
            /*string[] allMenus = new string[]
            {
            "MainMenu",
            "Movement",
            "Movement2",
            "Strafe Options",
            "Speed Options",
            "Visual",
            "Visual2",
            "Tracers",
            "NameTags",
            "Player",
            "Player2",
            "Computer",
            "Exploits",
            "Exploits2",
            "Gamemodes",
            "Cosmetics Spoofer",
            "Safety",
            "Settings",
            "Info",
            "Macro",
            "MusicPlayer",
            "ColourSettings"
            };

            foreach (string menu in allMenus)
            {
                if (!GUICreator.panelMap.ContainsKey(menu))
                {
                    (GameObject panelObj, UnityEngine.UI.Text _) = GUICreator.NewUI(menu);
                    CustomConsole.Debug($"Registered panel: {menu}");
                }
            }*/
        }
    }
}