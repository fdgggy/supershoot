// Simplified Diffuse shader. Differences from regular Diffuse one:
// - no Main Color
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Mobile/Half Diffuse" 
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" }
		LOD 150

		CGPROGRAM
		#pragma surface surf HalfLambert noforwardadd

		sampler2D _MainTex;

		struct Input 
		{
			float2 uv_MainTex;
		};

		half4 LightingHalfLambert (SurfaceOutput s, half3 lightDir, half atten) 
		{
        	half NdotL = dot (s.Normal, lightDir);
        	half diff = NdotL * 0.5 + 0.5;
        	half4 c;
        	c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);
        	c.a = s.Alpha;
        	return c;
		}


		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}

	Fallback "Mobile/Diffuse"
}
