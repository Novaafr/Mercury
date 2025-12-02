using BepInEx;
using Colossal;
using Colossal.Patches;
using ExitGames.Client.Photon;
using GorillaLocomotion;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using WebSocketSharp;
using static BuilderMaterialOptions;
using static UnityEngine.Rendering.DebugUI;

namespace Colossal.Menu
{
    public class PanelElement
    {
        public GameObject RootObject;
        public GameObject grabInstance;
        private GameObject back;
        public List<GameObject> uiElements = new List<GameObject>();

        public Text MenuText;
        public MenuOption[] CurrentViewingMenu { get; private set; } // Add this to store the menu options per panel

        public PanelElement(GameObject root)
        {
            CustomConsole.Debug($"Initializing PanelElement for root: {root.name}");
            RootObject = root;


            RootObject.layer = 14; // Already correct
            RootObject.transform.LookAt(GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position);
            RootObject.transform.Rotate(0f, 180f, 0f, Space.Self);

            grabInstance = GameObject.Instantiate(AssetBundleLoader.grab, RootObject.transform, false);
            grabInstance.transform.localPosition = new Vector3(0, 0.11f, 0);
            grabInstance.transform.localRotation = Quaternion.identity;
            grabInstance.layer = 14;
            grabInstance.name = "grab";
            GameObject x = grabInstance.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
            x.layer = 14;

            back = GameObject.Instantiate(AssetBundleLoader.back, RootObject.transform, false);
            back.transform.localRotation = Quaternion.identity;
            back.layer = 14;
            var outline = back.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = grabInstance.GetComponent<Renderer>().material.color;
            outline.OutlineWidth = 16f;

            MenuText = grabInstance.transform.GetChild(0).GetChild(0).GetComponent<Text>();
        }
        public static void UpdatePanel(PanelElement panel, MenuOption[] options)
        {
            if (panel == null || options == null)
            {
                CustomConsole.Error($"Panel or options array is null - Panel: {(panel == null ? "null" : panel.RootObject.name)}, Options: {(options == null ? "null" : options.Length.ToString())}");
                return;
            }

            if (panel.MenuText != null)
            {
                panel.MenuText.text = panel.RootObject.name + "\n"; // Use the panel's name as the title
                CustomConsole.Debug($"Set MenuText to: {panel.RootObject.name} for panel {panel.RootObject.name}");
            }
            else
            {
                CustomConsole.Error("MenuText is null in UpdatePanel!");
            }

            // Clear existing UI elements except grab and back
            foreach (var element in panel.uiElements)
            {
                if (element != panel.grabInstance && element != panel.back)
                {
                    GameObject.Destroy(element);
                }
            }
            panel.uiElements.Clear();
            panel.uiElements.Add(panel.grabInstance);
            panel.uiElements.Add(panel.back);

            panel.CurrentViewingMenu = options; // Update the menu options

            float yOffset = -0.14f;
            float spacing = 0.06f;
            float defaultWidth = 0.2f;
            float defaultHeight = 0.06f;

            CustomConsole.Debug($"UpdatePanel called - Menu.SelectedOptionIndex: {Menu.SelectedOptionIndex}");

            for (int i = 0; i < options.Length; i++)
            {
                MenuOption option = options[i];
                GameObject uiElement = null;

                string bindsDisplay = CustomBinding.GetBinds(option.DisplayName.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower());
                bool isSelected = (i == Menu.SelectedOptionIndex);

                if (option._type == BepInPatcher.togglethingy)
                {
                    uiElement = GameObject.Instantiate(AssetBundleLoader.toggle, panel.RootObject.transform, false);
                    Text modText = uiElement.transform.GetChild(0).GetChild(0).GetComponent<Text>();
                    Text bindText = uiElement.transform.GetChild(0).GetChild(1).GetComponent<Text>();
                    GameObject indicator = uiElement.transform.GetChild(1).gameObject;

                    modText.text = option.DisplayName;
                    bindText.text = string.IsNullOrEmpty(bindsDisplay) ? "" : $"[{bindsDisplay}]";
                    bindText.enabled = !string.IsNullOrEmpty(bindsDisplay);

                    Renderer indicatorRenderer = indicator.GetComponent<Renderer>();
                    indicatorRenderer.enabled = option.AssociatedBool;

                    uiElement.name = $"Toggle_{i}";
                    uiElement.layer = 14; 
                    indicator.layer = 14;
                }
                else if (option._type == BepInPatcher.buttonthingy || option._type == BepInPatcher.submenuthingy)
                {
                    uiElement = GameObject.Instantiate(AssetBundleLoader.button, panel.RootObject.transform, false);
                    Text modText = uiElement.transform.GetChild(0).GetChild(0).GetComponent<Text>();
                    modText.text = option.DisplayName;

                    uiElement.name = option._type == BepInPatcher.buttonthingy ? $"Button_{i}" : $"Submenu_{i}";
                    uiElement.layer = 14;
                    GameObject back = uiElement.transform.GetChild(1).gameObject;
                    back.layer = 14;
                }
                else if (option._type == BepInPatcher.sliderthingy)
                {
                    bool hasBind = !string.IsNullOrEmpty(bindsDisplay);
                    uiElement = GameObject.Instantiate(hasBind ? AssetBundleLoader.slider_bind : AssetBundleLoader.slider, panel.RootObject.transform, false);

                    Transform canvas = uiElement.transform.GetChild(0); // canvas
                                                                        // Debug the hierarchy to confirm child order
                    UnityEngine.Debug.Log($"Slider_{i} canvas child count: {canvas.childCount}");
                    for (int j = 0; j < canvas.childCount; j++)
                    {
                        UnityEngine.Debug.Log($"Child {j}: {canvas.GetChild(j).name}");
                    }

                    Text modText = canvas.GetChild(0).GetComponent<Text>();//  text
                    Text bindText = null;
                    if (hasBind) bindText = canvas.GetChild(1).GetComponent<Text>();//  bindText
                                                                                    //  Adjust indices based on actual hierarchy
                    int sliderTextIndex = hasBind ? 2 : 1; // This might be wrong; let's find the correct index
                    int leftArrowIndex = hasBind ? 3 : 2;
                    int rightArrowIndex = hasBind ? 4 : 3;

                    // Find the correct slider_text by name since indices are unreliable
                    Text sliderText = null;
                    for (int j = 0; j < canvas.childCount; j++)
                    {
                        if (canvas.GetChild(j).name == "slider_text")
                        {
                            sliderText = canvas.GetChild(j).GetComponent<Text>();
                            sliderTextIndex = j;
                            break;
                        }
                    }

                    // Find the arrows by name to avoid index issues
                    GameObject leftArrow = null;
                    GameObject rightArrow = null;
                    for (int j = 0; j < canvas.childCount; j++)
                    {
                        if (canvas.GetChild(j).name == "l_arrow")
                        {
                            leftArrow = canvas.GetChild(j).gameObject;
                            leftArrowIndex = j;
                        }
                        else if (canvas.GetChild(j).name == "r_arrow")
                        {
                            rightArrow = canvas.GetChild(j).gameObject;
                            rightArrowIndex = j;
                        }
                    }

                    GameObject indicator = uiElement.transform.GetChild(1).gameObject; // toggleIndicator

                    // Set up text displays
                    modText.text = option.DisplayName;
                    if (hasBind && bindText != null)
                    {
                        bindText.text = $"[{bindsDisplay}]";
                        bindText.enabled = true;
                    }
                    if (sliderText != null)
                    {
                        sliderText.text = option.StringArray[option.stringsliderind].Replace("[", "").Replace("]", "").Replace("\n", "");
                    }
                    else
                    {
                        UnityEngine.Debug.LogError($"Slider text not found for Slider_{i}. Check prefab hierarchy.");
                    }

                    // Set up indicator
                    Renderer indicatorRenderer = indicator.GetComponent<Renderer>();
                    indicatorRenderer.enabled = option.stringsliderind != 0;

                    // Proper naming for arrows to match their functionality
                    uiElement.name = $"Slider_{i}";
                    if (leftArrow != null)
                    {
                        leftArrow.name = $"SliderLArrow_{i}"; //  This will be the left arrow (decreases)
                    }
                    if (rightArrow != null)
                    {
                        rightArrow.name = $"SliderRArrow_{i}"; // This will be the right arrow (increases)
                    }

                    // Disable any extra r_arrow children
                    for (int j = 0; j < canvas.childCount; j++)
                    {
                        var child = canvas.GetChild(j).gameObject;
                        if (child.name == "r_arrow" && child != rightArrow)
                        {
                            child.SetActive(false);
                            UnityEngine.Debug.Log($"Disabled extra r_arrow for Slider_{i}");
                        }
                    }

                    // Layer setup
                    uiElement.layer = 14;
                    if (leftArrow != null) leftArrow.layer = 14;
                    if (rightArrow != null) rightArrow.layer = 14;
                    indicator.layer = 14;
                }

                if (uiElement != null)
                {
                    uiElement.transform.localPosition = new Vector3(0, yOffset, 0);
                    uiElement.transform.localRotation = Quaternion.identity;

                    Renderer renderer = uiElement.GetComponentInChildren<Renderer>();
                    Vector3 size = renderer != null ? renderer.bounds.size : new Vector3(defaultWidth, defaultHeight, 0.01f);
                    if (option._type == BepInPatcher.sliderthingy) size.x *= 1.5f;

                    panel.uiElements.Add(uiElement);
                    yOffset -= spacing;
                }
            }

            if (panel.back != null)
            {
                float grabTopY = panel.grabInstance.transform.localPosition.y + 0.25f;
                float lastItemBottomY = yOffset - spacing;
                float backHeight = grabTopY - lastItemBottomY - 0.46f;
                Vector3 backScale = panel.back.transform.localScale;
                backScale.y = backHeight;
                panel.back.transform.localScale = backScale;
                panel.back.transform.localPosition = new Vector3(0, (grabTopY + lastItemBottomY) / 2f, 0);
            }
            else
            {
                CustomConsole.Error("Back object is null in UpdatePanel!");
            }
        }
    }
    internal class GUICreator : MonoBehaviour
    {
        private static Material mat = new Material(Shader.Find("GUI/Text Shader"));

        public static Dictionary<string, PanelElement> panelMap = new Dictionary<string, PanelElement>();
        public static List<PanelElement> openPanels = new List<PanelElement>();
        public static List<PanelElement> panelsToDisable = new List<PanelElement>();

        public const float panelOffset = 0.3f;
        public static readonly LayerMask UILayerMask = 1 << 14;

        public static ValueTuple<GameObject, Text> CreateTextGUI(string text, string name, TextAnchor alignment, Vector3 loctrans, bool parent)
        {
            return GUICreator.LegacyUI(text, name, alignment, loctrans, parent);
        }

        public static (GameObject, Text) NewUI(string name)
        {
            if (AssetBundleLoader.panel == null || AssetBundleLoader.hud == null)
            {
                CustomConsole.Debug("New UI: Panel or HUD is missing!");
                return (null, null);
            }

            if (panelMap.ContainsKey(name))
            {
                PanelElement existingPanel = panelMap[name];
                existingPanel.RootObject.SetActive(true);
                PanelElement.UpdatePanel(existingPanel, GetMenuOptions(name));
                CustomConsole.Debug($"Reusing existing UI: {name}");
                return (existingPanel.RootObject, null);
            }

            GameObject newPanel = new GameObject();
            newPanel.name = name;
            newPanel.transform.SetParent(AssetBundleLoader.hud.transform, false);

            Animator animator = newPanel.AddComponent<Animator>();
            if (AssetBundleLoader.Menu_In != null)
            {
                animator.runtimeAnimatorController = AssetBundleLoader.Menu_Controller;
                animator.Play("Menu_In");
                CustomConsole.Debug($"Added Animator to {newPanel.name} and playing Menu_In animation");
            }
            else
            {
                CustomConsole.Error("AssetBundleLoader.Menu_In is null, cannot play animation!");
            }


            Vector3 basePosition = Camera.main.transform.position + Camera.main.transform.forward * 1f;
            float panelOffset = 0.5f;
            Vector3 spawnPosition = basePosition;
            int panelIndex = 0;

            bool positionValid = false;
            while (!positionValid)
            {
                spawnPosition = basePosition + new Vector3(panelOffset * panelIndex, 0, 0);
                positionValid = true;

                foreach (var panel in openPanels)
                {
                    if (panel.RootObject.activeSelf)
                    {
                        float distance = Vector3.Distance(panel.RootObject.transform.position, spawnPosition);
                        if (distance < panelOffset * 0.8f)
                        {
                            positionValid = false;
                            panelIndex++;
                            break;
                        }
                    }
                }
            }

            newPanel.transform.position = spawnPosition;
            newPanel.transform.rotation = Quaternion.identity;


            PanelElement element = new PanelElement(newPanel);
            openPanels.Add(element);
            panelMap[name] = element;

            MenuOption[] options = GetMenuOptions(name);
            if (options == null)
            {
                CustomConsole.Error($"GetMenuOptions returned null for menu: {name}");
                return (newPanel, null);
            }

            PanelElement.UpdatePanel(element, options);
            CustomConsole.Debug($"New UI Created: {name} with {options.Length} options at {spawnPosition}");
            return (newPanel, null);
        }

        public static MenuOption[] GetMenuOptions(string menuName)
        {
            switch (menuName)
            {
                case "MainMenu": return Menu.MainMenu;
                case "Movement": return Menu.Movement;
                case "Movement2": return Menu.Movement2;
                case "Strafe Options": return Menu.Strafe;
                case "Speed Options": return Menu.Speed;
                case "Visual": return Menu.Visual;
                case "Visual2": return Menu.Visual2;
                case "Tracers": return Menu.Tracers;
                case "NameTags": return Menu.NameTags;
                case "Player": return Menu.Player;
                case "Player2": return Menu.Player2;
                case "Computer": return Menu.Computer;
                case "Exploits": return Menu.Exploits;
                case "Exploits2": return Menu.Exploits2;
                case "Gamemodes": return Menu.Gamemodes;
                case "Cosmetics Spoofer": return Menu.CosmeticsSpoofer;
                case "Safety": return Menu.Safety;
                case "Settings": return Menu.Settings;
                case "Info": return Menu.Info;
                case "MusicPlayer": return Menu.MusicPlayer;
                case "ColourSettings": return Menu.ColourSettings;
                case "Dev": return Menu.Dev;
                default: return Menu.MainMenu; // Default to MainMenu if unknown
            }
        }

        private static (GameObject, Text) LegacyUI(string text, string name, TextAnchor alignment, Vector3 loctrans, bool parent)
        {
            GameObject HUDObj = new GameObject(name);

            Canvas canvas = HUDObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            HUDObj.AddComponent<CanvasScaler>();
            HUDObj.AddComponent<GraphicRaycaster>();

            RectTransform rectTransform = HUDObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(5, 5);
            HUDObj.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);

            GameObject menuTextObj = new GameObject();
            menuTextObj.transform.SetParent(HUDObj.transform);
            Text MenuText = menuTextObj.AddComponent<Text>();
            MenuText.text = text;
            MenuText.fontSize = 10;
            MenuText.font = BepInPatcher.gtagfont;
            MenuText.rectTransform.sizeDelta = new Vector2(260, 180);
            MenuText.rectTransform.localScale = new Vector3(0.01f, 0.01f, 1f);
            MenuText.rectTransform.localPosition = loctrans;
            MenuText.material = mat;
            MenuText.alignment = alignment;

            if (parent)
            {
                HUDObj.transform.SetParent(Camera.main.transform);
            }
            HUDObj.transform.position = Camera.main.transform.position;
            HUDObj.transform.rotation = Camera.main.transform.rotation;

            CustomConsole.Debug($"Created Legacy UI {name}");
            return (HUDObj, MenuText);
        }
    }
}