Shader "Unlit/Flatten"
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
                float index = floor(i.uv.y * _MainTex_TexelSize.z * _MainTex_TexelSize.w);
                return tex2Dlod(_MainTex, float4(
                    ((fmod(index, _MainTex_TexelSize.z) + .5) * _MainTex_TexelSize.x),
                    ((floor(index * _MainTex_TexelSize.x) + .5) * _MainTex_TexelSize.y),
                    0, 0)).r;
            }

            ENDCG
        }
    }
}
