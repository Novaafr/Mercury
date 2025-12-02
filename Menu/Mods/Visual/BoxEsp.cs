using Colossal.Mods;
using Colossal.Menu;
using GorillaNetworking;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Animations.Rigging;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit;

namespace Colossal.Mods
{
    public class BoxEsp : MonoBehaviour
    {
        public static float objectScale;
        private Color espcolor;
        private Dictionary<GameObject, GameObject> rigBoxes = new Dictionary<GameObject, GameObject>();
        private Material espMaterial;

        private ThrowableBug[] creatures;

        //private NavMeshAgent[] GREntitys;

        public void Start()
        {
            espMaterial = new Material(Shader.Find("GUI/Text Shader"));

            //if(PhotonNetwork.InRoom)
            //{
            //    if (GREntitys == null)
            //        GREntitys = Resources.FindObjectsOfTypeAll<NavMeshAgent>();
            //}
        }

        public void Update()
        {
            if (PluginConfig.boxesp && PhotonNetwork.InRoom)
            {
                switch (PluginConfig.ESPColour)
                {
                    case 0: espcolor = new Color(0.6f, 0f, 0.8f, 0.4f); break;
                    case 1: espcolor = new Color(1f, 0f, 0f, 0.4f); break;
                    case 2: espcolor = new Color(1f, 1f, 0f, 0.4f); break;
                    case 3: espcolor = new Color(0f, 1f, 0f, 0.4f); break;
                    case 4: espcolor = new Color(0f, 0f, 1f, 0.4f); break;
                    default: espcolor = new Color(0.6f, 0f, 0.8f, 0.4f); break;
                }

                foreach (VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if (rig != null && !rig.isOfflineVRRig)
                    {
                        if (!rigBoxes.ContainsKey(rig.gameObject))
                        {
                            CreateBoxForRig(rig.gameObject);
                        }

                        UpdateBoxForRig(rig.gameObject);
                    }
                }

                //if (creatures != null && creatures.Length > 0)
                //{
                //    foreach (ThrowableBug thing in creatures)
                //    {
                //        GameObject parentObject = thing.GetComponentInParent<Transform>().gameObject;
                //        Renderer renderer = parentObject.GetComponentInChildren<Renderer>();

                //        if (!rigBoxes.ContainsKey(thing.gameObject))
                //        {
                //            CreateBoxForRig(thing.gameObject);
                //        }
                //    }
                //}

                //if (GREntitys != null || GREntitys.Length != 0)
                //{
                //    foreach (NavMeshAgent thing in GREntitys)
                //    {
                //        if (thing == null || thing.gameObject == null)
                //            continue;

                //        if (!rigBoxes.ContainsKey(thing.gameObject))
                //        {
                //            CreateBoxForRig(thing.gameObject);
                //        }

                //        UpdateBoxForRig(thing.gameObject);
                //    }
                //}
            }
            else
            {
                DestroyAllBoxes();
            }
        }

        private void CreateBoxForRig(GameObject rig)
        {
            GameObject go = new GameObject("box");

            GameObject face = GameObject.CreatePrimitive(PrimitiveType.Plane);
            Destroy(face.GetComponent<Collider>());
            face.transform.SetParent(go.transform, false);
            face.transform.localPosition = Vector3.zero;
            face.transform.localRotation = Quaternion.Euler(90, 0, 0);
            face.transform.localScale = new Vector3(1f, 1f, 1f);

            face.GetComponent<Renderer>().material = espMaterial;

            rigBoxes[rig] = go;
        }

        private void UpdateBoxForRig(GameObject rig)
        {
            Camera mainCamera = Camera.main;
            Vector3 objectWorldPosition = rig.transform.position;
            float objectDistanceFromCamera = Vector3.Distance(objectWorldPosition, mainCamera.transform.position);
            Matrix4x4 worldToCameraMatrix = mainCamera.worldToCameraMatrix;
            Vector4 objectClipPosition = mainCamera.projectionMatrix * worldToCameraMatrix * new Vector4(objectWorldPosition.x, objectWorldPosition.y, objectWorldPosition.z, 1);
            objectClipPosition /= objectClipPosition.w;

            objectScale = Mathf.Clamp(objectDistanceFromCamera / objectClipPosition.w, 2f, 8.5f);

            GameObject go = rigBoxes[rig];
            GameObject face = go.transform.GetChild(0).gameObject;

            go.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
            go.transform.position = rig.transform.position;
            face.transform.localScale = new Vector3(objectScale / 40, objectScale / 40, objectScale / 40);

            Color boxColor;
            if (WhatAmI.IsInfected(rig.GetComponent<VRRig>().Creator))
                boxColor = new Color(1f, 0f, 0f, 0.4f);
            else if (!WhatAmI.IsAliveGhostReactor(rig.GetComponent<VRRig>()) || rig.GetComponent<NavMeshAgent>())
                boxColor = new Color(1f, 1f, 1f, 0.4f);
            else
                boxColor = espcolor;

            face.GetComponent<Renderer>().material.color = boxColor;

            AntiScreenShare.SetAntiScreenShareLayer(face);
        }

        private void DestroyAllBoxes()
        {
            foreach (var entry in rigBoxes)
            {
                Destroy(entry.Value);
            }
            rigBoxes.Clear();

            Destroy(this.GetComponent<BoxEsp>());
        }
    }
}
