using Colossal.Menu;
using Colossal.Patches;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using static Colossal.Plugin;

namespace Colossal.Console.Mods
{
    public class DevKickGun : MonoBehaviour
    {
        private GameObject pointer;
        private LineRenderer radiusLine;
        private Material lineMaterial = new Material(Shader.Find("GUI/Text Shader"));
        public void Update()
        {
            if (PluginConfig.devkickgun && PhotonNetwork.InRoom)
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

                if (pointer == null)
                {
                    pointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    UnityEngine.Object.Destroy(pointer.GetComponent<Rigidbody>());
                    UnityEngine.Object.Destroy(pointer.GetComponent<SphereCollider>());
                    pointer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    pointer.GetComponent<Renderer>().material = Boards.boardmat;
                }


                RaycastHit raycastHit2;
                LayerMask combinedLayerMask = GorillaLocomotion.GTPlayer.Instance.locomotionEnabledLayers | 16384;
                Physics.Raycast(GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position - GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.up, -GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.up, out raycastHit2, float.PositiveInfinity, combinedLayerMask);
                pointer.transform.position = raycastHit2.point;

                if (Controls.RightJoystick())
                {
                    if (radiusLine == null)
                    {
                        radiusLine = new GameObject("RadiusLine")
                        {
                            transform =
                            {
                                parent = pointer.transform
                            }
                        }.AddComponent<LineRenderer>();
                        radiusLine.positionCount = 2;
                        radiusLine.startWidth = 0.05f;
                        radiusLine.endWidth = 0.05f;
                        radiusLine.material = lineMaterial;
                        radiusLine.startColor = lineMaterial.color;
                        radiusLine.endColor = lineMaterial.color;
                    }
                    radiusLine.SetPosition(0, raycastHit2.point);
                    radiusLine.SetPosition(1, GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position);
                    radiusLine.GetPosition(0);

                    switch (PluginConfig.AntiScreenShare)
                    {
                        case 1:
                            if (radiusLine.gameObject.layer != 25)
                            {
                                radiusLine.gameObject.layer = 25;
                            }
                            break;
                        case 2:
                            if (radiusLine.gameObject.layer != 16)
                            {
                                radiusLine.gameObject.layer = 16;
                            }
                            break;
                    }

                    VRRig player = raycastHit2.collider.GetComponentInParent<VRRig>();
                    if (player != null)
                    {
                        PhotonNetwork.RaiseEvent(68, new object[] { "kick", Console.GetPlayerFromVRRig(player).UserId },
                            new RaiseEventOptions { Receivers = ReceiverGroup.All },
                            SendOptions.SendReliable);
                        return;
                    }
                    return;
                }

                if (radiusLine != null)
                {
                    UnityEngine.Object.Destroy(radiusLine);
                    radiusLine = null;
                    return;
                }
            }
            else
            {
                UnityEngine.Object.Destroy(holder.GetComponent<DevKickGun>());
                if (pointer != null)
                {
                    UnityEngine.Object.Destroy(pointer);
                }
            }
        }
    }
}
