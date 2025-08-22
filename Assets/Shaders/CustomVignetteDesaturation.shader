Shader "Hidden/CustomVignetteDesaturation"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VignetteIntensity ("Vignette Intensity", Range(0, 1)) = 0.5
        _Desaturation ("Desaturation", Range(0, 1)) = 0.5
        _HealthThreshold ("Health Threshold", Range(0, 1)) = 0.3
    }
    SubShader
    {
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

            sampler2D _MainTex;
            float _VignetteIntensity;
            float _Desaturation;
            float _HealthThreshold;
            float _CurrentHealth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Виньетирование
                float2 uvCenter = i.uv - 0.5;
                float vignette = 1.0 - dot(uvCenter, uvCenter) * _VignetteIntensity;
                
                // Обесцвечивание
                float luminance = 0.3 * col.r + 0.59 * col.g + 0.11 * col.b;
                col.rgb = lerp(col.rgb, luminance.xxx, _Desaturation);
                
                // Применяем эффект только если здоровье ниже порога
                float effectFactor = step(_CurrentHealth, _HealthThreshold);
                col.rgb *= lerp(1.0, vignette, effectFactor);
                
                return col;
            }
            ENDCG
        }
    }
}
