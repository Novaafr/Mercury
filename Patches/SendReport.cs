using Colossal;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(GorillaNot), "SendReport")]
    internal class SendReport
    {
        private static void Prefix(string susReason, string susId, string susNick)
        {
            Debug.Log(string.Concat(new string[]
            {
                "Reported, Reason: ",
                susReason,
                ", ID: ",
                susId,
                ", NickName: ",
                susNick
            }));
            susReason = null;
            susId = null;
            susNick = null;
        }
    }
}
