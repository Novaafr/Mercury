using Mercury.Menu;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace Mercury.Mods
{
    public class NoLeaves : MonoBehaviour
    {
        public List<GameObject> leaves = new List<GameObject>();

        public void Start()
        {
            GameObject forest = GameObject.Find("Environment Objects/LocalObjects_Prefab/Forest");
            if (forest == null)
            {
                return;
            }
            foreach (Transform child in forest.GetComponentsInChildren<Transform>())
            {
                GameObject obj = child.gameObject;
                if (obj.name.Contains("UnityTempFile-1a1350753b2f46f438d1b5f2c3b9f9db (combined by EdMeshCombiner)"))
                {
                    leaves.Add(obj);
                }
            }
        }

        public void Update()
        {
            foreach (GameObject leaf in leaves)
            {
                if (leaf != null)
                    leaf.SetActive(!PluginConfig.NoLeaves);
            }
        }
    }
}