Shader "Custom/TransparentUI"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}  // 主紋理貼圖
        _Color ("Tint Color", Color) = (1,1,1,0)  // 顏色設置，默認透明
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Overlay"  // 確保 UI 層渲染
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
        }

        Pass
        {
            ZTest Always  // 深度測試設為 Always，讓 UI 顯示在前面
            ZWrite Off    // 關閉深度寫入
            Cull Off      // 雙面渲染
            Blend SrcAlpha OneMinusSrcAlpha  // 設置透明混合模式

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color * _Color;  // 乘以顏色，保留透明度
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.texcoord) * i.color;  // 使用透明貼圖和顏色
                return col;
            }
            ENDCG
        }
    }

    FallBack "Transparent"
}
