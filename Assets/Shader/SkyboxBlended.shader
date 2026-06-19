Shader "RenderFX/Skybox Blended" {
Properties {
 _Tint ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
 _Blend ("Blend", Range(0,1)) = 0.5
 _FrontTex ("Front (+Z)", 2D) = "white" {}
 _BackTex ("Back (-Z)", 2D) = "white" {}
 _LeftTex ("Left (+X)", 2D) = "white" {}
 _RightTex ("Right (-X)", 2D) = "white" {}
 _UpTex ("Up (+Y)", 2D) = "white" {}
 _DownTex ("Down (-Y)", 2D) = "white" {}
 _FrontTex2 ("2 Front (+Z)", 2D) = "white" {}
 _BackTex2 ("2 Back (-Z)", 2D) = "white" {}
 _LeftTex2 ("2 Left (+X)", 2D) = "white" {}
 _RightTex2 ("2 Right (-X)", 2D) = "white" {}
 _UpTex2 ("2 Up (+Y)", 2D) = "white" {}
 _DownTex2 ("2 Down (-Y)", 2D) = "white" {}
}
SubShader { 
 Tags { "QUEUE"="Background" }
 Pass {
  Tags { "QUEUE"="Background" }
  Color [_Tint]
  Cull Off
  Fog { Mode Off }
  SetTexture [_FrontTex] { combine texture }
  SetTexture [_FrontTex2] { ConstantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }
  SetTexture [_FrontTex2] { combine previous +- primary, previous alpha * primary alpha }
 }
 Pass {
  Tags { "QUEUE"="Background" }
  Color [_Tint]
  Cull Off
  Fog { Mode Off }
  SetTexture [_BackTex] { combine texture }
  SetTexture [_BackTex2] { ConstantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }
  SetTexture [_BackTex2] { combine previous +- primary, previous alpha * primary alpha }
 }
 Pass {
  Tags { "QUEUE"="Background" }
  Color [_Tint]
  Cull Off
  Fog { Mode Off }
  SetTexture [_LeftTex] { combine texture }
  SetTexture [_LeftTex2] { ConstantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }
  SetTexture [_LeftTex2] { combine previous +- primary, previous alpha * primary alpha }
 }
 Pass {
  Tags { "QUEUE"="Background" }
  Color [_Tint]
  Cull Off
  Fog { Mode Off }
  SetTexture [_RightTex] { combine texture }
  SetTexture [_RightTex2] { ConstantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }
  SetTexture [_RightTex2] { combine previous +- primary, previous alpha * primary alpha }
 }
 Pass {
  Tags { "QUEUE"="Background" }
  Color [_Tint]
  Cull Off
  Fog { Mode Off }
  SetTexture [_UpTex] { combine texture }
  SetTexture [_UpTex2] { ConstantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }
  SetTexture [_UpTex2] { combine previous +- primary, previous alpha * primary alpha }
 }
 Pass {
  Tags { "QUEUE"="Background" }
  Color [_Tint]
  Cull Off
  Fog { Mode Off }
  SetTexture [_DownTex] { combine texture }
  SetTexture [_DownTex2] { ConstantColor (0,0,0,[_Blend]) combine texture lerp(constant) previous }
  SetTexture [_DownTex2] { combine previous +- primary, previous alpha * primary alpha }
 }
}
Fallback "RenderFX/Skybox"
}