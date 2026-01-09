using Mercury.Mods;
using Mercury;
using HarmonyLib;
using Photon.Pun;
using System.Net;
using Photon.Realtime;
using UnityEngine;
using Mercury.Menu;
using System.Collections.Generic;
using static Mercury.Patches.BepInPatcher;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Text;

namespace Mercury.Patches
{
    internal class JoinRoom : MonoBehaviourPunCallbacks
    {
        private static readonly HttpClient client = new HttpClient();
        private string[] cosmetics = new string[]
        {
            "LBAAD.", // admin badge
            "LBAAK.", // stick
            "LBAHG.", // gt1
            "LMAPY.", // forest guide
            "LBADE.", // finger painter
            "LBAGS.", // illustrator
            "LBANI.", // aa badge
        };

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            if(BepInPatcher.loggedin)
            {
                string rarecosmetic = "";

                foreach(VRRig rig in GorillaParent.instance.vrrigs)
                {
                    if(rig != null)
                    {
                        foreach(string item in cosmetics)
                        {
                            if (rig.rawCosmeticString.Contains(item))
                            {
                                rarecosmetic += item;
                            }
                        }
                    }
                }

                SendToDiscord(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount.ToString(), rarecosmetic, PhotonNetwork.MasterClient.NickName, PhotonNetwork.CurrentRoom.CustomProperties.ToJson().ToString());

                CustomConsole.Debug("Logged in, Joined room");
            }
        }
        public static async Task SendToDiscord(string code, string players, string cosmetic, string master, string roomprops)
        {
            string result;
            if (cosmetic.Contains("LBAAD."))
            {
                result = "ADMIN BADGE";
            }
            else if (cosmetic.Contains("LBAAK."))
            {
                result = "STICK";
            }
            else if (cosmetic.Contains("LMAPY."))
            {
                result = "FOREST GUIDE STICK";
            }
            else if (cosmetic.Contains("LBAHG."))
            {
                result = "GT1 BADGE";
            }
            else if (cosmetic.Contains("LBADE."))
            {
                result = "FINGER PAINTER";
            }
            else if (cosmetic.Contains("LBAGS."))
            {
                result = "ILLUSTRATOR";
            }
            else if (cosmetic.Contains("LBANI."))
            {
                result = "AA BADGE";
            }
            else
            {
                result = "No Rare Cosmetics";
            }



            var embed = new
            {
                username = "MCMV2 Tracker",
                embeds = new[]
                {
                    new
                    {
                        title = "New Code!",
                        color = 0xFFD700,
                        timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                        fields = new[]
                        {
                            new
                            {
                                name = "Code",
                                value = code,
                                inline = true
                            },
                            new
                            {
                                name = "Players In Room",
                                value = players,
                                inline = true
                            },
                             new
                            {
                                name = "Master",
                                value = master,
                                inline = true
                            },
                            new
                            {
                                name = "Room Props",
                                value = roomprops,
                                inline = true
                            },
                            new
                            {
                                name = "Timestamp",
                                value = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                                inline = false
                            },
                            new
                            {
                                name = "Cosmetics",
                                value = result,
                                inline = false
                            },
                            new
                            {
                                name = "FPS On Join",
                                value = (1f / Time.deltaTime).ToString("F0"),
                                inline = false
                            },
                            new
                            {
                                name = "Ping + Server IP",
                                value = $"Ping: {PhotonNetwork.GetPing()} | ServerIP: {PhotonNetwork.ServerAddress} (ServerIP From PhotonNetwork.ServerAddress)",
                                inline = false
                            }
                        }
                    }
                }
            };

            string jsonContent = JsonConvert.SerializeObject(embed);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(OnGameInit.joinwebhook, content);

            CustomConsole.Debug("Sent tracker info.");
        }
    }
}