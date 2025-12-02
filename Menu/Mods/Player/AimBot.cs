﻿using Colossal.Menu;
using UnityEngine;
using HarmonyLib;
using System;
using System.Collections.Generic;
using Photon.Pun;
using ColossalV2.Mods;
using Colossal.Patches;

namespace Colossal.Mods
{
    public class Aimbot : MonoBehaviour
    {
        // copy pasted from hitboxes!!!!!!! - starry
        public void Update()
        {
            if (PluginConfig.Aimbot == 0)
            {
                Destroy(this.GetComponent<Aimbot>());

                if (AimbotPatch.targetIndicator != null)
                    Destroy(AimbotPatch.targetIndicator);
                return;
            }
        }
    }
}

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(Slingshot), "GetLaunchVelocity", MethodType.Normal)]
    public class AimbotPatch
    {
        public static GameObject targetIndicator;
        private static VRRig lastTargetRig = null;  // Track the last target rig

        static bool Prefix(Slingshot __instance, ref Vector3 __result)
        {
            if (PhotonNetwork.InRoom)
            {
                switch (PluginConfig.Aimbot)
                {
                    case 1:
                        float num = float.PositiveInfinity;
                        VRRig closestRig = null;

                        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                        {
                            if (!vrrig.isOfflineVRRig)
                            {
                                float sqrMagnitude = (vrrig.headMesh.transform.position - __instance.transform.position).sqrMagnitude;
                                if (sqrMagnitude < num)
                                {
                                    if (!WhatAmI.IsOnSameTeam(PhotonNetwork.LocalPlayer, vrrig.Creator))
                                    {
                                        num = sqrMagnitude;
                                        closestRig = vrrig;
                                    }
                                }
                            }
                        }

                        if (closestRig != null)
                        {
                            Vector3 dir = closestRig.headMesh.transform.position - __instance.transform.position;
                            __result = dir * 6;

                            // Check if the target has changed
                            if (closestRig != lastTargetRig)
                            {
                                // If the target changed, destroy the old indicator if it exists
                                if (targetIndicator != null)
                                {
                                    TargetIndicator.Destroy3D(targetIndicator);
                                }

                                // Create a new target indicator for the new target
                                targetIndicator = TargetIndicator.Create3D(closestRig.nameTagAnchor.transform, true);

                                // Update the last target rig
                                lastTargetRig = closestRig;
                            }

                            return false;
                        }
                        break;

                    case 2:
                        float num2 = float.PositiveInfinity;
                        VRRig closestRig2 = null;

                        foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                        {
                            if (!vrrig.isOfflineVRRig)
                            {
                                float sqrMagnitude = (vrrig.headMesh.transform.position - __instance.transform.position).sqrMagnitude;
                                if (sqrMagnitude < num2)
                                {
                                    if (!WhatAmI.IsOnSameTeam(PhotonNetwork.LocalPlayer, vrrig.Creator))
                                    {
                                        num2 = sqrMagnitude;
                                        closestRig2 = vrrig;
                                    }
                                }
                            }
                        }

                        if (closestRig2 != null)
                        {
                            Vector3 currentPosition = closestRig2.headMesh.transform.position;
                            Vector3 currentVelocity = closestRig2.GetComponent<Rigidbody>().velocity;

                            Vector3 futurePosition = currentPosition + (currentVelocity * 5);
                            Vector3 dir = futurePosition - __instance.transform.position;

                            __result = dir * 6;

                            // Check if the target has changed
                            if (closestRig2 != lastTargetRig)
                            {
                                // If the target changed, destroy the old indicator if it exists
                                if (targetIndicator != null)
                                {
                                    TargetIndicator.Destroy3D(targetIndicator);
                                }

                                // Create a new target indicator for the new target
                                targetIndicator = TargetIndicator.Create3D(closestRig2.nameTagAnchor.transform, true);

                                // Update the last target rig
                                lastTargetRig = closestRig2;
                            }

                            return false;
                        }
                        break;
                }
            }

            return true;
        }
    }
}