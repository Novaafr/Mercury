using Colossal.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace Colossal.Mods {
    public class WhyIsEveryoneLookingAtMe : MonoBehaviour {
        public void Update() {
            if (PluginConfig.whyiseveryonelookingatme) {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs) 
                {
                    if (!vrrig.isOfflineVRRig) 
                        vrrig.headConstraint.LookAt(GorillaTagger.Instance.headCollider.transform);
                }
            } 
            else 
                Destroy(this.GetComponent<WhyIsEveryoneLookingAtMe>());
        }
    }
}
