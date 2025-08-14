using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflatableAnimation : MonoBehaviour
{
    [SerializeField] private Pump Pump;
    [SerializeField] private Animator animator;
    [SerializeField] private string animationStateName = "鐵充氣_鐵充氣Action";
    [SerializeField] private InflableHand InflableHand;

    [SerializeField] private float animationSpeed = 1.0f;

    private float currentTime = 0f; // 0~1 normalized time
    public bool isPlayingForward = false;
    public bool isPlayingBackward = false;

    public bool handFlag = false;
    public bool flag = false;

    void Start()
    {
        animator.Play(animationStateName, 0, 0);
        animator.speed = 0; // 由我們自己控制播放時間
    }

    void Update()
    {
        // 判斷播放方向
        if (Pump != null && Pump.flag)
        {
            isPlayingForward = true;
            isPlayingBackward = false;
        }
        else if (Pump != null && !Pump.flag && currentTime > 0 && InflableHand != null && InflableHand.flag)
        {
            isPlayingForward = false;
            isPlayingBackward = true;
        }
        else
        {
            isPlayingForward = false;
            isPlayingBackward = false;
        }

        // 手動更新播放時間
        if (isPlayingForward)
        {
            currentTime += Time.deltaTime * animationSpeed;
            if (currentTime >= 1f)
            {
                currentTime = 1f;
                isPlayingForward = false;
                handFlag = true;
            }
        }
        else if (isPlayingBackward)
        {
            currentTime -= Time.deltaTime * animationSpeed;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isPlayingBackward = false;
                handFlag = false;
                flag = true;
            }
        }

        // 播放動畫當前進度
        animator.Play(animationStateName, 0, currentTime);
        animator.speed = 0; // 始終由我們控制
    }
}
