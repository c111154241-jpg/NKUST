using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 必須加入這一行

public class flash : MonoBehaviour
{
    public float blinkSpeed = 1f; // 閃爍速度
    private Image arrowImage;     // 箭頭的 Image 組件

    void Start()
    {
        arrowImage = GetComponent<Image>(); // 獲取箭頭的 Image 組件
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        for (int i = 0; i < 1000; i++)
        {
            // 逐漸減少透明度
            for (float alpha = 1f; alpha >= 0f; alpha -= Time.deltaTime * blinkSpeed)
            {
                SetAlpha(alpha);
                yield return null;
            }
            // 逐漸增加透明度
            for (float alpha = 0f; alpha <= 1f; alpha += Time.deltaTime * blinkSpeed)
            {
                SetAlpha(alpha);
                yield return null;
            }
        }
    }

    void SetAlpha(float alpha)
    {
        Color color = arrowImage.color;
        color.a = alpha; // 設置透明度
        arrowImage.color = color;
    }
}
