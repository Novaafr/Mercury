using BepInEx;
using Colossal.Menu;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Colossal.Mods
{
    public class WASDFly : MonoBehaviour
    {
        private static readonly float[] flySpeeds = { 5f, 7f, 10f, 13f, 16f };
        private float speed;
        private float X = -1;

        public void FixedUpdate()
        {
            if (PluginConfig.WASDFly != 0 && GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody != null)
            {
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(-Physics.gravity, ForceMode.Acceleration);
                GorillaLocomotion.GTPlayer.Instance.bodyCollider.attachedRigidbody.velocity = Vector3.zero;
            }
        }

        public void Update()
        {
            if (PluginConfig.WASDFly == 0)
            {
                Destroy(this.GetComponent<WASDFly>());
                return;
            }

            speed = flySpeeds[Mathf.Min(PluginConfig.WASDFly, flySpeeds.Length) - 1];

            MovePlayer();
            HandleCameraRotation();
        }

        private void MovePlayer()
        {
            Vector3 movement = Vector3.zero;

            if (UnityInput.Current.GetKey(KeyCode.W)) movement += Camera.main.transform.forward;
            if (UnityInput.Current.GetKey(KeyCode.S)) movement -= Camera.main.transform.forward;
            if (UnityInput.Current.GetKey(KeyCode.A)) movement -= Camera.main.transform.right;
            if (UnityInput.Current.GetKey(KeyCode.D)) movement += Camera.main.transform.right;
            if (UnityInput.Current.GetKey(KeyCode.Space)) movement += Camera.main.transform.up;
            if (UnityInput.Current.GetKey(KeyCode.LeftControl)) movement -= Camera.main.transform.up;

            GorillaTagger.Instance.rigidbody.transform.position += movement * Time.deltaTime * speed;
        }

        private void HandleCameraRotation()
        {
            if (Mouse.current.rightButton.isPressed)
            {
                Vector3 eulerAngles = Camera.main.transform.rotation.eulerAngles;
                if (X < 0f) X = eulerAngles.y;

                eulerAngles = new Vector3(eulerAngles.x, X + (Mouse.current.position.ReadValue().x / (float)Screen.width - 5) * 360f * 1.33f, eulerAngles.z);
                Camera.main.transform.rotation = Quaternion.Euler(eulerAngles);
            }
            else
            {
                X = -1f;
            }
        }
    }
}
