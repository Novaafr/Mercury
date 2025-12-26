using HarmonyLib;
using JetBrains.Annotations;
using Liv.Lck.Telemetry;
using Photon.Pun;
using PlayFab.EventsModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mercury.Patches
{
    [HarmonyPatch(typeof(GorillaTelemetry), "EnqueueTelemetryEvent")]
    public class TelemetryPatch1
    {
        private static bool Prefix(string eventName, object content, [CanBeNull] string[] customTags = null)
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(GorillaTelemetry), "EnqueueTelemetryEventPlayFab")]
    public class TelemetryPatch2
    {
        private static bool Prefix(EventContents eventContent)
        {
            return false;
        } 
    }

    [HarmonyPatch(typeof(GorillaTelemetry), "FlushPlayFabTelemetry")]
    public class TelemetryPatch3
    {
        private static bool Prefix()
        {
            return false;
        }
    }

    [HarmonyPatch(typeof(GorillaTelemetry), "FlushMothershipTelemetry")]
    public class TelemetryPatch4
    {
        private static bool Prefix()
        {
            return false;
        }
    }

    /*[HarmonyPatch(typeof(ILckTelemetryClient), "SendTelemetry")]
    public class TelemetryPatch5
    {
        private static bool Prefix(LckTelemetryEvent lckTelemetryEvent)
        {
            return false;
        }
    }*/

    [HarmonyPatch(typeof(VRRig), "IncrementRPC", typeof(PhotonMessageInfoWrapped), typeof(string))]
    public class NoIncrementRPC
    {
        private static bool Prefix(PhotonMessageInfoWrapped info, string sourceCall) =>
            false;
    }

    [HarmonyPatch(typeof(GorillaNot), "IncrementRPCCall", typeof(PhotonMessageInfo), typeof(string))]
    public class NoIncrementRPCCall
    {
        private static bool Prefix(PhotonMessageInfo info, string callingMethod = "") =>
            false;
    }

    [HarmonyPatch(typeof(GorillaNot), "IncrementRPCCallLocal")]
    public class NoIncrementRPCCallLocal
    {
        private static bool Prefix(PhotonMessageInfoWrapped infoWrapped, string rpcFunction) =>
            false;
    }
}
