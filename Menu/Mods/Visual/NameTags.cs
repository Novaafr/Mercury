using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using TMPro;
using GorillaNetworking;
using Mercury.Menu;
using HarmonyLib;
using static GorillaTagCompetitiveServerApi;

namespace Mercury.Mods
{
    public class NameTags : MonoBehaviour
    {
        //private HashSet<string> requestedIds = new HashSet<string>();
        private Dictionary<VRRig, TextMeshPro> clonedTextComponents = new Dictionary<VRRig, TextMeshPro>();
        /*private Vector3 height;
        private Vector3 size;
        private Color colour;

        private void Start()
        {
            if (PluginConfig.NameTags && PhotonNetwork.InRoom)
            {
                StartCoroutine(FetchCompetitiveDataCoroutine());
            }
        }

        private IEnumerator FetchCompetitiveDataCoroutine()
        {
            yield return RankedInfo.FetchCompetitiveDataAsync();
        }


        private string nametagtext;
        private string fps;
        private string colourcode;
        private string platform = "Quest";*/

        public void Update()
        {
            if (PluginConfig.NameTags)
            {
                if (PhotonNetwork.InRoom)
                {
                    foreach (VRRig rig in GorillaParent.instance.vrrigs)
                    {
                        if (rig != null && rig != VRRig.LocalRig)
                        {
                            TextMeshPro tag = CreateTag(rig);
                            UpdateTag(rig, tag);
                        }
                    }
                }
            }
            else
            {
                Clear();
                Destroy(this);
            }
        }

        public TextMeshPro CreateTag(VRRig rig)
        {
            if (clonedTextComponents.TryGetValue(rig, out TextMeshPro tag) && tag != null)
                return tag;
            GameObject holder = new GameObject();
            holder.transform.SetParent(transform);
            tag = holder.AddComponent<TextMeshPro>();
            tag.alignment = TextAlignmentOptions.Center;
            tag.font = GorillaTagger.Instance.offlineVRRig.playerText1.font;
            tag.color = Color.white;
            tag.fontSize = 1.2f;
            tag.richText = true;
            clonedTextComponents[rig] = tag;
            return tag;
        }

        public void UpdateTag(VRRig rig, TextMeshPro tag)
        {
            Transform head = rig.headMesh.transform;
            tag.text = rig.OwningNetPlayer.NickName;
            tag.transform.position = head.position + Vector3.up * 0.5f;
            tag.transform.LookAt(Camera.main.transform);
            tag.transform.Rotate(0f, 180f, 0f);
        }

        public void Clear()
        {
            foreach (var tag in clonedTextComponents.Values)
            {
                if (tag != null)
                    Destroy(tag.gameObject);
            }
            clonedTextComponents.Clear();
        }
    }
}