Shader "Kubezumie/Alpha No light" {
    Properties{
     _Color("Main Color", Color) = (1,1,1,1)
     _MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
    }
        SubShader
    {
     Pass
     {
      Material {
       Emission[_Emission]
       Specular[_SpecColor]
       Shininess[_Shininess]

      }
        Lighting On
      ZWrite On
      Blend SrcAlpha OneMinusSrcAlpha
      AlphaTest Greater 0
      ColorMask RGB

      ColorMaterial Emission
      SetTexture[_MainTex]
       {
        combine texture * primary, texture alpha * primary alpha
         }
     }

    }

        Fallback "Alpha/Diffuse"
}