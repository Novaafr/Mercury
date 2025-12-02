using UnityEngine;

namespace Colossal.Menu
{
    public class PointerLine : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        public static Vector3 lastPointerPos; // For smoothing
        private GameObject lockedElement; // For locking
        private const float smoothingSpeed = 15f;//  Smoothing factor
        private const float lockDistance = 0.05f; // Distance to lock (world units)
        private const float unlockDistance = 0.2f; // Distance to unlock
        private const float shortRangeDistance = 1f; // Short range limit
        private const float longRangeDistance = 512f; // Default long range limit

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

            // Initialize LineRenderer
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = 0.01f;
            lineRenderer.endWidth = 0.01f;
            lineRenderer.positionCount = 2;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));

            // Set initial color (updated later in UpdateLine)
            PanelElement currentPanel = GetActivePanel();
            if (currentPanel != null)
            {
                lineRenderer.startColor = currentPanel.grabInstance.GetComponent<Renderer>().material.color;
                lineRenderer.endColor = currentPanel.grabInstance.GetComponent<Renderer>().material.color;
            }
            lineRenderer.enabled = false;
            CustomConsole.Debug("Initialized PointerLine");
        }

        public void UpdateLine(Ray ray, bool rayHit, RaycastHit hit, PanelElement panel)
        {
            if (lineRenderer == null || panel == null)
            {
                CustomConsole.Error("LineRenderer or panel is null in PointerLine!");
                return;
            }

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, ray.origin);

            // Update line color based on current panel
            lineRenderer.startColor = panel.grabInstance.GetComponent<Renderer>().material.color;
            lineRenderer.endColor = panel.grabInstance.GetComponent<Renderer>().material.color;

            Vector3 targetPos;
            float maxDistance = ShortRangeMode ? shortRangeDistance : longRangeDistance;

            if (rayHit && Vector3.Distance(ray.origin, hit.point) <= maxDistance)
            {
                targetPos = hit.point;
            }
            else
            {
                Plane menuPlane = new Plane(-panel.RootObject.transform.forward, panel.RootObject.transform.position); // Face player
                if (menuPlane.Raycast(ray, out float distance) && distance <= maxDistance)
                {
                    targetPos = ray.GetPoint(distance);
                }
                else
                {
                    targetPos = ray.origin + ray.direction * maxDistance;
                }
            }

             // Locking logic - only lock to grab, not UI elements
            if (lockedElement != null)
            {
                float distanceToLocked = Vector3.Distance(targetPos, lockedElement.transform.position);
                if (distanceToLocked > unlockDistance)
                {
                    lockedElement = null;
                }
                else
                {
                    targetPos = lockedElement.transform.position;
                }
            }
            else if (rayHit && Vector3.Distance(ray.origin, hit.point) <= maxDistance)
            {
                GameObject hitObject = hit.collider.gameObject;
                string hitName = hitObject.name;

                // Only lock to grab element
                if (hitName.Contains("grab") && Vector3.Distance(targetPos, hitObject.transform.position) < lockDistance)
                {
                    lockedElement = hitObject;
                    targetPos = lockedElement.transform.position;
                }
            }

            // Smoothing
            lastPointerPos = Vector3.Lerp(lastPointerPos, targetPos, Time.deltaTime * smoothingSpeed);
            lineRenderer.SetPosition(1, lastPointerPos);
        }

        public void DisableLine()
        {
            if (lineRenderer != null)
            {
                lineRenderer.enabled = false;
            }
        }

        public GameObject GetLockedElement()
        {
            return lockedElement;
        }

        private PanelElement GetActivePanel()
        {
            foreach (var panel in GUICreator.openPanels)
            {
                if (panel.RootObject.activeSelf)
                {
                    return panel;
                }
            }
            return null;
        }
    }
}