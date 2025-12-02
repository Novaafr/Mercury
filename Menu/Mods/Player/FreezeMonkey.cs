using Colossal.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace Colossal.Mods
{
    public class FreezeMonkey : MonoBehaviour
    {
        public void Update()
        {
            /*if (PluginConfig.freezemonkey)
            {
                if (ControllerInputPoller.instance.leftGrab)
                {
                    if(DisableRig.disablerig)
                        DisableRig.disablerig = false;
                    VRRig.LocalRig.transform.position = GorillaLocomotion.GTPlayer.Instance.transform.position;
                    VRRig.LocalRig.transform.rotation = GorillaLocomotion.GTPlayer.Instance.transform.rotation;
                }
                else
                {
                    if (!DisableRig.disablerig)
                        DisableRig.disablerig = true;
                }
            }
            else
            {
                Destroy(GorillaTagger.Instance.GetComponent<FreezeMonkey>());
            }*/
        }
    }
}
