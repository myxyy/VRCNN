Shader "Unlit/MaxOnehot"
{
    Properties
    {
        _MainTex ("Input", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM

            #include "UnityCG.cginc"

            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float maxVal = -1000000;
                for (int j=0; j<_MainTex_TexelSize.w; j++)
                {
                    maxVal = max(maxVal, tex2Dlod(_MainTex, float4(i.uv.x, (j + 0.5) * _MainTex_TexelSize.y, 0, 0))).r;
                }
                return tex2Dlod(_MainTex, float4(i.uv, 0, 0)).r == maxVal ? 1 : 0;
            }

            ENDCG
        }
    }
}
