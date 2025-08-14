using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NRMAnimator : MonoBehaviour
{
    [SerializeField] private CatheterTipRemake CatheterTip;
    [SerializeField] private SteelBall steelBall;
    [SerializeField] private Animator Animator;
    public bool flag; // 用于检查是否正在播放动画

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("NRMCollider") && CatheterTip.Oxygenflag && steelBall.flag)
        {
            flag = true;
            Animator.SetBool("begin", true);
        }
    }
}
