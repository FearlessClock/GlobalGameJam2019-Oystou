Shader "Custom/HomeSick"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_fadeScale ("Fade", float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
			/**
			 * @author jonobr1 / http://jonobr1.com/
			 */

			 /**
			  * Convert r, g, b to normalized vec3
			  */
			float3 rgb(float r, float g, float b) {
				return float3(r / 255.0, g / 255.0, b / 255.0);
			}

			/**
			 * Draw a circle at vec2 `pos` with radius `rad` and
			 * color `color`.
			 */
			float4 circle(float2 uv, float2 pos, float rad, float3 color) {
				float d = length(uv - float2(0.5, 0.5)) - rad;
				float t = clamp(d, 0.0, 1.0);
				return float4(1-t, 1 - t, 1 - t, t);
			}
            sampler2D _MainTex;
			float _fadeScale;

            fixed4 frag (v2f i) : SV_Target
            {
				float2 uv = i.uv;
				float2 pos = i.uv;
				float4 circleRGB = circle(uv, uv, _fadeScale, float3(1.0, 1.0, 1.0));
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                col.rgba = circleRGB;
                return col;
            }
            ENDCG
        }
    }
}
