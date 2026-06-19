Shader "Custom/WeaponShadows"
{
	Properties{
	_Color("Color", Color) = (1, 1, 1, 1)
	_MainTex("Main Texture", 2D) = "white" {}
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert vertex:vert
		#pragma target 3.0

		struct Input {
			float4 color : COLOR; // Use 'color' semantic for vertex color
			float2 uv_MainTex;
		};

		sampler2D _MainTex;
		fixed4 _Color;

		void vert(inout appdata_full v, out Input o) {
			UNITY_INITIALIZE_OUTPUT(Input, o);
			o.color = v.color; // Assign vertex color directly
		}

		void surf(Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 col = IN.color * _Color - 0.1; // Multiply by global color
			o.Albedo = c.rgb * col.rgb;
			o.Alpha = col.a;
		}

		ENDCG
	}

	FallBack "Diffuse"
}