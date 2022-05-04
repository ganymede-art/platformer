// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/WaterVertex"
{
	Properties
	{
		_BaseColor("Base Color",Color)=(1,1,1,1)
		_EffectColor("Effect Color", Color) = (1,1,1,1)
		_Emission("Emission", Color) = (0,0,0,0)

		_1Tex("Layer 1", 2D) = "white" {}
		_2Tex("Layer 2", 2D) = "white" {}

		_EffectCutoffBegin("Alpha Cutoff Begin", Range(0,1)) = 0.1
		_EffectCutoffEnd("Alpha Cutoff End", Range(0,1)) = 1.0

		_BaseOpacityModifier("Effect Opacity Modifier",Range(0,1)) = 1.0
		_EdgeOpacityModifier("Edge Opacity Modifier",Range(0,1)) = 1.0

		_DepthMultiplier("Depth Multiplier", Range(0,10)) = 7.5
	}

	SubShader
	{
		Tags { "RenderType" = "Transparent" "IgnoreProjector" = "True"  "Queue" = "Transparent"}

		Blend SrcAlpha OneMinusSrcAlpha

		CGPROGRAM
		#pragma surface surf BlinnPhong keepalpha 

		struct Input
		{
			half4 color : COLOR;
		};

		fixed4 _BaseColor;

		void surf(Input IN, inout SurfaceOutput o)
		{
			o.Albedo = _BaseColor.rgb;
			o.Alpha = _BaseColor.a * IN.color.a;

		}

		ENDCG

		CGPROGRAM
		#pragma surface surf BlinnPhong  keepalpha

		struct Input
		{
			float2 uv_1Tex;
			float2 uv_2Tex;
			half4 color : COLOR;
			float4 screenPos;
		};

		sampler2D _CameraDepthTexture;
		sampler2D _1Tex;
		sampler2D _2Tex;
		fixed4 _EffectColor;
		fixed4 _Emission;

		half _EffectCutoffBegin;
		half _EffectCutoffEnd;

		half _BaseOpacityModifier;
		half _EdgeOpacityModifier;

		float _DepthMultiplier;

		void surf(Input IN, inout SurfaceOutput o)
		{
			// get uvs.

			fixed4 c1 = tex2D(_1Tex, IN.uv_1Tex);
			fixed4 c2 = tex2D(_2Tex, IN.uv_2Tex);
			
			// get the depth sample.

			float4 depthSample = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, IN.screenPos);
			float depth = LinearEyeDepth(depthSample).r;

			// get the 0 to 1 depth alpha value from the depth 
			// (taking the depth multiplier into account).

			float depthAlpha = 1 - saturate(_DepthMultiplier * (depth - IN.screenPos.w));

			// get and clip both the base alpha and edge alpha.

			float baseAlpha = (c1.a + c2.a) * _EffectColor.a ;
			float edgeAlpha = (c1.a + c2.a) * 2 * depthAlpha;

			if (baseAlpha > _EffectCutoffBegin && baseAlpha < _EffectCutoffEnd)
				baseAlpha = 0;

			baseAlpha *= _BaseOpacityModifier;

			if (edgeAlpha > _EffectCutoffBegin && edgeAlpha < _EffectCutoffEnd)
				edgeAlpha = 0;

			edgeAlpha *= _EdgeOpacityModifier;

			// set base colour and alpha.

			o.Albedo = _EffectColor.rgb;
			o.Alpha = baseAlpha + edgeAlpha;

			// fade alpha by vertex alpha.

			o.Alpha = o.Alpha * IN.color.a;
			
			// add emission.

			o.Emission = _Emission;
		}

		ENDCG
	}
	Fallback "Diffuse"
}