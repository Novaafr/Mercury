﻿using System;
using System.Collections.Generic;
using Mercury.Menu;
using Mercury;
using UnityEngine;

namespace MercuryV2.Mods
{
    internal class TargetIndicator
    {
        private static List<GameObject> indicators = new List<GameObject>();
        private static Color indicatorColour;

        // Original method: Create indicator at Transform position
        public static GameObject Create3D(Transform transform, bool parent)
        {
            if (transform == null)
            {
                UnityEngine.Debug.LogError("[TargetIndicator] Transform is null");
                return null;
            }

            GameObject indicator = GameObject.Instantiate(AssetBundleLoader.targetIndicator);
            if (indicator != null)
            {
                indicator.SetActive(true);
                GameObject obj = indicator.transform.GetChild(0).gameObject;
                if (obj != null)
                {
                    switch (PluginConfig.TargetIndicatorColour)
                    {
                        case 0:
                            indicatorColour = new Color32(204, 51, 255, 80);
                            break;
                        case 1:
                            indicatorColour = new Color32(255, 0, 0, 80);
                            break;
                        case 2:
                            indicatorColour = new Color32(255, 255, 0, 80);
                            break;
                        case 3:
                            indicatorColour = new Color32(0, 255, 0, 80);
                            break;
                        case 4:
                            indicatorColour = new Color32(0, 0, 255, 80);
                            break;
                        default:
                            indicatorColour = new Color32(255, 255, 255, 255);
                            break;
                    }

                    obj.GetComponent<Renderer>().material.color = indicatorColour;

                    AntiScreenShare.SetAntiScreenShareLayer(obj);
                }

                indicator.transform.position = transform.position;
                if (parent)
                    indicator.transform.SetParent(transform);

                indicators.Add(indicator);
                UnityEngine.Debug.Log($"[TargetIndicator] Created indicator at Transform position: {transform.position}");
                return indicator;
            }

            UnityEngine.Debug.LogWarning("[TargetIndicator] Failed to instantiate indicator");
            return null;
        }

        // NEW: Create indicator at Vector3 position
        public static GameObject Create3D(Vector3 position, bool parent, Transform parentTransform = null)
        {
            GameObject indicator = GameObject.Instantiate(AssetBundleLoader.targetIndicator);
            if (indicator != null)
            {
                indicator.SetActive(true);
                GameObject obj = indicator.transform.GetChild(0).gameObject;
                if (obj != null)
                {
                    switch (PluginConfig.TargetIndicatorColour)
                    {
                        case 0:
                            indicatorColour = new Color32(204, 51, 255, 80);
                            break;
                        case 1:
                            indicatorColour = new Color32(255, 0, 0, 80);
                            break;
                        case 2:
                            indicatorColour = new Color32(255, 255, 0, 80);
                            break;
                        case 3:
                            indicatorColour = new Color32(0, 255, 0, 80);
                            break;
                        case 4:
                            indicatorColour = new Color32(0, 0, 255, 80);
                            break;
                        case 5:
                            indicatorColour = new Color(0, 0, 0, 80);
                            break;
                        default:
                            indicatorColour = new Color32(255, 255, 255, 255);
                            break;
                    }

                    obj.GetComponent<Renderer>().material.color = indicatorColour;

                    AntiScreenShare.SetAntiScreenShareLayer(obj);
                }

                indicator.transform.position = position;
                if (parent && parentTransform != null)
                    indicator.transform.SetParent(parentTransform);

                indicators.Add(indicator);
                UnityEngine.Debug.Log($"[TargetIndicator] Created indicator at Vector3 position: {position}");
                return indicator;
            }

            UnityEngine.Debug.LogWarning("[TargetIndicator] Failed to instantiate indicator");
            return null;
        }

        public static void Destroy3D(GameObject indicator)
        {
            if (indicators.Contains(indicator))
            {
                indicators.Remove(indicator);
                GameObject.Destroy(indicator);
                UnityEngine.Debug.Log("[TargetIndicator] Destroyed indicator");
            }
            else
            {
                UnityEngine.Debug.LogWarning("[TargetIndicator] Indicator not found in list");
            }
        }
    }
}