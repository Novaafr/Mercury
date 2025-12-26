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
    internal class JoinNotifacation : MonoBehaviourPunCallbacks
    {
        private static List<Player> notifiedPlayers = new List<Player>();

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);

            if (!notifiedPlayers.Contains(newPlayer) && PluginConfig.Notifications)
            {
                notifiedPlayers.Add(newPlayer);
                Notifacations.SendNotification($"<color=cyan>[JOIN]</color> Name: {newPlayer.NickName}");
            }
        }
    }
}