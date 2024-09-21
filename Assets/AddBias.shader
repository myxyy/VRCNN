Shader "Unlit/AddBias"
{
    Properties
    {
        _MainTex ("Bias/BiasGrad", 2D) = "black" {}
        _Input ("Input", 2D) = "black" {}
        _OutGrad ("Out Grad", 2D) = "black" {}
        _MInd ("Index to add bias", Int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGINCLUDE

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
            Name "Forward"
            CGPROGRAM

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            sampler2D _Input;
            float4 _Input_TexelSize;

            float4 frag (v2f i) : SV_Target
            {
                float input = tex2Dlod(_Input, float4(i.uv,0,0)).r;
                float bias = tex2Dlod(_MainTex, float4(i.uv,0,0)).r;
                return input + bias;
            }
            ENDCG
        }
        
        Pass
        {
            Name "Backward"
            CGPROGRAM

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            sampler2D _OutGrad;
            float4 _OutGrad_TexelSize;
            int _MInd;

            float4 frag (v2f i) : SV_Target
            {
                float outGrad = tex2Dlod(_OutGrad, float4((_MInd + 0.5) * _OutGrad_TexelSize.x, i.uv.y,0,0)).r;
                float biasGrad = tex2Dlod(_MainTex, float4(i.uv,0,0)).r;
                return biasGrad + outGrad;
            }
            ENDCG
        }

    }
}
