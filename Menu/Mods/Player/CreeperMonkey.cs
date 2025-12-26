
using Mercury.Menu;
using Mercury.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace Mercury.Mods
{
    public class CreeperMonkey : MonoBehaviour
    {
        public void Update()
        {
            if (PluginConfig.creepermonkey)
            {
                if (Controls.LeftTrigger())
                {
                    float num = float.PositiveInfinity;
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (!vrrig.isOfflineVRRig)
                        {
                            float sqrMagnitude = (vrrig.transform.position - GorillaLocomotion.GTPlayer.Instance.transform.position).sqrMagnitude;
                            if (sqrMagnitude < num)
                            {
                                num = sqrMagnitude;
                                VRRig.LocalRig.headConstraint.LookAt(vrrig.headMesh.transform);
                                GorillaTagger.Instance.rightHandTransform.position = vrrig.headMesh.transform.position;
                            }
                        }
                    }
                }
            }
            else
                Destroy(this.GetComponent<CreeperMonkey>());
        }
    }
}
