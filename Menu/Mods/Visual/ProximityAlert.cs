﻿using Colossal.Menu;
using GorillaNetworking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Colossal.Mods
{
    public class ProximityAlert : MonoBehaviour
    {
        public static GameObject AlertHub;
        public static Text AlertHubText;
        public void Start()
        {
            if (AlertHub == null && AlertHubText == null)
                (AlertHub, AlertHubText) = GUICreator.CreateTextGUI("", "AlertHub", TextAnchor.LowerCenter, new Vector3(0, 0, 2), true);
        }
        public void Update()
        {
            if (PluginConfig.ProximityAlert)
            {
                if (PhotonNetwork.InRoom && WhatAmI.infectionmanager.currentInfectedArray.Length > 0)
                {
                    AntiScreenShare.SetAntiScreenShareLayer(AlertHub);

                    float closestDistance = float.MaxValue;
                    string distanceText = "";

                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        if (WhatAmI.IsInfected(vrrig.Creator) && !vrrig.Creator.IsLocal)
                        {
                            float distance = Vector3.Distance(Camera.main.transform.position, vrrig.transform.position);
                            if (distance < closestDistance)
                                closestDistance = distance;
                        }
                    }

                    if (closestDistance < 8f)
                    {
                        distanceText = "Very Close!";
                        AlertHubText.color = Color.red;
                    }
                    else if (closestDistance < 16f)
                    {
                        distanceText = "Close";
                        AlertHubText.color = Color.yellow;
                    }
                    else if (closestDistance < 20f)
                    {
                        distanceText = "Nearby";
                        AlertHubText.color = Color.cyan;
                    }
                    else
                    {
                        distanceText = "Good";
                        AlertHubText.color = Color.green;
                    }

                    AlertHubText.text = $"[{(int)closestDistance}M]\n{distanceText}";
                }
                else if (AlertHubText.text != "")
                    AlertHubText.text = "";
            }
            else
            {
                Destroy(AlertHub);
                Destroy(AlertHubText);

                Destroy(this.GetComponent<ProximityAlert>());
            }
        }
    }
}
