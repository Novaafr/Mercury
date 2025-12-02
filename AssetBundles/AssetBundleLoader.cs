using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using Colossal.Patches;
using PlayFab.MultiplayerModels;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Viveport;
using static Photon.Voice.OpusCodec;

namespace Colossal
{
    internal class AssetBundleLoader : MonoBehaviour
    {
        public static AssetBundle bundle;
        public static GameObject assetBundleParent;
        public static string parentName = "CCMV2 Custom Assets";

        public static GameObject Comic_Canvas;
        public static GameObject Poppins_Canvas;
        public static TMP_FontAsset PoppinsFontTMP;
        public static Font PoppinsFont;

        public static GameObject DarkZone;
        public static GameObject FadeBox;
        public static bool isFadeComplete = false;
        private static float fadeDuration = 1.5f;
        private static float fadeTimer = 0f;
        private static bool isFadingIn = true;
        private static Renderer fadeRenderer;

        public static GameObject targetIndicator;


        public static GameObject hud;
        public static GameObject panel;

        public static GameObject grab;
        public static GameObject toggle;
        public static GameObject button;
        public static GameObject slider;
        public static GameObject slider_bind;
        public static GameObject text;
        public static GameObject back;

        public static RuntimeAnimatorController Menu_Controller;
        public static string Menu_In = "Menu_In";
        public static string Menu_Out = "Menu_Out";
        public static string Menu_Press = "Menu_Press";


        public static Material outlineMaskMaterial;
        public static Material outlineFillMaterial;

        public static Shader chamsShader;

        public void Start()
        {
            CustomConsole.Debug("Asset Bundle Loader Start");

            bundle = LoadAssetBundle("ColossalV3.AssetBundles.ccmv2assets");
            if (bundle == null)
            {
                CustomConsole.Error("bundle is null - check resource path and bundle contents!");
                return;
            }

            GameObject assetBundleParent = Instantiate(bundle.LoadAsset<GameObject>(parentName));
            if (assetBundleParent == null)
            {
                CustomConsole.Error("assetBundleParent is null");
                return;
            }
            assetBundleParent.transform.position = new Vector3(-67.235f, 11.54f, -82.6f);


            outlineMaskMaterial = bundle.LoadAsset<Material>("OutlineMask");
            outlineFillMaterial = bundle.LoadAsset<Material>("OutlineFill");


            chamsShader = bundle.LoadAsset<Shader>("Cham");


            hud = assetBundleParent.transform.GetChild(4).gameObject;
            if (hud != null)
            {
                hud.transform.position = Camera.main.transform.position + Vector3.forward * 0.6f;

                panel = hud.transform.GetChild(0).gameObject;
                panel.transform.position = new Vector3(0, -6969, 0);

                grab = panel.transform.GetChild(0).gameObject;
                toggle = panel.transform.GetChild(1).gameObject;
                button = panel.transform.GetChild(2).gameObject;
                slider = panel.transform.GetChild(3).gameObject;
                slider_bind = panel.transform.GetChild(4).gameObject;
                text = panel.transform.GetChild(5).gameObject;
                back = panel.transform.GetChild(6).gameObject;

                if (grab != null) CustomConsole.Debug("Loaded grab");
                if (toggle != null) CustomConsole.Debug("Loaded toggle");
                if (button != null) CustomConsole.Debug("Loaded button");
                if (slider != null) CustomConsole.Debug("Loaded slider");
                if (slider_bind != null) CustomConsole.Debug("Loaded slider_bind");
                if (text != null) CustomConsole.Debug("Loaded text");
                if (back != null) CustomConsole.Debug("Loaded back");


                Menu_Controller = panel.GetComponent<RuntimeAnimatorController>();
                bundle.LoadAsset<AnimationClip>("Menu_In");
                bundle.LoadAsset<AnimationClip>("Menu_Out");
                bundle.LoadAsset<AnimationClip>("Menu_Press");
            }
            else
            {
                CustomConsole.Error("hud is null - check asset bundle hierarchy");
            }


            Comic_Canvas = assetBundleParent.transform.GetChild(0).gameObject;
            Poppins_Canvas = assetBundleParent.transform.GetChild(1).gameObject;
            DarkZone = assetBundleParent.transform.GetChild(2).gameObject;
            FadeBox = assetBundleParent.transform.GetChild(3).gameObject;
            targetIndicator = assetBundleParent.transform.GetChild(5).gameObject;

            if (Comic_Canvas != null) CustomConsole.Debug("Loaded Comic_Canvas");
            if (Poppins_Canvas != null) CustomConsole.Debug("Loaded Poppins_Canvas");
            if (DarkZone != null) CustomConsole.Debug("Loaded DarkZone");
            if (FadeBox != null) CustomConsole.Debug("Loaded FadeBox");
            if (targetIndicator != null) CustomConsole.Debug("Loaded targetIndicator");


            SpawnVoidBubbles();
        }

        public AssetBundle LoadAssetBundle(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            if (stream == null)
            {
                CustomConsole.Debug("could not find resource at path: " + path);
                return null;
            }

            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();
            return bundle;
        }

        void Update()
        {
            if (Menu.Menu.agreement && !isFadeComplete && Menu.Menu.AgreementHub == null)
            {
                //DespawnVoidBubbles();
            }

            // breaks gravity/movement
            //if (DarkZone.activeSelf)
            //{
             //   GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(-Physics.gravity, ForceMode.Acceleration);
             //   GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.linearVelocity = Vector3.zero;
            //}
        }

        public static void SpawnVoidBubbles()
        {
            CustomConsole.Debug("SpawnVoidBubbles Called");

            if (DarkZone != null)
            {
                DarkZone.SetActive(true);

                GorillaLocomotion.GTPlayer.Instance.TeleportTo(DarkZone.transform.position, new Quaternion(0, 0, 0, 0));
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.linearVelocity = Vector3.zero;
            }
        }

        public static void DespawnVoidBubbles()
        {
            CustomConsole.Debug("DespawnVoidBubbles Called");
            Debug.Log("DespawnVoidBubbles Called");

            if (FadeBox != null)
            {
                if (fadeRenderer == null)
                {
                    fadeRenderer = FadeBox.GetComponent<Renderer>();
                }

                if (fadeRenderer != null)
                {
                    Color startColor = fadeRenderer.material.color;
                    startColor.a = 0f;
                    fadeRenderer.material.color = startColor;
                }

                fadeTimer += Time.deltaTime;

                if (isFadingIn)
                {
                    FadeBox.transform.position = GorillaLocomotion.GTPlayer.Instance.headCollider.transform.position;

                    // Fade in (alpha from 0 to 1)
                    float alpha = Mathf.Lerp(0f, 1f, fadeTimer / fadeDuration);
                    Color currentColor = fadeRenderer.material.color;
                    currentColor.a = alpha;
                    fadeRenderer.material.color = currentColor;

                    // When the fade reaches full opacity (alpha == 1)
                    if (fadeTimer >= fadeDuration && fadeRenderer.material.color.a >= 1f)
                    {
                        DarkZone.SetActive(false);

                        GorillaLocomotion.GTPlayer.Instance.TeleportTo(new Vector3(-67, 11.8f, -82f), new Quaternion(0, 0, 0, 0));

                        //SceneManager.GetActiveScene().GetRootGameObjects()
                        //           .FirstOrDefault(obj => obj.name == "Environment Objects")?.SetActive(true);
                    }
                }
                else
                {
                    // Fade out (alpha from 1 to 0)
                    float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
                    Color currentColor = fadeRenderer.material.color;
                    currentColor.a = alpha;
                    fadeRenderer.material.color = currentColor;

                    // When fade out is complete
                    if (fadeTimer >= fadeDuration && fadeRenderer.material.color.a <= 0f)
                    {

                        fadeTimer = 0f;
                        isFadingIn = true;

                        isFadeComplete = true;

                        FadeBox.SetActive(false);
                    }
                }

                // Once fade-in is complete, start fading out
                if (fadeTimer >= fadeDuration && isFadingIn)
                {
                    fadeTimer = 0f;  // Reset the fade timer
                    isFadingIn = false;  // Start fading out
                }
            }
        }

        //public static void ApplyNoGrav()
        //{
        //    if (!isFadeComplete)
        //    {
        //        GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.linearVelocity = Vector3.zero;
        //       GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(-Physics.gravity, ForceMode.Acceleration);
        //   }
        //}
    }

    public class CoroutineStarter : MonoBehaviour
    {
        public static CoroutineStarter Instance;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void StartStaticCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }
    }

}
