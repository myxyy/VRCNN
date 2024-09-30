Shader "Unlit/ShowSoftmax"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float maxVal = -100000;
                for (int j=0; j<_MainTex_TexelSize.w; j++)
                {
                    maxVal = max(maxVal, tex2Dlod(_MainTex, float4(0.5, (j+0.5) * _MainTex_TexelSize.y, 0, 0)).r);
                }
                float sum = 0;
                for (int k=0; k<_MainTex_TexelSize.w; k++)
                {
                    sum += exp(tex2Dlod(_MainTex, float4(0.5, (k+0.5) * _MainTex_TexelSize.y, 0, 0)).r - maxVal);
                }
                float index = floor(i.uv.x * _MainTex_TexelSize.w);
                float val = exp(tex2Dlod(_MainTex, float4(0.5, (index+0.5) * _MainTex_TexelSize.y, 0, 0)).r - maxVal)/sum;

                fixed4 col = float4(step(i.uv.y, val).xxx, 1);
                return col;
            }
            ENDCG
        }
    }
}
