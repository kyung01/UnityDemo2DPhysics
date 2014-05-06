Shader "Custom/TextureFader" {
	Properties {
		_Ratio ("Ratio", Float) = .3
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue" = "Transparent" }
		Pass {
			Blend SrcAlpha OneMinusSrcAlpha     // Alpha blending
			Cull Off ZWrite Off
			ColorMask RGBA
			
			Color (0,0,0,.1)
			/**
			SetTexture [_MainTex] {
				ConstantColor(1 ,1,1,.9)
				combine texture*constant 
			}
			**/
		}
		
		

	}
	FallBack "Diffuse"
}
