using System.Collections;
using UnityEngine;

public class HunmanA : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] Speak1 speak1;
    public bool flag;
    [SerializeField] int animatorCount;

    void Start()
    {
        speak1 = FindObjectOfType<Speak1>();
        animator.CrossFade("z|zAction", 0.5f);
        StartCoroutine(AnimateSequence());
    }

    void Update()
    {

    }
    private IEnumerator AnimateSequence()
    {
        yield return new WaitUntil(() => speak1.count == 4);
        flag = true;
        StartCoroutine(TriggerWithDelay("躺下時", 0.5f));
        yield return new WaitForSeconds(1);
        flag = false;

        yield return new WaitUntil(() => speak1.count == 5);
        flag = true;
        StartCoroutine(TriggerWithDelay("胸悶", 0.5f));
        yield return new WaitForSeconds(1);
        flag = false;

        yield return new WaitUntil(() => speak1.count == 6);
        flag = true;
        StartCoroutine(TriggerWithDelay("z|zAction", 0.5f));
        yield return new WaitForSeconds(1);
        flag = false;
    }
    private IEnumerator TriggerWithDelay(string stateName, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.CrossFade(stateName, 0.5f); // 使用 CrossFade 切換動畫，過渡 0.5 秒
    }
}
