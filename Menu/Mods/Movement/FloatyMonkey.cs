using Colossal.Menu;
using Colossal.Patches;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Colossal.Mods
{
    public class FloatyMonkey : MonoBehaviour
    {
        private float[] floatyLevels = new float[]
        {
            0f, 1.1f, 1.2f, 1.4f, 1.6f, 1.8f, 2f, 2.2f, 2.4f, 2.6f, 2.8f, 3f, 3.2f, 3.4f, 3.6f, 3.8f, 4f, -Physics.gravity.y // Level 17
        };

        private float ammount;

        public void FixedUpdate()
        {
            int floatyIndex = PluginConfig.floatymonkey;

            // Handle destruction of the component for FloatyMonkey = 0
            if (floatyIndex == 0)
            {
                if (this.GetComponent<FloatyMonkey>() != null)
                    Destroy(this.GetComponent<FloatyMonkey>());
                return;
            }

            // Ensure the index is within the valid range
            if (floatyIndex >= 1 && floatyIndex <= 17)
            {
                ammount = floatyLevels[floatyIndex];
            }

            string bind = CustomBinding.GetBinds("floatymonkey");
            if (string.IsNullOrEmpty(bind) || bind == "UNBOUND")
            {
                return;
            }

            if (ControlsV2.GetControl(bind))
            {
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * ammount, ForceMode.Acceleration);
            }
        }
    }
}
