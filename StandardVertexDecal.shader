Shader "Custom/StandardVertexDecal"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_Emission("Emission", Color) = (0,0,0,0)

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

			// Use a high render queue to make sure we draw after everything (however anything in the transparent queue won't get shadow info, so 2500 should work fine)
			Tags { "RenderType" = "Transparent"  "Queue" = "AlphaTest+50"}

			Cull Off

			CGPROGRAM
			// Use the decal blend function to keep the shadow rendering code - any normal blend will strip shadows
			#pragma surface surf BlinnPhong fullforwardshadows decal:blend

			struct Input
			{
				float2 uv_MainTex;
				half4 color : COLOR;
			};

			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _Emission;

			void surf(Input IN, inout SurfaceOutput o)
			{
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb * IN.color.rgb;
				o.Alpha = IN.color.a;
				o.Emission = _Emission;
			}
			ENDCG
		}
			Fallback "Diffuse"
}