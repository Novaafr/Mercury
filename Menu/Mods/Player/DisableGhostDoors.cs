﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Colossal.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Colossal.Mods
{
    public class DisableGhostDoors : MonoBehaviour
    {
        private GameObject doors;
        private GameObject door2;
        private GameObject door3;
        public void Update()
        {
            if (!PluginConfig.disableghostdoors)
            {
                if (!doors.activeSelf)
                    doors.SetActive(true);

                if (!door2.activeSelf)
                    door2.SetActive(true);

                if (!door3.activeSelf)
                    door3.SetActive(true);

                Destroy(this.GetComponent<DisableGhostDoors>());
            }

            if (doors == null)
                doors = GameObject.Find("GhostReactorRoot/GhostReactorZone/GhostReactorReadyRoom_Prefab/EntranceDoors");
            else
            {
                if (doors.activeSelf)
                    doors.SetActive(false);
            }

            if (door2 == null)
                door2 = GameObject.Find("GhostReactorRoot/GhostReactorZone/GhostReactorShiftManager/GhostReactorEnergyCostGate/EnergyGate/EnergyGate_Door_Bottom");
            else
            {
                if (door2.activeSelf)
                    door2.SetActive(false);
            }

            if (door3 == null)
                door3 = GameObject.Find("GhostReactorRoot/GhostReactorZone/GhostReactorShiftManager/GhostReactorEnergyCostGate/EnergyGate/EnergyGate_Door_Top");
            else
            {
                if (door3.activeSelf)
                    door3.SetActive(false);
            }
        }
    }
}