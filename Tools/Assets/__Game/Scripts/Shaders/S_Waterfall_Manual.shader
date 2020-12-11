Shader "JMD_Manual/WaterfallManual"
{
	Properties
	{
		_WhiteColor("White Color", Color) = (0.9, 0.9, 1.0, 1.0)
		_DarkenColor("Darken Color", Color) = (0.2, 0.2, 0.5, 1.0)
		_BlackColor("Black Color", Color) = (0.5, 0.5, 1.0, 1.0)
		_WaterfallMasks("Waterfall Masks", 2D) = "white" {}
		_Offset("Offset", Range(0.0, 1.0)) = 0.0
		_Tiling("Tiling", Range(1.0, 10.0)) = 0.0
		_FlowSpeed("Flow Speed", Range(0.1, 1.0)) = 0.0
	}
	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Geometry+5" "IgnoreProjector" = "True" "ForceNoShadowCasting" = "True" }

		LOD 100

		Pass
		{
			Cull Back
			CGPROGRAM

			#pragma exclude_renderers xbox360 xboxone ps4 psp2 n3ds wiiu 

			#pragma vertex vert
			#pragma fragment frag

			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			uniform sampler2D _WaterfallMasks;
			uniform half _FlowSpeed;
			uniform half _Tiling;
			uniform half _Offset;
			uniform fixed4 _BlackColor;
			uniform fixed4 _WhiteColor;
			uniform fixed4 _DarkenColor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//The meshes UVs have already been changed inside of Maya, so the rest of this is pretty straight forward.
				//Pan the UVs.
				half2 panningUVs = (_Time.y * half2(0.0, _FlowSpeed) + half2((_Tiling * (i.uv.x + _Offset)), i.uv.y));
				//Assign the panning UVs to our texture sampler.
				fixed3 WaterfallMasksTex = tex2D(_WaterfallMasks, panningUVs);
				//Allow the user to change the color of the water.
				fixed3 maskColoring = lerp(_BlackColor , _WhiteColor , WaterfallMasksTex.r);
				//This is the "magic" line, here we saturate all of the colors for safety (this will be free, should we not need it and if we do, then it's there and almost free)
				//We fake reflection in the water, based upon vertex color G.
				//We also add some deeper water areas using the vertex color B.
				fixed3 deepAndReflectiveAreas = saturate(lerp(((pow(i.color.g , 2.54) * WaterfallMasksTex.g) + maskColoring) , _DarkenColor , (i.color.b * WaterfallMasksTex.b)));
				fixed4 finalColor = fixed4(deepAndReflectiveAreas, 1.0);
				return finalColor;
			}
			ENDCG
		}
	}
}
