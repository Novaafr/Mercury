//using Colossal.Menu;
//using Colossal.Patches;
//using GorillaNetworking;
//using Photon.Pun;
//using System;
//using System.Collections;
//using System.Linq;
//using UnityEngine;
//using UnityEngine.InputSystem;

//namespace Colossal.Mods
//{
//    public class Disable
//    {
//        public static void disable()
//        {
//            if (PhotonNetwork.InRoom)
//            {
//                PhotonNetwork.CurrentRoom.IsOpen = true;
//                PhotonNetwork.CurrentRoom.IsVisible = true;

//                NetworkSystem.Instance.BroadcastMyRoom(true, NetworkSystem.Instance.LocalPlayer.UserId + PhotonNetworkController.Instance.keyStr, PhotonNetworkController.Instance.shuffler);

//                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
//                hashtable.Add("gameMode", "MODDED_CASUAL");
//                hashtable.Add("joinedGameMode", "MODDED_CASUAL");

//                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
//            }
//        }
//    }
//}
