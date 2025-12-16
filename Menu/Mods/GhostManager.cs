using Colossal.Patches;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Valve.VR.Extras;


namespace Colossal.Menu
{
    internal class GhostManager
    {
        private static List<GameObject> ghosts = new List<GameObject>();

        public static byte opacity;
        public static Color ghostColor;

        public static GameObject SpawnGhost()
        {
            GameObject ghost = GameObject.Instantiate(VRRig.LocalRig.gameObject);
            var vrrig = ghost.GetComponent<VRRig>();

            switch (PluginConfig.GhostOpacity)
            { 
                case 0:
                    opacity = 100;
                    break;
                case 1:
                    opacity = 80;
                    break;
                case 2:
                    opacity = 60;
                    break;
                case 3:
                    opacity = 30;
                    break;
                case 4:
                    opacity = 20;
                    break;
                case 5:
                    opacity = 0;
                    break;
            }
            switch (PluginConfig.GhostColour)
            {
                case 0:
                    ghostColor = new Color32(204, 51, 255, opacity);
                    break;
                case 1:
                    ghostColor = new Color32(255, 0, 0, opacity);
                    break;
                case 2:
                    ghostColor = new Color32(255, 255, 0, opacity);
                    break;
                case 3:
                    ghostColor = new Color32(0, 255, 0, opacity);
                    break;
                case 4:
                    ghostColor = new Color32(0, 0, 255, opacity);
                    break;
                default:
                    ghostColor = new Color32(255, 255, 255, 255);
                    break;
            }

            GameObject.Destroy(vrrig.GetComponent<Rigidbody>());

            vrrig.mainSkin.material.color = ghostColor;
            vrrig.mainSkin.material.shader = AssetBundleLoader.chamsShader; //Shader.Find("GUI/Text Shader");
            vrrig.SetPlayerMeshHidden(false);
            vrrig.handTapSound = null;
            //vrrig.rightHandPlayer.transform.parent.GetChild(1).gameObject.SetActive(false);
            //vrrig.leftHandPlayer.transform.parent.GetChild(1).gameObject.SetActive(false);

            AntiScreenShare.SetAntiScreenShareLayer(ghost);

            ghosts.Add(ghost);
            //return null;
            return ghost;
        }

        public static void DestroyGhost(GameObject ghost)
        {
            if (ghosts.Contains(ghost))
            {
                ghosts.Remove(ghost);
                GameObject.Destroy(ghost);
            }
        }
    }
}