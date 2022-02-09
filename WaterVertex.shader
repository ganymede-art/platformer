Shader "Custom/WaterVertex"
{
	Properties
	{
		_BaseColor("Base Color", Color) = (1,1,1,1)
		_HighlightColor("Color", Color) = (1,1,1,1)
		_1Tex("Layer 1", 2D) = "white" {}
		_2Tex("Layer 2", 2D) = "white" {}
		_Emission("Emission", Color) = (0,0,0,0)
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent"  "Queue" = "Transparent"}
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf Standard alpha:blend

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _1Tex;
			sampler2D _2Tex;

			struct Input
			{
				float2 uv_1Tex;
				float2 uv_2Tex;
				half4 color : COLOR;
			};

			fixed4 _BaseColor;
			fixed4 _HighlightColor;
			fixed4 _Emission;

			// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
			// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
			// #pragma instancing_options assumeuniformscaling
			UNITY_INSTANCING_BUFFER_START(Props)
				// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				// Albedo comes from a texture tinted by color
				fixed4 c = tex2D(_1Tex, IN.uv_1Tex);
				fixed4 b = tex2D(_2Tex, IN.uv_2Tex);
				fixed4 d = (c * b);
		
				o.Albedo = (_BaseColor.rgb + (d.rgb * _HighlightColor.rgb * _HighlightColor.a)) * IN.color.rgb;
				o.Alpha = (_BaseColor.a + (d.rgb * _HighlightColor.a)) * IN.color.a;
				o.Emission = _Emission;
			}
			ENDCG
		}
			FallBack "Diffuse"
}
