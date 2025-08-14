using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpaAnimator : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    [SerializeField] private bool isAnimating = false; // 用于检查是否正在播放动画
    [SerializeField] private float animationThreshold = 0.95f;
    void Start()
    {
        Animator.SetLayerWeight(0, 1.0f);
        Animator.SetLayerWeight(1, 1.0f);
    }

    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        // Debug.Log("持续停留在触发器内: " + other.gameObject.layer);

        // 确保只在"Hand"层对象上触发动画
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand"))
        {
            // 检查动画是否已经完成
            if (!isAnimating)
            {
                isAnimating = true; // 标记动画正在播放
                Animator.SetTrigger("high_poly|high_polyAction");
                Animator.SetTrigger("Cube|CubeAction");
            }

            // 检查当前动画是否已经播放完毕
            if (IsAnimationFinished("high_poly|high_polyAction", 0) && IsAnimationFinished("Cube|CubeAction", 1))
            {
                isAnimating = false; // 动画播放完毕，重置标志
                Debug.LogError("动画播放完毕");
            }
        }
    }

    private bool IsAnimationFinished(string animName, int layer)
    {
        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(0);
        return !stateInfo.IsName(animName) && stateInfo.normalizedTime >= animationThreshold;
    }
}
