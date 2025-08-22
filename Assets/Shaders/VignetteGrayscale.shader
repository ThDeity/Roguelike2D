Shader "Custom/VignetteGrayscale"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VignetteIntensity ("Vignette Intensity", Range(0, 1)) = 0.5
        _VignetteSoftness ("Vignette Softness", Range(0, 1)) = 0.5
        _GrayscaleIntensity ("Grayscale Intensity", Range(0, 1)) = 0.5
        _HealthThreshold ("Health Threshold", Range(0, 1)) = 0.3
        _CurrentHealth ("Current Health", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
            float4 _MainTex_ST;
            float _VignetteIntensity;
            float _VignetteSoftness;
            float _GrayscaleIntensity;
            float _HealthThreshold;
            float _CurrentHealth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Получаем исходный цвет
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Рассчитываем виньетирование (затемнение по краям)
                float2 uvCenter = i.uv - 0.5;
                float vignette = 1.0 - dot(uvCenter, uvCenter) * _VignetteIntensity;
                vignette = smoothstep(0.0, _VignetteSoftness, vignette);
                
                // Рассчитываем уровень эффекта в зависимости от здоровья
                float healthFactor = saturate((_HealthThreshold - _CurrentHealth) / _HealthThreshold);
                float effectStrength = healthFactor * (1.0 - vignette);
                
                // Преобразуем в оттенки серого
                float luminance = dot(col.rgb, float3(0.299, 0.587, 0.114));
                float3 grayscale = lerp(col.rgb, luminance.xxx, _GrayscaleIntensity * effectStrength);
                
                // Применяем виньетирование
                col.rgb = grayscale * vignette;
                
                return col;
            }
            ENDCG
        }
    }
}