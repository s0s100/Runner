Shader "Unlit/TwoPasses"
{
	SubShader
	{
		LOD 100

		Tags { "Queue" = "Transparent" }

		Pass  // Executes this first
		{
			Tags { "LightMode" = "SRPDefaultUnlit" }

			Cull Off
			ZWrite Off
			ZTest Always

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return float4(0,1,0,0);  // Green
			}

			ENDCG
		}

		Pass  // Executes this second
		{
			Tags { "LightMode" = "UniversalForward" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return float4(1,0,0,0);  // Red
			}
			ENDCG
		}
	}
}