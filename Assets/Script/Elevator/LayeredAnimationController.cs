using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LayeredAnimationController : MonoBehaviour
{
    public Animator animator;

    void Start()
    {
        // 立即播放 Base Layer（第一個動畫）
        animator.Play("按鈕", 0);

        // 延遲 20 秒後啟動 Layer2（第二個動畫）
        Invoke("ActivateSecondLayer", 2f);
    }

    void ActivateSecondLayer()
    {
        // 設定 Layer2 的權重為 1（開啟）
        animator.SetLayerWeight(1, 1);
        animator.Play("N1", 1); // 播放 Layer2 的動畫
    }
}
