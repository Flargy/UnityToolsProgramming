using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

namespace WaterfallShader
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(MeshRenderer))]
    public class WaterfallShader : MonoBehaviour
    {
        public static Renderer meshRenderer;
        public Material material;
        public Shader shader;
        public float flowSpeed;
        public float offset;
        public float tiling;
        public Color whiteColor;
        public Color darkenedColor;
        public Color blackColor;
        public Texture2D waterfallMasks;
        private void OnEnable()
        {
            if (!meshRenderer)
            {
                meshRenderer = GetComponent<MeshRenderer>();
            }
            if (!material)
            {
                material = meshRenderer.sharedMaterial;
            }

            GetProperties();
            SetProperties();
        }

        public void GetProperties()
        {
            flowSpeed = material.GetFloat("_FlowSpeed");
            offset = material.GetFloat("_Offset");
            tiling = material.GetFloat("_Tiling");

            whiteColor = material.GetColor("_WhiteColor");
            darkenedColor = material.GetColor("_DarkenedColor");
            blackColor = material.GetColor("_BlackColor");

            waterfallMasks = material.GetTexture("_WaterfallMasks") as Texture2D;
        }
        public void SetProperties()
        {
            material.SetFloat("_FlowSpeed", flowSpeed);
            material.SetFloat("_Offset", offset);
            material.SetFloat("_Tiling", tiling);
            
            material.SetColor("_WhiteColor", whiteColor);
            material.SetColor("_DarkenedColor", darkenedColor);
            material.SetColor("_BlackColor", blackColor);
            
            material.SetTexture("_WaterfallMasks", waterfallMasks);
        }

    }
}