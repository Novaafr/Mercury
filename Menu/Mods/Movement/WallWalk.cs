﻿using Colossal.Menu;
using Colossal.Patches;
using UnityEngine;

namespace Colossal.Mods
{
    public class WallWalk : MonoBehaviour
    {
        private Vector3 surfaceNormal;
        private Vector3 velocityAdjustment;
        private float surfaceDistance;
        private LayerMask walkableLayers;
        private bool isWallWalking;
        private float maxDistance = 1f;
        private float wallWalkStrength;

        private static readonly float[] wallWalkAmounts = { 0f, 6.8f, 7f, 7.5f, 7.8f, 8f, 8.5f, 8.8f, 9f, 9.5f, 9.8f };

        private void Start()
        {
            // Define walkable layers (adjust based on your game's layer setup)
            walkableLayers = LayerMask.GetMask("Default", "Environment"); // Replace with your walkable surface layers
        }

        private void FixedUpdate()
        {
            // Check if GorillaTagger or its components are null
            if (GorillaTagger.Instance == null || GorillaTagger.Instance.bodyCollider == null || GorillaTagger.Instance.bodyCollider.attachedRigidbody == null)
            {
                Destroy(this);
                return;
            }

            // Check wall walk setting
            int wallWalkSetting = PluginConfig.wallwalk;
            if (wallWalkSetting == 0)
            {
                ResetGravity();
                Destroy(this);
                return;
            }

            // Set wall walk strength
            wallWalkStrength = wallWalkAmounts[Mathf.Min(wallWalkSetting, wallWalkAmounts.Length - 1)];

            // Check if wall walk is bound and active
            string bind = CustomBinding.GetBinds("wallwalk");
            if (string.IsNullOrEmpty(bind) || bind == "UNBOUND" || !ControlsV2.GetControl(bind))
            {
                ResetGravity();
                return;
            }

            // Perform raycasts from both hands
            RaycastHit hitRight, hitLeft;
            bool didHitRight = Physics.Raycast(
                GorillaTagger.Instance.rightHandTransform.position,
                -GorillaTagger.Instance.rightHandTransform.up,
                out hitRight,
                maxDistance,
                walkableLayers
            );
            bool didHitLeft = Physics.Raycast(
                GorillaTagger.Instance.leftHandTransform.position,
                -GorillaTagger.Instance.leftHandTransform.up,
                out hitLeft,
                maxDistance,
                walkableLayers
            );

            // Determine closest surface
            if (didHitRight || didHitLeft)
            {
                isWallWalking = true;
                GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = false; // Disable gravity while wall-walking

                if (didHitRight && didHitLeft)
                {
                    // Choose the closer surface
                    if (hitRight.distance < hitLeft.distance)
                    {
                        surfaceNormal = hitRight.normal;
                        surfaceDistance = hitRight.distance;
                    }
                    else
                    {
                        surfaceNormal = hitLeft.normal;
                        surfaceDistance = hitLeft.distance;
                    }
                }
                else if (didHitRight)
                {
                    surfaceNormal = hitRight.normal;
                    surfaceDistance = hitRight.distance;
                }
                else
                {
                    surfaceNormal = hitLeft.normal;
                    surfaceDistance = hitLeft.distance;
                }

                // Apply velocity to pull player toward the surface
                if (surfaceDistance < maxDistance)
                {
                    // Invert the surface normal to pull toward the wall
                    Vector3 directionToSurface = -surfaceNormal;
                    // Scale the force based on distance for smoother attraction
                    float distanceFactor = 1f - (surfaceDistance / maxDistance); // Closer = stronger pull
                    velocityAdjustment = directionToSurface * (wallWalkStrength * distanceFactor * Time.fixedDeltaTime);
                    Rigidbody rb = GorillaTagger.Instance.bodyCollider.attachedRigidbody;
                    rb.velocity += velocityAdjustment; // Apply the adjusted velocity
                }
            }
            else
            {
                ResetGravity();
            }
        }

        private void ResetGravity()
        {
            // Revert to normal gravity when not wall-walking
            if (isWallWalking && GorillaTagger.Instance != null && GorillaTagger.Instance.bodyCollider != null)
            {
                GorillaTagger.Instance.bodyCollider.attachedRigidbody.useGravity = true;
                isWallWalking = false;
            }
        }

        private void OnDestroy()
        {
            // Ensure gravity is re-enabled when the component is destroyed
            ResetGravity();
        }
    }
}