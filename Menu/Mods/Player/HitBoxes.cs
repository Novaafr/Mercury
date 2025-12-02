using Colossal.Menu;
using UnityEngine;
using HarmonyLib;
using System;
using Colossal.Mods;
using Colossal.Patches;

namespace Colossal.Mods
{
    public class HitBoxes : MonoBehaviour
    {
        private GameObject visualizerL;
        private GameObject visualizerR;
        public static float ammount;
        private static Color color;

        public void Start()
        {
            if(visualizerL == null) 
            { 
                visualizerL = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                visualizerL.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");

                visualizerL.transform.position = GorillaTagger.Instance.leftHandTransform.position;
                visualizerL.transform.SetParent(GorillaTagger.Instance.leftHandTransform);

                Destroy(visualizerL.GetComponent<Collider>());
            }

            if(visualizerR == null)
            {
                visualizerR = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                visualizerR.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");

                visualizerR.transform.position = GorillaTagger.Instance.rightHandTransform.position;
                visualizerR.transform.SetParent(GorillaTagger.Instance.rightHandTransform);

                Destroy(visualizerR.GetComponent<Collider>());
            }
        }
        public void Update()
        {
            if (PluginConfig.hitboxes == 0)
            {
                if(visualizerL != null)
                    Destroy(visualizerL);
                if (visualizerR != null)
                    Destroy(visualizerR);

                Destroy(this.GetComponent<HitBoxes>());

                return;
            }

            if (visualizerL != null && visualizerR != null)
            {
                switch(PluginConfig.hitboxes)
                {
                    case 1:
                        ammount = 0.05f;
                        break;
                    case 2:
                        ammount = 0.07f;
                        break;
                    case 3:
                        ammount = 0.09f;
                        break;
                    case 4:
                        ammount = 0.11f;
                        break;
                    case 5:
                        ammount = 0.13f;
                        break;
                    case 6:
                        ammount = 0.15f;
                        break;
                    case 7:
                        ammount = 0.2f;
                        break;
                }

                switch (PluginConfig.HitBoxesColour)
                {
                    case 0:
                        color = new Color(0.6f, 0f, 0.8f, GetHitBoxOpacity(PluginConfig.HitBoxesOpacity));
                        break;
                    case 1:
                        color = new Color(1f, 0f, 0f, GetHitBoxOpacity(PluginConfig.HitBoxesOpacity));
                        break;
                    case 2:
                        color = new Color(1f, 1f, 0f, GetHitBoxOpacity(PluginConfig.HitBoxesOpacity));
                        break;
                    case 3:
                        color = new Color(0f, 1f, 0f, GetHitBoxOpacity(PluginConfig.HitBoxesOpacity));
                        break;
                    case 4:
                        color = new Color(0f, 0f, 1f, GetHitBoxOpacity(PluginConfig.HitBoxesOpacity));
                        break;
                }


                if(visualizerR.GetComponent<Renderer>().material.color != color)
                    visualizerR.GetComponent<Renderer>().material.color = color;
                if(visualizerL.GetComponent<Renderer>().material.color != color)
                    visualizerL.GetComponent<Renderer>().material.color = color;

                
                Vector3 scale = new Vector3(HitBoxesPatch.displayRadius, HitBoxesPatch.displayRadius, HitBoxesPatch.displayRadius);
                if (visualizerR.transform.localScale != scale)
                    visualizerR.transform.localScale = scale;
                if(visualizerL.transform.localScale != scale)
                    visualizerL.transform.localScale = scale;

                AntiScreenShare.SetAntiScreenShareLayer(visualizerR);
                AntiScreenShare.SetAntiScreenShareLayer(visualizerL);
            }
        }
        private static float GetHitBoxOpacity(int setting)
        {
            switch (setting)
            {
                case 1:
                    return 0.8f; // 80%
                case 2:
                    return 0.6f; // 60%
                case 3:
                    return 0.3f; // 30%
                case 4:
                    return 0.2f; // 20%
                case 5:
                    return 0.0f; // 0%
                default:
                    return 1.0f; // 100%
            }
        }
    }
}

namespace Colossal.Patches
{
    [HarmonyPatch(typeof(GorillaTagger), "get_sphereCastRadius")]
    public class HitBoxesPatch
    {
        public static float displayRadius;
        public static void Postfix(GorillaTagger __instance, ref float __result)
        {
            if (PluginConfig.hitboxes != 0)
            {
                __result = HitBoxes.ammount;
                displayRadius = HitBoxes.ammount * 2f;
            }
            else if (__result != 0.03f)
                __result = 0.03f;
        }
    }
}
