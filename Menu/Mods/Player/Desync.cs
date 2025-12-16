
using Colossal.Menu;
using Colossal.Patches;
using GorillaExtensions;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace Colossal.Mods
{
    public class Desync : MonoBehaviour
    {
        private GameObject ghost;

        private float prevtime;
        private Vector3 prevpos;
        private Quaternion prevrot;

        private GameObject lefthand;
        private GameObject righthand;
        private GameObject head;
        private Vector3 prevrpos;
        private Vector3 prevlpos;
        private Vector3 prevheadpos;
        private Quaternion prevrrot;
        private Quaternion prevlrot;
        private Quaternion prevheadrot;
        public void Update()
        {
            if (PluginConfig.desync)
            {
                if (Time.time - prevtime >= (1 / 28))
                {
                    prevtime = Time.time;
                    if (Time.time - prevtime >= (PhotonNetwork.GetPing() / 1000))
                    {
                        if (ghost == null)
                            ghost = GhostManager.SpawnGhost();

                        var vrrig = ghost.GetComponent<VRRig>();

                        ghost.transform.position = prevpos;
                        ghost.transform.rotation = prevrot;

                        if (lefthand.IsNull() || righthand.IsNull())
                        {
                            lefthand = vrrig.leftHandTransform.gameObject;
                            righthand = vrrig.rightHandTransform.gameObject;
                            head = vrrig.headMesh;
                        }
                        lefthand.transform.position = prevlpos;
                        lefthand.transform.rotation = prevlrot;

                        righthand.transform.position = prevrpos;
                        righthand.transform.rotation = prevrrot;

                        head.transform.position = prevheadpos;
                        head.transform.rotation = prevheadrot;

                        vrrig.leftHandPlayer.Pause();
                        vrrig.rightHandPlayer.Pause();

                        vrrig.mainSkin.material.color = GhostManager.ghostColor;
                        vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                        vrrig.enabled = false;

                        prevpos = GorillaTagger.Instance.offlineVRRig.transform.position;
                        prevrot = GorillaTagger.Instance.offlineVRRig.transform.rotation;

                        prevlpos = GorillaTagger.Instance.offlineVRRig.leftHandTransform.position;
                        prevlrot = GorillaTagger.Instance.offlineVRRig.leftHandTransform.rotation;

                        prevrpos = GorillaTagger.Instance.offlineVRRig.rightHandTransform.position;
                        prevrrot = GorillaTagger.Instance.offlineVRRig.rightHandTransform.rotation;

                        prevheadpos = GorillaTagger.Instance.offlineVRRig.headMesh.transform.position;
                        prevheadrot = GorillaTagger.Instance.offlineVRRig.headMesh.transform.rotation;

                        prevtime = Time.time;
                    }
                }
            }
            else
            {
                if (ghost != null)
                    GhostManager.DestroyGhost(ghost);

                Destroy(this.GetComponent<Desync>());
            }
        }
    }
}