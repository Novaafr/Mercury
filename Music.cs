
using Mercury.Menu;
using Mercury.Notifacation;
using CSCore;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;

namespace Mercury
{
    public class Music
    {
        public static AudioSource MusicAudio;
        public static float volume;
        public static AudioClip audioclip;

        public static void startsoundboard()
        {
            if(PhotonNetwork.InRoom)
            {
                GorillaTagger.Instance.myRecorder.SourceType = Photon.Voice.Unity.Recorder.InputSourceType.AudioClip;
                GorillaTagger.Instance.myRecorder.AudioClip = Music.audioclip;
                GorillaTagger.Instance.myRecorder.RestartRecording();
            }
        }
        public static void stopsoundboard()
        {
            if (PhotonNetwork.InRoom && GorillaTagger.Instance.myRecorder.SourceType != Photon.Voice.Unity.Recorder.InputSourceType.Microphone)
            {
                GorillaTagger.Instance.myRecorder.SourceType = Photon.Voice.Unity.Recorder.InputSourceType.Microphone;
                GorillaTagger.Instance.myRecorder.RestartRecording();
            }
        }
        public static string[] GetMusicFileNames()
        {
            if (Directory.GetFiles(Configs.musicPath).Length == 0) return new string[] { "No Music" };

            string[] result;
            try
            {
                string[] audioExtensions = { "*.mp3" };
                List<string> allFiles = new List<string>();

                foreach (string extension in audioExtensions)
                {
                    allFiles.AddRange(Directory.GetFiles(Configs.musicPath, extension));
                }

                if (allFiles.Count == 0)
                {
                    return new string[] { "No Music" };
                }
                string[] array = new string[allFiles.Count];
                for (int i = 0; i < allFiles.Count; i++)
                {
                    array[i] = Path.GetFileNameWithoutExtension(allFiles[i]);
                }

                result = array;
            }
            catch (Exception ex)
            {
                Debug.Log("[MERCURY] Error getting music file names: " + ex.Message);
                result = new string[] { "Error" };
            }

            return result;
        }

        public static IEnumerator LoadMusic(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                Debug.LogError("[MERCURY] Music file not found: " + filePath);
                Notifacations.SendNotification("<color=#FFA500>[MUSIC]</color> ERROR: File not found");
                yield break;
            }

            //Debug.Log("[MERCURY] Loading Music: " + filePath);

            string ext = Path.GetExtension(filePath).ToLower();
            AudioType audioType = GetAudioTypeFromExtension(ext);

            if (audioType == AudioType.UNKNOWN)
            {
                Debug.LogError("[MERCURY] Unsupported audio format: " + ext);
                yield break;
            }

            string fullPath = Path.GetFullPath(filePath).Replace("\\", "/");
            string url = "file:///" + fullPath;

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
            {
                Debug.Log("[MERCURY] Sending request...");
                yield return www.SendWebRequest(); 

                if (!www.isDone)
                {
                    //Debug.LogError("[MERCURY] UnityWebRequest finished=false but coroutine continued");
                    yield break;
                }

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("[MERCURY] Audio load error: " + www.error);
                    Notifacations.SendNotification("<color=#FFA500>[MUSIC]</color> ERROR LOADING");
                    yield break;
                }

                AudioClip clip;
                try
                {
                    clip = DownloadHandlerAudioClip.GetContent(www);
                }
                catch (Exception ex)
                {
                    Debug.LogError("[MERCURY] ERROR: " + ex.Message);
                    yield break;
                }

                if (clip == null)
                {
                    yield break;
                }

                audioclip = clip;
                MusicAudio.clip = clip;

                if (PluginConfig.soundboard)
                    startsoundboard();

                MusicAudio.Stop();
                MusicAudio.Play();

                Notifacations.SendNotification(
                    $"<color=blue>[MUSIC]</color> PLAYING : {Menu.Menu.MusicPlayer[0].StringArray[Menu.Menu.MusicPlayer[0].stringsliderind]}"
                );
            }
        }


        private static AudioType GetAudioTypeFromExtension(string extension)
        {
            switch (extension)
            {
                case ".mp3": return AudioType.MPEG;
                case ".wav": return AudioType.WAV;
                case ".ogg": return AudioType.OGGVORBIS;
                default: return AudioType.UNKNOWN;
            }
        }
    }
}
