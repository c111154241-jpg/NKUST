using System.Collections;
using UnityEngine;

public class BVMAnimator : MonoBehaviour
{
    [SerializeField] private Animator Animator;
    [SerializeField] private string animationName = "hands_handsAction_002"; // 替換為你動畫的名稱
    [SerializeField] private int animationLayer = 0;
    [SerializeField] private float animationThreshold = 0.95f;

    private void Start()
    {
        StartCoroutine(AnimationLoop());
    }

    private IEnumerator AnimationLoop()
    {
        while (true)
        {
            Animator.SetTrigger("On");
            // 等待動畫播放完畢
            yield return new WaitUntil(() => IsAnimationFinished(animationName, animationLayer));
            // 播完兩次後等待 6 秒
            yield return new WaitForSeconds(6f);
        }
    }

    private bool IsAnimationFinished(string animName, int layer)
    {
        AnimatorStateInfo stateInfo = Animator.GetCurrentAnimatorStateInfo(layer);
        return stateInfo.IsName(animName) && stateInfo.normalizedTime >= animationThreshold;
    }
}
