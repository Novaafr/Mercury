using Colossal;
using Colossal.Menu;
using Colossal.Mods;
using HarmonyLib;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Colossal.Notifacation
{
    [HarmonyPatch(typeof(GorillaNot), "SendReport")]
    internal class ReportNotifacation {
        private static List<string> notifiedPlayers = new List<string>();
        private static float reporttiemr = 0;

        [HarmonyPrefix]
        private static void Postfix(string susReason, string susId, string susNick) {
            if (!notifiedPlayers.Contains(susId) && PluginConfig.Notifications && !susReason.ToLower().Contains("handtap")) {
                notifiedPlayers.Add(susId);
                Notifacations.SendNotification($"<color=yellow>[ANTICHEAT]</color> Name: {susNick}");
                if(reporttiemr <= 20) {
                    notifiedPlayers.Remove(susId);
                }
            }
        }
    }
}
