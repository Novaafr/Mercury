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
            "LMAPY." // forest guide
        };

        static string userid;
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
                                userid = rig.OwningNetPlayer.UserId;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(userid))
                                {
                                    userid = "";
                                }
                            }
                        }
                    }
                }

                SendToDiscord(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount.ToString(), rarecosmetic);

                CustomConsole.Debug("Logged in, Joined room");
            }
        }

        private static bool special = false;
        public static async Task SendToDiscord(string code, string players, string cosmetic)
        {
            string result;
            if (cosmetic.Contains("LBAAD."))
            {
                result = "ADMIN BADGE";
                special = true;
            }
            else if (cosmetic.Contains("LBAAK."))
            {
                result = "STICK";
                special = true;
            }
            else if (cosmetic.Contains("LMAPY."))
            {
                result = "FOREST GUIDE STICK";
                special = true;
            }
            else if (cosmetic.Contains("LBAHG."))
            {
                result = "GT1 BADGE";
                special = true;
            }
            else
            {
                result = "No Rare Cosmetics";
                special = false;
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
                                value = special ? result + $" | USERID: {userid}" : result,
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