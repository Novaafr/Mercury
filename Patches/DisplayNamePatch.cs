using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using PlayFab.ClientModels;
using PlayFab;

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(PlayFabClientAPI), "UpdateUserTitleDisplayName")]
    public class DisplayNamePatch
    {
        public static void Prefix(ref UpdateUserTitleDisplayNameRequest request, Action<UpdateUserTitleDisplayNameResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null) =>
            request.DisplayName = UnityEngine.Random.Range(0, 9999).ToString();
    }
}
