using Colossal.Menu;
using UnityEngine;

namespace Colossal.Mods
{
    public class Tracers : MonoBehaviour
    {
        private Color espcolor;
        private Vector3 pos;
        private float size;

        // Cache the material for performance, since it's used repeatedly
        private static readonly Material lineMaterial = new Material(Shader.Find("GUI/Text Shader"));

        public void Update()
        {
            if(PluginConfig.tracers == 0)
            {
                Destroy(this.GetComponent<Tracers>());
            }

            // Set ESP color based on config
            espcolor = GetEspColor(PluginConfig.ESPColour);

            // Set tracer position based on config
            pos = GetTracerPosition(PluginConfig.tracers);

            // Set tracer size based on config
            size = GetTracerSize(PluginConfig.tracersize);

            // Iterate through VR rigs and create tracers
            foreach (VRRig vrrig in GorillaParent.instance.vrrigs)
            {
                if (vrrig != VRRig.LocalRig)
                {
                    CreateTracer(vrrig);
                }
            }
        }

        private Color GetEspColor(int colorIndex)
        {
            switch (colorIndex)
            {
                case 0: return new Color(0.6f, 0f, 0.8f, 0.4f);  // Purple
                case 1: return new Color(1f, 0f, 0f, 0.4f);    // Red
                case 2: return new Color(1f, 1f, 0f, 0.4f);    // Yellow
                case 3: return new Color(0f, 1f, 0f, 0.4f);    // Green
                case 4: return new Color(0f, 0f, 1f, 0.4f);    // Blue
                default: return new Color(0.6f, 0f, 0.8f, 0.4f);  // Default Purple
            }
        }

        private Vector3 GetTracerPosition(int tracerIndex)
        {
            switch (tracerIndex)
            {
                case 1: return GorillaTagger.Instance.rightHandTransform.position;
                case 2: return GorillaTagger.Instance.leftHandTransform.position;
                case 3: return GorillaTagger.Instance.headCollider.transform.position + (Vector3.up * 0.2f);
                case 4: return GorillaTagger.Instance.headCollider.transform.position + GorillaTagger.Instance.headCollider.transform.forward / 2;
                default: return Vector3.zero;  // Default to zero position if invalid config
            }
        }

        private float GetTracerSize(int sizeIndex)
        {
            switch (sizeIndex)
            {
                case 0: return 0.002f;
                case 1: return 0.01f;
                case 2: return 0.025f;
                case 3: return 0.05f;
                case 4: return 0.065f;
                case 5: return 0.08f;
                case 6: return 0.1f;
                default: return 0.01f;  // Default to size 0.01f
            }
        }

        private void CreateTracer(VRRig vrrig)
        {
            GameObject lineObject = new GameObject("Line");
            AntiScreenShare.SetAntiScreenShareLayer(lineObject);

            LineRenderer lineRenderer = lineObject.AddComponent<LineRenderer>();

            // Set line color
            lineRenderer.startColor = (vrrig.mainSkin.material.name.Contains("fected")) ? Color.red : espcolor;
            lineRenderer.endColor = lineRenderer.startColor;

            // Set line size
            lineRenderer.startWidth = size;
            lineRenderer.endWidth = size;

            // Set line position
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
            lineRenderer.SetPosition(0, pos);
            lineRenderer.SetPosition(1, vrrig.transform.position);

            // Set material
            lineRenderer.material = lineMaterial;

            // Destroy the tracer object after the frame
            Destroy(lineObject, Time.deltaTime);
        }
    }
}
