﻿using Colossal.Menu;
using Colossal.Patches;
using ColossalV2.Mods;
using GorillaGameModes;
using HarmonyLib;
using Photon.Pun;
using PlayFab.GroupsModels;
using System.Collections.Generic;
using UnityEngine;

namespace Colossal.Mods
{
    public class FakeLag : MonoBehaviour
    {
        private float lagTimer = 0f;
        private float lagInterval = 0f;
        private bool isLagging = false;

        public void Update()
        {
            if (!PluginConfig.fakelag)
            {
                if (GorillaTagger.Instance != null && VRRig.LocalRig != null)
                {
                    if (!VRRig.LocalRig.enabled)
                        VRRig.LocalRig.enabled = true;
                }

                Destroy(this.GetComponent<FakeLag>());
                return;
            }

            if (!PhotonNetwork.InRoom)
                return;

            if (GorillaTagger.Instance == null || VRRig.LocalRig == null)
            {
                Debug.LogError("FakeLag: Essential components are null!");
                return;
            }

            lagTimer += Time.deltaTime;
            if (lagTimer >= lagInterval)
            {
                isLagging = !isLagging;
                lagTimer = 0f;
                lagInterval = Random.Range(0.2f, 1f); // Random toggle interval, 200ms-1s

                if (isLagging)
                {
                    if (VRRig.LocalRig.enabled)
                        VRRig.LocalRig.enabled = false;
                }
                else
                {
                    if (!VRRig.LocalRig.enabled)
                        VRRig.LocalRig.enabled = true;
                }
            }
        }
    }
}