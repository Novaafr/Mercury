﻿using Colossal.Menu;
using UnityEngine;
using System.Collections.Generic;
using Colossal.Patches;

namespace Colossal.Mods
{
    public class NoClip : MonoBehaviour
    {
        private static readonly List<MeshCollider> colliders = new List<MeshCollider>();
        private static readonly Dictionary<MeshCollider, Vector3> originalScales = new Dictionary<MeshCollider, Vector3>();
        private static readonly Vector3 scaleFactor = new Vector3(0.0001f, 0.0001f, 0.0001f); // 1/10000
        private string bind;
        public static bool enabledWithoutInput = false;

        private void Start()
        {
            // Cache the binding once at start
            bind = CustomBinding.GetBinds("noclip");

            // Early exit if binding is invalid
            if (string.IsNullOrEmpty(bind) || bind == "UNBOUND")
            {
                Destroy(this);
                return;
            }

            // Clear existing collections to avoid duplicates
            colliders.Clear();
            originalScales.Clear();

            // Populate colliders and store their original scales
            var foundColliders = FindObjectsByType<MeshCollider>(FindObjectsSortMode.None);
            if (foundColliders != null)
            {
                foreach (var collider in foundColliders)
                {
                    if (collider != null && collider.transform != null) // Null check for safety
                    {
                        colliders.Add(collider);
                        originalScales[collider] = collider.transform.localScale; // Store original scale
                    }
                }
            }
        }

        private void Update()
        {
            if (!PluginConfig.NoClip)
            {
                ResetColliders();
                Destroy(this);
                return;
            }

            // Skip update if binding is invalid
            if (string.IsNullOrEmpty(bind))
                return;

            var isActive = ControlsV2.GetControl(bind) || enabledWithoutInput;
            foreach (var collider in colliders)
            {
                if (collider != null && collider.transform != null) // Null check for safety
                {
                    // Set to small scale if NoClip is active, otherwise restore original scale
                    collider.transform.localScale = isActive ? scaleFactor : originalScales[collider];
                }
            }
        }

        private void ResetColliders()
        {
            foreach (var collider in colliders)
            {
                if (collider != null && collider.transform != null && originalScales.ContainsKey(collider)) // Null check and key check
                {
                    collider.transform.localScale = originalScales[collider]; // Restore original scale
                }
            }
        }

        private void OnDestroy()
        {
            ResetColliders();
            colliders.Clear();
            originalScales.Clear(); // Clear stored scales
        }
    }
}