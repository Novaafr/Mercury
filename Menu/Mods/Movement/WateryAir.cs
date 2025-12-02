using Colossal.Menu;
using Colossal.Patches;
using GorillaNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;

namespace Colossal.Mods {
    public class WateryAir : MonoBehaviour {
        private GameObject waterbox;
        public void Update() 
        {
            if (PluginConfig.wateryair) 
            {
                if (waterbox == null)
                {
                    GameObject gameObject = GameObject.Find("Environment Objects/LocalObjects_Prefab/ForestToBeach/ForestToBeach_Prefab_V4/CaveWaterVolume");

                    if (!gameObject.activeSelf)
                        gameObject.SetActive(true);
                    waterbox = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    if (waterbox.GetComponent<Renderer>() != null)
                        GameObject.Destroy(waterbox.GetComponent<Renderer>());
                }


                string bind = CustomBinding.GetBinds("wateryair");
                if (string.IsNullOrEmpty(bind) || bind == "UNBOUND")
                {
                    return;
                }

                // Get mirrored versions for left and right hand
                string leftBind = CustomBinding.MirrorBind(bind, true);
                string rightBind = CustomBinding.MirrorBind(bind, false);


                if (ControlsV2.GetControl(leftBind) && ControlsV2.GetControl(rightBind))
                    waterbox.transform.position = GorillaTagger.Instance.headCollider.transform.position + new Vector3(0f, 1f, 0f);
                else if(waterbox != null)
                    GameObject.Destroy(waterbox);
            }
            else 
            {
                Destroy(this.GetComponent<WateryAir>());
                if (waterbox != null) 
                    GameObject.Destroy(waterbox);
            }
        }
    }
}
