using Colossal.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace Colossal.Mods
{
    public class Panic : MonoBehaviour
    {
        public void Update()
        {
            if (PluginConfig.Panic)
            {
                // All face buttons idk
                if (ControllerInputPoller.instance.leftControllerPrimaryButton && ControllerInputPoller.instance.rightControllerPrimaryButton && ControllerInputPoller.instance.leftControllerSecondaryButton && ControllerInputPoller.instance.rightControllerSecondaryButton)
                {
                    foreach (var prop in typeof(PluginConfig).GetFields(BindingFlags.Public | BindingFlags.Static))
                        prop.SetValue(null, false);
                }
            }
            else
                Destroy(this.GetComponent<Panic>());
        }
    }
}
