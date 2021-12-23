Shader "Unlit/Circle That Gets More Transparent Toward Center"
{
    Properties
    {
        _DesiredColor ("Desired Color", Color) = (1, 1, 1, 1)
        _Transparency ("Transparency", Float) = 1
    }
    SubShader
    {
        Tags
		{
            // Metadata that tells Unity to treat this object as a transparent object,
            // thus affecting the render order - among other things.
			"RenderType" = "Transparent"
			"Queue" = "Transparent"
		}
        Pass
        {
            ZWrite Off // Don't want transparent shaders to write to depth buffer (which will occlude objects behind it).
            Blend SrcAlpha OneMinusSrcAlpha  // Enable alpha blending.

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct MeshData
            {
                float4 vertex : POSITION;
                float4 objectSpaceVerts : POSITION;
            };

            struct Interpolators
            {
                float4 vertex : SV_POSITION;
                float4 objectSpaceVerts : TEXCOORD0;
            };

            float4 _DesiredColor;
			float _Transparency;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.objectSpaceVerts = v.vertex;
                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                float len = length(float2(i.objectSpaceVerts.x, i.objectSpaceVerts.y));
                return float4(_DesiredColor.xyz, len * _Transparency);
            }
            ENDCG
        }
    }
}
