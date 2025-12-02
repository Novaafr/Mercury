﻿using Colossal.Menu;
using Colossal.Patches;
using ExitGames.Client.Photon;
using GorillaLocomotion.Climbing;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.XR;

namespace Colossal.Mods
{
    public class Platforms : MonoBehaviour
    {
        public static GameObject PlatL;
        private bool PlatLonce = false;

        public static GameObject PlatR;
        private bool PlatRonce = false;

        private Material PlatMaterial;
        private Color PlatColor;

        public void Start()
        {
            if (PlatMaterial == null)
            {
                PlatMaterial = new Material(Shader.Find("GorillaTag/UberShader"));
            }
        }
        public void Update()
        {
            if (!PluginConfig.platforms)
            {
                Destroy(this.GetComponent<Platforms>());
                if (PlatL != null) Destroy(PlatL);
                if (PlatR != null) Destroy(PlatR);
                return;
            }


            switch (PluginConfig.PlatformsColour)
            {
                case 0:
                    PlatMaterial.color = new Color(0.6f, 0f, 0.8f, 0.5f);
                    break;
                case 1:
                    PlatMaterial.color = new Color(1f, 0f, 0f, 0.5f);
                    break;
                case 2:
                    PlatMaterial.color = new Color(1f, 1f, 0f, 0.5f);
                    break;
                case 3:
                    PlatMaterial.color = new Color(0f, 1f, 0f, 0.5f);
                    break;
                case 4:
                    PlatMaterial.color = new Color(0f, 0f, 1f, 0.5f);
                    break;
            }


            // Get the bind for platforms
            string bind = CustomBinding.GetBinds("platforms");
            if (string.IsNullOrEmpty(bind) || bind == "UNBOUND")
            {
                return;
            }

            // Get mirrored versions for left and right hand
            string leftBind = CustomBinding.MirrorBind(bind, true);
            string rightBind = CustomBinding.MirrorBind(bind, false);

            // Left hand platform
            if (ControlsV2.GetControl(leftBind))
            {
                if (!PlatLonce)
                {
                    PlatL = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    PlatL.GetComponent<Renderer>().material = PlatMaterial;
                    PlatL.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    PlatL.transform.position = GorillaTagger.Instance.leftHandTransform.position - Vector3.up * 0.045f;
                    PlatL.transform.rotation = GorillaTagger.Instance.leftHandTransform.rotation;
                    PlatLonce = true;
                }
            }
            else if (PlatLonce)
            {
                Destroy(PlatL);
                PlatLonce = false;
            }

            // Right hand platform
            if (ControlsV2.GetControl(rightBind))
            {
                if (!PlatRonce)
                {
                    PlatR = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    PlatR.GetComponent<Renderer>().material = PlatMaterial;
                    PlatR.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                    PlatR.transform.position = GorillaTagger.Instance.rightHandTransform.position - Vector3.up * 0.045f;
                    PlatR.transform.rotation = GorillaTagger.Instance.rightHandTransform.rotation;
                    PlatRonce = true;
                }
            }
            else if (PlatRonce)
            {
                Destroy(PlatR);
                PlatRonce = false;
            }
        }
    }
}