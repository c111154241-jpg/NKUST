using System.Collections;
using UnityEngine;

public class Hunman : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] Speak1 speak1;
    public bool flag;
    [SerializeField] int animatorCount;

    void Start()
    {
        speak1 = FindObjectOfType<Speak1>();
        animator.CrossFade("breath", 0.5f); // 初始播放 breath 動畫，過渡時間為 0.5 秒
        StartCoroutine(AnimateSequence());
    }

    void Update()
    {

    }
    private IEnumerator AnimateSequence()
    {
        yield return new WaitUntil(() => speak1.count == 1);
        animatorCount++;
        flag = true;
        StartCoroutine(TriggerWithDelay("head1_000", 0.5f));
        StartCoroutine(Delay(2));
        flag = false;
        //
        yield return new WaitUntil(() => speak1.count == 3);
        flag = true;
        StartCoroutine(TriggerWithDelay("head1_000", 0.5f));
        StartCoroutine(Delay(2));
        flag = false;
        //
        yield return new WaitUntil(() => speak1.count == 4);
        flag = true;
        StartCoroutine(TriggerWithDelay("breath 0", 0.5f));
        StartCoroutine(Delay(2));
        flag = false;
        //
        yield return new WaitUntil(() => speak1.count == 5);
        flag = true;
        StartCoroutine(TriggerWithDelay("situpbothhand", 0.5f));
        StartCoroutine(Delay(2));
        flag = false;
        //
        yield return new WaitUntil(() => speak1.count == 6);
        flag = true;
        StartCoroutine(TriggerWithDelay("breath", 0.5f));
        StartCoroutine(Delay(2));
        flag = false;
        //
        yield return new WaitUntil(() => speak1.count == 8);
        flag = true;
        StartCoroutine(TriggerWithDelay("head1_000", 0.5f));
        StartCoroutine(Delay(2));
        flag = false;
    }
    private IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);

    }
    private IEnumerator TriggerWithDelay(string stateName, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.CrossFade(stateName, 0.5f); // 使用 CrossFade 切換動畫，過渡 0.5 秒
    }
}
