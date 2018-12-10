///////////////////////////////////////////////////////////
//  Alpha_Test_Transprent_LM:
//
//  Created on:      2014-5-4
//  Original author: xlcw
//  History: 
//
//
///////////////////////////////////////////////////////////

Shader "WL/Alpha_Test_Transprent_LM" 
{
    Properties 
    {
        _Color ("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _MainTex ("Base (RGB) Alpha (A)", 2D) = "white" {}
        _Cutoff ("Base Alpha cutoff", Range (0,.9)) = .5
		//_LightMap ("Lightmap(RGB)", 2D) = "black" {LightmapMode}
    }

    SubShader 
    {
        // Set up basic lighting
		Tags 
		{ 
			"IgnoreProjector"="True"
			"RenderType"="TransparentCutOut"
			"ForceNoShadowCasting" = "True"
		}
		
		Lod 30

		
        Material 
       	{
            Diffuse [_Color]
            Ambient [_Color]
            Emission [_Color]
        }
        
        
        Lighting Off

        // Render both front and back facing polygons.
        Cull Off

        // first pass:
        //   render any pixels that are more than [_Cutoff] opaque

        Pass 
		{
			Tags { "LightMode" = "VertexLM" }
			BindChannels
			{
				Bind "Vertex", vertex
				Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
				Bind "texcoord", texcoord1 // main uses 1st uv
				//Bind "Color", color
			}

			//ColorMaterial Emission
			//Emission (.39, .39, .39, .0)
            AlphaTest Greater [_Cutoff]
			SetTexture [unity_Lightmap] 
			{
				matrix [unity_LightmapMatrix]
				combine texture
			}
			SetTexture [_MainTex] 
			{
				combine texture * previous DOUBLE, texture
			}
        }

	
		// Lightmapped, encoded as RGBM
		Pass 
		{
			Tags { "LightMode" = "VertexLMRGBM" }
		
			BindChannels 
			{
				Bind "Vertex", vertex
				Bind "texcoord1", texcoord0 // lightmap uses 2nd uv
				Bind "texcoord", texcoord1 // main uses 1st uv
				//Bind "Color", color
			}
		
			AlphaTest Greater [_Cutoff]			
			SetTexture [unity_Lightmap] 
			{
				matrix [unity_LightmapMatrix]
				combine texture * texture alpha DOUBLE
			}
			SetTexture [_MainTex] 
			{
				combine texture * previous QUAD, texture
			}
			
			
		}


        // Second pass:
        //   render in the semitransparent details.

        Pass 
		{
            // Dont write to the depth buffer

            ZWrite off
            // Don't write pixels we have already written.

            ZTest Less
            // Only render pixels less or equal to the value

            AlphaTest LEqual [_Cutoff]

            // Set up alpha blending

            Blend SrcAlpha OneMinusSrcAlpha

			SetTexture [_MainTex] 
	    	{
	    		//constantColor [_Color]
                combine texture, texture*previous //primary Previous
            }        
		}
	}

    Fallback "VertexLit", 1
}