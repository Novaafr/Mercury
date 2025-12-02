
using Colossal.Menu;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Audio;
using CSCore;
using System.Net;
using Colossal.Notifacation;
using Photon.Pun;

namespace Colossal
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
                Debug.Log("[COLOSSAL] Error getting music file names: " + ex.Message);
                result = new string[] { "Error" };
            }

            return result;
        }

        public static void LoadMusic(string filePath)
        {
            try
            {
                Debug.Log("[COLOSSAL] Loading Music");

                if (File.Exists(filePath))
                {
                    try
                    {
                        string extension = System.IO.Path.GetExtension(filePath).ToLower();
                        AudioType audioType = GetAudioTypeFromExtension(extension);

                        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///" + filePath, audioType);
                        www.SendWebRequest();

                        // Wait for the request to complete
                        while (!www.isDone)
                        {
                        }

                        if (www.result == UnityWebRequest.Result.Success)
                        {
                            audioclip = DownloadHandlerAudioClip.GetContent(www);
                            if (audioclip != null)
                            {
                                MusicAudio.clip = audioclip;

                                if (!MusicAudio.isPlaying)
                                {
                                    if(PluginConfig.soundboard)
                                        startsoundboard();
                                    MusicAudio.Play();
                                }
                                else
                                {
                                    if (PluginConfig.soundboard)
                                        startsoundboard();
                                    MusicAudio.Stop();
                                    MusicAudio.Play();
                                }
                            }
                        }
                        else
                        {
                            Debug.LogError("Error loading audio file: " + www.error);
                        }

                        Notifacations.SendNotification($"<color=blue>[MUSIC]</color> PLAYING : {Menu.Menu.MusicPlayer[0].StringArray[Menu.Menu.MusicPlayer[0].stringsliderind]}");
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("Error loading music: " + ex.Message);
                        Notifacations.SendNotification($"<color=#FFA500>[MUSIC]</color> ERROR : {filePath}");
                    }
                }
                else
                {
                    Notifacations.SendNotification($"<color=#FFA500>[MUSIC]</color> ERROR : {filePath}");
                }
            }
            catch (Exception ex)
            {
                Notifacations.SendNotification($"<color=#FFA500>[MUSIC]</color> ERROR : {filePath}");
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
