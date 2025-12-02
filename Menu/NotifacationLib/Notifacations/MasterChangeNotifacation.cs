using Colossal;
using Colossal.Menu;
using Colossal.Mods;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

namespace Colossal.Notifacation
{
    internal class MasterChangeNotifacation : MonoBehaviourPunCallbacks {

        private static string mastername;

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);

            if (PluginConfig.Notifications)
            {
                if (mastername != newMasterClient.NickName)
                {
                    string nickname = Regex.Replace(newMasterClient.NickName, @"[^<>/\a-zA-Z0-9]", "");
                    if (nickname.Length > 14) nickname = nickname.Substring(0, 14);

                    Notifacations.SendNotification($"<color=green>[MASTER]</color> Changed, Name: {nickname}");
                    mastername = newMasterClient.NickName;
                }
            }
        }
    }
}
