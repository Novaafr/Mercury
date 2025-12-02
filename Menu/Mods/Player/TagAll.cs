﻿using Colossal.Menu;
using Colossal.Patches;
using ColossalV2.Mods;
using GorillaGameModes;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace Colossal.Mods
{
    public class TagAll : MonoBehaviour
    {
        private LineRenderer radiusLine;
        private Material lineMaterial = new Material(Shader.Find("GUI/Text Shader"));
        public void Update()
        {
            if (PluginConfig.tagall)
            {
                switch (PluginConfig.BeamColour)
                {
                    case 0:
                        lineMaterial.color = new Color(0.6f, 0f, 0.8f, 0.5f);
                        break;
                    case 1:
                        lineMaterial.color = new Color(1f, 0f, 0f, 0.5f);
                        break;
                    case 2:
                        lineMaterial.color = new Color(1f, 1f, 0f, 0.5f);
                        break;
                    case 3:
                        lineMaterial.color = new Color(0f, 1f, 0f, 0.5f);
                        break;
                    case 4:
                        lineMaterial.color = new Color(0f, 0f, 1f, 0.5f);
                        break;
                }

                if (PhotonNetwork.InRoom)
                {
                    if (WhatAmI.infectionmanager.currentInfected.Count < 10)
                    {
                        if (PhotonNetwork.LocalPlayer.IsMasterClient)
                        {
                            foreach (Photon.Realtime.Player v in PhotonNetwork.PlayerList)
                                WhatAmI.infectionmanager.AddInfectedPlayer(v);

                            PluginConfig.tagall = false;
                            return;
                        }

                        if (WhatAmI.IsInfected(PhotonNetwork.LocalPlayer))
                        {
                            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                            {
                                if (!WhatAmI.IsInfected(vrrig.Creator))
                                {
                                    if (VRRig.LocalRig.enabled)
                                        VRRig.LocalRig.enabled = false;


                                    VRRig.LocalRig.transform.position = vrrig.transform.position + new Vector3(0f, -2, 0f);
                                    GameMode.ReportTag(vrrig.Creator);
                                    //HyperSerialization.HyperSerialize();


                                    if (radiusLine == null)
                                    {
                                        GameObject lineObject = new GameObject("RadiusLine");
                                        lineObject.transform.parent = vrrig.transform;
                                        radiusLine = lineObject.AddComponent<LineRenderer>();
                                        radiusLine.positionCount = 2;
                                        radiusLine.startWidth = 0.05f;
                                        radiusLine.endWidth = 0.05f;
                                        radiusLine.material = lineMaterial;
                                        radiusLine.startColor = lineMaterial.color;
                                        radiusLine.endColor = lineMaterial.color;
                                    }
                                    radiusLine.SetPosition(0, vrrig.transform.position);
                                    radiusLine.SetPosition(1, GorillaTagger.Instance.mainCamera.transform.position);
                                    if (radiusLine.GetPosition(0) == null)
                                    {
                                        if (radiusLine != null)
                                        {
                                            Destroy(radiusLine);
                                            radiusLine = null;
                                        }
                                    }

                                    AntiScreenShare.SetAntiScreenShareLayer(radiusLine.gameObject);
                                }
                            }

                            //PhotonNetwork.SendAllOutgoingCommands();
                        }
                        else
                        {
                            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                            {
                                if (WhatAmI.IsInfected(vrrig.Creator))
                                {
                                    if (VRRig.LocalRig.enabled)
                                        VRRig.LocalRig.enabled = false;

                                    VRRig.LocalRig.transform.position = vrrig.rightHandTransform.position;
                                    //HyperSerialization.HyperSerialize();
                                }
                            }

                            //PhotonNetwork.SendAllOutgoingCommands();
                        }
                    }
                    else
                    {
                        PluginConfig.tagall = false;

                        if (!VRRig.LocalRig.enabled)
                            VRRig.LocalRig.enabled = true;

                        if (radiusLine != null)
                        {
                            Destroy(radiusLine.gameObject);
                            radiusLine = null;
                        }

                        Destroy(this.GetComponent<TagAll>());
                    }
                }
            }
            else
            {
                if (!VRRig.LocalRig.enabled)
                    VRRig.LocalRig.enabled = true;

                if (radiusLine != null)
                {
                    Destroy(radiusLine.gameObject);
                    radiusLine = null;
                }

                Destroy(this.GetComponent<TagAll>());
            }
        }
    }
}