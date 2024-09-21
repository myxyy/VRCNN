Shader "Unlit/ShowMNIST64LabelPredict"
{
    Properties
    {
        _MainTex ("Predict", 2D) = "white" {}
        _LabelTex ("Answer", 2D) = "white" {}
        _DigitTex ("Digit", 2D) = "white" {}
        _OKTex ("OK", 2D) = "white" {}
        _NGTex ("NG", 2D) = "white" {}
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
            sampler2D _LabelTex;
            sampler2D _DigitTex;
            sampler2D _OKTex;
            sampler2D _NGTex;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.y = 1 - uv.y;
                float2 uv8i = floor(uv * 8);
                float2 uv8f = frac(uv * 8);
                float index = uv8i.y * 8 + uv8i.x;

                float dind;
                for (int j=0; j<10; j++)
                {
                    if (tex2Dlod(_MainTex, float4((index+0.5)/64, (j+0.5)/10, 0, 0)).r == 1)
                    {
                        dind = j;
                    }
                }

                fixed4 col = tex2D(_DigitTex, float2((dind + uv8f.x) / 10, 1-uv8f.y));
                fixed4 okngtex = tex2Dlod(_LabelTex, float4((index+0.5)/64, (dind+0.5)/10, 0, 0)).r == 1 ? tex2Dlod(_OKTex, float4(uv8f, 0, 0)) : tex2Dlod(_NGTex, float4(uv8f.x, 1-uv8f.y, 0, 0));
                col = lerp(col, okngtex, okngtex.a);
                return col;
            }
            ENDCG
        }
    }
}
