using UnityEngine;
using Photon.Pun;
using System;
using GorillaLocomotion;
using GorillaTagScripts;
using Colossal.Menu;
using Colossal.Patches;
using GorillaNetworking;

namespace Colossal.Mods
{
    public class RainbowMonkey : MonoBehaviour
    {
        private Color[] rainbowColors = new Color[]
        {
            Color.red, Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta
        };
        private int currentColorIndex = 0;
        private float colorChangeInterval = 0.2f;
        private float transitionProgress = 0f;
        private Color currentColor;
        private Color targetColor;

        private float antikick = 0;

        //private FittingRoomButton[] buttons;
        //public static bool hasMoodRing = false;
        //private bool disableRing = false;

        //private bool safeRPC = false;

        //public void Start()
        //{
        //    buttons = Resources.FindObjectsOfTypeAll<FittingRoomButton>();
        //    disableRing = false;
        //}

        public void Update()
        {
            if (!PluginConfig.rainbowmonkey)
            {
                Destroy(this.GetComponent<RainbowMonkey>());
                //disableRing = true;
                //GetMoodRing();

                //if (!VRRig.LocalRig.enabled)
                //{
                //    VRRig.LocalRig.enabled = true;
                //}

                return;
            }

            //GameObject tryOnRoom = GameObject.Find("Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Cosmetics Room Triggers/TryOnRoom");
            //GameObject city = GameObject.Find("Environment Objects/LocalObjects_Prefab/City_WorkingPrefab");
            //if(city != null && city.activeSelf)
            //{
            //    if (tryOnRoom != null && tryOnRoom.activeSelf) // Means your not in city, cant get ss ring anyway
            //    {
            //        if (PhotonNetwork.InRoom)
            //        {
            //            GetMoodRing();
            //        }
            //    }
            //}
            if (PhotonNetwork.InRoom && (GorillaComputer.instance.friendJoinCollider.playerIDsCurrentlyTouching.Contains(PhotonNetwork.LocalPlayer.UserId) || CosmeticWardrobeProximityDetector.IsUserNearWardrobe(PhotonNetwork.LocalPlayer.UserId)))
            {
                transitionProgress += Time.deltaTime / colorChangeInterval;
                Color color = Color.Lerp(currentColor, targetColor, transitionProgress);
                VRRig.LocalRig.playerColor = color;
                if (Time.time > antikick)
                {
                    antikick = Time.time + 0.1f;
                    GorillaTagger.Instance.myVRRig.SendRPC("RPC_InitializeNoobMaterial", RpcTarget.All, new object[]
                    {
                        color.r,
                        color.g,
                        color.b
                    });
                    RPCProtection.SkiddedRPCProtection();
                }

                if (transitionProgress >= 1f)
                {
                    transitionProgress = 0f;
                    currentColor = targetColor;
                    currentColorIndex = (currentColorIndex + 1) % rainbowColors.Length;
                    targetColor = rainbowColors[currentColorIndex];
                }
            }

            //if (hasMoodRing)
            //{
            //    ControllerInputPoller.instance.rightControllerIndexFloat = 90f;
            //}
        }

        //private void GetMoodRing()
        //{
        //    try
        //    {
        //        GameObject triggerZone = GameObject.Find("Environment Objects/TriggerZones_Prefab/ZoneTransitions_Prefab/Cosmetics Room Triggers");
        //        if (triggerZone != null)
        //        {
        //            triggerZone.SetActive(true);
        //        }
        //        else
        //        {
        //            return;
        //        }

        //        if (VRRig.LocalRig.enabled)
        //            VRRig.LocalRig.enabled = false;

        //        VRRig.LocalRig.transform.position = new Vector3(-51.7931f, 16.9328f, -120.0452f);

        //        if (VRRig.LocalRig.inTryOnRoom)
        //        {
        //            if (disableRing)
        //                goto disablering;

        //            string cosmetic = FindMoodRing();
        //            if (!string.IsNullOrEmpty(cosmetic))
        //            {
        //                Debug.Log("1");

        //                CosmeticsController.instance.currentCart.Insert(0, CosmeticsController.instance.GetItemFromDict(cosmetic));
        //                CosmeticsController.instance.UpdateShoppingCart();

        //                foreach (FittingRoomButton thing in buttons)
        //                {
        //                    if (thing.currentCosmeticItem.displayName.Contains(cosmetic))
        //                    {
        //                        thing.ButtonActivationWithHand(false);
        //                        Debug.Log("2");
        //                    }
        //                }

        //                if ((CosmeticsController.instance.currentWornSet.ToDisplayNameArray().Contains("LMAJU.") || CosmeticsController.instance.currentWornSet.ToDisplayNameArray().Contains("MOOD RING")) && !safeRPC)
        //                {
        //                    Debug.Log("3");
        //                    safeRPC = true;

        //                    string[] archiveCosmetics;
        //                    archiveCosmetics = CosmeticsController.instance.currentWornSet.ToDisplayNameArray();
        //                    string[] itjustworks = new string[] { "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU.", "LMAJU." };
        //                    CosmeticsController.instance.currentWornSet = new CosmeticsController.CosmeticSet(itjustworks, CosmeticsController.instance);
        //                    VRRig.LocalRig.cosmeticSet = new CosmeticsController.CosmeticSet(itjustworks, CosmeticsController.instance);
        //                    GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", RpcTarget.All, new object[] { itjustworks, CosmeticsController.instance.tryOnSet.ToDisplayNameArray() }); // this bullshit 4 weeked 3 of my accs
        //                    RPCProtection.SkiddedRPCProtection();
        //                }
        //            }

        //            return;

        //        disablering:
        //            if (CosmeticsController.instance.currentWornSet.ToDisplayNameArray().Contains("LMAJU.") || CosmeticsController.instance.currentWornSet.ToDisplayNameArray().Contains("MOOD RING"))
        //            {
        //                if (disableRing)
        //                {
        //                    //GorillaTagger.Instance.myVRRig.SendRPC("RPC_UpdateCosmeticsWithTryon", RpcTarget.Others, new object[] { new string[16], new string[16] });
        //                    //RPCProtection.SkiddedRPCProtection();
        //                    hasMoodRing = false;
        //                }

        //                if (!VRRig.LocalRig.enabled)
        //                {
        //                    VRRig.LocalRig.enabled = true;
        //                }
        //                return;
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogException(e);
        //    }
        //}

        //public static string FindMoodRing()
        //{
        //    foreach (GorillaNetworking.CosmeticsController.CosmeticItem ring in GorillaNetworking.CosmeticsController.instance.allCosmetics)
        //    {
        //        if (ring.canTryOn && ring.overrideDisplayName.Contains("MOOD RING"))
        //        {
        //            return ring.itemName;
        //        }
        //    }

        //    return null;
        //}
    }
}
