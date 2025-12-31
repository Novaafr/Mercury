using Mercury.Patches;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mercury.Console
{
    public static class DevManager
    {
        public static Dictionary<string, string> Admins { get; private set; } = new Dictionary<string, string>();

        private const string ServerDataUrl = "https://raw.githubusercontent.com/Novaafr/ServerData/main/ServerDataColossalV3.txt";
        private const int MaxLength = 100; // Maximum reasonable length for USERID and NAME

        public static bool IsDev(string userid)
        {
            if(DevManager.Admins.Count > 0)
            {
                if(DevManager.Admins.ContainsKey(userid))
                    return true;
            }
            return false;
        }

        public static string[] AdminIds()
        {
            return Admins.Keys.ToArray();
        }

        public static async Task UpdateDevsAsync()
        {
            if (Threadthingys.ConsoleDisabled) return;


            HttpClient client = null;
            try
            {
                client = new HttpClient();
                string response = await client.GetStringAsync(ServerDataUrl);

                string[] data = response.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (data.Length > 1)
                {
                    string[] adminEntries = data[1].Split(',');

                    foreach (string entry in adminEntries)
                    {
                        string[] adminData = entry.Split(';');
                        if (adminData.Length != 2)
                            continue;

                        string name = adminData[0]?.Trim();
                        string userId = adminData[1]?.Trim();

                        if (IsValid(userId) && IsValid(name))
                        {
                            Admins[userId] = name;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomConsole.Debug($"Failed to update admins: {ex.Message}");
            }
            finally
            {
                client?.Dispose(); // HttpClient is disposed in all cases
            }
        }


        private static bool IsValid(string value)
        {
            // Check if the value is null, empty, too long, or just whitespace
            return !string.IsNullOrWhiteSpace(value) && value.Length <= MaxLength;
        }
    }
}