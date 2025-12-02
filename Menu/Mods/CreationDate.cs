using Colossal;
using PlayFab.ClientModels;
using PlayFab;
using System;
using UnityEngine;
using System.Threading.Tasks;

namespace Colossal.Menu
{
    public static class CreationDate
    {
        private static float RequestTimeoutSeconds = 10f;

        public static async Task<string> GetCreationDateAsync(VRRig vrrig)
        {
            var request = new GetAccountInfoRequest
            {
                PlayFabId = vrrig.Creator.UserId
            };

            try
            {
                GetAccountInfoResult result = null;
                bool requestCompleted = false;

                PlayFabClientAPI.GetAccountInfo(request,
                    (response) =>
                    {
                        result = response;
                        requestCompleted = true;
                    },
                    (error) =>
                    {
                        OnPlayFabError(error, vrrig);
                        requestCompleted = true;
                    });

                float startTime = Time.time;
                while (!requestCompleted && Time.time - startTime < RequestTimeoutSeconds)
                {
                    await Task.Delay(100);
                }

                if (requestCompleted)
                    return OnAccountInfoReceived(result);
                else
                {
                    Debug.Log($"[COLOSSAL] PlayFab request timed out for user {vrrig.Creator.UserId}");
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Error retrieving account info: {e.Message}");
                return null;
            }
        }
        private static string OnAccountInfoReceived(GetAccountInfoResult result)
        {
            if (result != null && result.AccountInfo != null)
            {
                DateTime creationDateTime = result.AccountInfo.Created;
                return creationDateTime.ToString("yyyy-MM-dd");
            }
            else
                return null;
        }
        private static void OnPlayFabError(PlayFabError error, VRRig vrrig)
        {
            Debug.Log($"[COLOSSAL] PlayFab error: {error.ErrorMessage}");
        }
    }
}
