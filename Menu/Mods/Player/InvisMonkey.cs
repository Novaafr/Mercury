
﻿using Colossal.Menu;
using Colossal.Patches;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace Colossal.Mods
{
    public class InvisMonkey : MonoBehaviour
    {
        private GameObject ghost;
        public void Update()
        {
            if (PluginConfig.invismonkey && PhotonNetwork.InRoom)
            {
                string bind = CustomBinding.GetBinds("invismonkey");
                if (string.IsNullOrEmpty(bind) || bind == "UNBOUND")
                {
                    return;
                }

                if (ControlsV2.GetControl(bind))
                {
                    if(ghost == null) 
                        ghost = GhostManager.SpawnGhost();

                    if (VRRig.LocalRig.enabled)
                        VRRig.LocalRig.enabled = false;

                    if(ghost != null)
                    {
                        ghost.GetComponent<VRRig>().mainSkin.material.color = GhostManager.ghostColor;
                        ghost.GetComponent<VRRig>().mainSkin.material.shader = Shader.Find("GUI/Text Shader");
                    }

                    VRRig.LocalRig.transform.position = new Vector3(VRRig.LocalRig.transform.position.x, -6969f, VRRig.LocalRig.transform.position.z);
                }
                else
                {
                    if (ghost != null)
                        GhostManager.DestroyGhost(ghost);

                    if (!VRRig.LocalRig.enabled)
                        VRRig.LocalRig.enabled = true;
                }
            }
            else
            {
                if(ghost != null)
                    GhostManager.DestroyGhost(ghost);

                if (!VRRig.LocalRig.enabled)
                    VRRig.LocalRig.enabled = true;

                Destroy(this.GetComponent<InvisMonkey>());
            }
        }
    }
}
