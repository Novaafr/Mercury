using Colossal.Menu;
using Colossal.Patches;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Colossal.Mods
{
    public class Throw : MonoBehaviour
    {
        LocalGorillaVelocityTracker right;
        LocalGorillaVelocityTracker left;
        public void Awake()
        {
            right = GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.AddComponent<LocalGorillaVelocityTracker>();
            left = GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.AddComponent<LocalGorillaVelocityTracker>();
        }
        public void Update()
        {
            if(PluginConfig.Throw)
            {
                string bind = CustomBinding.GetBinds("throw");
                if (string.IsNullOrEmpty(bind) || bind == "UNBOUND")
                {
                    return;
                }

                // Get mirrored versions for left and right hand
                string leftBind = CustomBinding.MirrorBind(bind, true);
                string rightBind = CustomBinding.MirrorBind(bind, false);

                if (ControlsV2.GetControl(rightBind))
                {
                    GorillaTagger.Instance.rigidbody.velocity -= right.GetVelocity() / 8 * GorillaLocomotion.GTPlayer.Instance.scale;
                }
                if (ControlsV2.GetControl(leftBind))
                {
                    GorillaTagger.Instance.rigidbody.velocity -= left.GetVelocity() / 8 * GorillaLocomotion.GTPlayer.Instance.scale;
                }
            }
            else
            {
                Destroy(this.GetComponent<Throw>());
            }
        }
    }
    public class LocalGorillaVelocityTracker : MonoBehaviour
    {
        private Vector3 previousLocalPosition;
        private Vector3 velocity;
        public void Start()
        {
            previousLocalPosition = transform.localPosition;
        }
        public void Update()
        {
            if (PluginConfig.Throw)
            {
                Vector3 localDisplacement = transform.localPosition - previousLocalPosition;
                Vector3 localVelocity = localDisplacement / Time.deltaTime;

                velocity = transform.parent.TransformDirection(localVelocity);

                previousLocalPosition = transform.localPosition;
            }
        }

        public Vector3 GetVelocity()
        {
            return velocity;
        }
    }
}
