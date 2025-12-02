using Colossal.Menu;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Colossal.Mods
{
    public class HzHands : MonoBehaviour
    {
        private static readonly int[] vols = { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
        private int vol;

        public void Update()
        {
            int Setting = PluginConfig.hzhands;
            Setting = Mathf.Clamp(Setting - 1, 0, vols.Length - 1);

            if (Setting == 0)
            {
                GorillaLocomotion.GTPlayer.Instance.velocityHistorySize = 6;
                Destroy(this.GetComponent<HzHands>());
            }
            else
            {
                vol = vols[Setting];
                if(GorillaLocomotion.GTPlayer.Instance.velocityHistorySize != vol)
                {
                    GorillaLocomotion.GTPlayer.Instance.velocityHistorySize = vol;
                    GorillaLocomotion.GTPlayer.Instance.InitializeValues();
                }
            }
        }
    }
}
