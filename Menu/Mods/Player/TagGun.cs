﻿using Colossal.Menu;
using Colossal.Patches;
using ColossalV2.Mods;
using GorillaGameModes;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Colossal.Mods
{
    public class TagGun : MonoBehaviour
    {
        private GameObject pointer;
        private LineRenderer radiusLine;
        private Material lineMaterial = new Material(Shader.Find("GUI/Text Shader"));
        private Vector3 originalPosition;

        private Color beamColour;

        public void Update()
        {
            if (PluginConfig.taggun && PhotonNetwork.InRoom)
            {
                // Set beam color
                switch (PluginConfig.BeamColour)
                {
                    case 0: beamColour = new Color(0.6f, 0f, 0.8f, 0.5f); break; // Purple
                    case 1: beamColour = new Color(1f, 0f, 0f, 0.5f); break;    // Red
                    case 2: beamColour = new Color(1f, 1f, 0f, 0.5f); break;    // Yellow
                    case 3: beamColour = new Color(0f, 1f, 0f, 0.5f); break;    // Green
                    case 4: beamColour = new Color(0f, 0f, 1f, 0.5f); break;    // Blue
                }

                // Initialize pointer if not present
                if (pointer == null)
                {
                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                    UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                    pointer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    pointer.GetComponent<Renderer>().material = new Material(Shader.Find("GUI/Text Shader"));
                    pointer.GetComponent<Renderer>().material.color = beamColour;
                }


                RaycastHit raycastHit;
                LayerMask combinedLayerMask = GorillaLocomotion.GTPlayer.Instance.locomotionEnabledLayers | 16384;
                Physics.Raycast(GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position - GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.up,
                                -GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.up, out raycastHit, float.PositiveInfinity, combinedLayerMask);
                pointer.transform.position = raycastHit.point;


                string bind = CustomBinding.GetBinds("taggun");
                if (string.IsNullOrEmpty(bind) || bind == "UNBOUND")
                {
                    return;
                }


                originalPosition = VRRig.LocalRig.transform.position;


                if (ControlsV2.GetControl(bind))
                {
                    if (radiusLine == null)
                    {
                        lineMaterial.color = beamColour;

                        radiusLine = new GameObject("RadiusLine") { transform = { parent = pointer.transform } }.AddComponent<LineRenderer>();
                        radiusLine.positionCount = 2;
                        radiusLine.startWidth = 0.05f;
                        radiusLine.endWidth = 0.05f;
                        radiusLine.material = lineMaterial;
                        radiusLine.startColor = lineMaterial.color;
                        radiusLine.endColor = lineMaterial.color;
                    }
                    radiusLine.SetPosition(0, raycastHit.point);
                    radiusLine.SetPosition(1, GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position);


                    AntiScreenShare.SetAntiScreenShareLayer(radiusLine.gameObject);


                    VRRig targetRig = null;
                    float tagRadius = GorillaGameManager.instance.tagDistanceThreshold;


                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (vrrig.isMyPlayer) continue;

                        float distanceToRig = Vector3.Distance(raycastHit.point, vrrig.transform.position);
                        if (distanceToRig < 1f)
                        {
                            targetRig = vrrig;
                            break;
                        }
                    }


                    if (targetRig != null)
                    {
                        float distanceToTarget = Vector3.Distance(originalPosition, targetRig.transform.position);

                        if (distanceToTarget <= tagRadius)
                        {
                            GameMode.ReportTag(targetRig.Creator);
                        }
                        else
                        {
                            // Move into tag radius (just inside the threshold)
                            Vector3 directionToTarget = (targetRig.transform.position - originalPosition).normalized;
                            Vector3 newPosition = targetRig.transform.position - (directionToTarget * (tagRadius * 0.9f)); // 90% of radius to ensure inside

                            if (VRRig.LocalRig.enabled)
                                VRRig.LocalRig.enabled = false;
                            VRRig.LocalRig.transform.position = newPosition;

                            // Use HyperSerialization to send this state only to the target
                            //int[] targetActors = new int[] { targetRig.Creator.ActorNumber };
                            //HyperSerialization.HyperSerialize(targetActors);
                            //PhotonNetwork.SendAllOutgoingCommands();

                            GameMode.ReportTag(targetRig.Creator);
                        }
                    }

                    return;
                }

                if (!VRRig.LocalRig.enabled)
                    VRRig.LocalRig.enabled = true;
                if (VRRig.LocalRig.transform.position != originalPosition)
                    VRRig.LocalRig.transform.position = originalPosition;

                if (radiusLine != null)
                {
                    UnityEngine.Object.Destroy(radiusLine);
                    radiusLine = null;
                }
            }
            else
            {
                UnityEngine.Object.Destroy(this.GetComponent<TagGun>());
                if (pointer != null)
                {
                    UnityEngine.Object.Destroy(pointer);
                }
            }
        }
    }
}