using BepInEx;
using Colossal.Menu;
using UnityEngine;

namespace Colossal.Mods
{
    public class Timer : MonoBehaviour
    {
        public static float timespeed;
        private static readonly float[] timeScales = { 0f, 1.03f, 1.06f, 1.09f, 1.1f, 1.13f, 1.16f, 1.19f, 1.2f, 1.23f, 1.26f, 1.29f, 1.3f, 2f, 3f, 4f, 5f };

        public void Update()
        {
            int timerSetting = PluginConfig.Timer;

            if (timerSetting == 0)
            {
                if (Time.timeScale != 1)
                    Time.timeScale = 1;

                Destroy(this.GetComponent<Timer>());
            }
            else
            {
                timespeed = timeScales[Mathf.Min(timerSetting, timeScales.Length - 1)];
                Time.timeScale = timespeed;
            }
        }
    }
}
