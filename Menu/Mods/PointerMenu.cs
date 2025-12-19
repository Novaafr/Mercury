/*using UnityEngine;

namespace Colossal.Menu
{
    public class PointerLine : MonoBehaviour
    {
        private LineRenderer lineRenderer;

        public static Vector3 lastPointerPos = Vector3.zero;
        private GameObject lockedElement;

        private const float smoothingSpeed = 15f;
        private const float lockDistance = 0.05f;
        private const float unlockDistance = 0.2f;
        private const float shortRangeDistance = 1f;
        private const float longRangeDistance = 512f;

        public static PointerLine Instance { get; private set; }
        public static bool ShortRangeMode { get; set; } = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0.01f;
            lineRenderer.positionCount = 2;

            // Avoid Shader.Find null issues
            Shader spriteShader = Shader.Find("Sprites/Default");
            lineRenderer.material = spriteShader != null
                ? new Material(spriteShader)
                : new Material(Shader.Find("Unlit/Color"));

            PanelElement activePanel = GetActivePanel();
            if (activePanel != null && activePanel.grabInstance != null)
            {
                var r = activePanel.grabInstance.GetComponent<Renderer>();
                if (r != null)
                {
                    Color c = r.material.color;
                    lineRenderer.startColor = c;
                    lineRenderer.endColor = c;
                }
            }

            lineRenderer.enabled = false;
            CustomConsole.Debug("PointerLine initialized");
        }

        public void UpdateLine(Ray ray, bool rayHit, RaycastHit hit, PanelElement panel)
        {
            if (lineRenderer == null || panel == null)
                return;

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, ray.origin);

            // Safe color assignment
            if (panel.grabInstance != null)
            {
                var r = panel.grabInstance.GetComponent<Renderer>();
                if (r != null)
                {
                    Color c = r.material.color;
                    lineRenderer.startColor = c;
                    lineRenderer.endColor = c;
                }
            }

            float maxDist = ShortRangeMode ? shortRangeDistance : longRangeDistance;

            Vector3 targetPos;

            // Ray hit logic
            if (rayHit && hit.collider != null && Vector3.Distance(ray.origin, hit.point) <= maxDist)
            {
                targetPos = hit.point;
            }
            else
            {
                // Menu plane fallback
                Plane menuPlane = new Plane(-panel.RootObject.transform.forward,
                                            panel.RootObject.transform.position);

                if (menuPlane.Raycast(ray, out float planeDist) && planeDist <= maxDist)
                    targetPos = ray.GetPoint(planeDist);
                else
                    targetPos = ray.origin + ray.direction * maxDist;
            }

            // Locking logic
            if (lockedElement != null)
            {
                float dist = Vector3.Distance(targetPos, lockedElement.transform.position);
                if (dist > unlockDistance)
                {
                    lockedElement = null;
                }
                else
                {
                    targetPos = lockedElement.transform.position;
                }
            }
            else if (rayHit && hit.collider != null)
            {
                GameObject hitObj = hit.collider.gameObject;

                if (hitObj.name.Contains("grab"))
                {
                    float toObj = Vector3.Distance(targetPos, hitObj.transform.position);
                    if (toObj < lockDistance)
                    {
                        lockedElement = hitObj;
                        targetPos = lockedElement.transform.position;
                    }
                }
            }

            // Smooth pointer end
            lastPointerPos = Vector3.Lerp(lastPointerPos, targetPos, Time.deltaTime * smoothingSpeed);
            lineRenderer.SetPosition(1, lastPointerPos);
        }

        public void DisableLine()
        {
            if (lineRenderer != null)
                lineRenderer.enabled = false;
        }

        public GameObject GetLockedElement() => lockedElement;

        private PanelElement GetActivePanel()
        {
            foreach (var p in GUICreator.openPanels)
            {
                if (p != null && p.RootObject != null && p.RootObject.activeSelf)
                    return p;
            }

            return null;
        }
    }
}
*/