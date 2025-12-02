﻿using System.Collections.Generic;
using Colossal.Menu;
using Colossal.Patches;
using ColossalV2.Mods;
using GorillaGameModes;
using Photon.Pun;
using PlayFab.GroupsModels;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Colossal.Mods
{
    public class TagAura : MonoBehaviour
    {
        private static Dictionary<Transform, GameObject> targetIndicators = new Dictionary<Transform, GameObject>();
        private float ammount;

        private void Start()
        {
            ammount = GetAmmountFromConfig();
        }

        public void Update()
        {
            if (PluginConfig.tagaura == 0)
            {
                ClearAllIndicators();
                Destroy(this);
                return;
            }

            float newAmmount = GetAmmountFromConfig();
            if (newAmmount != ammount)
            {
                ammount = newAmmount;
                ClearAllIndicators();
            }

            if (PhotonNetwork.InRoom && WhatAmI.IsInfected(PhotonNetwork.LocalPlayer))
            {
                UpdateIndicators();
            }
            else
            {
                ClearAllIndicators();
            }
        }

        private void UpdateIndicators()
        {
            List<Transform> toRemove = new List<Transform>();

            // Check existing indicators
            foreach (var kvp in targetIndicators)
            {
                if (kvp.Key == null || !kvp.Key.gameObject.activeInHierarchy || WhatAmI.IsInfected(kvp.Key.GetComponent<VRRig>().Creator))
                {
                    toRemove.Add(kvp.Key);
                }
            }

            // Remove invalid or tagged indicators
            foreach (var transform in toRemove)
            {
                TargetIndicator.Destroy3D(targetIndicators[transform]);
                targetIndicators.Remove(transform);
            }

            // Update indicators for all players
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig == null || vrrig.isMyPlayer || WhatAmI.IsInfected(vrrig.Creator))
                {
                    // Remove indicator if player is tagged
                    if (targetIndicators.ContainsKey(vrrig.transform))
                    {
                        TargetIndicator.Destroy3D(targetIndicators[vrrig.transform]);
                        targetIndicators.Remove(vrrig.transform);
                    }
                    continue;
                }

                float distance = Vector3.Distance(GorillaTagger.Instance.offlineVRRig.transform.position, vrrig.transform.position);
                float indicatorThreshold = GorillaGameManager.instance.tagDistanceThreshold / ammount * 1.5f;
                float tagThreshold = GorillaGameManager.instance.tagDistanceThreshold / ammount;

                bool shouldHaveIndicator = distance <= indicatorThreshold;
                bool hasIndicator = targetIndicators.ContainsKey(vrrig.transform);

                if (shouldHaveIndicator && !hasIndicator)
                {
                    GameObject newIndicator = TargetIndicator.Create3D(vrrig.nameTagAnchor.transform, true);
                    if (newIndicator != null)
                    {
                        targetIndicators[vrrig.transform] = newIndicator;
                    }
                }
                else if (!shouldHaveIndicator && hasIndicator)
                {
                    TargetIndicator.Destroy3D(targetIndicators[vrrig.transform]);
                    targetIndicators.Remove(vrrig.transform);
                }

                if (distance <= tagThreshold)
                {
                    GameMode.ReportTag(vrrig.Creator);
                }
            }
        }

        private void ClearAllIndicators()
        {
            foreach (var indicator in targetIndicators.Values)
            {
                TargetIndicator.Destroy3D(indicator);
            }
            targetIndicators.Clear();
        }

        private float GetAmmountFromConfig()
        {
            switch (PluginConfig.tagaura)
            {
                case 1: return 4.5f;
                case 2: return 4f;
                case 3: return 3.5f;
                case 4: return 3f;
                case 5: return 2.5f;
                case 6: return 2f;
                case 7: return 1f;
                default: return 0f;
            }
        }
    }
}