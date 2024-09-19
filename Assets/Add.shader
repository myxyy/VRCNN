Shader "Unlit/Add"
{
    Properties
    {
        _MatA ("Matrix A", 2D) = "black" {}
        _MatB ("Matrix B", 2D) = "black" {}
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
            // make fog work
            #pragma multi_compile_fog

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

            sampler2D _MatA;
            float4 _MatA_TexelSize;
            sampler2D _MatB;
            float4 _MatB_TexelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 a = tex2Dlod(_MatA, float4(i.uv,0,0));
                float4 b = tex2Dlod(_MatB, float4(i.uv,0,0));
                return a + b;
            }
            ENDCG
        }
    }
}
