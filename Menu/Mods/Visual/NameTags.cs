using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Photon.Pun;
using TMPro;
using GorillaNetworking;
using Mercury.Menu;
using HarmonyLib;
using static GorillaTagCompetitiveServerApi;

namespace Mercury.Mods
{
    public class NameTags : MonoBehaviour
    {
        // Doing it this way because im bored 

        private Dictionary<VRRig, TextMeshPro> clonedTextComponents = new Dictionary<VRRig, TextMeshPro>();
        private Dictionary<VRRig, List<string>> lineData = new Dictionary<VRRig, List<string>>();
        private Dictionary<VRRig, StringBuilder> liners = new Dictionary<VRRig, StringBuilder>();
        private Color nametagcolor = Color.white;

        public void Update()
        {
            if (!PluginConfig.NameTags)
            {
                Clear();
                Destroy(this);
                return;
            }

            if (!PhotonNetwork.InRoom)
            {
                Clear();
                return;
            }

            HashSet<VRRig> activeRigs = new HashSet<VRRig>(GorillaParent.instance.vrrigs);

            List<VRRig> toRemove = new List<VRRig>();
            foreach (var rig in clonedTextComponents.Keys)
            {
                if (rig == null || !activeRigs.Contains(rig))
                    toRemove.Add(rig);
            }

            foreach (VRRig rig in toRemove)
                RemoveRig(rig);

            switch (PluginConfig.nametagcolour)
            {
                case 0: nametagcolor = Color.white; break;
                case 1: nametagcolor = Color.yellow; break;
                case 2: nametagcolor = Color.green; break;
                case 3: nametagcolor = Color.blue; break;
                case 4: nametagcolor = Color.red; break;
                case 5: nametagcolor = Color.cyan; break;
                case 6: nametagcolor = Color.black; break;
            }
            foreach (VRRig rig in GorillaParent.instance.vrrigs)
            {
                if (rig != null && rig != VRRig.LocalRig)
                {
                    TextMeshPro tag = CreateTag(rig);
                    tag.richText = true;
                    tag.color = nametagcolor;
                    UpdateTag(rig, tag);
                }
            }
        }

        private TextMeshPro CreateTag(VRRig rig)
        {
            if (clonedTextComponents.TryGetValue(rig, out TextMeshPro tag) && tag != null)
                return tag;

            GameObject holder = new GameObject("NameTag");
            holder.transform.SetParent(transform);

            tag = holder.AddComponent<TextMeshPro>();
            tag.alignment = TextAlignmentOptions.Center;
            tag.font = GorillaTagger.Instance.offlineVRRig.playerText1.font;
            tag.color = Color.white;
            tag.fontSize = 1.2f;
            tag.richText = true;

            clonedTextComponents[rig] = tag;
            liners[rig] = new StringBuilder(64);
            lineData[rig] = new List<string>();

            return tag;
        }

        public void AddLine(VRRig rig, int index, string text)
        {
            if (!lineData.TryGetValue(rig, out List<string> lines))
            {
                lines = new List<string>();
                lineData[rig] = lines;
            }

            while (lines.Count <= index)
                lines.Add(string.Empty);

            lines[index] = text;
        }

        public void RemoveLine(VRRig rig, int index)
        {
            if (!lineData.TryGetValue(rig, out List<string> lines))
                return;

            if (index < 0 || index >= lines.Count)
                return;

            lines[index] = string.Empty;
        }

        private void UpdateTag(VRRig rig, TextMeshPro tag)
        {
            if (!liners.TryGetValue(rig, out StringBuilder builder))
            {
                builder = new StringBuilder(64);
                liners[rig] = builder;
            }

            builder.Clear();

            AddLine(rig, 0, rig.OwningNetPlayer.NickName);
            if (PluginConfig.ShowColourCode)
            {
                Color playerColor = rig.playerColor;
                string colorText = $"{(int)(playerColor.r * 9)}{(int)(playerColor.g * 9)}{(int)(playerColor.b * 9)}";
                AddLine(rig, 1, colorText);
            }
            else { RemoveLine(rig, 1); }
            if (PluginConfig.ShowDistance)
            {
                float distance = Vector3.Distance(Camera.main.transform.position, rig.headMesh.transform.position);
                string distancestring = $"[{distance.ToString("F1")}]M";
                AddLine(rig, 2, distancestring);
            }
            else { RemoveLine(rig, 2); }
            if (PluginConfig.ShowFPS)
            {
                int fps = rig.fps;
                string fpss = "FPS: " + fps.ToString();
                AddLine(rig, 3, fpss);
            }
            else { RemoveLine(rig, 3); }
            if (PluginConfig.showplatform)
            {
                if (rig.concatStringOfCosmeticsAllowed.Contains("S. FIRST LOGIN"))
                {
                    AddLine(rig, 4, "STEAM");
                }
                else
                {
                    AddLine(rig, 4, "QUEST");
                }
            }
            else { RemoveLine(rig, 4); }

            if (lineData.TryGetValue(rig, out List<string> lines))
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    if (string.IsNullOrEmpty(lines[i]))
                        continue;

                    if (builder.Length > 0)
                        builder.Append('\n');

                    builder.Append(lines[i]);
                }
            }

            tag.text = builder.ToString();

            Transform head = rig.headMesh.transform;
            tag.transform.position = head.position + Vector3.up * 0.65f;
            tag.transform.LookAt(Camera.main.transform);
            tag.transform.Rotate(0f, 180f, 0f);
        }

        private void RemoveRig(VRRig rig)
        {
            if (clonedTextComponents.TryGetValue(rig, out TextMeshPro tag) && tag != null)
                Destroy(tag.gameObject);

            clonedTextComponents.Remove(rig);
            liners.Remove(rig);
            lineData.Remove(rig);
        }

        public void Clear()
        {
            foreach (var tag in clonedTextComponents.Values)
            {
                if (tag != null)
                    Destroy(tag.gameObject);
            }

            clonedTextComponents.Clear();
            liners.Clear();
            lineData.Clear();
        }
    }
}
