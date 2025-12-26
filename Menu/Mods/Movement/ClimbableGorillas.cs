using BepInEx;
using Mercury.Menu;
using GorillaLocomotion.Climbing;
using Photon.Pun;
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
    public class ClimbableGorillas : MonoBehaviour
    {
        void Start()
        {
            foreach (GameObject Gos in Resources.FindObjectsOfTypeAll<GameObject>())
                if (Gos.name == "BodyTrigger")
                    if (Gos.GetComponent<GorillaClimbable>() == null)
                        Gos.AddComponent<GorillaClimbable>().colliderCache = Gos.GetComponent<Collider>();
        }
        public void Update()
        {
            if(!PluginConfig.ClimbableGorillas)
            {
                foreach (GameObject Gos in Resources.FindObjectsOfTypeAll<GameObject>())
                    if (Gos.name == "BodyTrigger")
                        if (Gos.GetComponent<GorillaClimbable>() != null)
                            Destroy(Gos.GetComponent<GorillaClimbable>());

                Destroy(this.GetComponent<ClimbableGorillas>());
            }
        }
    }
}
