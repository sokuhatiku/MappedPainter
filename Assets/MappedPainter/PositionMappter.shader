Shader "MappedPainter/PositionMapper"
{
	Properties
	{
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Cull off
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float3 pos : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};
		
			v2f vert (appdata v)
			{
				v2f o;

				float2 uv = v.uv * 2 - 1;
				uv.y = - uv.y;

				o.vertex = float4(uv, 1, 1);
				o.pos = v.vertex;
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				return float4(i.pos, 1); // a==1ならばマッピングが有効
			}
			ENDCG
		}
	}
}
