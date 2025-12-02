using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace Colossal.Patches
{
    public class ControlsV2
    {
        public static bool GetControl(string controlName)
        {
            switch (controlName)
            {
                case "LJoystick":
                    return ControlsV2.LeftJoystick();
                case "RJoystick":
                    return ControlsV2.RightJoystick();
                case "RTrigger":
                    return ControlsV2.RightTrigger();
                case "LTrigger":
                    return ControlsV2.LeftTrigger();
                case "RGrip":
                    return ControlsV2.RightGrip();
                case "LGrip":
                    return ControlsV2.LeftGrip();
                case "LPrimary":
                    return ControlsV2.LeftPrimaryButton();
                case "RPrimary":
                    return ControlsV2.RightPrimaryButton();
                case "LSecondary":
                    return ControlsV2.LeftSecondaryButton();
                case "RSecondary":
                    return ControlsV2.RightSecondaryButton();
                default:
                    return false;
            }
        }

        public static bool LeftJoystick()
        {
            bool Value;
            if (WhatAmI.oculus)
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out Value);
            else
                Value = SteamVR_Actions.gorillaTag_LeftJoystickClick.GetState(SteamVR_Input_Sources.LeftHand);
            return Value;
        }

        public static Vector2 LeftJoystickAxis()
        {
            Vector2 Value;
            if (WhatAmI.oculus)
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out Value);
            else
                Value = SteamVR_Actions.gorillaTag_LeftJoystick2DAxis.axis;
            return Value;
        }

        public static bool RightJoystick()
        {
            bool Value;
            if (WhatAmI.oculus)
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primary2DAxisClick, out Value);
            else
                Value = SteamVR_Actions.gorillaTag_RightJoystickClick.GetState(SteamVR_Input_Sources.RightHand);
            return Value;
        }

        public static Vector2 RightJoystickAxis()
        {
            Vector2 Value;
            if (WhatAmI.oculus)
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primary2DAxis, out Value);
            else
                Value = SteamVR_Actions.gorillaTag_RightJoystick2DAxis.axis;
            return Value;
        }

        public static bool RightTrigger()
        {
            bool Value;
            if (WhatAmI.oculus)
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.triggerButton, out Value);
            else
                Value = SteamVR_Actions.gorillaTag_RightTriggerClick.GetState(SteamVR_Input_Sources.RightHand);
            return Value;
        }

        public static bool LeftTrigger()
        {
            bool Value;
            if (WhatAmI.oculus)
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.triggerButton, out Value);
            else
                Value = SteamVR_Actions.gorillaTag_LeftTriggerClick.GetState(SteamVR_Input_Sources.LeftHand);
            return Value;
        }

        public static bool RightGrip()
        {
            bool Value;
            if (WhatAmI.oculus)
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.gripButton, out Value);
            else
                Value = SteamVR_Actions.gorillaTag_RightGripClick.GetState(SteamVR_Input_Sources.RightHand);
            return Value;
        }

        public static bool LeftGrip()
        {
            bool Value;
            if (WhatAmI.oculus)
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.gripButton, out Value);
            else
                Value = SteamVR_Actions.gorillaTag_LeftGripClick.GetState(SteamVR_Input_Sources.LeftHand);
            return Value;
        }

        public static bool LeftPrimaryButton()
        {
            bool Value;
            if (WhatAmI.oculus)
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.primaryButton, out Value);
            else
                Value = SteamVR_Actions.gorillaTag_LeftPrimaryClick.GetState(SteamVR_Input_Sources.LeftHand);
            return Value;
        }

        public static bool RightPrimaryButton()
        {
            bool Value;
            if (WhatAmI.oculus)
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.primaryButton, out Value);
            else
                Value = SteamVR_Actions.gorillaTag_RightPrimaryClick.GetState(SteamVR_Input_Sources.RightHand);
            return Value;
        }

        public static bool LeftSecondaryButton()
        {
            bool Value;
            if (WhatAmI.oculus)
                InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.secondaryButton, out Value);
            else
                Value = SteamVR_Actions.gorillaTag_LeftSecondaryClick.GetState(SteamVR_Input_Sources.LeftHand);
            return Value;
        }

        public static bool RightSecondaryButton()
        {
            bool Value;
            if (WhatAmI.oculus)
                InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.secondaryButton, out Value);
            else
                Value = SteamVR_Actions.gorillaTag_RightSecondaryClick.GetState(SteamVR_Input_Sources.RightHand);
            return Value;
        }
    }
}
