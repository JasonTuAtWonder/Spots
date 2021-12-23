// A shader that draws a progress bar on the border of a quad.
//
// Used to provide feedback on the player's number of connected dots.
Shader "Unlit/Line Bar Progress"
{
    Properties
    {
		_BorderSize ("_BorderSize", Float) = .1
        _Health ("_Health", Range(0, 1)) = .2
        _Color ("_Color", Color) = (1, 0, 0, 1)
        _BackgroundColor ("_BackgroundColor", Color) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
		}

        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
			    float3 objectScale : TEXCOORD1;
                float2 uv : TEXCOORD2;
			    float2 scaledUv : TEXCOORD3;
            };

            float _BorderSize;
            float _Health;
            float4 _Color;
            float4 _BackgroundColor;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // Get object's scale: https://forum.unity.com/threads/can-i-get-the-scale-in-the-transform-of-the-object-i-attach-a-shader-to-if-so-how.418345/#post-6039230
				float3 scale = float3(
				    length(unity_ObjectToWorld._m00_m10_m20),
				    length(unity_ObjectToWorld._m01_m11_m21),
				    length(unity_ObjectToWorld._m02_m12_m22)
				);
                o.objectScale = scale;

                // Scale UV coordinates by object's scale.
                o.uv = v.uv;
                o.scaledUv = v.uv * scale.xy;

                return o;
            }

            float4 frag (Interpolators i) : SV_Target
            {
                float range = i.objectScale.x * .5 + i.objectScale.y * .5 - _BorderSize;
                float health = range * _Health;

                // Vertical mask.
                float borderTop = i.scaledUv.y > (i.objectScale.y - _BorderSize);
                float borderBottom = i.scaledUv.y < _BorderSize;
                float healthMaskX = (i.scaledUv.x - .5 * i.objectScale.x) < health;
                float moreMask = 0;
                moreMask = i.scaledUv.x > (.5 * i.objectScale.x - health);
                float maskY = (borderTop || borderBottom) && healthMaskX && moreMask;

                // Horizontal mask.
                float borderRightOrLeft = (
					i.scaledUv.x > (i.objectScale.x - _BorderSize) ||
                    i.scaledUv.x < _BorderSize
				);
                float noBorderTop = i.objectScale.y - i.scaledUv.y > _BorderSize;
                float noBorderBottom = i.scaledUv.y > _BorderSize;
                float healthInYSegment = health - (.5 * i.objectScale.x) + _BorderSize;
                float healthMaskY = (
					(i.objectScale.y - i.scaledUv.y) < healthInYSegment ||
                    i.scaledUv.y < healthInYSegment
				);
                float maskX = borderRightOrLeft && noBorderTop && noBorderBottom && healthMaskY;

                // Fill the border with the border color, but otherwise fill in the background color.
			    return _Color * (maskX || maskY) + _BackgroundColor * !(maskX || maskY);
            }
            ENDCG
        }
    }
}
