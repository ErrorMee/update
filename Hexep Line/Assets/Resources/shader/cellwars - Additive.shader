// Simplified Additive Particle shader. Differences from regular Additive Particle one:
// - no Tint color
// - no Smooth particle support
// - no AlphaTest
// - no ColorMask

Shader "cellwars/Additive" 
{
    Properties 
    {
	    _MainTex ("Particle Texture", 2D) = "white" {}
	    _Multiple ("multiple",Color) = (1,1,1,1)
    }

	Category 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
		Cull Off Lighting Off ZWrite Off Fog { Color (0,0,0,0) }

		BindChannels {
			Bind "Color", color
			Bind "Vertex", vertex
			Bind "TexCoord", texcoord
		}
		
		SubShader {
			Pass {

				SetTexture [_MainTex] 
				{
					combine texture * primary
				}
				SetTexture [_MainTex]
				{
					ConstantColor [_Multiple]
					combine previous * constant 
				}
			}
		}
	}
}

