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
    public class FakeReportMenu : MonoBehaviour
    {
        public static bool fakeQuestMenuFinger = false;
        public void Update()
        {
            if (PluginConfig.fakereportmenu)
            {
                if (!GorillaLocomotion.GTPlayer.Instance.inOverlay)
                    GorillaLocomotion.GTPlayer.Instance.inOverlay = true;

                PluginConfig.nofinger = true;

                GorillaLocomotion.GTPlayer.Instance.leftHand.wasColliding = false;
                GorillaLocomotion.GTPlayer.Instance.rightHand.wasColliding = false;
            }
            else
            {
                if (GorillaLocomotion.GTPlayer.Instance.inOverlay)
                    GorillaLocomotion.GTPlayer.Instance.inOverlay = false;

                PluginConfig.nofinger = false;

                Destroy(this.GetComponent<FakeReportMenu>());
            }
        }
    }
}
