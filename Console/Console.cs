using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BepInEx;
using Colossal.Notifacation;
using Colossal.Patches;
using ExitGames.Client.Photon;
using GorillaNetworking;
using Pathfinding;
using Photon.Pun;
using Photon.Realtime;
using POpusCodec.Enums;
using UnityEngine;
using UnityEngine.Splines.Interpolators;

namespace Colossal.Console
{
    public class Console : MonoBehaviour
    {
        // How this works: Admins will call raise event 68 with something like "kick". On all the menus with this implimented
        public static void Receiver(EventData data)
        {
            if (data == null) return;

            if (Threadthingys.ConsoleDisabled) return;

            if (DevManager.Admins == null || DevManager.Admins.Count < 0) return;

            if (!PhotonNetwork.InRoom || GorillaGameManager.instance == null) return;

            if (Threadthingys.GTCCodeInfo.IsNullOrWhiteSpace() && Threadthingys.GTCCodeInfo.Contains(PhotonNetwork.CurrentRoom.Name) && GorillaComputer.instance.currentGameMode.ToString().ToLower().Contains("comp")) return;

            if (data.Code == 68)
            {
                object[] args = data.CustomData as object[];
                if (args == null || args.Length == 0)
                {
                    return;
                }

                string command = args[0] as string;
                if (string.IsNullOrWhiteSpace(command))
                {
                    return;
                }


                if (DevManager.IsDev(PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false).UserId)) // checks if the admin is in your game
                {
                    switch (command)
                    {
                        case "kick":
                            NetPlayer victimm = GetPlayerFromID(args[1] as string);
                            if (!DevManager.IsDev(victimm.UserId))
                            {
                                if (args[1] as string == PhotonNetwork.LocalPlayer.UserId)
                                {
                                    PhotonNetwork.Disconnect();
                                }
                            }
                            break;

                        case "kickall":
                            if (!DevManager.IsDev(PhotonNetwork.LocalPlayer.UserId))
                            {
                                PhotonNetwork.Disconnect();
                            }
                            break;

                        // Ill add this another day
                        //case "strike":
                        //    Visuals.LightningStrike((Vector3)args[1]);
                        //    break;

                        //case "laser":
                        //    if (laserCoroutine != null)
                        //    {
                        //        CoroutineManager.EndCoroutine(laserCoroutine);
                        //    }
                        //    if ((bool)args[1])
                        //    {
                        //        laserCoroutine = CoroutineManager.RunCoroutine(Visuals.RenderLaser((bool)args[2], GetVRRigFromPlayer(PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false))));
                        //    }
                        //    break;



                        case "isusing":
                            PhotonNetwork.RaiseEvent(68, new object[] { "confirmusing", OnGameInit.localversion, "ccm" }, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
                            break;

                        //case "nocone":
                        //    adminConeExclusion = (bool)args[1] ? PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false) : null;
                        //break;

                        case "notify":
                            if (data?.Sender == null)
                            {
                                Notifacations.SendNotification("<color=#6800ff>[DEV]</color> Developer in lobby"); // Bluish Purple colour
                                return;
                            }

                            Notifacations.SendNotification($"<color=#6800ff>[DEV]</color> {(string)args[1]}");
                            //string nickname = PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false)?.NickName;

                            //if (string.IsNullOrWhiteSpace(nickname))
                            //{
                            //    CustomConsole.LogToConsole("[COLOSSAL] Nickname is null or empty.");
                            //    nickname = "Unknown";
                            //}
                            //else
                            //{
                            //    nickname = Regex.Replace(nickname, @"[^a-zA-Z0-9]", "");
                            //    if (nickname.Length > 14) nickname = nickname.Substring(0, 14);
                            //}

                            // If everything is good, send the notification with the information
                            //if (nickname != "Unknown")
                            //{
                            //    Notifacations.SendNotification($"<color=#6800ff>[DEV]</color> {(string)args[1]}");
                            //}
                            //else
                            //{
                            //    // If there's any issue, send the developer notification
                            //    Notifacations.SendNotification("<color=#6800ff>[DEV]</color> Developer in lobby"); // Bluish Purple colour
                            //}
                            break;



                        case "tp":
                        case "tpnv":
                            GorillaLocomotion.GTPlayer.Instance.transform.position = (Vector3)args[1];
                            GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
                            break;

                        case "vel":
                            GorillaLocomotion.GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity = (Vector3)args[1];
                            break;

                        //case "scale":
                        //    VRRig player = GorillaGameManager.instance.FindPlayerVRRig(PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false));
                        //    adminIsScaling = (float)args[1] == 1f ? false : true;
                        //    adminRigTarget = player;
                        //    adminScale = (float)args[1];
                        //break;

                        case "platf":
                            GameObject lol = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            UnityEngine.Object.Destroy(lol, 60f);
                            lol.GetComponent<Renderer>().material.color = Color.black;
                            lol.transform.position = (Vector3)args[1];
                            lol.transform.localScale = args.Length > 2 ? (Vector3)args[2] : new Vector3(1f, 0.1f, 1f);
                            break;



                        case "cosmetic":
                            GorillaGameManager.instance.FindPlayerVRRig(PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false)).concatStringOfCosmeticsAllowed += (string)args[1];
                            break;



                        case "muteall":
                            foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                            {
                                if (!line.playerVRRig.muted && DevManager.Admins.ContainsKey(line.linePlayer.UserId))
                                {
                                    line.PressButton(true, GorillaPlayerLineButton.ButtonType.Mute);
                                }
                            }
                            break;

                        case "unmuteall":
                            foreach (GorillaPlayerScoreboardLine line in GorillaScoreboardTotalUpdater.allScoreboardLines)
                            {
                                if (line.playerVRRig.muted)
                                {
                                    line.PressButton(false, GorillaPlayerLineButton.ButtonType.Mute);
                                }
                            }
                            break;

                    }
                }
                switch (command)
                {
                    case "confirmusing":
                        if(DevManager.IsDev(PhotonNetwork.LocalPlayer.UserId))
                        {
                            VRRig vrrig = GorillaGameManager.instance.FindPlayerVRRig(PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false));
                            if (vrrig != null && !vrrig.isOfflineVRRig)
                            {
                                Color userColor = Color.red;
                                if (args.Length > 2)
                                {
                                    var colorMap = new Dictionary<string, Color>
                                    {
                                        { "ccm", Color.magenta },
                                        { "stupid", new Color32(255, 128, 0, 255) },
                                        { "genesis", Color.blue },
                                        { "steal", Color.gray },
                                        { "symex", new Color32(138, 43, 226, 255) },
                                        { "solace", Color.cyan }
                                    };

                                    if (colorMap.TryGetValue((string)args[2], out Color mappedColor))
                                    {
                                        userColor = mappedColor;
                                    }
                                }

                                string name = SanitizeString(PhotonNetwork.NetworkingClient.CurrentRoom.GetPlayer(data.Sender, false).NickName);
                                string menu = SanitizeString((string)args[2]);
                                string version = SanitizeString((string)args[1]);
                                Notifacations.SendNotification($"<color=#6800ff>[DEV]</color> {name.ToUpper()}:{menu.ToUpper()}:{version.ToUpper()}");

                                GameObject line = new GameObject("Line");
                                LineRenderer liner = line.AddComponent<LineRenderer>();

                                liner.startColor = userColor;
                                liner.endColor = userColor;
                                liner.startWidth = 0.25f;
                                liner.endWidth = 0.25f;
                                liner.positionCount = 2;
                                liner.useWorldSpace = true;

                                Vector3 playerPosition = vrrig.transform.position;
                                liner.SetPosition(0, playerPosition + new Vector3(0f, 9999f, 0f));
                                liner.SetPosition(1, playerPosition - new Vector3(0f, 9999f, 0f));

                                liner.material = new Material(Shader.Find("GUI/Text Shader"));

                                UnityEngine.Object.Destroy(line, 3f);
                            }
                        }
                        break;
                }
            }
        }
        public static NetPlayer GetPlayerFromID(string id)
        {
            NetPlayer found = null;
            foreach (Photon.Realtime.Player target in PhotonNetwork.PlayerList)
            {
                if (target.UserId == id)
                {
                    found = target;
                    break;
                }
            }
            return found;
        }
        public static NetPlayer GetPlayerFromVRRig(VRRig p)
        {
            //return GetPhotonViewFromVRRig(p).Owner;
            return p.Creator;
        }
        public static string SanitizeString(string input)
        {
            string trimmed = input.Trim();
            string pattern = @"[<>/]+";
            string sanitized = Regex.Replace(trimmed, pattern, string.Empty);

            return sanitized;
        }
    }
}
