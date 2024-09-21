Shader "Unlit/MatMul"
{
    Properties
    {
        _MatA ("Matrix LxM", 2D) = "black" {}
        _MatB ("Matrix MxN", 2D) = "black" {}
        _TransposeA ("Transpose A", Float) = 0
        _TransposeB ("Transpose B", Float) = 0
        _MainTex ("Matrix LxN To Accumulate", 2D) = "black" {}
        _MInd ("M Index To Accumulate", Int) = 1
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
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _MInd;
            float _TransposeA;
            float _TransposeB;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float4 prev = tex2Dlod(_MainTex, float4(i.uv,0,0));
                float2 uva = _TransposeA ?  float2((_MInd + .5) * _MatA_TexelSize.x, i.uv.x) : float2(i.uv.x, (_MInd + .5) * _MatA_TexelSize.y);
                float4 a = tex2Dlod(_MatA, float4(uva, 0, 0));
                float2 uvb = _TransposeB ? float2(i.uv.y, (_MInd + .5) * _MatB_TexelSize.y) : float2((_MInd + .5) * _MatB_TexelSize.x, i.uv.y);
                float4 b = tex2Dlod(_MatB, float4(uvb, 0, 0));
                return prev + a*b;
            }
            ENDCG
        }
    }
}
