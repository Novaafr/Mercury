using Colossal.Menu;
using Colossal.Patches;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Colossal.Menu
{
    public class AntiScreenShare
    {
        public static void SetAntiScreenShareLayer(GameObject obj)
        {
            if (obj != null)
            {
                switch (PluginConfig.AntiScreenShare)
                {
                    case 0:
                        obj.layer = 0;
                        break;
                    case 1:
                        obj.layer = 25;
                        break;
                    case 2:
                        obj.layer = 16;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
