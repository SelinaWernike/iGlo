Shader "Custom/OutlineShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _OutlineColor("Outline color", Color) = (1, 0, 0, 0.5)
        _OutlineWidth("Outlines width", Range(0.0, 2.0)) = 0.15
    }

    CGINCLUDE
    #include "UnityCG.cginc"

    struct appdata {
        float4 vertex : POSITION;
        float4 normal : NORMAL;
    };

    uniform float4 _OutlineColor;
    uniform float _OutlineWidth;
    uniform sampler2D _MainTex;

    ENDCG

    SubShader {
        Pass {
            Tags { "LightMode" = "Always" }
            ZWrite Off
            CGPROGRAM

            struct v2f {
                float4 pos : SV_POSITION;
            };

            #pragma vertex vert
            #pragma fragment frag

            v2f vert(appdata v) {
                v.vertex.xyz += normalize(v.normal.xyz) * _OutlineWidth;
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : COLOR {
                return _OutlineColor;
            }

            ENDCG
        }

        Tags { "Queue" = "Transparent" }

        CGPROGRAM
        #pragma surface surf Lambert noshadow

        struct Input {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput  o) {
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    Fallback "Diffuse"
}
