using BepInEx;
using Mercury.Console;
using Mercury.Patches;
using ExitGames.Client.Photon;
using GorillaNetworking;
using HarmonyLib;
using Photon.Pun;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Mono.Security.X509.X520;

namespace Mercury.Menu
{
    public class ThisGuyIsUsingColossal : MonoBehaviour
    {
        public static string userid;
        public static string ccmprefix;

        public static Color gradientColor;
        private Color[] rainbowColors = new Color[]
        {
            Color.red,
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            new Color(0.5f, 0.0f, 0.5f), // Purple
            Color.red
        };
        private float duration = 5.0f;
        private float timer = 0.0f;

        private Color GetGradientColor(float t)
        {
            int colorCount = rainbowColors.Length;
            float scaledTime = t * (colorCount - 1);
            int colorIndex = Mathf.FloorToInt(scaledTime);
            float lerpFactor = scaledTime - colorIndex;

            return Color.Lerp(rainbowColors[colorIndex], rainbowColors[Mathf.Min(colorIndex + 1, colorCount - 1)], lerpFactor);
        }

        private void Start()
        {
            userid = DevManager.Admins.Keys.ToString();
            ccmprefix = "mercury";
        }

        void Update()
        {
            if (PhotonNetwork.InRoom && GorillaTagger.Instance.myVRRig != null && GorillaTagger.Instance.myVRRig.GetView != null && GorillaTagger.Instance.myVRRig.GetView.Controller != null)
            {
                timer += Time.deltaTime;
                float t = Mathf.PingPong(timer / duration, 1);
                gradientColor = GetGradientColor(t);

                if (PluginConfig.MCMSight == 0)
                {
                    return;
                }

                // Option 1 - Self & Others
                if (PluginConfig.MCMSight == 1)
                {
                    if (GorillaTagger.Instance.myVRRig.GetView.Controller.CustomProperties != null && !GorillaTagger.Instance.myVRRig.GetView.Controller.CustomProperties.ContainsKey(ccmprefix))
                    {
                        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
                            {
                                { ccmprefix, ccmprefix }
                            });
                    }
                }

                // Option 2 - Others (No custom property for self)
                if (PluginConfig.MCMSight == 2)
                {
                    if (GorillaTagger.Instance.myVRRig.GetView.Controller.CustomProperties != null && GorillaTagger.Instance.myVRRig.GetView.Controller.CustomProperties.ContainsKey(ccmprefix))
                    {
                        PhotonNetwork.LocalPlayer.CustomProperties.Remove(new Hashtable
                            {
                                { ccmprefix, ccmprefix }
                            });
                    }
                }

                // Option 3 - Self (Set properties only for self)
                if (PluginConfig.MCMSight == 3)
                {
                    if (GorillaTagger.Instance.myVRRig.GetView.Controller.CustomProperties != null && !GorillaTagger.Instance.myVRRig.GetView.Controller.CustomProperties.ContainsKey(ccmprefix))
                    {
                        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable
                            {
                                { ccmprefix, ccmprefix }
                            });
                    }
                }


                HashSet<VRRig> processedVRRigs = new HashSet<VRRig>();
                if (PluginConfig.MCMSight == 1 || PluginConfig.MCMSight == 2)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig != null && !processedVRRigs.Contains(vrrig))
                        {
                            //if (vrrig.playerText2 != null && vrrig.playerText2.isActiveAndEnabled)
                            //{
                            //    vrrig.playerText2.enabled = false;
                            //}


                            if (vrrig.Creator != null)
                            {
                                if (!userid.IsNullOrWhiteSpace())
                                {
                                    if (userid.Split(',').Any(id => id.Trim().Equals(vrrig.Creator.UserId, System.StringComparison.OrdinalIgnoreCase))) // admin check
                                    {
                                        vrrig.playerText1.color = gradientColor;
                                        vrrig.playerText1.text = "[ADMIN] " + vrrig.Creator.NickName;
                                        //vrrig.playerText2.text = "[ADMIN] " + vrrig.Creator.NickName;
                                        if (PluginConfig.chams && !vrrig.Creator.IsLocal)
                                        {
                                            vrrig.mainSkin.material.color = gradientColor;
                                        }

                                        processedVRRigs.Add(vrrig);
                                        return;
                                    }
                                }

                                if (vrrig.Creator.GetPlayerRef() != null && vrrig.Creator.GetPlayerRef().CustomProperties != null && vrrig.Creator.GetPlayerRef().CustomProperties.ContainsKey(ccmprefix))
                                {
                                    vrrig.playerText1.color = Color.gray;
                                    vrrig.playerText1.text = "[MCM] " + vrrig.Creator.NickName;
                                    //vrrig.playerText2.text = "[CCM] " + vrrig.Creator.NickName;
                                    if (PluginConfig.chams && !vrrig.Creator.IsLocal)
                                    {
                                        vrrig.mainSkin.material.color = new Color(1.0f, 0.0f, 0.6666667f, 0.4f);
                                    }
                                }
                                else if (vrrig.Creator.GetPlayerRef() != null && vrrig.Creator.GetPlayerRef().CustomProperties != null && vrrig.Creator.GetPlayerRef().CustomProperties.ContainsKey("colossal"))
                                {
                                    vrrig.playerText1.color = Color.magenta;
                                    vrrig.playerText1.text = "[CCM -- old mercury] " + vrrig.Creator.NickName;
                                    //vrrig.playerText2.text = "[CCM] " + vrrig.Creator.NickName;
                                    if (PluginConfig.chams && !vrrig.Creator.IsLocal)
                                    {
                                        vrrig.mainSkin.material.color = new Color(1.0f, 0.0f, 0.6666667f, 0.4f);
                                    }
                                }
                            }
                            processedVRRigs.Add(vrrig);
                        }
                    }
                }
            }
        }
    }
    [HarmonyPatch(typeof(GorillaScoreBoard))]
    [HarmonyPatch("RedrawPlayerLines", MethodType.Normal)]
    internal class GorillaScoreBoardRedrawPlayerLines
    {
        private static bool Prefix(GorillaScoreBoard __instance)
        {
            if (PluginConfig.ShowBoards)
            {
                __instance.boardText.text = __instance.GetBeginningString();
                __instance.buttonText.text = "";
                __instance.boardText.richText = true;  // Ensure richText is enabled

                for (int i = 0; i < __instance.lines.Count; ++i)
                {
                    try
                    {
                        if (__instance.lines[i].gameObject.activeInHierarchy)
                        {
                            __instance.lines[i].gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, (float)(__instance.startingYValue - __instance.lineHeight * i), 0f);
                            if (__instance.lines[i].linePlayer != null)
                            {
                                var usrid = __instance.lines[i].linePlayer.UserId;
                                bool isLocalPlaya = __instance.lines[i].linePlayer.IsLocal;

                                TextMeshPro boardText = __instance.boardText;

                                Color gradientColor = Color.magenta;
                                if (ThisGuyIsUsingColossal.gradientColor != null)
                                {
                                    gradientColor = ThisGuyIsUsingColossal.gradientColor;
                                }
                                string colorHex = ColorUtility.ToHtmlStringRGB(gradientColor);

                                if (ThisGuyIsUsingColossal.userid.Split(',').Any(id => id.Trim().Equals(usrid, System.StringComparison.OrdinalIgnoreCase)))
                                {
                                    boardText.text += "\n " + $"<color=#{colorHex}>[Admin] {__instance.NormalizeName(true, __instance.lines[i].linePlayer.NickName)}</color>";
                                }
                                else if (__instance.lines[i].linePlayer.GetPlayerRef().CustomProperties.ContainsKey(ThisGuyIsUsingColossal.ccmprefix))
                                {
                                    boardText.text += "\n " + $"<color=#FF00FF>[CCM] {__instance.NormalizeName(true, __instance.lines[i].linePlayer.NickName)}</color>";
                                }
                                else
                                {
                                    boardText.text += "\n " + __instance.NormalizeName(true, __instance.lines[i].linePlayer.NickName);
                                }

                                if (isLocalPlaya != true)
                                {
                                    if (__instance.lines[i].reportButton.isActiveAndEnabled)
                                    {
                                        __instance.buttonText.text += "MUTE                              DONT NARC\n";
                                    }
                                    else
                                    {
                                        __instance.buttonText.text += "MUTE      HATE SPEECH    TOXICITY      CHEATING      CANCEL\n";
                                    }
                                }
                                else
                                {
                                    __instance.buttonText.text += "\n";
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Error message supposed to be here?!
                    }
                }
                return false;
            }

            return true;
        }
    }
}