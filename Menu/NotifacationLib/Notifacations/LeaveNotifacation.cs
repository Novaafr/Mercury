using Mercury.Mods;
using Mercury;
using HarmonyLib;
using Photon.Pun;
using System.Net;
using Photon.Realtime;
using UnityEngine;
using Mercury.Menu;
using System.Collections.Generic;

namespace Mercury.Notifacation
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