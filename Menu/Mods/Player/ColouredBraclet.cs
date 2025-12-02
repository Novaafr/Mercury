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
    public class ColouredBraclet : MonoBehaviour
    {
        public void Update()
        {
            if (PluginConfig.colouredbraclet == 0)
            {
                if (!VRRig.LocalRig.nonCosmeticRightHandItem.IsEnabled)
                {
                    GorillaTagger.Instance.myVRRig.SendRPC("EnableNonCosmeticHandItemRPC", RpcTarget.Others, new object[]
                    {
                        false,
                        false
                    });
                    RPCProtection.SkiddedRPCProtection();

                    VRRig.LocalRig.nonCosmeticRightHandItem.EnableItem(false);
                }

                VRRig.LocalRig.reliableState.isBraceletLeftHanded = false;
                VRRig.LocalRig.reliableState.braceletSelfIndex = 0;
                VRRig.LocalRig.reliableState.braceletBeadColors.Clear();
                VRRig.LocalRig.UpdateFriendshipBracelet();

                Destroy(this.GetComponent<ColouredBraclet>());
                return;
            }

            if (PhotonNetwork.InRoom)
            {
                List<Color> rgbColors;
                switch (PluginConfig.colouredbraclet)
                {
                    case 1: // Rainbow
                        rgbColors = new List<Color>();
                        for (int i = 0; i < 10; i++)
                            rgbColors.Add(Color.HSVToRGB(((Time.frameCount / 180f) + (i / 10f)) % 1f, 1f, 1f));
                        break;
                    case 2: // Purple
                        rgbColors = new List<Color> { Color.magenta };
                        break;
                    case 3: // Black
                        rgbColors = new List<Color> { Color.black };
                        break;
                    case 4: // White
                        rgbColors = new List<Color> { Color.white };
                        break;
                    case 5: // Red
                        rgbColors = new List<Color> { Color.red };
                        break;
                    case 6: // Green
                        rgbColors = new List<Color> { Color.green };
                        break;
                    case 7: // Blue
                        rgbColors = new List<Color> { Color.blue };
                        break;
                    case 8: // Yellow
                        rgbColors = new List<Color> { Color.yellow };
                        break;
                    default:
                        rgbColors = new List<Color> { Color.white };
                        break;
                }

                if (!VRRig.LocalRig.nonCosmeticRightHandItem.IsEnabled)
                {
                    GorillaTagger.Instance.myVRRig.SendRPC("EnableNonCosmeticHandItemRPC", RpcTarget.Others, new object[]
                    {
                        true,
                        false
                    });
                    RPCProtection.SkiddedRPCProtection();

                    VRRig.LocalRig.nonCosmeticRightHandItem.EnableItem(true);
                }

                VRRig.LocalRig.reliableState.isBraceletLeftHanded = false;
                VRRig.LocalRig.reliableState.braceletSelfIndex = 99;
                VRRig.LocalRig.reliableState.braceletBeadColors = rgbColors;
                VRRig.LocalRig.friendshipBraceletRightHand.UpdateBeads(rgbColors, 99);
            }
        }
    }

    [HarmonyPatch(typeof(VRRig), "UpdateFriendshipBracelet")]
    public class ColouredBracletPatch : MonoBehaviour
    {
        public static bool Prefix(VRRig __instance)
        {
            if (PluginConfig.colouredbraclet == 0)
                return true;
            return false;
        }
    }
}
