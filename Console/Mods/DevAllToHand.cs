using Colossal.Menu;
using Colossal.Patches;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using static Colossal.Plugin;

namespace Colossal.Console.Mods
{
    public class DevAllToHand : MonoBehaviour
    {
        public void Update()
        {
            if (PluginConfig.devalltohand && PhotonNetwork.InRoom)
            {
                PhotonNetwork.RaiseEvent(68, new object[] { "tpnv", GorillaTagger.Instance.rightHandTransform.position + GorillaTagger.Instance.rightHandTransform.forward }, new RaiseEventOptions { Receivers = ReceiverGroup.Others }, SendOptions.SendReliable);
            }
            else
            {
                UnityEngine.Object.Destroy(holder.GetComponent<DevAllToHand>());
            }
        }
    }
}
