using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using TMPro;
using GorillaNetworking;
using Colossal.Menu;
using HarmonyLib;
using static GorillaTagCompetitiveServerApi;

namespace Colossal.Mods
{
    public class NameTags : MonoBehaviour
    {
        private HashSet<string> requestedIds = new HashSet<string>();
        private Dictionary<VRRig, TextMeshPro> clonedTextComponents = new Dictionary<VRRig, TextMeshPro>();
        private Vector3 height;
        private Vector3 size;
        private Color colour;

        private void Start()
        {
            if (PluginConfig.NameTags && PhotonNetwork.InRoom)
            {
                StartCoroutine(FetchCompetitiveDataCoroutine());
            }
        }

        private IEnumerator FetchCompetitiveDataCoroutine()
        {
            yield return RankedInfo.FetchCompetitiveDataAsync();
        }

        public void Update()
        {
            if (PluginConfig.NameTags)
            {
                switch (PluginConfig.nametagheight)
                {
                    case 0: // Chest
                        height = new Vector3(25.30f, 25.00f, 0f);
                        break;
                    case 1: // Above Head
                        height = new Vector3(25.30f, 220.00f, 0f);
                        break;
                }

                switch (PluginConfig.nametagsize)
                {
                    case 0: // Chest size
                        size = new Vector3(1, 1, 1);
                        break;
                    case 1: // Small
                        size = new Vector3(3f, 3, 3);
                        break;
                    case 2: // Medium
                        size = new Vector3(4f, 4, 4);
                        break;
                    case 3: // Large
                        size = new Vector3(5f, 5, 5);
                        break;
                }

                switch (PluginConfig.nametagcolour)
                {
                    case 0:
                        colour = Color.white;
                        break;
                    case 1:
                        colour = Color.yellow;
                        break;
                    case 2:
                        colour = Color.green;
                        break;
                    case 3:
                        colour = Color.blue;
                        break;
                    case 4:
                        colour = Color.red;
                        break;
                    case 5:
                        colour = Color.cyan;
                        break;
                    case 6:
                        colour = Color.black;
                        break;
                }

                if (PhotonNetwork.InRoom)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (!vrrig.isOfflineVRRig)
                        {
                            TextMeshPro nameTagText = GetOrCreateClonedText(vrrig);

                            AntiScreenShare.SetAntiScreenShareLayer(nameTagText.gameObject);

                            if (!vrrig.Creator.GetPlayerRef().CustomProperties.ContainsValue(ThisGuyIsUsingColossal.ccmprefix))
                            {
                                if (nameTagText.color != colour)
                                {
                                    nameTagText.color = colour;
                                }
                            }

                            if (nameTagText.transform.localPosition != height)
                                nameTagText.transform.localPosition = height;

                            if (PluginConfig.nametagheight == 1)
                            {
                                Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
                                nameTagText.transform.rotation = rotation;

                                float dist = Vector3.Distance(Camera.main.transform.position, vrrig.transform.position);
                                float multiplier = Mathf.Clamp(dist / 10f, 0.5f, 3f);
                                nameTagText.transform.localScale = size * multiplier;
                            }
                            else
                            {
                                nameTagText.transform.rotation = vrrig.transform.rotation;
                                nameTagText.transform.localScale = size;
                            }


                            if (PluginConfig.AlwaysVisible)
                            {
                                if (nameTagText.fontMaterial.shader != Shader.Find("GUI/Text Shader"))
                                    nameTagText.fontMaterial.shader = Shader.Find("GUI/Text Shader");
                            }
                            else if (nameTagText.fontMaterial.shader == Shader.Find("GUI/Text Shader"))
                                nameTagText.fontMaterial.shader = Shader.Find("TextMeshPro/Distance Field");

                            UpdateCreationDateTag(vrrig, nameTagText);
                            UpdateTag(vrrig, nameTagText, "Colour", PluginConfig.ShowColourCode, () => $"{vrrig.playerColor.r * 9}{vrrig.playerColor.g * 9}{vrrig.playerColor.b * 9}");
                            UpdateDistanceTag(vrrig, nameTagText);
                            UpdateEloTag(vrrig, nameTagText);
                            UpdateTierTag(vrrig, nameTagText);
                            UpdatePlatformTag(vrrig, nameTagText);
                        }
                    }
                }
            }
            else
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (!vrrig.isOfflineVRRig)
                        {
                            requestedIds.Remove(vrrig.Creator.UserId);

                            if (clonedTextComponents.ContainsKey(vrrig))
                            {
                                if (clonedTextComponents[vrrig].fontMaterial.shader == Shader.Find("GUI/Text Shader"))
                                    clonedTextComponents[vrrig].fontMaterial.shader = Shader.Find("TextMeshPro/Distance Field");

                                Destroy(clonedTextComponents[vrrig].gameObject);
                                clonedTextComponents.Remove(vrrig);
                            }
                        }
                    }
                }
                else
                {
                    RankedInfo.ClearData();
                }

                requestedIds.Clear();
                Destroy(this);
            }
        }

        private TextMeshPro GetOrCreateClonedText(VRRig vrrig)
        {
            if (clonedTextComponents.TryGetValue(vrrig, out TextMeshPro existingText))
            {
                return existingText;
            }

            GameObject nametagClone = Instantiate(vrrig.playerText1.gameObject);

            nametagClone.transform.SetParent(vrrig.playerText1.transform.parent, false);

            nametagClone.transform.localPosition = Vector3.zero;
            nametagClone.transform.localRotation = Quaternion.identity;
            nametagClone.transform.localScale = Vector3.one;

            TextMeshPro text = nametagClone.GetComponent<TextMeshPro>();
            //text.alignment = TextAlignmentOptions.Center;
            text.fontSize = 1;
            text.text = $"{vrrig.OwningNetPlayer.NickName}";

            clonedTextComponents[vrrig] = text;
            return text;
        }


        private void UpdateFPSTag(VRRig vrrig, TextMeshPro nameTagText)
        {
            if (PluginConfig.ShowFPS)
            {
                string fps = Traverse.Create(vrrig).Field("fps").GetValue().ToString();
                AddOrUpdateLine(vrrig, nameTagText, "FPS", fps);
            }
            else
            {
                RemoveLine(vrrig, nameTagText, "FPS");
            }
        }

        private void UpdatePlatformTag(VRRig vrrig, TextMeshPro nameTagText)
        {
            if (PluginConfig.showplatform)
            {
                string platform = "Quest";

                if (vrrig.concatStringOfCosmeticsAllowed.Contains("S. FIRST LOGIN"))
                {
                    platform = "Steam";
                }
                else if (vrrig.concatStringOfCosmeticsAllowed.Contains("FIRST LOGIN") || vrrig.OwningNetPlayer.GetPlayerRef().CustomProperties.Count < 2)
                {
                    platform = "QuestPC";
                }
                else
                {
                    platform = "Quest";
                }
                AddOrUpdateLine(vrrig, nameTagText, "Platform", platform);
            }
            else
            {
                RemoveLine(vrrig, nameTagText, "Platform");
            }
        }

        private async void UpdateCreationDateTag(VRRig vrrig, TextMeshPro nameTagText)
        {
            if (PluginConfig.ShowCreationDate)
            {
                if (!requestedIds.Contains(vrrig.Creator.UserId))
                {
                    requestedIds.Add(vrrig.Creator.UserId);
                    string creationDate = await CreationDate.GetCreationDateAsync(vrrig);
                    AddOrUpdateLine(vrrig, nameTagText, "Creation", creationDate);
                }
            }
            else
            {
                requestedIds.Remove(vrrig.Creator.UserId);
                RemoveLine(vrrig, nameTagText, "Creation");
            }
        }

        private void UpdateDistanceTag(VRRig vrrig, TextMeshPro nameTagText)
        {
            if (PluginConfig.ShowDistance)
            {
                string distanceValue = $"[{(int)Vector3.Distance(vrrig.transform.position, Camera.main.transform.position)}M]";
                AddOrUpdateDistanceLine(vrrig, nameTagText, distanceValue);
            }
            else
            {
                RemoveDistanceLine(vrrig, nameTagText);
            }
        }

        private void UpdateEloTag(VRRig vrrig, TextMeshPro nameTagText)
        {
            if (PluginConfig.showelo)
            {
                string userId = vrrig.Creator.UserId;
                var playerTierData = RankedInfo.GetPlayerTierData();
                if (playerTierData.TryGetValue(userId, out var tierData))
                {
                    string pcElo = tierData.pcData != null ? $"PC({(int)tierData.pcData.elo})" : "PC(N/A)";
                    string questElo = tierData.questData != null ? $"Q({(int)tierData.questData.elo})" : "Q(N/A)";
                    AddOrUpdateLine(vrrig, nameTagText, "Elo", $"{pcElo} {questElo}");
                }
            }
            else
            {
                RemoveLine(vrrig, nameTagText, "Elo");
            }
        }

        private void UpdateTierTag(VRRig vrrig, TextMeshPro nameTagText)
        {
            if (PluginConfig.showelo)
            {
                string userId = vrrig.Creator.UserId;
                var playerTierData = RankedInfo.GetPlayerTierData();
                if (playerTierData.TryGetValue(userId, out var tierData))
                {
                    string pcTier = tierData.pcData != null ? $"PC({tierData.pcData.majorTier}.{tierData.pcData.minorTier})" : "PC(N/A)";
                    string questTier = tierData.questData != null ? $"Q({tierData.pcData.majorTier}.{tierData.pcData.minorTier})" : "Q(N/A)";
                    AddOrUpdateLine(vrrig, nameTagText, "Tier", $"{pcTier} {questTier}");
                }
            }
            else
            {
                RemoveLine(vrrig, nameTagText, "Tier");
            }
        }

        private void AddOrUpdateDistanceLine(VRRig vrrig, TextMeshPro nameTagText, string value)
        {
            string[] lines = nameTagText.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> newLines = new List<string>();
            bool distanceFound = false;

            foreach (string line in lines)
            {
                if (line.StartsWith("[") && line.EndsWith("M]"))
                {
                    newLines.Add(value);
                    distanceFound = true;
                }
                else
                {
                    newLines.Add(line);
                }
            }

            if (!distanceFound)
            {
                newLines.Add(value);
            }

            nameTagText.text = string.Join("\n", newLines);
        }

        private void RemoveDistanceLine(VRRig vrrig, TextMeshPro nameTagText)
        {
            string[] lines = nameTagText.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> newLines = new List<string>();

            foreach (string line in lines)
            {
                if (!(line.StartsWith("[") && line.EndsWith("M]")))
                {
                    newLines.Add(line);
                }
            }

            nameTagText.text = string.Join("\n", newLines);
        }

        private void UpdateTag(VRRig vrrig, TextMeshPro nameTagText, string tagName, bool shouldShow, Func<string> getValue)
        {
            if (shouldShow)
            {
                AddOrUpdateLine(vrrig, nameTagText, tagName, getValue());
            }
            else
            {
                RemoveLine(vrrig, nameTagText, tagName);
            }
        }

        private void AddOrUpdateLine(VRRig vrrig, TextMeshPro nameTagText, string tagName, string value)
        {
            string[] lines = nameTagText.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> newLines = new List<string>();
            bool tagFound = false;

            foreach (string line in lines)
            {
                if (line.StartsWith(tagName + ":"))
                {
                    newLines.Add($"{tagName}: {value}");
                    tagFound = true;
                }
                else
                {
                    newLines.Add(line);
                }
            }

            if (!tagFound)
            {
                newLines.Add($"{tagName}: {value}");
            }

            nameTagText.text = string.Join("\n", newLines);
        }

        private void RemoveLine(VRRig vrrig, TextMeshPro nameTagText, string tagName)
        {
            string[] lines = nameTagText.text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> newLines = new List<string>();

            foreach (string line in lines)
            {
                if (!line.StartsWith(tagName + ":"))
                {
                    newLines.Add(line);
                }
            }

            nameTagText.text = string.Join("\n", newLines);
        }

        private void OnDestroy()
        {
            foreach (var pair in clonedTextComponents)
            {
                if (pair.Value != null)
                {
                    Destroy(pair.Value.gameObject);
                }
            }
            clonedTextComponents.Clear();
            requestedIds.Clear();
        }
    }
}