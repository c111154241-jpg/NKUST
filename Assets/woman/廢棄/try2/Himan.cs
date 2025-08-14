using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Himan : MonoBehaviour
{
    [SerializeField] private GameObject targetCube;
    [SerializeField] private Animator animator;
    private int clickCount = 0;

    void Start()
    {
        animator.CrossFade("breath", 0.5f); // 初始播放 breath 動畫
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == targetCube)
                {
                    clickCount++;

                    switch (clickCount)
                    {
                        case 1:
                            StartCoroutine(TriggerWithDelay("PlayHead", 0.5f)); // 切換到 head1_001，過渡 0.5 秒
                            break;
                        case 2:
                            StartCoroutine(TriggerWithDelay("PlaySingle", 0.5f)); // 切換到 handmove，過渡 0.5 秒（只播放一次）
                            break;
                        case 3:
                            StartCoroutine(TriggerWithDelay("PlayHand", 0.5f)); // 切換到 hand，過渡 0.5 秒
                            break;
                        case 4:
                            StartCoroutine(TriggerWithDelay("PlayFinal", 0.5f)); // 切換到 finalpose，過渡 0.5 秒
                            break;
                        default:
                            Debug.Log("所有動畫已播放完成！");
                            break;
                    }
                }
            }
        }
    }

    IEnumerator TriggerWithDelay(string triggerName, float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.SetTrigger(triggerName);
    }
}
