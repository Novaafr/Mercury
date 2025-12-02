using Colossal;
using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace Colossal.Patches
{
    //[HarmonyPatch(typeof(PhotonNetwork), "OnEvent")]
    internal class Instantate
    {
        /*private static void Prefix(ExitGames.Client.Photon.EventData photonEvent)
        {
            if (PhotonNetwork.InRoom && photonEvent.Code == 202)
            {
                Plugin.called += 1;
            }
        }
        private static bool Prefix()
        {
            if(Plugin.called >= 30)
            {
                return false;
            }
            return true;
        }*/
    }
}