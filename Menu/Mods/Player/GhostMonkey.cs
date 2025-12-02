
﻿using Colossal.Menu;
using Colossal.Patches;
using GorillaNetworking;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace Colossal.Mods
{
    public class GhostMonkey : MonoBehaviour
    {
        private GameObject ghost;
        private GameObject pointl;
        private GameObject pointr;
        private GameObject pointh;

        public void Update()
        {
            if (PluginConfig.ghostmonkey && PhotonNetwork.InRoom)
            {
                string bind = CustomBinding.GetBinds("ghostmonkey");
                if (string.IsNullOrEmpty(bind) || bind == "UNBOUND")
                {
                    return;
                }

                if (ControlsV2.GetControl(bind))
                {
                    if (ghost == null)
                        ghost = GhostManager.SpawnGhost();

                    if (VRRig.LocalRig.enabled)
                        VRRig.LocalRig.enabled = false;

                    ghost.GetComponent<VRRig>().mainSkin.material.color = GhostManager.ghostColor;
                    ghost.GetComponent<VRRig>().mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                }
                else
                {
                    if (!VRRig.LocalRig.enabled)
                        VRRig.LocalRig.enabled = true;

                    if (ghost != null)
                        GhostManager.DestroyGhost(ghost);
                }
            }
            else
            {
                if (ghost != null)
                    GhostManager.DestroyGhost(ghost);

                if (!VRRig.LocalRig.enabled)
                    VRRig.LocalRig.enabled = true;

                Destroy(this.GetComponent<GhostMonkey>());
            }
        }
    }
}
