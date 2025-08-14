// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class AaaaaA : MonoBehaviour
// {
//     public Animator animator; // 指向 Animator
//     public Slider slider; // 指向 Slider UI

//     void Start()
//     {
//         if (slider != null)
//         {
//             slider.onValueChanged.AddListener(SetProgress);
//         }
//     }

//     // 設定動畫進度 (value 介於 0~1)
//     public void SetProgress(float value)
//     {
//         if (animator == null) return;
//         animator.SetFloat("Progress", value); // 直接設定 Progress 參數
//     }

//     void Update()
//     {
//         if (animator != null && slider != null)
//         {
//             animator.SetFloat("Progress", slider.value); // 每幀強制同步
//         }
//     }
// }
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AaaaaA : MonoBehaviour
{
    public Animator animator;  // Animator 元件
    public Slider slider;      // UI Slider 控制條

    void Start()
    {
        // 確保 Slider 存在，並且綁定 OnValueChanged 事件
        if (slider != null)
        {
            slider.onValueChanged.AddListener(SetProgress);
            Debug.Log("Slider 成功綁定事件！");
        }
        else
        {
            Debug.LogError("⚠️ 錯誤：Slider 尚未綁定！");
        }

        // 確保 Animator 存在
        if (animator == null)
        {
            Debug.LogError("⚠️ 錯誤：Animator 尚未綁定！");
        }
        else
        {
            // 設定 Animator Update Mode，確保動畫能夠即時更新
            animator.updateMode = AnimatorUpdateMode.Normal;
            animator.applyRootMotion = true;  // 確保動畫會被應用
        }
    }

    // 這個方法會在 Slider 拖動時執行，設定動畫進度
    public void SetProgress(float value)
    {
        if (animator == null) return;

        animator.SetFloat("Progress", value); // 設定 Animator 參數
        animator.Update(0); // 強制更新動畫狀態

        Debug.Log("🎬 Animator Progress 更新為: " + value);
    }

    void Update()
    {
        // 在遊戲內同步 Slider 的值與 Animator 參數，確保一致
        if (animator != null && slider != null)
        {
            float progress = animator.GetFloat("Progress");
            if (slider.value != progress)
            {
                slider.value = progress; // 確保 UI 同步
                Debug.Log("🔄 Slider 與 Animator 同步: " + progress);
            }
        }
    }
}
