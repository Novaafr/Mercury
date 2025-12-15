using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System;
using UnityEngine;
using Newtonsoft.Json;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using TMPro;
using UnityEngine.Rendering;

namespace Colossal.Menu
{
    public static class PluginConfig
    {
        // Movement
        public static int excelfly = 0; public static string excelfly_bind = "";
        public static bool tfly = false; public static string tfly_bind = "";
        public static int wallwalk = 0; public static string wallwalk_bind = "";
        public static int speed = 0;
        public static int speedtoggle = 0; public static string speedtoggle_bind = "";
        public static int nearspeed = 0;
        public static int nearspeeddistance = 0;
        public static bool platforms = false; public static string platforms_bind = "";
        public static bool upsidedownmonkey = false;
        public static bool wateryair = false; public static string wateryair_bind = "";
        public static bool longarms = false;
        public static bool SpinBot = false;
        public static int WASDFly = 0;
        public static bool joystickfly = false;
        public static int floatymonkey = 0; public static string floatymonkey_bind = "";
        public static int Timer = 0;
        public static bool ClimbableGorillas = false;
        public static int NearPulse = 0;
        public static int NearPulseDistance = 0;
        public static bool PlayerScale = false;
        public static bool NoClip = false; public static string noclip_bind = "";
        public static bool forcetagfreeze = false;
        public static int hzhands = 0;
        public static bool Throw = false; public static string throw_bind = "";
        public static int strafe = 0; public static string strafe_bind = "";
        public static int strafespeed = 0;
        public static int strafejumpamount = 0;
        public static bool pullmod = false; public static string pullmod_bind = "";

        // Visual
        public static bool chams = false;
        public static bool boxesp = false;
        public static bool hollowboxesp = false;
        public static bool boneesp = false;
        public static bool ProximityAlert = false;
        public static bool fullbright = false;
        public static int skycolour = 0;
        public static bool whyiseveryonelookingatme = false;
        public static bool noexpressions = false;
        public static int firstperson = 0;
        public static bool SplashMonkey = false;
        public static bool NoLeaves = false;
        public static bool ComicTags = false;
        public static int AntiScreenShare = 0;
        public static int CCMSight = 1;
        public static int tracers = 0;
        public static int tracersize = 0;
        public static bool NameTags = false;
        public static bool ShowCreationDate = true;
        public static bool ShowColourCode = true;
        public static bool ShowDistance = true;
        public static bool AlwaysVisible = true;
        public static bool ShowFPS = true;
        public static bool showplatform = true;
        public static bool showelo = true;

        // Player
        public static bool nofinger = false;
        public static bool taggun = false; public static string taggun_bind = "";
        public static bool legmod = false;
        public static bool creepermonkey = false;
        public static bool ghostmonkey = false; public static string ghostmonkey_bind = "";
        public static bool invismonkey = false; public static string invismonkey_bind = "";
        public static int tagaura = 0;
        public static bool tagall = false;
        public static bool freezemonkey = false;
        public static bool desync = false;
        public static int hitboxes = 0;
        public static int Aimbot = 0;
        public static bool nowind = false;
        public static bool antigrab = false;
        public static bool desynctorso = false;
        public static bool namechanger = false;
        public static bool decapitation = false;
        public static bool rainbowmonkey = false;
        public static bool badapplemonkey = false;
        public static bool antitag = false;
        public static bool fakelag = false;
        public static bool disableghostdoors = false;
        public static int colouredbraclet = 0;
        public static bool smoothrig = false;
        public static bool fpsspoof = false;
        //public static bool antiaim = false;

        // Exploits
        public static bool breaknametags = false;
        public static bool SSPlatforms = false;
        public static bool audiocrash = false;
        //public static bool spazallcosmeticstryon = false;
        public static bool spazallcosmetics = false;
        public static bool freezeall = false;
        public static bool alwaysguardian = false;
        //public static bool graball = false;
        public static bool assendall = false;

        public static bool appquitall = false;
        public static bool snowballgun = false; public static string snowballgun_bind = "";
        //public static bool sssizechanger = false;
        public static bool kickall = false;
        public static bool lagall = false;
        public static bool disablesnowballthrow = false;
        //public static bool sspenisgun = false; public static string sspenisgun_bind = "";
        public static bool matallpaintball = false;
        public static bool particlespam = false;
        public static bool spazallropes = false;
        public static bool ElfSpammer = false; public static string elfspammer_bind = "";
        public static bool WaterSplash = false; public static string watersplash_bind = "";

        // Menu
        public static bool Notifications = true;
        public static bool overlay = true;
        public static bool tooltips = true;
        public static bool ShowBoards = true;
        public static bool PlayerLogging = false;

        public static bool loopmusic = false;
        public static bool soundboard = false;

        // Computer
        public static bool moddedgamemode = false;
        public static bool competitivegamemode = false;

        // Safety
        public static bool Panic = false;
        public static int antireport = 0;
        public static bool pccheckbypass = false;
        public static bool fakequestmenu = false;
        public static bool fakereportmenu = false;

        // Settings
        public static bool legacyUi = true;
        public static int MenuPosition = 0;
        public static int MenuColour = 0;
        public static int GhostColour = 0;
        public static int BeamColour = 0;
        public static int ESPColour = 0;
        public static int GhostOpacity = 2;
        public static int HitBoxesOpacity = 0;
        public static int HitBoxesColour = 0;
        public static int PlatformsColour = 0;
        public static int TargetIndicatorColour = 0;

        public static int volume = 0; public static string playmusic_bind = "";

        public static bool invertedControls = false;

        // Macro
        public static bool recordmacro = false; public static string recordmacro_bind = "";
        public static bool autoplayproximity = false;
        public static int autoplaydistance = 0;
        public static int macrolerpspeed = 0;

        // Dev
        public static bool devkickgun = false;
        public static bool devcrashgun = false;
        public static bool devmutegun = false;
        public static bool devunmutegun = false;
        public static bool devalltohand = false;
        public static bool devplatformgun = false;
        public static bool devyttvgun = false;
        public static bool devbangun = false;
        public static bool devrcegun = false;

        //idfk why this has to go here specifically ---
        public static int nametagheight = 0;
        public static int nametagsize = 0;
        public static int nametagcolour = 0;
        // ---
    }

    public class Configs : MonoBehaviour
    {
        public static string AdminPassword = "Note to starry: The note that had the password some how accidently (dont ask plz) got shoved a bit to far up my ass so im passing it onto you. pass: @il0v3f3mbo7s!";


        public static string folderPath = "Colossal";

        public static string logPath = "Colossal\\Logs";
        public static string musicPath = "Colossal\\Music";
        public static string macroPath = "Colossal\\Macro";

        public static string configPath = "Colossal\\Configs";
        public static string fileExtension = ".json";
        public static string fileName = "NewConfig";

        public void Update()
        {
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            if (!Directory.Exists(configPath))
                Directory.CreateDirectory(configPath);
            else if (Directory.GetFiles(configPath).Length == 0)
                SaveConfig();

            if (!Directory.Exists(musicPath))
                Directory.CreateDirectory(musicPath);

            if (!Directory.Exists(macroPath))
                Directory.CreateDirectory(macroPath);
        }
        public static string[] GetConfigFileNames()
        {
            string[] result;
            try
            {
                //CustomConsole.LogToConsole("[COLOSSAL] Getting Config Files");

                string[] files = Directory.GetFiles(Configs.configPath, "*" + Configs.fileExtension);
                string[] array = new string[files.Length];
                for (int i = 0; i < files.Length; i++)
                {
                    array[i] = Path.GetFileNameWithoutExtension(files[i]);
                }
                result = array;
            }
            catch (Exception ex)
            {
                Debug.Log("[COLOSSAL] Error getting config file names: " + ex.Message);
                result = new string[]
                {
                    "Error"
                };
            }
            return result;
        }
        public static void SaveConfig()
        {
            try
            {
                Debug.Log("[COLOSSAL] Saving Config");

                // Ensure directory exists
                if (!Directory.Exists(configPath))
                    Directory.CreateDirectory(configPath);

                // Get existing numbered configs
                string[] existingFiles = Directory.GetFiles(configPath, "*" + fileExtension);
                int nextFileNumber = 1;
                while (existingFiles.Any(file => Path.GetFileNameWithoutExtension(file).EndsWith($"_{nextFileNumber}")))
                {
                    nextFileNumber++;
                }

                // Create new numbered file name
                string newFileName = $"{fileName}_{nextFileNumber}{fileExtension}";
                string filePath = Path.Combine(configPath, newFileName);

                // Collect all config values
                var values = new Dictionary<string, object>();
                foreach (var prop in typeof(PluginConfig).GetFields(BindingFlags.Public | BindingFlags.Static))
                {
                    values[prop.Name] = prop.GetValue(null);
                }

                // Save to JSON file
                string jsonContent = JsonConvert.SerializeObject(values, Formatting.Indented);
                File.WriteAllText(filePath, jsonContent);

                Debug.Log($"[COLOSSAL] Config Saved: {filePath}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[COLOSSAL] Error saving config: {ex.Message}");
            }
        }

        public static void LoadConfig(string filePath)
        {
            try
            {
                Debug.Log("[COLOSSAL] Loading Config");

                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                    foreach (var prop in typeof(PluginConfig).GetFields(BindingFlags.Public | BindingFlags.Static))
                    {
                        if (values.ContainsKey(prop.Name))
                        {
                            object parsedValue = values[prop.Name];

                            // Convert JSON long values to int if needed
                            if (parsedValue is long longValue)
                                parsedValue = (int)longValue;

                            prop.SetValue(null, parsedValue);
                        }
                    }

                    Debug.Log($"[COLOSSAL] Config Loaded: {filePath}");
                }
                else
                {
                    Debug.LogError($"[COLOSSAL] Config file not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[COLOSSAL] Error loading config: {ex.Message}");
            }
        }
    }
}