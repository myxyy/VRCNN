Shader "Unlit/ReLU"
{
    Properties
    {
        _MainTex ("Input", 2D) = "black" {}
        _Grad ("OutputGrad", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGINCLUDE

        #include "UnityCG.cginc"

        #pragma vertex vert
        #pragma fragment frag

        sampler2D _MainTex;
        float4 _MainTex_TexelSize;
        sampler2D _Grad;
        float4 _Grad_TexelSize;

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

            float4 frag (v2f i) : SV_Target
            {
                return max(0, tex2Dlod(_MainTex, float4(i.uv, 0, 0)).r);
            }

            ENDCG
        }

        Pass
        {
            Name "Backward"
            CGPROGRAM

            float4 frag (v2f i) : SV_Target
            {
                return step(0, tex2Dlod(_MainTex, float4(i.uv, 0, 0)).r) * tex2Dlod(_Grad, float4(i.uv, 0, 0)).r;
            }

            ENDCG
        }
    }
}
