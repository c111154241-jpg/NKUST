using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntubationAnimation : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private Animator animator;
    [SerializeField] private string animationStateName = "插管";
    public bool flag = false;
    public bool loneFlag = false;
    public bool shortFlag = false;
    public bool reverse = false; // 是否倒放
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    void Update()
    {
        float progress = CalculateProgress();
        SetProgress(progress);

        if (reverse)
        {
            // 倒放時，flag 在 progress == 1 時觸發
            if (progress <= 0.01f)
            {
                flag = true;
            }
        }
        else
        {
            // 正向播放時的 flag 設定
            if (progress > 0.89f)
            {
                flag = false;
                loneFlag = true;
                shortFlag = false;
            }
            else if (progress <= 0.895f && progress >= 0.825f)
            {
                flag = true;
                loneFlag = false;
                shortFlag = false;
            }
            else if (progress < 0.825f)
            {
                flag = false;
                loneFlag = false;
                shortFlag = true;
            }
        }
    }

    public void SetProgress(float value)
    {
        if (animator == null || string.IsNullOrEmpty(animationStateName)) return;

        animator.Play(animationStateName, 0, value);
        animator.speed = 0; // 停留在當前幀
    }

    private float CalculateProgress()
    {
        if (startPoint == null || endPoint == null || cube == null)
        {
            Debug.LogWarning("StartPoint, EndPoint, or Cube is not assigned!");
            return 0f;
        }

        float totalDistance = Vector3.Distance(startPoint.position, endPoint.position);
        float currentDistance = Vector3.Distance(startPoint.position, cube.transform.position);

        // 依據 reverse 來決定動畫播放方向
        return Mathf.Clamp01(currentDistance / totalDistance);
    }

    public void ToggleReverse()
    {
        reverse = !reverse; // 切換播放方向
    }
}
