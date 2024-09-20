Shader "Unlit/ShowMNIST64"
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
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.y = 1 - uv.y;
                float2 uv8i = floor(uv * 8);
                float2 uv8f = frac(uv * 8);
                float index = uv8i.y * 8 + uv8i.x;
                float2 uv8_28i = floor(uv8f * 28);
                float vindex = uv8_28i.y * 28 + uv8_28i.x;

                fixed4 col = tex2Dlod(_MainTex, float4((index + 0.5) / 64, (vindex + 0.5) / (28*28), 0, 0)).r;
                return col;
            }
            ENDCG
        }
    }
}
