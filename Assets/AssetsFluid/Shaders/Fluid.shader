Shader "Custom/Fluid" {
	Properties {
		//_Color ("Main Color", Color) = (1,1,1,0.5)
		_MainTex ("Texture", 2D) = "white" { }
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
			
			float4 _Color;
			sampler2D _MainTex;
			
			struct v2f {
				float4  pos : SV_POSITION;
				float2  uv : TEXCOORD0;
			};
			
			float4 _MainTex_ST;
			
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
						
				if(texcol.r > .15f) return colorIn;
				else if (texcol.r >= .1f) return colorOut;
				return texcol;
			}
			ENDCG
	
		}
}
Fallback "VertexLit"
} 