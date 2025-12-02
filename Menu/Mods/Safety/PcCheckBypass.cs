using Colossal.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace Colossal.Mods {
    public class PcCheckBypass : MonoBehaviour {
        public void Update() 
        {
            if (PluginConfig.pccheckbypass)
            {
                if (GameObject.Find("Mountain/Geometry/goodigloo").activeSelf)
                    GameObject.Find("Mountain/Geometry/goodigloo").SetActive(false);
            }
            else
            {
                if (!GameObject.Find("Mountain/Geometry/goodigloo").activeSelf)
                    GameObject.Find("Mountain/Geometry/goodigloo").SetActive(true);
                Destroy(this.GetComponent<PcCheckBypass>());
            }
        }
    }
}
