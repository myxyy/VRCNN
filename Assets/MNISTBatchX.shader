Shader "Unlit/MNISTBatchX"
{
    Properties
    {
        _MNIST ("MNIST", 2D) = "white" {}
        _BatchSize ("Batch Size", Int) = 64
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

            sampler2D _MNIST;
            float4 _MNIST_TexelSize;
            uint _IndexList[64];
            uint _BatchSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }


            float getvaluefromtexture(uint index)
            {
                int x = (index % (int)(_MNIST_TexelSize.z * 4)) / 4;
                int y = index / (int)(_MNIST_TexelSize.z * 4);
                //int x = (index / 4 - y * _MNIST_TexelSize * 4);
                float2 uv = (float2(x,y)+.5)/_MNIST_TexelSize.zw;
                uv.y = 1-uv.y;
                switch (index % 4)
                {
                    case 0:
                        return tex2D(_MNIST, uv).r;
                    case 1:
                        return tex2D(_MNIST, uv).g;
                    case 2:
                        return tex2D(_MNIST, uv).b;
                    case 3:
                        return tex2D(_MNIST, uv).a;
                    default:
                        return 1;
                }
            }

            float4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uint index = _IndexList[(uint)floor(i.uv.x * _BatchSize)];

                return getvaluefromtexture(uint(floor(uv.y*28*28))+uint(index*(28*28+1)));
            }
            ENDCG
        }
    }
}
