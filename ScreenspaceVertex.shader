// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/ScreenspaceVertex" 
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_EmissionTex("Emissive", 2D) = "white" {}
		_Emission("Emission", Color) = (0,0,0,0)
		_ScreenTex("Screenspace", 2D) = "gray" {}
		_ScreenTexScale("Screenspace Scale", Range(1,10)) = 1
		_ScreenAlbedoMult("Screenspace Brightness", Range(0, 2)) = 1
		

		_GIAlbedoColor("Color Albedo (GI)", Color) = (1,1,1,1)
		_GIAlbedoTex("Albedo (GI)",2D) = "white"{}
		
	}
	SubShader
	{
		Pass
		{
			Name "META"
			Tags {"LightMode" = "Meta"}
			Cull Off
			CGPROGRAM

			#include"UnityStandardMeta.cginc"

			sampler2D _GIAlbedoTex;
			fixed4 _GIAlbedoColor;
			float4 frag_meta2(v2f_meta i) : SV_Target
			{
				// We're interested in diffuse & specular colors
				// and surface roughness to produce final albedo.

				FragmentCommonData data = UNITY_SETUP_BRDF_INPUT(i.uv);
				UnityMetaInput o;
				UNITY_INITIALIZE_OUTPUT(UnityMetaInput, o);
				fixed4 c = tex2D(_GIAlbedoTex, i.uv);
				o.Albedo = fixed3(c.rgb * _GIAlbedoColor.rgb);
				o.Emission = Emission(i.uv.xy);
				return UnityMetaFragment(o);
			}

			#pragma vertex vert_meta
			#pragma fragment frag_meta2
			#pragma shader_feature _EMISSION
			#pragma shader_feature _METALLICGLOSSMAP
			#pragma shader_feature ___ _DETAIL_MULX2
			ENDCG
		}


		Tags 
		{ 
			"RenderType" = "Opaque"
		}

		CGPROGRAM
		#pragma surface surf BlinnPhong 

		struct Input 
		{
			float2 uv_MainTex;
			half4 color : COLOR;
			float4 screenPos;
		};

		sampler2D _MainTex;
		sampler2D _EmissionTex;
		sampler2D _ScreenTex;
		float4 _MainTex_TexelSize;
		fixed4 _Color;
		fixed4 _Emission;
		float _ScreenTexScale;
		float _ScreenAlbedoMult;

		void surf(Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			fixed4 e = tex2D(_EmissionTex, IN.uv_MainTex);
			o.Albedo = c.rgb * IN.color.rgb;
			o.Emission = e.rgb * _Emission;

			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			screenUV *= float2(_ScreenTexScale, _ScreenTexScale);
			o.Albedo *= tex2D(_ScreenTex, screenUV).rgb * _ScreenAlbedoMult;
		}


		ENDCG
	}
	Fallback "Diffuse"
}