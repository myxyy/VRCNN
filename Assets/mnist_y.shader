Shader "mnist/mnist_y"
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
            Cull Off
            ZWrite Off
            ZTest Always
            CGPROGRAM
            #include "UnityCustomRenderTexture.cginc"
            #pragma vertex CustomRenderTextureVertexShader 
            #pragma fragment frag

            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            uint _Index;

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

            float frag (v2f_customrendertexture i) : SV_Target
            {
                float2 uv = i.globalTexcoord;
                return floor(getvaluefromtexture((_Index+1)*(28*28+1)-1)*255+.5) == floor(uv.x*10) ? 1 : 0;
            }
            ENDCG
        }
    }
}
