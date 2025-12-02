//using Colossal.Menu;
//using Colossal.Patches;
//using HarmonyLib;
//using Photon.Pun;
//using System;
//using UnityEngine;
//using UnityEngine.InputSystem;

//namespace Colossal.Mods
//{
//    public class SmoothRig : MonoBehaviour
//    {
//        public void Start()
//        {
//            if(PhotonNetwork.SerializationRate != 30) PhotonNetwork.SerializationRate = 30;
//        }
//        public void Update()
//        {
//            if (!PluginConfig.smoothrig)
//            {
//                if(PhotonNetwork.SerializationRate != 10) PhotonNetwork.SerializationRate = 10;

//                Destroy(this);
//                return;
//            }
//        }
//    }
//}
