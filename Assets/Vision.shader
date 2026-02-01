Shader "UI/VisionHole"
{
    Properties
    {
        _Color ("Overlay Color", Color) = (0,0,0,0.85)
        _HoleCenter ("Hole Center (UV)", Vector) = (0.5, 0.5, 0, 0)
        _HoleRadius ("Hole Radius", Float) = 0.2
        _Softness ("Edge Softness", Float) = 0.02
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _Color;
            float4 _HoleCenter;
            float _HoleRadius;
            float _Softness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Distance from hole center
                float dist = distance(i.uv, _HoleCenter.xy);

                // Smooth circular mask
                float mask = smoothstep(
                    _HoleRadius,
                    _HoleRadius - _Softness,
                    dist
                );

                fixed4 col = _Color;
                col.a *= mask;

                return col;
            }
            ENDCG
        }
    }
}
