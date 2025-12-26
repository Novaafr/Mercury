using Mercury.Menu;
using Mercury.Patches;
using UnityEngine;
using UnityEngine.XR;

namespace Mercury.Mods
{
    public class JoystickFly : MonoBehaviour
    {
        public void Update()
        {
            if (!PluginConfig.joystickfly)
            {
                Destroy(this.GetComponent<JoystickFly>());
            }

            Vector3 movement = Vector3.zero;
            if (XRSettings.isDeviceActive)
            {
                float leftJoystickX = Controls.LeftJoystickAxis().x;
                float leftJoystickY = Controls.LeftJoystickAxis().y;

                movement += Camera.main.transform.right * leftJoystickX;
                movement += Camera.main.transform.forward * leftJoystickY;

                float rightJoystickY = Controls.RightJoystickAxis().y;
                movement += Camera.main.transform.up * rightJoystickY;
            }

            if (GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody != null)
            {
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(-Physics.gravity, ForceMode.Acceleration);
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.linearVelocity = Vector3.zero;

                GorillaTagger.Instance.rigidbody.transform.position += movement * Time.deltaTime * 15;
            }
        }
    }
}