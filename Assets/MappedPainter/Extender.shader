Shader "MappedPainter/Extender"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }

		Pass
		{
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
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 col = tex2D(_MainTex, i.uv);

				if(col.a == 0)
				{
					col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(-1, -1));
					col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2( 0, -1));
					col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2( 1, -1));
					col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(-1,  0));
					//col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2( 0,  0));
					col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2( 1,  0));
					col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2(-1,  1));
					col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2( 0,  1));
					col += tex2D(_MainTex, i.uv + _MainTex_TexelSize.xy * float2( 1,  1));

					if(col.a > 0)
						col = col / col.a;
				}

				return col;
			}
			ENDCG
		}
	}
}
