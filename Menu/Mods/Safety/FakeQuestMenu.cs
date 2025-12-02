﻿using Colossal.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace Colossal.Mods
{
    public class FakeQuestMenu : MonoBehaviour
    {
        public void Update()
        {
            if (PluginConfig.fakequestmenu)
            {
                if (!GorillaLocomotion.GTPlayer.Instance.inOverlay)
                    GorillaLocomotion.GTPlayer.Instance.inOverlay = true;

                PluginConfig.nofinger = true;

                GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.localPosition = new Vector3(238f, -90f, 0f);
                GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.localPosition = new Vector3(-190f, 90f, 0f);
                GorillaLocomotion.GTPlayer.Instance.LeftHand.controllerTransform.rotation = Camera.main.transform.rotation * Quaternion.Euler(-55f, 90f, 0f);
                GorillaLocomotion.GTPlayer.Instance.RightHand.controllerTransform.rotation = Camera.main.transform.rotation * Quaternion.Euler(-55f, -49f, 0f);

                GorillaLocomotion.GTPlayer.Instance.leftHand.wasColliding = false;
                GorillaLocomotion.GTPlayer.Instance.rightHand.wasColliding = false;
            }
            else
            {
                if (GorillaLocomotion.GTPlayer.Instance.inOverlay)
                    GorillaLocomotion.GTPlayer.Instance.inOverlay = false;

                PluginConfig.nofinger = false;

                Destroy(this.GetComponent<FakeQuestMenu>());
            }
        }
    }
}
