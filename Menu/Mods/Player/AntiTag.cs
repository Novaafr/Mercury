﻿using Colossal.Menu;
using Colossal.Patches;
using ColossalV2.Mods;
using GorillaGameModes;
using HarmonyLib;
using Photon.Pun;
using PlayFab.GroupsModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Colossal.Mods
{
    public class AntiTag : MonoBehaviour
    {
        public void Update()
        {
            if (!PluginConfig.antitag)
            {
                if (!VRRig.LocalRig.enabled)
                    VRRig.LocalRig.enabled = true;

                Destroy(this.GetComponent<AntiTag>());
                return;
            }

            if (PhotonNetwork.InRoom)
            {
                if (!WhatAmI.IsInfected(PhotonNetwork.LocalPlayer))
                {
                    bool shouldDisable = false;
                    //List<int> infectedActorNumbers = new List<int>();

                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (!WhatAmI.IsInfected(vrrig.Creator) || vrrig.isMyPlayer)
                            continue;

                        float distance = Vector3.Distance(VRRig.LocalRig.transform.position, vrrig.transform.position);
                        if (distance <= GorillaGameManager.instance.tagDistanceThreshold * 1.6f)
                        {
                            shouldDisable = true;
                            //infectedActorNumbers.Add(vrrig.Creator.ActorNumber);
                        }
                    }

                    if (shouldDisable)
                    {
                        if (VRRig.LocalRig.enabled)
                            VRRig.LocalRig.enabled = false;

                        VRRig.LocalRig.transform.position = new Vector3(0, -6969, 0);

                        //if (infectedActorNumbers.Count > 0)
                        //    HyperSerialization.HyperSerialize(infectedActorNumbers.ToArray());

                        //PhotonNetwork.SendAllOutgoingCommands();
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
}
