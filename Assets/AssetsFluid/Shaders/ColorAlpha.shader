Shader "Custom/ColorAlpha" {
	Properties {
		_Color ("Main Color", Color) = (0,0,0,1)
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "Queue" = "Transparent" }
		Pass {  
			Blend SrcAlpha OneMinusSrcAlpha     // Alpha blending
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"
			
			float4 _MainTex_ST;//needed for the library 
			float4 _Color;
			sampler2D _MainTex;
			
			struct v2f {
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};
			
			
			v2f vert (appdata_base v)
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);
				return o;
			}
			
			half4 frag (v2f i) : COLOR
			{
				half4 texcol =  tex2D (_MainTex, i.uv);
				float4	colorIn = float4(0,0,1,1),
						colorOut = float4(1,0,0,1);
				texcol.rgb = _Color.rgb;
				return texcol;
			}  
			ENDCG 
		}
	} 
	FallBack "Unit/Transparent"
}
/**
//#pragma surface surf Lambert
struct v2f {
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};
			struct Input {
				float2 uv_MainTex;
			};
			struct Input {
				float2 uv_MainTex;
			};
**/