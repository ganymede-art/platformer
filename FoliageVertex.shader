Shader "Custom/FoliageVertex" 
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_Emission("Emission", Color) = (0,0,0,0)
		_Clip("Alpha cutoff", Range(0,1)) = 0.5

		_SwaySpeed("Sway Speed", range(0,10)) = 0.1
		_SwayDist("Sway Distance", range(0,1)) = 0.1
		_SwayJitter("Sway Jitter", range(0,1)) = 0.1

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

		Tags {"RenderType" = "TransparentCutout" "IgnoreProjector" = "True" "Queue" = "AlphaTest" }


		CGPROGRAM
		#pragma surface surf BlinnPhong alphatest:_Clip addshadow vertex:vert

		struct Input 
		{
			float2 uv_MainTex;
			half4 color : COLOR;
		};

		sampler2D _MainTex;
		fixed4 _Color;
		fixed4 _Emission;
		float _SwaySpeed;
		float _SwayDist;
		float _SwayJitter;

		void vert(inout appdata_full v) 
		{
			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;

			v.vertex.x
				+= (sin(_Time.z * _SwaySpeed + worldPos.x) * _SwayDist)
				* (cos(_Time.w * _SwaySpeed + worldPos.x) * _SwayJitter)
				* v.vertex.y;

			v.vertex.z
				+= (sin(_Time.z * _SwaySpeed + worldPos.z) * _SwayDist)
				* (cos(_Time.w * _SwaySpeed + worldPos.z) * _SwayJitter)
				* v.vertex.y;
		}

		void surf(Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb * IN.color.rgb;
			o.Emission = _Emission;
			
			float ca = tex2D(_MainTex, IN.uv_MainTex).a;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Transparent/Cutout/Diffuse"
}