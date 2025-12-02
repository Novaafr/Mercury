using Colossal.Menu;
using Colossal.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;

namespace Colossal.Mods {
    public class LongArm : MonoBehaviour {
        private float armlenght = 1;
        public void Update() {
            if (PluginConfig.longarms) {
                if (Controls.LeftTrigger() && Controls.RightJoystick())
                {
                    this.armlenght -= 0.01f;
                    GorillaTagger.Instance.transform.localScale = new Vector3(this.armlenght, this.armlenght, this.armlenght);
                }
                if (Controls.RightTrigger() && Controls.RightJoystick())
                {
                    this.armlenght += 0.01f;
                    GorillaTagger.Instance.transform.localScale = new Vector3(this.armlenght, this.armlenght, this.armlenght);
                }
                if (Controls.RightTrigger() && Controls.LeftTrigger() && Controls.RightJoystick())
                {
                    GorillaTagger.Instance.transform.localScale = new Vector3(1f, 1f, 1f);
                    return;
                }
            } else {
                Destroy(this.GetComponent<LongArm>());
                GorillaTagger.Instance.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
