// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Shield"
{
    Properties
    {
        _MainCol ("Main Color", Color) = (1,1,1,1)
        _FresnelMultiplier("Fresnel Multiplier", Float) = 2
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "Queue" = "Transparent"
        }
        LOD 100


        GrabPass
        {
            "_BackgroundTexture"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normalDir : NORMAL;
            };

            struct v_input
            {
                float4 vertex : SV_POSITION;
                float4 pos : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float4 grabPos : TEXCOORD0;
                float3 normalDir : NORMAL;
            };

            float4 _MainCol;

            sampler2D _BackgroundTexture;
            float _FresnelMultiplier;

            v_input vert(appdata v)
            {
                v_input o;
                o.pos = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // view dir:
                o.normalDir = v.normalDir;
                o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex.xyz));
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                return o;
            }

            fixed4 frag(v_input i) : SV_Target
            {
                half4 bg = tex2Dproj(_BackgroundTexture, i.grabPos);
                fixed4 col = bg;
                col = lerp(_MainCol, col, pow(1- length(cross(i.normalDir, i.viewDir)), _FresnelMultiplier));
                return col;
            }
            ENDCG
        }
    }
}