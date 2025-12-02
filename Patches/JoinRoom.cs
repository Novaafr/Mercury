using Colossal.Mods;
using Colossal;
using HarmonyLib;
using Photon.Pun;
using System.Net;
using Photon.Realtime;
using UnityEngine;
using Colossal.Menu;
using System.Collections.Generic;
using static Colossal.Patches.BepInPatcher;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Newtonsoft.Json;
using System.Text;

namespace Colossal.Patches
{
    internal class JoinRoom : MonoBehaviourPunCallbacks
    {
        private static readonly HttpClient client = new HttpClient();
        private string[] cosmetics = new string[]
        {
            "LBAAD.", // admin badge
            "LBAAK.", // stick
            "LBAHG." // gt1
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
                            if (rig.concatStringOfCosmeticsAllowed.Contains(item))
                            {
                                rarecosmetic += item;
                            }
                        }
                    }
                }

                SendToDiscord(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount.ToString(), rarecosmetic);

                CustomConsole.Debug("Logged in, Joined room");
            }
        }
        public static async Task SendToDiscord(string code, string players, string cosmetic)
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
            else if (cosmetic.Contains("LBAHG."))
            {
                result = "GT1 BADGE";
            }
            else
            {
                result = "No Rare Cosmetics";
            }


            var embed = new
            {
                username = "CCMV2 Tracker",
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
                                name = "Username",
                                value = "",
                                inline = false
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