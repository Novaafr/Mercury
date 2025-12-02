using System;
using System.Collections.Generic;
using Colossal;

using GorillaNetworking;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;
using TMPro;
using Valve.VR;
using Colossal.Patches;
using Colossal.Menu;
using System.Diagnostics;

namespace Colossal
{
    internal class Boards : MonoBehaviour
    {
        private static List<GameObject> objtochange = new List<GameObject>();
        private static GameObject cocktext = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/COC Text");
        private static GameObject cock = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConduct");

        public static Material boardmat;
        public static Material defaultboardmat;

        public static bool tempbool = false;

        //public static string coctext;
        public static string defaultcoctext;

        //public static void ChangeMaterialsRecursively(Transform parent, Color color)
        //{
        //    try
        //    {
        //        foreach (Transform child in parent)
        //        {
        //            if (child.GetComponent<TextMeshPro>() != null)
        //                child.GetComponent<TextMeshPro>().color = color;

        //            if (child.GetComponent<Text>() != null)
        //                child.GetComponent<Text>().color = color;

        //            ChangeMaterialsRecursively(child, color);
        //        }
        //    }
        //    catch(Exception e)
        //    {
        //        CustomConsole.Error(e.ToString());
        //    }
        //}
        //public static void DoBoardThingy(Material mat, Color color)
        //{
        //    try
        //    {
        //        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        //        foreach (GameObject obj in allObjects)
        //        {
        //            if (obj != null)
        //            {
        //                if (obj.name.ToLower().Contains("monitor") || obj.name.ToLower().Contains("keyboard") || obj.name.ToLower().Contains("UnityTempFile") && obj.transform.IsChildOf(GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom").transform))
        //                    objtochange.Add(obj);
        //            }
        //        }
        //        if (objtochange.Count > 0 && objtochange != null)
        //        {
        //            foreach (GameObject obj in objtochange)
        //            {
        //                if (obj.GetComponent<Renderer>() != null && obj.GetComponent<Renderer>().material != mat)
        //                    obj.GetComponent<Renderer>().material = mat;
        //            }
        //        }
        //        else
        //            CustomConsole.Error("WallMonitors is less than 0 or null");


        //        string parentPath = "Environment Objects/LocalObjects_Prefab";
        //        GameObject parentObject = GameObject.Find(parentPath);
        //        if (parentObject != null)
        //            ChangeMaterialsRecursively(parentObject.transform, color);
        //    }
        //    catch(Exception e)
        //    {
        //        CustomConsole.Error(e.ToString());
        //    }
        //}


        public void Start()
        {
            try
            {
                //Boards.defaultboardmat = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/UnityTempFile-0e668886bb0df974486eaa852fd0514a (combined by EdMeshCombiner)").GetComponent<Renderer>().material;
                Boards.defaultcoctext = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/COCBodyText_TitleData").GetComponent<TextMeshPro>().text;

                cocktext = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/COCBodyText_TitleData");
                cock = GameObject.Find("Environment Objects/LocalObjects_Prefab/TreeRoom/CodeOfConductHeadingText");

                Boards.boardmat = new Material(Shader.Find("GorillaTag/UberShader"));
                Boards.boardmat.color = new Color(0.6f, 0f, 0.8f);

                //DoBoardThingy(boardmat, Color.cyan);
            }
            catch (Exception e)
            {
                CustomConsole.Error(e.ToString());
            }
        }
        public void Update()
        {
            try
            {
                if (PluginConfig.ShowBoards)
                {
                    cocktext.GetComponent<TextMeshPro>().text = $"Thank you for using CCMV3, the successor to the first cheat menu!\n\nContributors:\nNova: Menu Maker/Reviver\nColossusYTTV: Menu Maker/Mod Creator\nLars/LHAX: Menu Base\nStarry: Dev/Tester\nMios: Tester/Manager/Pain In The Ass\nWM/Will: No Fingers/Full Bright/HzHands\nVentern: Anti Screen Share\n\nMenu Version: {OnGameInit.localversion}, Server Version: {OnGameInit.serverversion}\nCCMV3 Users Online: {BepInPatcher.playercount}".ToUpper(); // doing this because the font is so fucked
                    if(cock.GetComponent<TextMeshPro>().text != "COLOSSAL CHEAT MENU V3")
                        cock.GetComponent<TextMeshPro>().text = "COLOSSAL CHEAT MENU V3";

                    //if (Menu.Menu.agreement)
                    //{
                    //    AssetBundleLoader.Boards.SetActive(true);
                    //    if (AssetBundleLoader.Boards != null)
                    //    {
                    //        foreach (GameObject obj in AssetBundleLoader.Boards.transform)
                    //        {
                    //            if (obj != null)
                    //            {
                    //                obj.GetComponent<Renderer>().material = Boards.boardmat;
                    //            }
                    //        }
                    //    }
                    //}
                }
                else
                {
                    if(cocktext.GetComponent<TextMeshPro>().text != defaultcoctext)
                        cocktext.GetComponent<TextMeshPro>().text = defaultcoctext;
                    if (cock.GetComponent<TextMeshPro>().text != "GORILLA CODE OF CONDUCT")
                        cock.GetComponent<TextMeshPro>().text = "GORILLA CODE OF CONDUCT";

                    //AssetBundleLoader.Boards.SetActive(false);
                }
            }
            catch (Exception e)
            {
                CustomConsole.Error(e.ToString());
            }
        }
    }
}
