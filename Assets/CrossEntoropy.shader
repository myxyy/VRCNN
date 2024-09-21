Shader "Unlit/CrossEntropy"
{
    Properties
    {
        _X ("Input", 2D) = "black" {}
        _Y ("Label", 2D) = "black" {}
        _OutGrad ("Out Grad", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGINCLUDE

        #include "UnityCG.cginc"

        #pragma vertex vert
        #pragma fragment frag

        sampler2D _X;
        float4 _X_TexelSize;
        sampler2D _Y;
        float4 _Y_TexelSize;
        sampler2D _OutGrad;
        float4 _OutGrad_TexelSize;

        float softmax(float2 uv)
        {
            float2 index = floor(uv * _X_TexelSize.zw);
            float inputMax = -100000;
            for (int j=0; j<_X_TexelSize.w; j++)
            {
                inputMax = max(inputMax, tex2Dlod(_X, float4((index.x+0.5)*_X_TexelSize.x, (j+0.5)*_X_TexelSize.y, 0, 0)).r);
            }
            float denom = 0;
            for (int k=0; k<_X_TexelSize.w; k++)
            {
                denom += exp(tex2Dlod(_X, float4((index.x+0.5)*_X_TexelSize.x, (k+0.5)*_X_TexelSize.y, 0, 0)).r - inputMax);
            }
            return exp(tex2Dlod(_X, float4((index.x+0.5)*_X_TexelSize.x, (index.y+0.5)*_X_TexelSize.y, 0, 0)).r - inputMax) / denom;
        }

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

        ENDCG

        Pass
        {
            Name "CrossEntropy"
            CGPROGRAM

            float4 frag (v2f i) : SV_Target
            {
                float2 index = floor(i.uv * _X_TexelSize.zw);
                float inputMax = -100000;
                for (int j=0; j<_X_TexelSize.w; j++)
                {
                    inputMax = max(inputMax, tex2Dlod(_X, float4((index.x+0.5)*_X_TexelSize.x, (j+0.5)*_X_TexelSize.y, 0, 0)).r);
                }
                float denom = 0;
                float logNumer = 0;
                for (int k=0; k<_X_TexelSize.w; k++)
                {
                    float fetchInput = tex2Dlod(_X, float4((index.x+0.5)*_X_TexelSize.x, (k+0.5)*_X_TexelSize.y, 0, 0)).r;
                    float fetchLabel = tex2Dlod(_Y, float4((index.x+0.5)*_Y_TexelSize.x, (k+0.5)*_Y_TexelSize.y, 0, 0)).r;
                    logNumer += (fetchInput - inputMax) * fetchLabel;
                    denom += exp(fetchInput - inputMax);
                }
                return log(denom) - logNumer;
            }

            ENDCG
        }

        Pass
        {
            Name "CrossEntropyGrad"
            CGPROGRAM

            float4 frag (v2f i) : SV_Target
            {
                float2 index = floor(i.uv * _X_TexelSize.zw);
                float sf = softmax((index + 0.5) * _X_TexelSize.xy);
                float label = tex2Dlod(_Y, float4((index + 0.5) * _Y_TexelSize.xy, 0, 0));
                return (sf - label) * tex2Dlod(_OutGrad, float4((index.x + 0.5) * _OutGrad_TexelSize.x, 0.5, 0, 0));
            }

            ENDCG
        }
    }
}
