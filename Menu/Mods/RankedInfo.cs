using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Photon.Pun;
using static GorillaTagCompetitiveServerApi;

namespace Colossal.Mods
{
    public static class RankedInfo
    {
        private static readonly float RequestTimeoutSeconds = 10f;
        private static Dictionary<string, (RankedModeProgressionPlatformData pcData, RankedModeProgressionPlatformData questData)> playerTierData = new Dictionary<string, (RankedModeProgressionPlatformData, RankedModeProgressionPlatformData)>();
        private static HashSet<string> requestedIds = new HashSet<string>();
        private static bool hasFetchedRankData = false;

        public static async Task<bool> FetchCompetitiveDataAsync()
        {
            if (hasFetchedRankData || !PhotonNetwork.InRoom)
            {
                return hasFetchedRankData;
            }

            List<string> playfabIds = new List<string>();
            foreach (Photon.Realtime.Player p in PhotonNetwork.PlayerList)
            {
                if (!string.IsNullOrEmpty(p.UserId) && !requestedIds.Contains(p.UserId))
                {
                    playfabIds.Add(p.UserId);
                    requestedIds.Add(p.UserId);
                }
            }

            if (playfabIds.Count == 0)
            {
                return false;
            }

            try
            {
                RankedModeProgressionData result = null;
                bool requestCompleted = false;

                GorillaTagCompetitiveServerApi.Instance.RequestGetRankInformation(
                    playfabIds,
                    (response) =>
                    {
                        result = response;
                        requestCompleted = true;
                    });

                float startTime = Time.time;
                while (!requestCompleted && Time.time - startTime < RequestTimeoutSeconds)
                {
                    await Task.Delay(100);
                }

                if (requestCompleted && result != null && result.playerData != null && result.playerData.Count > 0)
                {
                    foreach (var player in result.playerData)
                    {
                        RankedModeProgressionPlatformData pcData = Array.Find(player.platformData, p => p.platform == "PC");
                        RankedModeProgressionPlatformData questData = Array.Find(player.platformData, p => p.platform == "Quest");
                        playerTierData[player.playfabID] = (pcData, questData);
                        requestedIds.Remove(player.playfabID);
                    }
                    hasFetchedRankData = true;
                    return true;
                }
                else
                {
                    foreach (var id in playfabIds)
                    {
                        requestedIds.Remove(id);
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                foreach (var id in playfabIds)
                {
                    requestedIds.Remove(id);
                }
                return false;
            }
        }

        public static Dictionary<string, (RankedModeProgressionPlatformData pcData, RankedModeProgressionPlatformData questData)> GetPlayerTierData()
        {
            return playerTierData;
        }

        public static void ClearData()
        {
            playerTierData.Clear();
            requestedIds.Clear();
            hasFetchedRankData = false;
        }
    }
}