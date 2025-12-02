﻿using Colossal.Menu;
using GorillaTag;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Colossal.Mods
{
    public class BoneESP : MonoBehaviour
    {
        private Color espcolor;
        private readonly int[] bones = new int[]
        {
            4, 3, 5, 4, 19, 18, 20, 19, 3, 18, 21, 20, 22, 21,
            25, 21, 29, 21, 31, 29, 27, 25, 24, 22, 6, 5, 7, 6,
            10, 6, 14, 6, 16, 14, 12, 10, 9, 7
        };

        // Dictionary to store LineRenderers for each VRRig
        private Dictionary<VRRig, (LineRenderer headLineRenderer, LineRenderer[] boneLineRenderers)> rigRenderers = new Dictionary<VRRig, (LineRenderer, LineRenderer[])>();

        public void Update()
        {
            if (!PluginConfig.boneesp)
            {
                Destroy(this);
                return;
            }

            // Set ESP color based on configuration
            switch (PluginConfig.ESPColour)
            {
                case 0: espcolor = new Color(0.6f, 0f, 0.8f, 0.4f); break;
                case 1: espcolor = new Color(1f, 0f, 0f, 0.4f); break;
                case 2: espcolor = new Color(1f, 1f, 0f, 0.4f); break;
                case 3: espcolor = new Color(0f, 1f, 0f, 0.4f); break;
                case 4: espcolor = new Color(0f, 0f, 1f, 0.4f); break;
                default: espcolor = new Color(0.6f, 0f, 0.8f, 0.4f); break;
            }

            // Track VRRigs that are still present
            var currentRigs = new HashSet<VRRig>(GorillaParent.instance.vrrigs);
            var rigsToRemove = new List<VRRig>();

            // Remove LineRenderers for VRRigs that are no longer present
            foreach (var rig in rigRenderers.Keys)
            {
                if (!currentRigs.Contains(rig))
                {
                    rigsToRemove.Add(rig);
                }
            }

            foreach (var rig in rigsToRemove)
            {
                DestroyRenderersForRig(rig);
                rigRenderers.Remove(rig);
            }

            // Process each VRRig
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig.isOfflineVRRig)
                    continue;

                Color color = WhatAmI.IsInfected(vrrig.Creator) ? new Color(1f, 0f, 0f, 0.4f) : espcolor;

                // Initialize LineRenderers for this VRRig if not already done
                if (!rigRenderers.ContainsKey(vrrig))
                {
                    InitializeRenderersForRig(vrrig);
                }

                var (headLineRenderer, boneLineRenderers) = rigRenderers[vrrig];

                // Update head LineRenderer
                headLineRenderer.startColor = color;
                headLineRenderer.endColor = color;
                headLineRenderer.SetPosition(0, vrrig.head.rigTarget.transform.position + new Vector3(0f, 0.16f, 0f));
                headLineRenderer.SetPosition(1, vrrig.head.rigTarget.transform.position - new Vector3(0f, 0.4f, 0f));

                // Update bone LineRenderers
                for (int i = 0; i < bones.Length; i += 2)
                {
                    int rendererIndex = i / 2;
                    boneLineRenderers[rendererIndex].startColor = color;
                    boneLineRenderers[rendererIndex].endColor = color;
                    boneLineRenderers[rendererIndex].SetPosition(0, vrrig.mainSkin.bones[bones[i]].position);
                    boneLineRenderers[rendererIndex].SetPosition(1, vrrig.mainSkin.bones[bones[i + 1]].position);
                }
            }
        }

        private void InitializeRenderersForRig(VRRig vrrig)
        {
            // Initialize head LineRenderer
            LineRenderer headLineRenderer = vrrig.head.rigTarget.gameObject.GetComponent<LineRenderer>();
            if (headLineRenderer == null)
            {
                headLineRenderer = vrrig.head.rigTarget.gameObject.AddComponent<LineRenderer>();
                headLineRenderer.startWidth = 0.055f;
                headLineRenderer.endWidth = 0.055f;
                headLineRenderer.material.shader = Shader.Find("GUI/Text Shader");
            }

            // Initialize bone LineRenderers
            LineRenderer[] boneLineRenderers = new LineRenderer[bones.Length / 2];
            for (int i = 0; i < bones.Length; i += 2)
            {
                int rendererIndex = i / 2;
                boneLineRenderers[rendererIndex] = vrrig.mainSkin.bones[bones[i]].gameObject.GetComponent<LineRenderer>();
                if (boneLineRenderers[rendererIndex] == null)
                {
                    boneLineRenderers[rendererIndex] = vrrig.mainSkin.bones[bones[i]].gameObject.AddComponent<LineRenderer>();
                    boneLineRenderers[rendererIndex].startWidth = 0.055f;
                    boneLineRenderers[rendererIndex].endWidth = 0.055f;
                    boneLineRenderers[rendererIndex].material.shader = Shader.Find("GUI/Text Shader");
                }
            }

            rigRenderers[vrrig] = (headLineRenderer, boneLineRenderers);
        }

        private void DestroyRenderersForRig(VRRig vrrig)
        {
            if (rigRenderers.TryGetValue(vrrig, out var renderers))
            {
                if (renderers.headLineRenderer != null)
                    Destroy(renderers.headLineRenderer);

                if (renderers.boneLineRenderers != null)
                {
                    foreach (var renderer in renderers.boneLineRenderers)
                    {
                        if (renderer != null)
                            Destroy(renderer);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            // Clean up all LineRenderers when the component is destroyed
            foreach (var rig in rigRenderers.Keys)
            {
                DestroyRenderersForRig(rig);
            }
            rigRenderers.Clear();
        }
    }
}