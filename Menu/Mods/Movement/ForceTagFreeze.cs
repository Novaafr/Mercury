using Colossal.Menu;
using Colossal.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace Colossal.Mods
{
    public class ForceTagFreeze : MonoBehaviour
    {
        public void Update()
        {
            if (PluginConfig.forcetagfreeze)
                GorillaLocomotion.GTPlayer.Instance.disableMovement = true;
            else
            {
                GorillaLocomotion.GTPlayer.Instance.disableMovement = false;
                Destroy(this.GetComponent<ForceTagFreeze>());
            }
        }
    }
}
