Shader "Unlit/ChoiceWall"
{
    Properties
    {
        _MainColor("Main Color", Color) = (1,1,1,1)
        _FresnelMultiplier("Fresnel Multiplier", Float) = 2
//        _Noise ("Noise", 2D) = "white" { }
        [NoScaleOffset] _WaveNormal ("Wave Normal", 2D) = "bump" { }
        [NoScaleOffset] _SecondNormal ("Second Normal", 2D) = "bump" { }

    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent" "Queue" = "Transparent"
        }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha

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
                float3 tangent: TANGENT;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 pos : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                float4 grabPos : TEXCOORD0;
                float3 normalDir : NORMAL;
                float3 worldNormal: NORMAL1;
                float3 worldPos: TEXCOORD5;
                float3 worldTangent: TEXCOORD3;
                float3 worldBinormal: TEXCOORD4;
            };


            float4 _MainColor;
            float _FresnelMultiplier;
            sampler2D _BackgroundTexture;
            sampler2D _Noise, _WaveNormal, _SecondNormal;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // view dir:
                o.normalDir = v.normalDir;
                o.viewDir = normalize(_WorldSpaceCameraPos - mul(unity_ObjectToWorld, v.vertex.xyz));
                o.grabPos = ComputeGrabScreenPos(o.vertex);

                // o.worldNormal = UnityObjectToWorldNormal(v.normal);

                o.worldNormal = UnityWorldToObjectDir(v.normalDir);
                // o.worldPos = UnityObjectToWorldPos(v.vertex);
                // o.worldPos = unity_ObjectToWorld;
                o.worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1));
                float3 worldTangent = mul(unity_ObjectToWorld, float4(v.tangent, 0)).xyz;
                float3 binormal = cross(v.normalDir, v.tangent);
                float3 worldBinormal = mul(unity_ObjectToWorld, float4(binormal, 0)).xyz;

                o.worldTangent = normalize(worldTangent);
                o.worldBinormal = normalize(worldBinormal);

                return o;
            }

            float3 getNormal(sampler2D normalMap, float2 uv, float3 normal, float3 binormal, float3 tangent)
            {
                float3 tangentNormal = tex2D(normalMap, uv).xyz;
                tangentNormal = normalize(tangentNormal * 2 - 1);
                float3x3 TBN = float3x3(normalize(tangent), normalize(binormal), normalize(normal));
                TBN = transpose(TBN);
                float3 worldNormal = mul(TBN, tangentNormal);
                return worldNormal;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 bump = getNormal(_WaveNormal, i.pos.xy + float2(-0.2, 0.8) * _Time.y * 0.35, i.worldNormal,
                                        i.worldBinormal, i.worldTangent);
                float3 bump2 = getNormal(_SecondNormal, i.pos.xy + float2(0.75, -0.15) * _Time.y * 0.4, i.worldNormal,
                                         i.worldBinormal, i.worldTangent);
                float4 offset = float4(bump.xyz * 0.5 + float4(0.5, -0.5, 0, 0) * 0.5, 0);
                float4 offset2 = float4(bump2.xyz * 0.5 + float4(0.5, -0.5, 0, 0) * 0.5, 0);
                
                half4 bg = tex2Dproj(_BackgroundTexture, i.grabPos + offset + offset2);
                fixed4 col = bg;
                // ripple effect:

                col = lerp(_MainColor, col,
                           pow(1 - length(cross(i.normalDir, i.viewDir)), _FresnelMultiplier));

                // col /= 2;
                // col = lerp(_MainColor, col, rippleMultiplier);
                col.a = _MainColor.a;
                col.rgb = lerp(col.rgb, _MainColor.rgb, col.a * 0.75);
                return col;
            }
            ENDCG
        }
    }
}