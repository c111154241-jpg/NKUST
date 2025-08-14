using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bracelet : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject UI;
    [SerializeField] private bool first = true;
    [SerializeField] private Speak1 speak1;
    public bool flag;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerStay(Collider other)
    {
        // 确保只在"Hand"层对象上触发动画
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand") && first && speak1.count == 4)
        {
            first = false;
            StartCoroutine(WaitForAnimation("舉手A"));
        }
    }
    IEnumerator WaitForAnimation(string animationName)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        // 確保當前播放的是指定的動畫
        while (!(stateInfo.IsName(animationName) && stateInfo.normalizedTime >= 1.0f && !animator.IsInTransition(0)))
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }
        UI.SetActive(true);
    }

    public void Exit()
    {
        UI.SetActive(false);
        flag = true;
    }
}

