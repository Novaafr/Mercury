using Colossal.Menu;
using Colossal.Patches;
using Photon.Pun;
using UnityEngine;

namespace Colossal.Mods
{
    public class SpeedMod : MonoBehaviour
    {
        float[] speeds = { 7f, 7.2f, 7.4f, 7.6f, 7.8f, 8f, 8.2f, 8.4f, 8.6f };

        public void Update()
        {
            bool speedApplied = false;

            // Apply default speed boost if enabled
            if (PluginConfig.speed > 0)
            {
                SetJumpSpeed(speeds[PluginConfig.speed]);
                speedApplied = true;
            }

            // Get speedbind and mirrored bind
            string speedBind = CustomBinding.GetBinds("speedbind");
            if (string.IsNullOrEmpty(speedBind) || speedBind == "UNBOUND") return;

            // Apply speed if bound key is pressed
            if (ControlsV2.GetControl(speedBind))
            {
                SetJumpSpeed(speeds[PluginConfig.speed]);
                speedApplied = true;
            }

            // Near infected speed boost
            if (PluginConfig.nearspeed > 0)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (!vrrig.isOfflineVRRig && !WhatAmI.IsInfected(PhotonNetwork.LocalPlayer))
                    {
                        if (WhatAmI.IsInfected(vrrig.Creator))
                        {
                            if (PluginConfig.nearspeeddistance <= Vector3.Distance(GorillaTagger.Instance.transform.position, vrrig.transform.position))
                            {
                                SetJumpSpeed(speeds[PluginConfig.nearspeed]);
                                speedApplied = true;
                            }
                        }
                    }
                }
            }

            // Reset speed if no speed mode is active
            if (!speedApplied)
            {
                ResetJumpSpeed();
            }
        }

        void SetJumpSpeed(float speed)
        {
            GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed = speed;
        }

        void ResetJumpSpeed()
        {
            GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed = WhatAmI.GetDefaultSpeeds();
        }
    }
}
