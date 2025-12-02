﻿using Colossal.Menu;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
using Valve.VR.InteractionSystem;

namespace Colossal.Mods
{
    public class Chams : MonoBehaviour
    {
        private Color espcolor;
        //private ThrowableBug[] creatures;

        private NavMeshAgent[] GREntitys;

        public void Start()
        {
            //if (creatures == null)
            //{
            //    creatures = Resources.FindObjectsOfTypeAll<ThrowableBug>();
            //}
            if (PhotonNetwork.InRoom)
            {
                if (GREntitys == null)
                    GREntitys = Resources.FindObjectsOfTypeAll<NavMeshAgent>();
            }
        }

        public void Update()
        {
            if (!PluginConfig.chams)
            {
                ResetChams();
                return;
            }

            if (PhotonNetwork.InRoom)
            {
                espcolor = GetESPColor(PluginConfig.ESPColour);
                UpdateVRRigs();
                UpdateGhostReactor();
            }

            //UpdateCreatures();
        }

        private Color GetESPColor(int espColour)
        {
            switch (espColour)
            {
                case 0: return new Color(0.6f, 0f, 0.8f, 0.4f);
                case 1: return new Color(1f, 0f, 0f, 0.4f);
                case 2: return new Color(1f, 1f, 0f, 0.4f);
                case 3: return new Color(0f, 1f, 0f, 0.4f);
                case 4: return new Color(0f, 0f, 1f, 0.4f);
                default: return new Color(0.6f, 0f, 0.8f, 0.4f);
            }
        }

        private void UpdateVRRigs()
        {
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != null && !vrrig.isOfflineVRRig)
                {
                    if (vrrig.mainSkin.material.shader != AssetBundleLoader.chamsShader)
                        vrrig.mainSkin.material.shader = AssetBundleLoader.chamsShader;
                    else
                    {
                        if (WhatAmI.IsInfected(vrrig.Creator))
                            vrrig.mainSkin.material.SetColor("_Color", new Color(1f, 0f, 0f, 0.4f));
                        else if (!WhatAmI.IsAliveGhostReactor(vrrig))
                            vrrig.mainSkin.material.SetColor("_Color", new Color(1f, 1f, 1f, 0.4f));
                        else
                            vrrig.mainSkin.material.SetColor("_Color", espcolor);
                    }
                }
            }
        }
        private void UpdateGhostReactor()
        {
            if (GREntitys == null || GREntitys.Length == 0)
                return;

            foreach (NavMeshAgent thing in GREntitys)
            {
                if (thing == null || thing.gameObject == null)
                    continue;

                Renderer[] renderers = thing.gameObject.GetComponentsInChildren<Renderer>();
                if (renderers == null || renderers.Length == 0)
                    continue;

                foreach (Renderer renderer in renderers)
                {
                    if (renderer == null)
                        continue;

                    Material mat = renderer.material;
                    if (mat == null)
                        continue;

                    if (mat.shader != AssetBundleLoader.chamsShader)
                    {
                        mat.shader = AssetBundleLoader.chamsShader;
                        mat.SetColor("_Color", new Color(1, 1, 1, 0.4f));
                    }
                }
            }
        }

        //private void UpdateCreatures()
        //{
        //    if (creatures != null && creatures.Length > 0)
        //    {
        //        foreach (ThrowableBug thing in creatures)
        //        {
        //            Material mat = thing.gameObject.GetComponent<Renderer>().material;

        //            if (mat.shader != AssetBundleLoader.chamsShader)
        //            {
        //                mat.shader = AssetBundleLoader.chamsShader;
        //                mat.SetColor("_Color", espcolor);
        //            }
        //        }
        //    }
        //}

        private void ResetChams()
        {
            if (PhotonNetwork.InRoom)
            {
                foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
                {
                    if (vrrig != null && !vrrig.isOfflineVRRig)
                    {
                        if (vrrig.mainSkin.material.shader == AssetBundleLoader.chamsShader)
                        {
                            vrrig.mainSkin.material.shader = Shader.Find("GorillaTag/UberShader");
                            vrrig.mainSkin.material.SetColor("_Color", Color.white);

                            if (!WhatAmI.IsAliveGhostReactor(vrrig))
                            {
                                vrrig.SetInvisibleToLocalPlayer(true);
                                vrrig.ChangeMaterialLocal(13);
                                vrrig.bodyRenderer.SetGameModeBodyType(GorillaBodyType.Skeleton);
                            }

                            //foreach (GorillaPlayerScoreboardLine gorillaPlayerScoreboardLine in new List<GorillaPlayerScoreboardLine>())
                            //{
                            //    if (gorillaPlayerScoreboardLine.linePlayer != null && vrrig != null && gorillaPlayerScoreboardLine.playerVRRig == vrrig)
                            //    {
                            //        vrrig.mainSkin.material = gorillaPlayerScoreboardLine.images[gorillaPlayerScoreboardLine.linePlayer.ActorNumber].material;
                            //    }
                            //}
                        }
                    }
                }

                if (GREntitys == null || GREntitys.Length == 0)
                    return;

                foreach (NavMeshAgent thing in GREntitys)
                {
                    if (thing == null || thing.gameObject == null)
                        continue;

                    Renderer[] renderers = thing.gameObject.GetComponentsInChildren<Renderer>();
                    if (renderers == null || renderers.Length == 0)
                        continue;

                    foreach (Renderer renderer in renderers)
                    {
                        if (renderer == null)
                            continue;

                        Material mat = renderer.material;
                        if (mat == null)
                            continue;

                        if (mat.shader == AssetBundleLoader.chamsShader)
                        {
                            mat.shader = Shader.Find("GorillaTag/UberShader");
                            mat.SetColor("_Color", Color.white);
                        }
                    }
                }
            }

            //if (creatures != null && creatures.Length > 0)
            //{
            //    foreach (ThrowableBug thing in creatures)
            //    {
            //        Material mat = thing.gameObject.GetComponent<Renderer>().material;

            //        if (mat.shader == AssetBundleLoader.chamsShader)
            //        {
            //            mat.shader = Shader.Find("GorillaTag/UberShader");
            //            mat.SetColor("_Color", Color.white);
            //        }
            //    }
            //}
        }
    }
}