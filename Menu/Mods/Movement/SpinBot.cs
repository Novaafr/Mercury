using Mercury.Menu;
using Mercury.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

namespace Mercury.Mods
{
    public class SpinBot : MonoBehaviour
    {
        private GameObject ghost;
        public void Update()
        {
            if (PluginConfig.SpinBot)
            {
                if (ghost == null)
                    ghost = GhostManager.SpawnGhost();

                if (ghost != null)
                {
                    //if (DisableRig.disablerig)
                    //    DisableRig.disablerig = false;

                    VRRig vrrig = ghost.GetComponent<VRRig>();
                    vrrig.mainSkin.material.color = GhostManager.ghostColor;
                    vrrig.mainSkin.material.shader = Shader.Find("GUI/Text Shader");

                    if (GorillaTagger.Instance.offlineVRRig.enabled)
                        GorillaTagger.Instance.offlineVRRig.enabled = false;


                    GorillaTagger.Instance.offlineVRRig.transform.Rotate(Vector3.up * 250 * Time.deltaTime);


                    GorillaTagger.Instance.offlineVRRig.transform.position = vrrig.transform.position;

                   // GorillaTagger.Instance.offlineVRRig.rightHandPlayer.transform.position = vrrig.rightHandPlayer.transform.position;
                    //GorillaTagger.Instance.offlineVRRig.leftHandPlayer.transform.position = vrrig.leftHandPlayer.transform.position;

                    //GorillaTagger.Instance.offlineVRRig.headConstraint.transform.rotation = vrrig.headConstraint.transform.rotation;
                }
            }
            else
            {
                if (!GorillaTagger.Instance.offlineVRRig.enabled)
                    GorillaTagger.Instance.offlineVRRig.enabled = true;

                if (ghost != null)
                    GhostManager.DestroyGhost(ghost);

                Destroy(this.GetComponent<SpinBot>());
            }
        }
    }
}
