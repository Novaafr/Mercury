using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ExitGames.Client.Photon;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace ColossalV2.Mods
{
    internal class HyperSerialization
    {
        //public static void HyperSerialize(int[] targetActors = null, bool exclude = false, List<PhotonView> viewFilter = null)
        //{
        //    // Serialization on crack

        //    if (viewFilter != null)
        //    {
        //        NonAllocDictionary<int, PhotonView> photonViewList = Traverse.Create(typeof(PhotonNetwork)).Field("photonViewList").GetValue<NonAllocDictionary<int, PhotonView>>();

        //        List<int> filteredViewIDs = new List<int> { };
        //        foreach (PhotonView view in viewFilter)
        //            filteredViewIDs.Add(view.ViewID);

        //        foreach (PhotonView photonView in photonViewList.Values)
        //        {
        //            if (exclude)
        //            {
        //                if (photonView.IsMine && filteredViewIDs.Contains(photonView.ViewID))
        //                    photonViewList.Remove(photonView.ViewID);
        //            }
        //            else
        //            {
        //                if (photonView.IsMine && !filteredViewIDs.Contains(photonView.ViewID))
        //                    photonViewList.Remove(photonView.ViewID);
        //            }
        //        }

        //        Traverse.Create(typeof(PhotonNetwork)).Field("photonViewList").SetValue(photonViewList);
        //    }
        //    RaiseEventOptions serializeRaiseEvOptions = Traverse.Create(typeof(PhotonNetwork)).Field("serializeRaiseEvOptions").GetValue<RaiseEventOptions>();

        //    if (targetActors != null)
        //    {
        //        serializeRaiseEvOptions.TargetActors = targetActors;

        //        Traverse.Create(typeof(PhotonNetwork)).Field("serializeRaiseEvOptions").SetValue(serializeRaiseEvOptions);
        //    }
        //    typeof(PhotonNetwork).GetMethod("RunViewUpdate", BindingFlags.Static | BindingFlags.NonPublic).Invoke(typeof(PhotonNetwork), Array.Empty<object>());

        //    if (targetActors != null)
        //    {
        //        serializeRaiseEvOptions = Traverse.Create(typeof(PhotonNetwork)).Field("serializeRaiseEvOptions").GetValue<RaiseEventOptions>();
        //        serializeRaiseEvOptions.TargetActors = null;
        //        Traverse.Create(typeof(PhotonNetwork)).Field("serializeRaiseEvOptions").SetValue(serializeRaiseEvOptions);
        //    }
        //}
    }
}
