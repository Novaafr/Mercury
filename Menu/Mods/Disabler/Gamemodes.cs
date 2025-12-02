//using Colossal.Menu;
//using Colossal.Patches;
//using GorillaNetworking;
//using Photon.Pun;
//using System;
//using UnityEngine;
//using UnityEngine.InputSystem;

//namespace Colossal.Mods
//{
//    public class Gamemodes
//    {
//        public static void PrepChange()
//        {
//            if (PhotonNetwork.InRoom)
//            {
//                PhotonNetwork.CurrentRoom.IsOpen = true;
//                PhotonNetwork.CurrentRoom.IsVisible = true;

//                NetworkSystem.Instance.BroadcastMyRoom(true, NetworkSystem.Instance.LocalPlayer.UserId + PhotonNetworkController.Instance.keyStr, PhotonNetworkController.Instance.shuffler);
//            }
//        }
//        public static void Infection()
//        {
//            PrepChange();
//            if (PhotonNetwork.CurrentRoom.IsOpen && PhotonNetwork.CurrentRoom.IsVisible)
//            {
//                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
//                hashtable.Add("gameMode", "publicDEFAULTinfection");
//                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
//            }
//        }
//        public static void Casual()
//        {
//            PrepChange();
//            if (PhotonNetwork.CurrentRoom.IsOpen && PhotonNetwork.CurrentRoom.IsVisible)
//            {
//                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
//                hashtable.Add("gameMode", "publicDEFAULTcasual");
//                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
//            }
//        }
//        public static void Hunt()
//        {
//            PrepChange();
//            if (PhotonNetwork.CurrentRoom.IsOpen && PhotonNetwork.CurrentRoom.IsVisible)
//            {
//                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
//                hashtable.Add("gameMode", "publicDEFAULThunt");
//                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
//            }
//        }
//        public static void PaintBrawl()
//        {
//            PrepChange();
//            if (PhotonNetwork.CurrentRoom.IsOpen && PhotonNetwork.CurrentRoom.IsVisible)
//            {
//                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
//                hashtable.Add("gameMode", "publicDEFAULTpaintbrawl");
//                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
//            }
//        }
//        public static void Guardian()
//        {
//            PrepChange();
//            if (PhotonNetwork.CurrentRoom.IsOpen && PhotonNetwork.CurrentRoom.IsVisible)
//            {
//                ExitGames.Client.Photon.Hashtable hashtable = new ExitGames.Client.Photon.Hashtable();
//                hashtable.Add("gameMode", "publicDEFAULTguardian");
//                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);
//            }
//        }
//    }
//}
