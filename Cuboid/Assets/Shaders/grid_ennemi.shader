Shader "Unlit/grid_ennemi"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" { }
		_Resolution("Résolution", Float) = 6
		_Intensity("Intensité", Float) = 10
		_Speed("Vitesse", Float) = 1
		_LightColor("Couleur highlight", Color) = (0,1,0.15,1)
		_BaseColor("Couleur de base", Color) = (0.7,0.7,0.7,1)

		/*
		uniform float time;  _Time Time since level load (t/20, t, t*2, t*3), use to animate things inside the shaders
uniform float resolution;
uniform float intensity;
uniform float speed;
uniform vec3 lightColor;
uniform vec3 baseColor;*/
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

			//atribute
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

	//vary
			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			//uniform
			sampler2D _MainTex;
			float _Resolution;
			float _Intensity;
			float _Speed;
			float4 _LightColor;
			float4 _BaseColor;
			
			float2 circuit(float2 p) {
				p = frac(p);
				float r = 0.01;
				float v = 0.0, g = 1.0;
				float d;

				const int iter = 7;
				for (int i = 0; i < iter; i++) {
					d = p.x - r;
					g += pow(saturate(1.0 - abs(d)), 200.0);

					if (d > 0.0) {
						p.x = (p.x - r) / (1.8 - r);
					} else {
						p.x = p.x;
					}
					p = p.yx;
				}
				v /= float(iter);
				return float2(g, v);
			}

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			float4 frag (v2f i) : SV_Target
			{
				float2 uv = (i.uv + 0.5) * _Resolution;
				float2 cid2 = floor(uv);
				float cid = (cid2.y + cid2.x);

				float2 dg = circuit(uv);
				float d = dg.x;
				float3 col1 = (0.2 - max(min(d, 2.0) - 1.0, 0.0)) * _BaseColor.rgb;
				float3 col2 = max(d - 1.0, 0.0) * _LightColor.rgb;

				float f = max(0.4 - fmod(uv.y - uv.x + (_Time.y * _Speed) + (dg.y * 0.2), 2.5), 0.0) * _Intensity;
				col2 *= f;

				float4 col = float4(5*col1 + col2, 1.0);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
				//return texcol*col;
			}
			ENDCG
		}
	}
}
