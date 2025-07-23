Shader "Custom/URP_TextureMirrorReflect"
{
    Properties
    {
        _BaseMap ("Texture", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _Tiling ("Tiling", Vector) = (1,1,0,0)
        _Offset ("Offset", Vector) = (0,0,0,0)
        _Rotation ("Rotation (Degrees)", Float) = 0
        _MirrorTextureX ("Mirror Texture X", Float) = 0
        _MirrorTextureY ("Mirror Texture Y", Float) = 0
        _AlphaClipThreshold ("Alpha Clip Threshold", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="TransparentCutout" "Queue"="AlphaTest" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _BaseMap;
            float4 _Color;
            float4 _Tiling;
            float4 _Offset;
            float _Rotation;
            float _MirrorTextureX;
            float _MirrorTextureY;
            float _AlphaClipThreshold;

            float2 MirrorUV(float2 uv, float mirrorX, float mirrorY)
            {
                if (mirrorX > 0.5)
                {
                    uv.x = fmod(uv.x, 1.0);
                    uv.x = abs(uv.x * 2.0 - 1.0);
                }
                if (mirrorY > 0.5)
                {
                    uv.y = fmod(uv.y, 1.0);
                    uv.y = abs(uv.y * 2.0 - 1.0);
                }
                return uv;
            }

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS);

                float2 uv = IN.uv;

                // Center
                uv -= 0.5;

                // Rotation
                float rad = radians(_Rotation);
                float2x2 rotMatrix = float2x2(cos(rad), -sin(rad), sin(rad), cos(rad));
                uv = mul(rotMatrix, uv);

                // Tiling + Offset
                uv *= _Tiling.xy;
                uv += _Offset.xy;

                // Return to 0–1 space
                uv += 0.5;

                // Apply true MIRROR REFLECTION of the texture
                uv = MirrorUV(uv, _MirrorTextureX, _MirrorTextureY);

                OUT.uv = uv;
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                float4 tex = tex2D(_BaseMap, IN.uv);
                clip(tex.a - _AlphaClipThreshold);
                return tex * _Color;
            }
            ENDHLSL
        }
    }

    FallBack "Universal Render Pipeline/Lit"
}
