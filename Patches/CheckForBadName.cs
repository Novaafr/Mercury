﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GorillaNetworking;
using HarmonyLib;
using PlayFab.CloudScriptModels;
using PlayFab;

namespace Colossal.Patches
{
    //[HarmonyPatch(typeof(GorillaServer), "CheckForBadName")]
    public static class CheckForBadName
    {
        static bool Prefix()
        {
            return false;
        }
    }
}