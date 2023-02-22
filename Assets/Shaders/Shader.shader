Shader "Unlit/Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_OutlineColor ("Outline color", Color) = (1,1,1,1)
		_OutlineWidth ("Outline width", Float) = 0.01
    }

    SubShader 
    {
         Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
			Name "Outline"

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
            float4 _MainTex_ST;
			float4 _OutlineColor;
			float _OutlineWidth;

            v2f vert (appdata v)
            {
                v2f o;
				float antiScale = 1.0 - _OutlineWidth;
                o.vertex = UnityObjectToClipPos(v.vertex * antiScale);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed4 col = tex2D(_MainTex, i.uv);
				// fixed4 col = _OutlineColor;
                return col;
            }
            ENDCG
        }
    }
}
