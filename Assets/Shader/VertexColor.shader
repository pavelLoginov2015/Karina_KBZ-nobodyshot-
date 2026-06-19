Shader "Custom/VertexColorWithTexture"
{
	Properties
	{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Main Texture", 2D) = "white"
		_LightEnabled("Enable Light", Float) = 1.0
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		#pragma target 3.0

		struct Input {
			float4 vertColor;
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		fixed4 _Color;
		float _LightEnabled;

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.vertColor = v.color;
		}

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

			IN.vertColor.rgb *= _Color.rgb + 0.12;

			if (_LightEnabled > 0) {
				o.Albedo = c.rgb * IN.vertColor.rgb;
			} else {
				o.Albedo = c.rgb;
			}

			o.Alpha = IN.vertColor.a;
		}

		ENDCG
	}

	FallBack "Diffuse"
}