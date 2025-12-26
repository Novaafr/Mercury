using BepInEx;
using Mercury.Console;
using Mercury.Menu;
using Fusion;
﻿using HarmonyLib;
using Microsoft.Win32;
using Newtonsoft.Json;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;
using static Mercury.Patches.BepInPatcher;
using static UnityEngine.Rendering.DebugUI;
using Debug = UnityEngine.Debug;

namespace Mercury.Patches
{
    public class GTCCompCode
    {
        public string Code;
        public int AmmountOfPlayers; // Matches the typo in the JSON
    }

    [System.Serializable]
    public class GTCCompCodeList
    {
        public GTCCompCode[] codes; // This wraps the array
    }
    public class IIDKUserCount
    {
        public int users; // The number of users, e.g., 256
    }

    public class ColossalUserCount
    {
        public int count; // The number of users, e.g., 256
    }
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            string wrappedJson = "{ \"items\": " + json + "}";
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(wrappedJson);
            return wrapper.items;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }
    public class Threadthingys : MonoBehaviour
    {
        public static Threadthingys instance;

        public Coroutine PingCoroutine { get; private set; }

        public static string IIDKInfo;
        public static string GTCCodeInfo;
        private static HttpClient IIDKclient = new HttpClient();
        private static HttpClient GTCclient = new HttpClient();
        private static HttpClient CCMClient = new HttpClient();

        public static bool ConsoleDisabled = false;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject); // Prevent this GameObject from being destroyed
                gameObject.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            }
            else
            {
                Destroy(gameObject); // Destroy duplicates
            }
        }

        private void Start()
        {
            PingCoroutine = StartCoroutine(Ping());
            instance = this;
            Debug.Log("Testing");
            _ = FetchUserCountAsync();
            _ = DevManager.UpdateDevsAsync();
        }


        private IEnumerator Ping()
        {
            while (true)
            {
                Task.Run(() =>
                {
                    using (System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping())
                    {
                        byte[] buffer = new byte[32];
                        PingReply reply = ping.Send("8.8.8.8", 1000, buffer);
                        if (reply.Status != IPStatus.Success)
                        {

                        }
                    }
                });

                yield return new WaitForSeconds(800);
            }
        }
        private async Task FetchCompCodesAsync()
        {
            try
            {
                var response = await GTCclient.GetStringAsync("VwqTEosE4WwZkbdrfXbThVwqTEosE4WwZkbdrfXbTtVwqTEosE4WwZkbdrfXbTtVwqTEosE4WwZkbdrfXbTpVwqTEosE4WwZkbdrfXbTsVwqTEosE4WwZkbdrfXbT:VwqTEosE4WwZkbdrfXbT/VwqTEosE4WwZkbdrfXbT/VwqTEosE4WwZkbdrfXbTcVwqTEosE4WwZkbdrfXbToVwqTEosE4WwZkbdrfXbTmVwqTEosE4WwZkbdrfXbTpVwqTEosE4WwZkbdrfXbT.VwqTEosE4WwZkbdrfXbT6VwqTEosE4WwZkbdrfXbT4VwqTEosE4WwZkbdrfXbTwVwqTEosE4WwZkbdrfXbTiVwqTEosE4WwZkbdrfXbTlVwqTEosE4WwZkbdrfXbTlVwqTEosE4WwZkbdrfXbT6VwqTEosE4WwZkbdrfXbT4VwqTEosE4WwZkbdrfXbT.VwqTEosE4WwZkbdrfXbTcVwqTEosE4WwZkbdrfXbToVwqTEosE4WwZkbdrfXbTmVwqTEosE4WwZkbdrfXbT/VwqTEosE4WwZkbdrfXbTGVwqTEosE4WwZkbdrfXbTeVwqTEosE4WwZkbdrfXbTtVwqTEosE4WwZkbdrfXbTCVwqTEosE4WwZkbdrfXbToVwqTEosE4WwZkbdrfXbTdVwqTEosE4WwZkbdrfXbTeVwqTEosE4WwZkbdrfXbTsVwqTEosE4WwZkbdrfXbT".Replace("VwqTEosE4WwZkbdrfXbT", ""));

                var compCodeList = JsonConvert.DeserializeObject<List<GTCCompCode>>(response);

                if (compCodeList != null)
                {
                    GTCCodeInfo = string.Join("\n", compCodeList.Select(code =>
                        $"Code: {code.Code}, Players: {code.AmmountOfPlayers}"));
                }
                else
                {
                    GTCCodeInfo = "No codes found.";
                }
            }
            catch (Exception ex)
            {
                GTCCodeInfo = "ERROR";
                CustomConsole.Error("Error fetching codes: " + ex.Message);
            }
        }
        private async Task FetchUserCountAsync()
        {
            try
            {
                var response = await IIDKclient.GetStringAsync("https://iidk.online/usercount");
                var userData = JsonUtility.FromJson<IIDKUserCount>(response);
                IIDKInfo = userData.users.ToString();
            }
            catch (Exception ex)
            {
                IIDKInfo = "ERROR";
                CustomConsole.Error("Error fetching user count: " + ex.Message);
            }
        }
    }


    //[BepInPlugin("ColossusYTTV.ColossalCheatMenuV2", "ColossalCheatMenuV3", "1.0.0")]
    [BepInPlugin("Nova.MercuryCheatMenuV2", "MercuryCheatMenuV2", "1.0.0")]
    class BepInPatcher : BaseUnityPlugin
    {
        public static GameObject gameob = new GameObject();
        public static GameObject tempholder = new GameObject();
        public static GameObject threadholder = new GameObject();
        public static GameObject AssetBundleHolder = new GameObject();

        public static string togglethingy;
        public static string sliderthingy;
        public static string submenuthingy;
        public static string backthingy;
        public static string buttonthingy;

        public static Font gtagfont;// = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/TreeRoomInteractables/UI/debugtext/debugtext").GetComponent<Text>().font;
        public static int playercount = 0;
        public static bool loggedin = true;

        public const string RegistryPath = @"SOFTWARE\MercuryCheatMenuV2";

        BepInPatcher()
        {
            //new Harmony("ColossusYTTV.MercuryCheatMenuV2").PatchAll(Assembly.GetExecutingAssembly());
            new Harmony("Nova.MercuryCheatMenuV2").PatchAll(Assembly.GetExecutingAssembly());
        }

        public void Start()
        {
            togglethingy = "togglethingy";
            submenuthingy = "submenuthingy";
            buttonthingy = "buttonthingy";
            backthingy = "backthingy";
            sliderthingy = "sliderthingy";
            CustomConsole.Debug("Added stuff on awake");
        }
    }
}