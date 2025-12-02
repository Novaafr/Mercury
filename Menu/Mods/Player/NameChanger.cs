using System;
using System.Collections;
using System.IO;
using Colossal.Menu;
using Photon.Pun;
using UnityEngine;

namespace Colossal.Mods
{
    public class NameChanger : MonoBehaviour
    {
        public string filepath = "Colossal\\NameChanger.txt";
        private int currentLineIndex;
        private string[] lines;
        public void Start()
        {
            if (Directory.Exists("Colossal"))
            {
                if (!File.Exists(this.filepath))
                {
                    File.WriteAllText(this.filepath, "");
                }
                if (File.Exists(this.filepath))
                {
                    this.lines = File.ReadAllLines(this.filepath);
                    base.StartCoroutine(this.ProcessLinesWithDelay());
                }
            }
        }

        public void Update()
        {
            if (!PluginConfig.namechanger)
            {
                UnityEngine.Object.Destroy(base.GetComponent<NameChanger>());
            }
        }

        private IEnumerator ProcessLinesWithDelay()
        {
            for (; ; )
            {
                yield return new WaitForSeconds(0.3f);
                if (!PhotonNetwork.InRoom)
                {
                    break;
                }
                if (PhotonNetwork.InRoom && this.lines.Length != 0)
                {
                    string text = this.lines[this.currentLineIndex];
                    Debug.Log(text);
                    PhotonNetwork.LocalPlayer.NickName = text;
                    this.currentLineIndex = (this.currentLineIndex + 1) % this.lines.Length;
                }
            }
            yield break;
        }
    }
}
