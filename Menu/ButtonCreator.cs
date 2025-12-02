﻿using Colossal;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Colossal.Menu
{
    internal class ButtonCreator : MonoBehaviour
    {
        private static Material mat = new Material(Shader.Find("GUI/Text Shader"));

        public static GameObject CreateButton(string name, int buttontype)
        {
            mat.color = new Color(0, 0, 0, 0.4f);

            GameObject ButtonObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ButtonObj.GetComponent<Renderer>().material = mat;
            ButtonObj.name = name;

            Canvas canvas = ButtonObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;

            ButtonObj.AddComponent<CanvasScaler>();
            ButtonObj.AddComponent<GraphicRaycaster>();

            RectTransform rectTransform = ButtonObj.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(5, 5);
            ButtonObj.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

            GameObject buttonTextObj = new GameObject();
            buttonTextObj.transform.SetParent(ButtonObj.transform);
            Text ButtonText = buttonTextObj.AddComponent<Text>();
            ButtonText.fontSize = 10;
            //ButtonText.font = Plugin.gtagfont;
            ButtonText.rectTransform.sizeDelta = new Vector2(260, 160);
            ButtonText.rectTransform.localScale = new Vector3(0.01f, 0.01f, 1f);
            ButtonText.material = mat;
            ButtonText.alignment = TextAnchor.MiddleCenter;

            switch (buttontype)
            {
                // Button to go up
                case 0:
                    ButtonObj.AddComponent<ButtonBehavior>().ButtonType = ButtonType.Up;
                    ButtonText.text = "^";
                    break;

                // Button to go down
                case 1:
                    ButtonObj.AddComponent<ButtonBehavior>().ButtonType = ButtonType.Down;
                    ButtonText.text = "^";
                    buttonTextObj.transform.Rotate(Vector3.forward, 180f);
                    break;

                // Button to select
                case 2:
                    ButtonObj.AddComponent<ButtonBehavior>().ButtonType = ButtonType.Select;
                    ButtonText.text = "O";
                    break;
            }
            BoxCollider collider = ButtonObj.AddComponent<BoxCollider>();
            collider.isTrigger = true;

            return ButtonObj;
        }
    }
    internal class ButtonBehavior : MonoBehaviour
    {
        public ButtonType ButtonType;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("colossal"))
            {
                switch (ButtonType)
                {
                    case ButtonType.Up:
                        Menu.inputcooldown = true;
                        if (Menu.SelectedOptionIndex == 0)
                            Menu.SelectedOptionIndex = Menu.CurrentViewingMenu.Count<MenuOption>() - 1;
                        else
                            Menu.SelectedOptionIndex--;
                        Menu.UpdateMenuState(new MenuOption(), null, null);
                        break;

                    case ButtonType.Down:
                        Menu.inputcooldown = true;
                        if (Menu.SelectedOptionIndex + 1 == Menu.CurrentViewingMenu.Count<MenuOption>())
                            Menu.SelectedOptionIndex = 0;
                        else
                            Menu.SelectedOptionIndex++;
                        Menu.UpdateMenuState(new MenuOption(), null, null);
                        break;

                    case ButtonType.Select:
                        if (Menu.CurrentViewingMenu[Menu.SelectedOptionIndex]._type == "STRINGslider")
                        {
                            if (!Menu.inputcooldown)
                            {
                                if (Menu.CurrentViewingMenu[Menu.SelectedOptionIndex].DisplayName == Menu.Settings[3].DisplayName)
                                {
                                    if (Menu.CurrentViewingMenu[Menu.SelectedOptionIndex].stringsliderind == 0)
                                        Menu.CurrentViewingMenu[Menu.SelectedOptionIndex].stringsliderind = Menu.CurrentViewingMenu[Menu.SelectedOptionIndex].StringArray.Count() + 1;
                                    else
                                        Menu.CurrentViewingMenu[Menu.SelectedOptionIndex].stringsliderind = Menu.CurrentViewingMenu[Menu.SelectedOptionIndex].stringsliderind + 1;
                                    Menu.inputcooldown = true;
                                }
                                else
                                {
                                    foreach (var prop in typeof(PluginConfig).GetFields(BindingFlags.Public | BindingFlags.Static))
                                    {
                                        if (prop.Name.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower() == Menu.CurrentViewingMenu[Menu.SelectedOptionIndex].DisplayName.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower())
                                        {
                                            object currentValue = prop.GetValue(null);
                                            int? currentIntValue = currentValue as int?;

                                            if (currentIntValue.HasValue)
                                            {
                                                int newValue = currentIntValue.Value + 1;
                                                int stringArrayCount = Menu.CurrentViewingMenu[Menu.SelectedOptionIndex].StringArray.Length;

                                                if (newValue >= stringArrayCount)
                                                    newValue = 0;

                                                prop.SetValue(null, newValue);

                                                //CustomConsole.LogToConsole($"\nIncremented {Menu.CurrentViewingMenu[Menu.SelectedOptionIndex].DisplayName} : {newValue}");
                                            }
                                            else
                                                Debug.LogError($"Field '{prop.Name}' is not of type int.");

                                            break;
                                        }
                                    }
                                }

                                Menu.inputcooldown = true;
                            }
                            Menu.UpdateMenuState(new MenuOption(), null, null);
                        }
                        else
                        {
                            Menu.inputcooldown = true;
                            Menu.UpdateMenuState(Menu.CurrentViewingMenu[Menu.SelectedOptionIndex], null, "optionhit");
                        }
                        break;
                }
            }
        }
        private void OnTriggerExit(Collider other) => Menu.inputcooldown = false;
    }
    internal enum ButtonType
    {
        Up,
        Down,
        Select
    }
}
