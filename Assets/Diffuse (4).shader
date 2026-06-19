Shader "KBZ3/PBR/Diffuse"
{
    Properties
    {
        // Основные цвета
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        
        // Normal Map
        _BumpMap ("Normal Map", 2D) = "bump" {}

        // Metallic: Текстура + Слайдер
        // Если текстуры нет (она белая), работает только слайдер.
        _MetallicMap ("Metallic Map", 2D) = "white" {}
        _Metallic ("Metallic Slider", Range(0,1)) = 0.0

        // Roughness: Текстура + Слайдер
        // Unity использует Smoothness, поэтому мы инвертируем значения внутри шейдера.
        // 0 - идеально гладко, 1 - грубо.
        _RoughnessMap ("Roughness Map", 2D) = "white" {}
        _Roughness ("Roughness Slider", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Использование Standard lighting model с полными тенями
        #pragma surface surf Standard fullforwardshadows

        // Shader Model 3.0 для поддержки текстур и сложной математики
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _MetallicMap;
        sampler2D _RoughnessMap;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv_MetallicMap; // Если UV отличаются, иначе можно использовать uv_MainTex
            float2 uv_RoughnessMap;
            
            // Получаем доступ к Vertex Color для AO
            float4 color : COLOR;
        };

        half _Metallic;
        half _Roughness;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // --- ALBEDO ---
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;

            // --- NORMAL ---
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

            // --- METALLIC ---
            // Читаем карту (если "white", то вернет 1). Умножаем на слайдер.
            // Итог: если карты нет, управляет слайдер.
            fixed4 metalSample = tex2D(_MetallicMap, IN.uv_MetallicMap);
            o.Metallic = metalSample.r * _Metallic;

            // --- ROUGHNESS -> SMOOTHNESS ---
            // Unity Standard использует Smoothness (0=Rough, 1=Smooth).
            // Вы просили Roughness (0=Smooth, 1=Rough).
            // Поэтому мы инвертируем (1 - roughness).
            fixed4 roughSample = tex2D(_RoughnessMap, IN.uv_RoughnessMap);
            
            // Комбинируем карту и слайдер, затем инвертируем для Standard Model
            float roughnessVal = roughSample.r * _Roughness;
            o.Smoothness = 1.0 - roughnessVal;

            // --- AMBIENT OCCLUSION (VERTEX COLORS) ---
            // Самая важная часть.
            // Vertex Color (Красный канал или яркость) назначаем в o.Occlusion.
            // Unity автоматически применит это как AO:
            // Тени будут видны в тени (Ambient), но исчезнут на ярком свету.
            o.Occlusion = IN.color.r; 
        }
        ENDCG
    }
    FallBack "Diffuse"
}