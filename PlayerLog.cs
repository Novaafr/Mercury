using Mercury;
using Mercury.Menu;
using Mercury.Mods;
using Mercury.Patches;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

namespace Mercury
{
    internal class PlayerLog : MonoBehaviour
    {
        private List<string> cachedLogs = new List<string>();
        private float updateInterval = 8f;
        private float timeSinceLastUpdate = 0f;

        private void Start() => InvokeRepeating(nameof(UpdateLogFile), updateInterval, updateInterval);

        private async void Update()
        {
            if (PluginConfig.PlayerLogging)
            {
                if (!Directory.Exists(Configs.logPath))
                    Directory.CreateDirectory(Configs.logPath);

                if (PhotonNetwork.InRoom)
                {
                    foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                    {
                        string info = $"{DateTime.Now},{vrrig.Creator.NickName},{vrrig.Creator.UserId},{vrrig.rawCosmeticString}";

                        bool found = false;
                        for (int i = 0; i < cachedLogs.Count; i++)
                        {
                            if (cachedLogs[i].Contains(vrrig.Creator.UserId))
                            {
                                cachedLogs[i] = info;
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                            cachedLogs.Add(info);
                    }
                }
            }
            else
                Destroy(Plugin.holder.GetComponent<PlayerLog>());
        }

        private void UpdateLogFile()
        {
            if (cachedLogs.Count > 0)
            {
                string[] lines = File.Exists($"{Configs.logPath}\\PlayerLog.txt") ? File.ReadAllLines($"{Configs.logPath}\\PlayerLog.txt") : new string[0];
                foreach (string log in cachedLogs)
                {
                    bool updated = false;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains(log.Split(',')[2])) 
                        {
                            lines[i] = log;
                            updated = true;
                            break;
                        }
                    }

                    if (!updated)
                    {
                        Array.Resize(ref lines, lines.Length + 1);
                        lines[lines.Length - 1] = log;
                    }
                }

                File.WriteAllLines($"{Configs.logPath}\\PlayerLog.txt", lines);

                cachedLogs.Clear();
            }
        }

        private void OnApplicationQuit() => UpdateLogFile();
    }
}
