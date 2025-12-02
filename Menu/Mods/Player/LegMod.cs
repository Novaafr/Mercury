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
    public class LegMod : MonoBehaviour
    {
        public void Update()
        {
            /*if (PluginConfig.legmod)
            {
                bool inroom = false;
                if (PluginConfig.legmod && !inroom && PhotonNetwork.InRoom)
                {
                    if (GameObject.Find("Global/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/shoulder.R/").transform.localPosition.y != 0f)
                    {
                        Vector3 localPosition = GameObject.Find("Global/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/shoulder.L/").transform.localPosition;
                        Vector3 localPosition2 = GameObject.Find("Global/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/shoulder.R/").transform.localPosition;
                        GameObject.Find("Global/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/shoulder.L/").transform.localPosition = new Vector3(localPosition2.x, -0f, localPosition2.z);
                        GameObject.Find("Global/GorillaParent/GorillaVRRigs/Gorilla Player Networked(Clone)/rig/body/shoulder.R/").transform.localPosition = new Vector3(localPosition.x, -0f, localPosition.z);
                    }
                    inroom = true;
                    if (!PhotonNetwork.InRoom)
                    {
                        inroom = false;
                    }
                }
            }
            else
            {
                Destroy(GorillaTagger.Instance.GetComponent<LegMod>());
            }*/
        }
    }
}
