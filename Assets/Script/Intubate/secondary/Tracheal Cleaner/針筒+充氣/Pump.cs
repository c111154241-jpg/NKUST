using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pump : MonoBehaviour
{
    [SerializeField] float Distance = 0.029f;
    [SerializeField] float ResetThreshold = 0.001f; // 拉回來的位置門檻
    [SerializeField] Syringe Syringe;

    public bool flag;           // 代表是否目前可進行注射行為
    public bool hasTriggered;  // 是否已經觸發過（防止重複觸發）

    void Update()
    {
        Vector3 localPosition = transform.localPosition;

        // 觸發條件：推到底 & Syringe 準備好 & 尚未觸發過
        if (!hasTriggered && localPosition.z > Distance && Syringe.flag)
        {
            flag = true;
            hasTriggered = true;
        }
        // 重置條件：已經觸發過，並且拉回到初始位置附近
        else if (hasTriggered && localPosition.z < ResetThreshold && Syringe.flag)
        {
            hasTriggered = false;
            flag = false;
        }
    }
}
