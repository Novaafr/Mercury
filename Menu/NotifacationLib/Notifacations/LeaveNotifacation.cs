using Colossal.Mods;
using Colossal;
using HarmonyLib;
using Photon.Pun;
using System.Net;
using Photon.Realtime;
using UnityEngine;
using Colossal.Menu;
using System.Collections.Generic;

namespace Colossal.Notifacation
{
    internal class LeaveNotifacation : MonoBehaviourPunCallbacks {
        private static List<Player> notifiedPlayers = new List<Player>();

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);

            if (!notifiedPlayers.Contains(otherPlayer) && PluginConfig.Notifications)
            {
                notifiedPlayers.Add(otherPlayer);
                Notifacations.SendNotification($"<color=cyan>[LEAVE]</color> Name: {otherPlayer.NickName}");
            }
        }
    }
}