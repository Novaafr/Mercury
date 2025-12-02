using Colossal.Menu;
using UnityEngine;
using UnityEngine.XR;

namespace Colossal.Mods
{
    public class SkyColour : MonoBehaviour
    {
        private MeshRenderer skyRenderer1, skyRenderer2;
        private Shader originalShader;
        private Shader uberShader;

        public void Start()
        {
            // Find and cache the MeshRenderers and Materials
            skyRenderer1 = GameObject.Find("Environment Objects/LocalObjects_Prefab/Standard Sky/newsky (1)").GetComponent<MeshRenderer>();
            skyRenderer2 = GameObject.Find("Environment Objects/LocalObjects_Prefab/Standard Sky").GetComponent<MeshRenderer>();

            originalShader = skyRenderer1.material.shader;
            uberShader = Shader.Find("GorillaTag/UberShader");
        }

        public void Update()
        {
            if (skyRenderer1 == null || skyRenderer2 == null)
                return;

            switch (PluginConfig.skycolour)
            {
                case 0:
                    // Reset to original materials (if needed)
                    skyRenderer1.material.color = Color.white;
                    skyRenderer2.material.color = Color.white;
                    skyRenderer1.material.shader = originalShader;
                    skyRenderer2.material.shader = originalShader;
                    break;

                case 1:
                    // Apply Magenta color with UberShader
                    ApplyShaderAndColor(skyRenderer1, Color.magenta);
                    ApplyShaderAndColor(skyRenderer2, Color.magenta);
                    break;

                case 2:
                    // Apply Red color with UberShader
                    ApplyShaderAndColor(skyRenderer1, Color.red);
                    ApplyShaderAndColor(skyRenderer2, Color.red);
                    break;

                case 3:
                    // Apply Cyan color with UberShader
                    ApplyShaderAndColor(skyRenderer1, Color.cyan);
                    ApplyShaderAndColor(skyRenderer2, Color.cyan);
                    break;

                case 4:
                    // Apply Green color with UberShader
                    ApplyShaderAndColor(skyRenderer1, Color.green);
                    ApplyShaderAndColor(skyRenderer2, Color.green);
                    break;

                case 5:
                    // Apply Black color with UberShader
                    ApplyShaderAndColor(skyRenderer1, Color.black);
                    ApplyShaderAndColor(skyRenderer2, Color.black);
                    break;

                default:
                    return;
            }
        }

        // Helper method to apply the original material
        private void ApplyMaterial(MeshRenderer renderer, Material material)
        {
            if (renderer.material != material)
            {
                renderer.material = material;
            }
        }

        // Helper method to apply the shader and color
        private void ApplyShaderAndColor(MeshRenderer renderer, Color color)
        {
            if (renderer.material.shader != uberShader)
            {
                renderer.material.shader = uberShader;
            }

            if (renderer.material.color != color)
            {
                renderer.material.color = color;
            }
        }
    }
}
