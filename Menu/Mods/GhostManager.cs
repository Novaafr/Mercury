using Photon.Pun;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
            GameObject ghost = GameObject.Instantiate(GorillaTagger.Instance.offlineVRRig.gameObject);
            var vrrig = ghost.GetComponent<VRRig>();

            switch (PluginConfig.GhostOpacity) { case 0: opacity = 100; break; case 1: opacity = 80; break; case 2: opacity = 60; break; case 3: opacity = 30; break; case 4: opacity = 20; break; case 5: opacity = 0; break; }
            switch (PluginConfig.GhostColour) { case 0: ghostColor = new Color32(204, 51, 255, opacity); break; case 1: ghostColor = new Color32(255, 0, 0, opacity); break; case 2: ghostColor = new Color32(255, 255, 0, opacity); break; case 3: ghostColor = new Color32(0, 255, 0, opacity); break; case 4: ghostColor = new Color32(64, 255, 0, opacity); break; case 5: ghostColor = new Color32(0, 0, 255, opacity); break; default: ghostColor = new Color32(255, 255, 255, 255); break; }

            // Prevent ghost rig from running GRPlayer / Tick logic
            foreach (var comp in ghost.GetComponentsInChildren<MonoBehaviour>(true))
            {
                if (comp is GRPlayer || comp.GetType().Name.Contains("Tick"))
                    GameObject.Destroy(comp);
                else if (!(comp is VRRig))
                    comp.enabled = false;
            }

            // Remove physics
            var rb = vrrig.GetComponent<Rigidbody>();
            if (rb) GameObject.Destroy(rb);

            // Apply opacity/color
            byte alpha = opacity;
            vrrig.mainSkin.material.color = ghostColor;
            vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");

            if (vrrig.headMesh != null)
                vrrig.headMesh.SetActive(true);

            if (vrrig.mainSkin != null)
                vrrig.mainSkin.enabled = true;

            vrrig.showName = true;
            vrrig.handTapSound = null;

            ghosts.Add(ghost);
            return ghost;
        }


        public static void DestroyGhost(GameObject ghost)
        {
            if(ghost == null) return;
            if(ghost != null)
            {
                GameObject.Destroy(ghost);
                if (ghosts.Contains(ghost))
                {
                    ghosts.Remove(ghost);
                }
            }
        }
    }
}