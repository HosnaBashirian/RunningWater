﻿// Stylized Water Shader by Staggart Creations http://u3d.as/A2R
// Online documentation can be found at http://staggart.xyz

Shader "StylizedWater/Particle/Mobile"
{
	Properties
	{
		[MaterialEnum(Off,0,Front,1,Back,2)]  _Cull("Cull", Float) = 1
		[NoScaleOffset]_MainTex("Alpha (A)", 2D) = "white" {}
		[NoScaleOffset]_Disolve("Disolve (R)", 2D) = "white" {}
	}
	
	SubShader
	{
		Tags { "RenderType" = "Transparent"  "Queue" = "Transparent+1" "IgnoreProjector" = "True" "IsEmissive" = "true" }
		
		Pass
		{

			Name "FORWARD"
			Tags{ "LightMode" = "ForwardBase" }
			Cull[_Cull]
			ZWrite On

			CGPROGRAM
			#pragma target 2.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma exclude_renderers psp2 n3ds wiiu
			#pragma fragmentoption ARB_precision_hint_fastest

			#include "UnityCG.cginc"

			struct VertexIn
			{
				float4 pos : POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float4 color : COLOR;
			};

			struct VertexOut
			{
				float4 pos : SV_POSITION;
				float2 uv0 : TEXCOORD0;
				float2 uv1 : TEXCOORD1;
				float4 color : COLOR;
				//fixed3 ambient : TEXCOORD2;
			};

			uniform sampler2D _MainTex;
			uniform sampler2D _Disolve;

			VertexOut vert(VertexIn v)
			{
				VertexOut o;

				o.pos = UnityObjectToClipPos(v.pos);

				o.uv0 = v.uv0;
				o.uv1 = v.uv1;
				o.color = v.color;
				//o.ambient = UNITY_LIGHTMODEL_AMBIENT;

				return o;
			}

			fixed4 frag(VertexOut i) : SV_Target
			{
				float clipMask = step((1.0 - i.color.a) , tex2D(_Disolve, i.uv1).r * tex2D(_MainTex, i.uv0).a);
				clip(clipMask - 0.5);

				return float4(i.color.rgb, i.color.a);
			}
			ENDCG
		}
	}//Subshader
	FallBack "Diffuse"
}//Shader
