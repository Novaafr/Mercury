using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Colossal.Patches;
using Photon.Pun;

namespace Colossal.Console
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
        public static async Task UpdateDevsAsync()
        {
            if (Threadthingys.ConsoleDisabled) return;


            HttpClient client = null;
            try
            {
                client = new HttpClient();
                string response = await client.GetStringAsync(ServerDataUrl);

                // Split and process the response
                string[] data = response.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (data.Length > 1)
                {
                    string[] adminEntries = data[1].Split(',');

                    foreach (string entry in adminEntries)
                    {
                        string[] adminData = entry.Split(';');
                        if (adminData.Length == 2)
                        {
                            string userId = adminData[0]?.Trim();
                            string name = adminData[1]?.Trim();

                            // Validate USERID and NAME
                            if (IsValid(userId) && IsValid(name))
                            {
                                // Find if the name already exists
                                string existingUserId = null;
                                foreach (var kvp in Admins)
                                {
                                    if (kvp.Value == name)
                                    {
                                        existingUserId = kvp.Key;
                                        break;
                                    }
                                }

                                if (existingUserId != null)
                                {
                                    // Update the userId for the existing name
                                    Admins.Remove(existingUserId);
                                    Admins[userId] = name;
                                }
                                else
                                {
                                    // Add new entry if the name doesn't exist
                                    Admins.Add(userId, name);
                                }
                            }
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