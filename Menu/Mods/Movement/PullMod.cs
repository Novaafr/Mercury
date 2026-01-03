
using Mercury.Menu;
using Mercury.Patches;
using GorillaLocomotion;
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
    public class PullMod : MonoBehaviour
    {
        public void FixedUpdate()
        {
            if (PluginConfig.pullmod)
            {
                string bind = CustomBinding.GetBinds("pullmod"); 

                if (string.IsNullOrEmpty(bind) || bind == "UNBOUND")
                {
                    return; 
                }

                if (ControlsV2.GetControl(bind))
                {
                    if (GTPlayer.Instance.IsHandTouching(true) || GTPlayer.Instance.IsHandTouching(false))
                    {
                        // Im lazy to code credits to ii
                        Vector3 normal = GTPlayer.Instance.lastHitInfoHand.normal;
                        Vector3 direction = GorillaTagger.Instance.rigidbody.linearVelocity.X_Z();
                        GTPlayer.Instance.transform.position += (direction - normal * Vector3.Dot(direction, normal)).normalized * (direction.magnitude / GTPlayer.Instance.maxJumpSpeed * 0.15f * 1f);
                    }
                }
            }
            else
            {
                UnityEngine.Object.Destroy(this.GetComponent<PullMod>());
            }
        }
    }
}
