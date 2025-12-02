using Colossal.Menu;

using Colossal.Mods;
using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Colossal.Notifacation
{
    internal class EventNotifacation : MonoBehaviourPunCallbacks {
        public override void OnConnected()
        {
            if(PhotonNetwork.InRoom)
            {
                base.OnConnected();
                PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
            }
        }

        void OnEvent(EventData eventData)
        {
            if (PluginConfig.Notifications)
            {
                if(eventData.Code != 202 || eventData.Code != 206 || eventData.Code != 201 || eventData.Code != 205 || eventData.Code != 255)
                    Notifacations.SendNotification($"<color=yellow>[EVENT]</color> Code: {eventData}");
            }
        }
    }
}
