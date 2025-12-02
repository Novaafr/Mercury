
using Colossal.Menu;
using Colossal.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
namespace Colossal.Mods
{
    public class TFly : MonoBehaviour
    {
        public void FixedUpdate()
        {
            if (PluginConfig.tfly)
            {
                string bind = CustomBinding.GetBinds("tfly"); // FIXED: Ensure correct bind retrieval

                if (string.IsNullOrEmpty(bind) || bind == "UNBOUND")
                {
                    return; // If no valid bind, do nothing
                }

                // Alternative button press (if needed)
                if (ControllerInputPoller.instance.leftControllerSecondaryButton)
                {
                    GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity = new Vector3(0f, 0.01f, 0f);
                }

                // FIXED: Use correct input checking method
                if (ControlsV2.GetControl(bind))
                {
                    GorillaLocomotion.GTPlayer.Instance.transform.position += GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.forward * 0.45f;
                    GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity = Vector3.zero;
                    return;
                }
            }
            else
            {
                UnityEngine.Object.Destroy(this.GetComponent<TFly>());
            }
        }
    }
}
