﻿using Colossal;
using Colossal.Menu;
using GorillaGameModes;
using HarmonyLib;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(GameMode), "ReportTag")]
    internal class ComicTag
    {
        public static GameObject comictext;
        public static VRRig taggedvrrig;
        public static HashSet<NetPlayer> executedPlayers = new HashSet<NetPlayer>();

        private static void Postfix(NetPlayer player)
        {
            player = null;

            if (player != null)
            {
                if (PluginConfig.ComicTags)
                {
                    if (!executedPlayers.Contains(player))
                    {
                        executedPlayers.Add(player);

                        float radius = 0.7f;
                        float randomAngle = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
                        float randomDistance = UnityEngine.Random.Range(0f, radius);
                        Vector3 randomPosition = new Vector3(
                            taggedvrrig.transform.position.x + randomDistance * Mathf.Cos(randomAngle),
                            taggedvrrig.transform.position.y + randomDistance * Mathf.Sin(randomAngle),
                            taggedvrrig.transform.position.z
                        );

                        comictext = GameObject.Instantiate(AssetBundleLoader.Comic_Canvas);
                        if (comictext != null)
                        {
                            Debug.Log("Spawned Comic Text");

                            comictext.transform.position = randomPosition + new Vector3(1.8f, -0.4f, 0);
                            comictext.AddComponent<ComicTextManager>();
                        }
                    }
                }
            }

            //taggedPlayer = null;

            //if (__result && hitInfo.collider != null)
            //{
            //    VRRig componentInParent = hitInfo.collider.GetComponentInParent<VRRig>();
            //    if (componentInParent != null)
            //    {
            //        taggedvrrig = componentInParent;
            //        taggedPlayer = taggedvrrig.Creator;

            //        if (taggedPlayer != null)
            //        {
            //            if (PluginConfig.ComicTags)
            //            {
            //                if (!executedPlayers.Contains(taggedPlayer))
            //                {
            //                    executedPlayers.Add(taggedPlayer);

            //                    float radius = 0.7f;
            //                    float randomAngle = UnityEngine.Random.Range(0f, 2 * Mathf.PI);
            //                    float randomDistance = UnityEngine.Random.Range(0f, radius);
            //                    Vector3 randomPosition = new Vector3(
            //                        taggedvrrig.transform.position.x + randomDistance * Mathf.Cos(randomAngle),
            //                        taggedvrrig.transform.position.y + randomDistance * Mathf.Sin(randomAngle),
            //                        taggedvrrig.transform.position.z
            //                    );

            //                    comictext = GameObject.Instantiate(GameObject.Find("CCMV2 Custom Assets").transform.GetChild(0).gameObject);
            //                    if (comictext != null)
            //                    {
            //                        comictext.transform.position = randomPosition + new Vector3(1.8f, -0.4f, 0);
            //                        comictext.AddComponent<ComicTextManager>();
            //                    }
            //                    else
            //                    {
            //                        Debug.Log("[COLOSSAL] Comic text is null");
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //else if (!__result)
            //{
            //    taggedPlayer = null;
            //}
        }
    }

    public class ComicTextManager : MonoBehaviour
    {
        private GameObject text;
        private void Start()
        {
            if (ComicTag.comictext != null)
            {
                text = ComicTag.comictext.transform.GetChild(0).gameObject;
                if (text != null)
                {
                    AntiScreenShare.SetAntiScreenShareLayer(text);

                    Random randomcolor = new Random();
                    int randomNumbercolor = randomcolor.Next(1, 6);
                    switch (randomNumbercolor)
                    {
                        case 1:
                            text.GetComponent<Text>().color = Color.red;
                            break;
                        case 2:
                            text.GetComponent<Text>().color = Color.green;
                            break;
                        case 3:
                            text.GetComponent<Text>().color = Color.cyan;
                            break;
                        case 4:
                            text.GetComponent<Text>().color = Color.magenta;
                            break;
                        case 5:
                            text.GetComponent<Text>().color = Color.white;
                            break;
                    }
                    Random randomtext = new Random();
                    int randomNumbertext = randomtext.Next(1, 11);
                    switch (randomNumbertext)
                    {
                        case 1:
                            text.GetComponent<Text>().text = "BOOM";
                            break;
                        case 2:
                            text.GetComponent<Text>().text = "KABOOM";
                            break;
                        case 3:
                            text.GetComponent<Text>().text = "WOW";
                            break;
                        case 4:
                            text.GetComponent<Text>().text = "POW";
                            break;
                        case 5:
                            text.GetComponent<Text>().text = "TAGGED";
                            break;
                        case 6:
                            text.GetComponent<Text>().text = "SKILL ISSUE";
                            break;
                        case 7:
                            text.GetComponent<Text>().text = "BAM";
                            break;
                        case 8:
                            text.GetComponent<Text>().text = "ZAP";
                            break;
                        case 9:
                            text.GetComponent<Text>().text = "SMASH";
                            break;
                        case 10:
                            text.GetComponent<Text>().text = "WHAM";
                            break;
                    }
                }
            }
        }
        private void Update()
        {
            if (text != null)
            {
                text.transform.LookAt(-GorillaTagger.Instance.headCollider.transform.eulerAngles);

                StartCoroutine(WaitForAnimation("Pop_Fade"));
            }
        }
        private IEnumerator WaitForAnimation(string animationName)
        {
            float animationLength = text.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
            yield return new WaitForSeconds(animationLength);

            Destroy(gameObject);
            ComicTag.executedPlayers.Remove(ComicTag.taggedvrrig.Creator);
        }
    }
}
