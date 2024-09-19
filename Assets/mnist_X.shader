Shader "mnist/mnist_X"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Index ("Index", Int) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry"}
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
            uint _Index;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            float getvaluefromtexture(uint index)
            {
                int x = (index % (int)(_MainTex_TexelSize.z * 4)) / 4;
                int y = index / (int)(_MainTex_TexelSize.z * 4);
                //int x = (index / 4 - y * _MainTex_TexelSize * 4);
                float2 uv = (float2(x,y)+.5)/_MainTex_TexelSize.zw;
                uv.y = 1-uv.y;
                switch (index % 4)
                {
                    case 0:
                        return tex2D(_MainTex, uv).r;
                    case 1:
                        return tex2D(_MainTex, uv).g;
                    case 2:
                        return tex2D(_MainTex, uv).b;
                    case 3:
                        return tex2D(_MainTex, uv).a;
                    default:
                        return 1;
                }
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.y = 1-uv.y;
                return getvaluefromtexture(uint(floor(uv.y*28)*28) + uint(floor(uv.x*28))+uint(_Index*(28*28+1)));
                //return getvaluefromtexture(floor(i.uv.x*28)*28 + floor(i.uv.y*28)+_Index*(28*28+1));
            }
            ENDCG
        }
    }
}
