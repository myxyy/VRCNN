Shader "Unlit/RandomUniform"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Seed ("Seed", Float) = 0
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

            sampler2D _MainTex;
            float _Seed;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float h31(float3 p)
            {
                return frac(sin(dot(float3(23.2545,43.62542,12.4254),p))*32412.245);
            }

            float4 frag (v2f i) : SV_Target
            {
                return (h31(float3(i.uv, _Seed)) * 2 - 1);
            }
            ENDCG
        }
    }
}
