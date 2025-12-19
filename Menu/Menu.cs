using Colossal.Notifacation;
using Colossal.Patches;
using ExitGames.Client.Photon;
using GorillaNetworking;
using GorillaTagScripts.AI;
using Photon.Pun;
using Photon.Realtime;
using PlayFab;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
        public static PanelElement activePanel; // Track the currently active panel
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
        public static MenuOption[] Macro;
        public static MenuOption[] MusicPlayer;

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
                    AssetBundleLoader.SpawnVoidBubbles();

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

                    if (Plugin.holder.GetComponent<MacroRecorder>() == null)
                        Plugin.holder.AddComponent<MacroRecorder>();

                    CustomConsole.Debug("Added all menu components");


                    //if (BepInPatcher.buttonthingy.IsNullOrEmpty().ToString().IsNullOrWhiteSpace() || BepInPatcher.backthingy.IsNullOrEmpty().ToString().IsNullOrWhiteSpace() || BepInPatcher.submenuthingy.IsNullOrEmpty().ToString().IsNullOrWhiteSpace() || BepInPatcher.sliderthingy.IsNullOrEmpty().ToString().IsNullOrWhiteSpace() || BepInPatcher.togglethingy.IsNullOrEmpty().ToString().IsNullOrWhiteSpace())
                    if (!BepInPatcher.buttonthingy.IsNullOrEmpty() || !BepInPatcher.backthingy.IsNullOrEmpty() || !BepInPatcher.submenuthingy.IsNullOrEmpty() || !BepInPatcher.sliderthingy.IsNullOrEmpty() || !BepInPatcher.togglethingy.IsNullOrEmpty())
                    {
                        GameObject coroutineHostObj = new GameObject("LoadOnceCoroutineHost");
                        LoadOnceCoroutineHost coroutineHost = coroutineHostObj.AddComponent<LoadOnceCoroutineHost>();
                        coroutineHost.StartCoroutine(LoadMenus(coroutineHostObj));
                    }
                    else
                    {
                        MenuHubText.text = "<color=red>Error Loading Menu Types (Code: 2)\nPlease Show This To Nova\nRestart Your Game</color>";
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

                    if (GUICreator.panelMap.TryGetValue("MainMenu", out var mainPanel))
                        activePanel = mainPanel;

                    UpdateMenuState(new MenuOption(), null, null);
                    CustomConsole.Debug("Updated Menu State");
                }
            }
            catch (Exception ex)
            {
                CustomConsole.Error(ex.ToString());
            }
        }

        public static int loadingNumber = 0;
        private class LoadOnceCoroutineHost : MonoBehaviour { }
        private static IEnumerator LoadMenus(GameObject coroutineHostObj)
        {
            void UpdateLoadingText()
            {
                string loadingText = $"<color=magenta>Loading {loadingNumber}/22</color>";
                if (AgreementHubText != null)
                {
                    AgreementHubText.text = loadingText;
                }
                else
                {
                    CustomConsole.Error("AgreementHubText is null");
                }
            }

            UpdateLoadingText(); // Initial text

            // Helper function to safely load a menu
            MenuOption[] SafeLoadMenu(string menuName, string debugIndex)
            {
                try
                {
                    MenuOption[] menu = MenuLoader.LoadMenu();
                    if (menu == null)
                    {
                        CustomConsole.Error($"Failed to load menu {menuName}: Returned null");
                    }
                    return menu;
                }
                catch (Exception ex)
                {
                    CustomConsole.Error($"Error loading menu {menuName}: {ex}");
                    return null;
                }
            }

            MainMenu = SafeLoadMenu("Menu_MainMenu", "1"); loadingNumber += 1; UpdateLoadingText(); yield return null;
            Movement = SafeLoadMenu("Menu_Movement", "2"); loadingNumber += 1; UpdateLoadingText(); yield return null;
            Movement2 = SafeLoadMenu("Menu_Movement2", "3"); loadingNumber += 1; UpdateLoadingText(); yield return null;
            Speed = SafeLoadMenu("Menu_Speed", "4"); loadingNumber += 1; UpdateLoadingText(); yield return null;
            Strafe = SafeLoadMenu("Menu_Strafe", "5"); loadingNumber += 1; UpdateLoadingText(); yield return null;

            Visual = SafeLoadMenu("Menu_Visual", "6"); loadingNumber += 1; UpdateLoadingText(); yield return null;
            Visual2 = SafeLoadMenu("Menu_Visual2", "7"); loadingNumber += 1; UpdateLoadingText(); yield return null;
            Tracers = SafeLoadMenu("Menu_Tracers", "8"); loadingNumber += 1; UpdateLoadingText(); yield return null;
            NameTags = SafeLoadMenu("Menu_NameTags", "9"); loadingNumber += 1; UpdateLoadingText(); yield return null;

            Player = SafeLoadMenu("Menu_Player", "10"); loadingNumber += 1; UpdateLoadingText(); yield return null;
            Player2 = SafeLoadMenu("Menu_Player2", "11"); loadingNumber += 1; UpdateLoadingText(); yield return null;

            Exploits = SafeLoadMenu("Menu_Exploits", "12"); loadingNumber += 1; UpdateLoadingText(); yield return null;
            Exploits2 = SafeLoadMenu("Menu_Exploits2", "13"); loadingNumber += 1; UpdateLoadingText(); yield return null;
            CosmeticsSpoofer = SafeLoadMenu("Menu_CosmeticsSpoofer", "14"); loadingNumber += 1; UpdateLoadingText(); yield return null;

            Computer = SafeLoadMenu("Menu_Computer", "15"); loadingNumber += 1; UpdateLoadingText(); yield return null;
            Gamemodes = SafeLoadMenu("Menu_Gamemodes", "16"); loadingNumber += 1; UpdateLoadingText(); yield return null;

            Safety = SafeLoadMenu("Menu_Safety", "17"); loadingNumber += 1; UpdateLoadingText(); yield return null;

            Settings = SafeLoadMenu("Menu_Settings", "18"); loadingNumber += 1; UpdateLoadingText(); yield return null;
            ColourSettings = SafeLoadMenu("Menu_ColourSettings", "19"); loadingNumber += 1; UpdateLoadingText(); yield return null;

            Info = SafeLoadMenu("Menu_Info", "20"); loadingNumber += 1; UpdateLoadingText(); yield return null;

            MusicPlayer = SafeLoadMenu("Menu_MusicPlayer", "21"); loadingNumber += 1; UpdateLoadingText(); yield return null;

            Macro = SafeLoadMenu("Menu_Macro", "22"); loadingNumber += 1; UpdateLoadingText(); yield return null;

            // Final update
            loadingNumber = 22;
            UpdateLoadingText();

            (MenuHub, MenuHubText) = GUICreator.CreateTextGUI("", "MenuHub", TextAnchor.UpperLeft, new Vector3(0, 0.4f, 3.6f), true);
            if (MenuHub == null || MenuHubText == null)
            {
                CustomConsole.Error("Failed to create MenuHub or MenuHubText");
                GameObject.Destroy(coroutineHostObj);
                UnityEngine.Object.Destroy(AgreementHub);
                AssetBundleLoader.DespawnVoidBubbles();
                yield break;
            }

            MenuState = "MainMenu";
            CurrentViewingMenu = MainMenu;
            CustomConsole.Debug("Build Menu");

            UnityEngine.Object.Destroy(AgreementHub);
            AssetBundleLoader.DespawnVoidBubbles();
            GameObject.Destroy(coroutineHostObj);
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
                        bool isSpecialSlider = CurrentViewingMenu[SelectedOptionIndex].DisplayName == Settings[2].DisplayName || CurrentViewingMenu[SelectedOptionIndex].DisplayName == MusicPlayer[0].DisplayName || CurrentViewingMenu[SelectedOptionIndex].DisplayName == Macro[0].DisplayName;
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
				MainMenu[10].AssociatedBool = PluginConfig.Notifications;
				MainMenu[11].AssociatedBool = PluginConfig.overlay;
				MainMenu[12].AssociatedBool = PluginConfig.tooltips;

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
				Movement[10].AssociatedBool = PluginConfig.joystickfly;

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
				Movement2[12].AssociatedBool = PluginConfig.pullmod;
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
				Visual[10].stringsliderind = PluginConfig.firstperson;

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
				Exploits[3].AssociatedBool = PluginConfig.freezeall;
                Exploits[4].AssociatedBool = PluginConfig.snowballgun;


                Exploits2[0].AssociatedBool = PluginConfig.disablesnowballthrow;
                Exploits2[1].AssociatedBool = PluginConfig.ElfSpammer;
                Exploits2[2].AssociatedBool = PluginConfig.WaterSplash;
				Exploits2[3].AssociatedBool = PluginConfig.spazallropes;

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

                Macro[3].AssociatedBool = PluginConfig.recordmacro;
                Macro[5].AssociatedBool = PluginConfig.autoplayproximity;
                Macro[6].stringsliderind = PluginConfig.autoplaydistance;
                Macro[7].stringsliderind = PluginConfig.macrolerpspeed;

                ColourSettings[0].stringsliderind = PluginConfig.MenuColour;
				ColourSettings[1].stringsliderind = PluginConfig.GhostColour;
				ColourSettings[2].stringsliderind = PluginConfig.BeamColour;
				ColourSettings[3].stringsliderind = PluginConfig.ESPColour;
				ColourSettings[4].stringsliderind = PluginConfig.GhostOpacity;
				ColourSettings[5].stringsliderind = PluginConfig.HitBoxesOpacity;
				ColourSettings[6].stringsliderind = PluginConfig.HitBoxesColour;
				ColourSettings[7].stringsliderind = PluginConfig.PlatformsColour;
				ColourSettings[8].stringsliderind = PluginConfig.TargetIndicatorColour;
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
                Macro[0].StringArray = MacroRecorder.GetMacroFileNames();

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
                            if (activePanel != null) activePanel.RootObject.SetActive(false); // Remove this if you want multiple panels open

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
                            //GhostReactorManager grm = GameObject.Find("GhostReactorManager").GetComponent<GhostReactorManager>();

                            foreach (GhostReactorManager grm in GameObject.FindObjectsByType<GhostReactorManager>(FindObjectsSortMode.None))
                            {
                                GREnemy[] GREntity = GameObject.FindObjectsByType<GREnemy>(FindObjectsSortMode.None);
                                if (PhotonNetwork.InRoom && GREntity != null && GREntity.Length > 0 && grm != null)
                                {
                                    if (!grm.reactor.shiftManager.ShiftActive)
                                        grm.RequestShiftStartAuthority(true);

                                    grm.RequestEnemyHitPlayer(GhostReactor.EnemyType.Chaser, GREntity[0].gameEntity.id, GRPlayer.Get(PhotonNetwork.LocalPlayer.ActorNumber), VRRig.LocalRig.transform.position);
                                    RPCProtection.SkiddedRPCProtection();
                                }
                            }
                               
                        }
                        if (option.AssociatedString == "ghostreviveself")
                        {
                            // Testing
                            foreach (GhostReactorManager grm in GameObject.FindObjectsByType<GhostReactorManager>(FindObjectsSortMode.None))
                            {
                                if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient)
                                {
                                    if (grm != null)
                                    {
                                        if (grm.reactor.shiftManager.ShiftActive)
                                        {
                                            GRReviveStation GRRevive = GameObject.Find("GhostReactorRoot/GhostReactorZone/GRReviveStation").GetComponent<GRReviveStation>();
                                            grm.RequestPlayerRevive(GRRevive, GRPlayer.GetLocal());
                                            RPCProtection.SkiddedRPCProtection();
                                        }
                                        else
                                        {
                                            grm.RequestShiftStartAuthority(false);
                                            RPCProtection.SkiddedRPCProtection();
                                        }
                                    }
                                }
                            }

                            /*GhostReactorManager grm = GameObject.Find("GhostReactorManager").GetComponent<GhostReactorManager>();
                            GRReviveStation GRRevive = GameObject.Find("GhostReactorRoot/GhostReactorZone/GRReviveStation").GetComponent<GRReviveStation>();
                            if (PhotonNetwork.InRoom && GRRevive != null && grm != null)
                            {
                                if (PhotonNetwork.LocalPlayer.IsMasterClient)
                                {
                                    if (!grm.reactor.shiftManager.ShiftActive)
                                        grm.RequestShiftStartAuthority(false);

                                    grm.RequestPlayerRevive(GRRevive, GRPlayer.Get(PhotonNetwork.LocalPlayer.ActorNumber));

                                    RPCProtection.SkiddedRPCProtection();
                                }
                            }*/
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
                        if (option.AssociatedString == "join CCMV3 Only")
                        {
                            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom("@CCMV3@", JoinType.Solo);
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
                            Plugin.test.StartCoroutine(Music.LoadMusic($"{Configs.musicPath}\\{MusicPlayer[0].StringArray[MusicPlayer[0].stringsliderind]}.mp3"));
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

                                Plugin.test.StartCoroutine(Music.LoadMusic($"{Configs.musicPath}\\{randomFile}.mp3"));
                            }
                        }

                        // Macro
                        if(option.AssociatedString == "loadmacro")
                        {
                            MacroRecorder.StartPlayback(Macro[0].stringsliderind);
                        }
                        if (option.AssociatedString == "stopmacro")
                        {
                            MacroRecorder.StopPlayback();
                        }
                        if (option.AssociatedString == "deletemacro")
                        {
                            MacroRecorder.DeleteMacro();
                        }


                        // Settings
                        if (option.AssociatedString == "logout")
                        {
                            Notifacations.SendNotification($"<color=red>[LOGOUT]</color> {PhotonNetwork.LocalPlayer.NickName} Why did you try ?? its free");
                            PhotonNetwork.Disconnect();
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
                        case 5:
                            MenuColour = "black";
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

                    Camera specificCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
                    int layerA = 25;
                    int layerB = 16;
                    int maskA = 1 << layerA;
                    int maskB = 1 << layerB; 
                    switch (PluginConfig.AntiScreenShare)
                    {
                        case 0:
                            specificCamera.cullingMask &= ~(maskA | maskB); 

                            if (MenuHub != null && Overlay.OverlayHub != null && Overlay.OverlayHubRoom != null && Notifacations.NotiHub != null)
                            {
                                MenuHub.layer = 0;
                                Overlay.OverlayHub.layer = 0;
                                Overlay.OverlayHubRoom.layer = 0;
                                Notifacations.NotiHub.layer = 0;
                            }
                            break;

                        case 1:
                            specificCamera.cullingMask |= maskA;   
                            specificCamera.cullingMask &= ~maskB;  
                            if (MenuHub != null && Overlay.OverlayHub != null && Overlay.OverlayHubRoom != null && Notifacations.NotiHub != null)
                            {
                                MenuHub.layer = layerA;
                                Overlay.OverlayHub.layer = layerA;
                                Overlay.OverlayHubRoom.layer = layerA;
                                Notifacations.NotiHub.layer = layerA;
                            }
                            break;

                        case 2:
                            specificCamera.cullingMask |= maskB;   
                            specificCamera.cullingMask &= ~maskA;  
                            if (MenuHub != null && Overlay.OverlayHub != null && Overlay.OverlayHubRoom != null && Notifacations.NotiHub != null)
                            {
                                MenuHub.layer = layerB;
                                Overlay.OverlayHub.layer = layerB;
                                Overlay.OverlayHubRoom.layer = layerB;
                                Notifacations.NotiHub.layer = layerB;
                            }
                            break;
                    }

                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log("[DEBUG] Colossal : " + e.Message);
            }
        }
    }
}