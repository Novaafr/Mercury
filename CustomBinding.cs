using System.Collections.Generic;
using System.Diagnostics;
using Mercury.Patches;
using UnityEngine;

namespace Mercury.Menu
{
    public class CustomBinding : MonoBehaviour
    {
        public static CustomBinding Instance;
        private bool isListeningForBind = false;
        private string bindingTargetKey = null;
        private bool waitingForRelease = false; // New flag to wait for all buttons to be released

        private void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            if (isListeningForBind)
            {
                if (waitingForRelease)
                {
                    // If any input is still being held, keep waiting
                    if (AnyInputPressed()) return;

                    // Once all inputs are released, allow new binding
                    waitingForRelease = false;
                }
                else
                {
                    CheckBindings();
                }
            }
        }

        private void CheckBindings()
        {
            Dictionary<string, bool> inputChecks = new Dictionary<string, bool>
                    {
                        { "LJoystick", ControlsV2.LeftJoystick() },
                        { "RJoystick", ControlsV2.RightJoystick() },
                        { "RTrigger", ControlsV2.RightTrigger() },
                        { "LTrigger", ControlsV2.LeftTrigger() },
                        { "RGrip", ControlsV2.RightGrip() },
                        { "LGrip", ControlsV2.LeftGrip() },
                        { "LPrimary", ControlsV2.LeftPrimaryButton() },
                        { "RPrimary", ControlsV2.RightPrimaryButton() },
                        { "LSecondary", ControlsV2.LeftSecondaryButton() },
                        { "RSecondary", ControlsV2.RightSecondaryButton() }
                    };

            foreach (var input in inputChecks)
            {
                if (input.Value) // Trigger on press
                {
                    AddBindKey(bindingTargetKey, input.Key);
                    isListeningForBind = false;
                    return;
                }
            }
        }

        public void StartListeningForBind(string featureKey)
        {
            if (isListeningForBind) return;

            isListeningForBind = true;
            bindingTargetKey = featureKey;
            waitingForRelease = true; // Start by waiting for all buttons to be released
        }

        private bool AnyInputPressed()
        {
            return ControlsV2.LeftJoystick() || ControlsV2.RightJoystick() ||
                   ControlsV2.RightTrigger() || ControlsV2.LeftTrigger() ||
                   ControlsV2.RightGrip() || ControlsV2.LeftGrip() ||
                   ControlsV2.LeftPrimaryButton() || ControlsV2.RightPrimaryButton() ||
                   ControlsV2.LeftSecondaryButton() || ControlsV2.RightSecondaryButton();
        }

        public static void AddBindKey(string featureKey, string key)
        {
            var field = typeof(PluginConfig).GetField(featureKey.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower() + "_bind");

            if (field != null)
            {
                field.SetValue(null, key); // Overwrite with new key
            }
        }

        public static string GetBinds(string featureKey)
        {
            var field = typeof(PluginConfig).GetField(featureKey.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower() + "_bind");

            if (field != null)
            {
                string bind = (string)field.GetValue(null);

                if (string.IsNullOrWhiteSpace(bind))
                    return "UNBOUND"; // No bind set

                return bind; // Return only the single bind
            }

            return ""; // Hide if `_bind` does not exist
        }

        public static string MirrorBind(string bind, bool isLeftHand)
        {
            switch (bind)
            {
                case "LTrigger": return isLeftHand ? "LTrigger" : "RTrigger";
                case "RTrigger": return isLeftHand ? "LTrigger" : "RTrigger";
                case "LGrip": return isLeftHand ? "LGrip" : "RGrip";
                case "RGrip": return isLeftHand ? "LGrip" : "RGrip";
                case "LPrimary": return isLeftHand ? "LPrimary" : "RPrimary";
                case "RPrimary": return isLeftHand ? "LPrimary" : "RPrimary";
                case "LSecondary": return isLeftHand ? "LSecondary" : "RSecondary";
                case "RSecondary": return isLeftHand ? "LSecondary" : "RSecondary";
                case "LeftJoystick": return isLeftHand ? "LeftJoystick" : "RightJoystick";
                case "RightJoystick": return isLeftHand ? "LeftJoystick" : "RightJoystick";
                default: return bind; // If it's an unrecognized input, return as is.
            }
        }

        public static void ClearBinds(string featureKey)
        {
            var field = typeof(PluginConfig).GetField(featureKey.Replace(" ", "").Replace("(", "").Replace(")", "").ToLower() + "_bind");
            if (field != null)
            {
                field.SetValue(null, "");
            }
        }
    }
}
