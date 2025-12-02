﻿using Colossal.Menu;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using UnityEngine.Splines.Interpolators;
using UnityEngine.XR.Interaction.Toolkit;

namespace Colossal.Mods
{
    public class HollowBoxEsp : MonoBehaviour
    {
        private Color espColor;
        private Dictionary<GameObject, TextMesh> rigBoxes = new Dictionary<GameObject, TextMesh>();

        private ThrowableBug[] creatures;

        //private NavMeshAgent[] GREntitys;

        //private void Start()
        //{
        //    if(PhotonNetwork.InRoom)
        //    {
        //        if (GREntitys == null)
        //            GREntitys = Resources.FindObjectsOfTypeAll<NavMeshAgent>();
        //    }
        //}
        public void Update()
        {
            if (PluginConfig.hollowboxesp && PhotonNetwork.InRoom)
            {
                switch (PluginConfig.ESPColour)
                {
                    case 0:
                        espColor = new Color(0.6f, 0f, 0.8f, 0.4f);
                        break;
                    case 1:
                        espColor = new Color(1f, 0f, 0f, 0.4f);
                        break;
                    case 2:
                        espColor = new Color(1f, 1f, 0f, 0.4f);
                        break;
                    case 3:
                        espColor = new Color(0f, 1f, 0f, 0.4f);
                        break;
                    case 4:
                        espColor = new Color(0f, 0f, 1f, 0.4f);
                        break;
                    default:
                        espColor = new Color(0.6f, 0f, 0.8f, 0.4f);
                        break;
                }


                foreach (var box in rigBoxes.Values)
                {
                    AntiScreenShare.SetAntiScreenShareLayer(box.gameObject);
                }


                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && !rig.isOfflineVRRig)
                    {
                        Color boxColor;
                        if (WhatAmI.IsInfected(rig.GetComponent<VRRig>().Creator))
                            CreateBoxForObject(rig.gameObject, new Color(1f, 0f, 0f, 0.4f));
                        else if (!WhatAmI.IsAliveGhostReactor(rig.GetComponent<VRRig>()))
                            CreateBoxForObject(rig.gameObject, new Color(1f, 1f, 1f, 0.4f));
                        else
                            CreateBoxForObject(rig.gameObject, espColor);
                    }
                }


                //if (GREntitys != null || GREntitys.Length != 0)
                //{
                //    foreach (NavMeshAgent thing in GREntitys)
                //    {
                //        if (thing == null || thing.gameObject == null)
                //            continue;

                //        CreateBoxForObject(thing.gameObject, Color.white);
                //    }
                //}


                //if (creatures != null && creatures.Length > 0)
                //{
                //    foreach (ThrowableBug thing in creatures)
                //    {
                //        GameObject parentObject = thing.GetComponentInParent<Transform>().gameObject;
                //        Renderer renderer = parentObject.GetComponentInChildren<Renderer>();

                //        CreateBoxForObject(thing.gameObject, espColor);
                //    }
                //}

                List<GameObject> keysToRemove = new List<GameObject>();
                foreach (var key in rigBoxes.Keys)
                {
                    if (key == null || !GorillaParent.instance.vrrigs.Contains(key.GetComponent<VRRig>()))
                    {
                        keysToRemove.Add(key);
                    }
                }
                foreach (var key in keysToRemove)
                {
                    Destroy(rigBoxes[key.gameObject].gameObject);
                    rigBoxes.Remove(key.gameObject);
                }
            }
            else
            {
                foreach (var box in rigBoxes.Values)
                {
                    Destroy(box.gameObject);
                }
                rigBoxes.Clear();
                Destroy(this.GetComponent<HollowBoxEsp>());
            }
        }

        private void CreateBoxForObject(GameObject obj, Color color)
        {
            if (!rigBoxes.TryGetValue(obj, out TextMesh box))
            {
                GameObject textObject = new GameObject("HollowBoxESP");
                box = textObject.AddComponent<TextMesh>();
                box.alignment = TextAlignment.Center;
                box.anchor = TextAnchor.MiddleCenter;
                box.text = "□";
                box.fontSize = 300; // Use a large font size for better quality
                box.characterSize = 0.05f; // Scale down to match the size
                box.transform.SetParent(obj.transform, false);
                rigBoxes[obj] = box;
            }

            box.transform.position = obj.transform.position;
            box.color = color;
            box.transform.LookAt(
                box.transform.position + Camera.main.transform.rotation * Vector3.forward,
                Camera.main.transform.rotation * Vector3.up);
        }
    }
}
