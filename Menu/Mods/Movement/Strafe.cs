﻿using Colossal.Menu;
using Colossal.Patches;
using ColossalV2.Mods;
using GorillaExtensions;
using GorillaNetworking;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Colossal.Mods
{
    public class Strafe : MonoBehaviour
    {
        private static readonly float[] speeds = { 6f, 8f, 10f, 12f, 14f, 16f, 18f, 20f };
        public float moveSpeed = 10f;

        private static readonly float[] jumps = { 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f };
        public float jumpForce = 3f;

        public float circleRadius = 0.5f;
        private Rigidbody rb;
        private bool isGrounded;
        private VRRig lockedTarget = null;
        private VRRig lockedTeamTarget = null;
        private float fovAngle = 60f;
        private GameObject targetIndicator;
        private float initialDistanceToTarget = -1f;

        private void Start()
        {
            if (GorillaLocomotion.GTPlayer.Instance == null || GorillaLocomotion.GTPlayer.Instance.bodyCollider == null)
            {
                Destroy(this);
                return;
            }

            rb = GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody;
            if (rb == null)
            {
                Destroy(this);
                return;
            }

            GorillaTagger.Instance.bodyCollider.material.bounciness = 0.3f;
            GorillaTagger.Instance.bodyCollider.material.bounceCombine = PhysicsMaterialCombine.Average;
            GorillaTagger.Instance.bodyCollider.material.dynamicFriction = 0.2f;
        }

        private void Update()
        {
            if (PluginConfig.strafe == 0) // [OFF]
            {
                if (GorillaTagger.Instance != null && GorillaTagger.Instance.bodyCollider != null)
                {
                    GorillaTagger.Instance.bodyCollider.material.bounciness = 0f;
                    GorillaTagger.Instance.bodyCollider.material.bounceCombine = PhysicsMaterialCombine.Average;
                    GorillaTagger.Instance.bodyCollider.material.dynamicFriction = 0.6f;
                }

                NoClip.enabledWithoutInput = false;

                Destroy(this.GetComponent<Strafe>());
                return;
            }

            if (GorillaTagger.Instance == null || VRRig.LocalRig == null)
            {
                return;
            }

            if (Physics.Raycast(GorillaTagger.Instance.bodyCollider.transform.position - new Vector3(0f, 0.2f, 0f),
                               Vector3.down, out RaycastHit hit, 0.5f,
                               GorillaLocomotion.GTPlayer.Instance.locomotionEnabledLayers))
            {
                isGrounded = hit.distance < 0.25f;
            }
            else
            {
                isGrounded = false;
            }


            moveSpeed = speeds[Mathf.Min(PluginConfig.strafespeed, speeds.Length - 1)];
            jumpForce = jumps[Mathf.Min(PluginConfig.strafejumpamount, jumps.Length - 1)];

            string bind = CustomBinding.GetBinds("strafe");
            bool isBindHeld = !string.IsNullOrEmpty(bind) && bind != "UNBOUND" && ControlsV2.GetControl(bind);


            Vector3 moveDirection = Vector3.zero;
            bool shouldStrafe = true;

            switch (PluginConfig.strafe)
            {
                case 1: // Look
                    if (GorillaTagger.Instance.headCollider != null && isBindHeld)
                    {
                        if (isGrounded)
                        {
                            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                        }

                        moveDirection = GorillaTagger.Instance.headCollider.transform.forward.normalized;
                        moveDirection.y = 0f;
                        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
                    }
                    break;

                case 2: // Target
                    if (isBindHeld)
                    {
                        if (lockedTarget == null)
                        {
                            lockedTarget = GetTargetInFOV(false);
                            if (lockedTarget != null)
                            {
                                initialDistanceToTarget = Vector3.Distance(VRRig.LocalRig.transform.position, lockedTarget.transform.position);
                            }
                        }

                        if (lockedTarget != null)
                        {
                            NoClip.enabledWithoutInput = true;

                            if (targetIndicator == null)
                                targetIndicator = TargetIndicator.Create3D(lockedTarget.nameTagAnchor.transform, true);

                            moveDirection = GetDirectionToLockedTarget(lockedTarget, true);
                            if (moveDirection != Vector3.zero)
                                rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0, moveDirection.z * moveSpeed);
                            else
                                shouldStrafe = false;
                        }
                        else
                        {
                            NoClip.enabledWithoutInput = false;

                            if (targetIndicator != null)
                                TargetIndicator.Destroy3D(targetIndicator);

                            shouldStrafe = false;
                        }
                    }
                    else
                    {
                        shouldStrafe = false;
                        lockedTeamTarget = null;

                        NoClip.enabledWithoutInput = false;

                        if (targetIndicator != null)
                            TargetIndicator.Destroy3D(targetIndicator);
                    }
                    break;

                case 3: // Target [TEAM]
                    if (isBindHeld)
                    {
                        if (lockedTeamTarget == null)
                        {
                            lockedTeamTarget = GetTargetInFOV(true);
                            if (lockedTeamTarget != null)
                            {
                                initialDistanceToTarget = Vector3.Distance(VRRig.LocalRig.transform.position, lockedTeamTarget.transform.position);
                            }
                        }

                        if (lockedTeamTarget != null)
                        {
                            NoClip.enabledWithoutInput = true;

                            if (targetIndicator == null)
                                targetIndicator = TargetIndicator.Create3D(lockedTarget.nameTagAnchor.transform, true);

                            moveDirection = GetDirectionToLockedTarget(lockedTeamTarget, true);
                            if (moveDirection != Vector3.zero)
                                rb.velocity = new Vector3(moveDirection.x * moveSpeed, 0, moveDirection.z * moveSpeed);
                            else
                                shouldStrafe = false;
                        }
                        else
                        {
                            NoClip.enabledWithoutInput = false;

                            if (targetIndicator != null)
                                TargetIndicator.Destroy3D(targetIndicator);

                            shouldStrafe = false;
                        }
                    }
                    else
                    {
                        shouldStrafe = false;
                        lockedTeamTarget = null;

                        NoClip.enabledWithoutInput = false;

                        if (targetIndicator != null)
                            TargetIndicator.Destroy3D(targetIndicator);
                    }
                    break;

                case 4: // L Joystick (Camera-Relative)
                    if (GorillaTagger.Instance.headCollider != null)
                    {
                        if (isGrounded)
                        {
                            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
                        }

                        Vector2 joystickInput = ControlsV2.LeftJoystickAxis();
                        if (joystickInput.magnitude >= 0.1f)
                        {
                            Vector3 forward = GorillaTagger.Instance.headCollider.transform.forward;
                            forward.y = 0f;
                            forward = forward.normalized;
                            Vector3 right = GorillaTagger.Instance.headCollider.transform.right;
                            right.y = 0f;
                            right = right.normalized;

                            moveDirection = (forward * joystickInput.y + right * joystickInput.x).normalized;
                            rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);
                        }
                        else
                            shouldStrafe = false;
                    }
                    break;
            }
        }

        private VRRig GetTargetInFOV(bool oppositeTeam)
        {
            if (GorillaParent.instance == null || GorillaParent.instance.vrrigs == null)
            {
                return null;
            }

            VRRig closestRig = null;
            float smallestAngle = float.MaxValue;
            float maxRange = 10f;
            Vector3 cameraForward = GorillaTagger.Instance.headCollider.transform.forward;
            bool localPlayerInfected = WhatAmI.IsInfected(PhotonNetwork.LocalPlayer);

            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig == null || vrrig.Creator == null) continue;
                if (vrrig.isOfflineVRRig) continue;

                bool rigInfected = WhatAmI.IsInfected(vrrig.Creator);
                bool sameTeam = WhatAmI.IsOnSameTeam(PhotonNetwork.LocalPlayer, vrrig.Creator);

                if (oppositeTeam)
                {
                    if (GorillaGameManager.instance != null && GorillaGameManager.instance is GorillaTagManager)
                    {
                        if (rigInfected == localPlayerInfected)
                            continue;
                    }
                    else if (sameTeam)
                        continue;
                }

                Vector3 directionToRig = (vrrig.transform.position - GorillaTagger.Instance.headCollider.transform.position).normalized;
                float angle = Vector3.Angle(cameraForward, directionToRig);
                float distance = Vector3.Distance(VRRig.LocalRig.transform.position, vrrig.transform.position);

                if (angle <= fovAngle / 2f && distance <= maxRange && angle < smallestAngle)
                {
                    smallestAngle = angle;
                    closestRig = vrrig;
                }
            }

            return closestRig;
        }

        private Vector3 GetDirectionToLockedTarget(VRRig target, bool circle)
        {
            if (target == null)
                return Vector3.zero;

            Vector3 selfPos = VRRig.LocalRig.transform.position;
            Vector3 targetPos = target.transform.position;

            Vector3 offset = selfPos - targetPos;
            offset.y = 0;

            float currentDistance = offset.magnitude;
            float desiredDistance = initialDistanceToTarget > 0 ? initialDistanceToTarget : circleRadius;
            float distanceError = currentDistance - desiredDistance;

            if (circle)
            {
                Vector3 radialDir = offset.normalized;
                Vector3 tangentDir = Vector3.Cross(radialDir, Vector3.up).normalized;

                // Apply correction toward/away from target to maintain distance
                Vector3 correction = -radialDir * distanceError;

                return (tangentDir + correction).normalized;
            }

            return (targetPos - selfPos).normalized;
        }
    }
}
