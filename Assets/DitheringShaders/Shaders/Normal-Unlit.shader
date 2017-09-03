// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Dithering Shaders/Normal/Unlit" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_ColorCount ("Mixed Color Count", float) = 4
		_PaletteHeight ("Palette Height", float) = 128
		_PaletteTex ("Palette", 2D) = "black" {}
		_DitherSize ("Dither Size (Width/Height)", float) = 8
		_DitherTex ("Dither", 2D) = "black" {}
	}

	SubShader {
		Tags { "IgnoreProjector"="True" "RenderType"="Opaque" }
		LOD 110
		Cull Off ZWrite Off ZTest Always

		Lighting Off
		BlendOp Max

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
				#pragma target 3.0
			#include "CGIncludes/Dithering.cginc"

			sampler2D _MainTex;
			sampler2D _PaletteTex;
			sampler2D _DitherTex;
			float _ColorCount;
			float _PaletteHeight;
			float _DitherSize;

			struct VertexInput {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct FragmentInput {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 ditherPos : TEXCOORD1;
			};

			FragmentInput vert(VertexInput i) {
				FragmentInput o;
				o.position = UnityObjectToClipPos(i.position);
				o.uv = i.uv;
				o.ditherPos = GetDitherPos(i.position, _DitherSize);
				return o;
			}

			fixed4 frag(FragmentInput i) : COLOR {
				fixed4 c = tex2D(_MainTex, i.uv);
				return fixed4(GetDitherColor(c.rgb, _DitherTex, _PaletteTex,
					_PaletteHeight, i.ditherPos, _ColorCount), c.a);
			}
			ENDCG
		}
	}

	Fallback "Unlit/Texture"
}
