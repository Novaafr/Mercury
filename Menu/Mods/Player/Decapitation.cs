using Colossal.Menu;
using Colossal.Patches;
using HarmonyLib;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Colossal.Mods
{
    public class Decapitation : MonoBehaviour
    {
        public static float yRotation;
        public void Update()
        {
            if (!PluginConfig.decapitation)
            {
                Destroy(this);
                return;
            }


            if (AreHandsDown())
            {
                float targetYRotation = CalculateTorsoYRotation();
                yRotation = Mathf.LerpAngle(yRotation, targetYRotation, .8f);
            }
            else
            {
                yRotation = GorillaTagger.Instance.mainCamera.transform.eulerAngles.y;
            }
        }
        private static bool AreHandsDown()
        {
            return GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.position.y < GorillaTagger.Instance.mainCamera.transform.position.y && GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position.y < GorillaTagger.Instance.mainCamera.transform.position.y;
        }
        private static float CalculateTorsoYRotation()
        {
            Vector3 headForward = GorillaTagger.Instance.mainCamera.transform.forward;
            headForward.y = 0;
            headForward.Normalize();

            Vector3 handCenter = (GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.position + GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.position) / 2f;
            Vector3 handDirection = handCenter - GorillaTagger.Instance.mainCamera.transform.position;
            handDirection.y = 0;
            handDirection.Normalize();
            Vector3 torsoDirection = Vector3.Lerp(headForward, handDirection, 0.45f);
            torsoDirection.Normalize();

            if (Vector3.Dot(torsoDirection, headForward) < 0)
                torsoDirection = headForward;

            return Quaternion.LookRotation(torsoDirection, Vector3.up).eulerAngles.y;
        }
    }
}
