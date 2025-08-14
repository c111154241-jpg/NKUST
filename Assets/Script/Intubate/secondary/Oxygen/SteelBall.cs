using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelBall : MonoBehaviour
{
    [SerializeField] float minDistance = 0.0155f;
    [SerializeField] float maxDistance = 0.0275f;
    [SerializeField] GameObject handle;
    public bool flag;
    // Start is called before the first frame update
    void Start()
    {
        //z=0.0175 0.0275    
    }

    // Update is called once per frame
    void Update()
    {
        // 獲取物體的 Transform 組件
        Transform objectTransform = handle.transform;

        // 獲取物體的歐拉角（角度形式）
        Vector3 eulerAngles = objectTransform.eulerAngles;
        float zAngle = eulerAngles.z; // 获取Z角度

        // 计算等比缩放比例
        float t = zAngle / 360f;
        float zDistance = Mathf.Lerp(minDistance, maxDistance, t);

        Vector3 localPosition = transform.localPosition;
        localPosition.z = zDistance;
        transform.localPosition = localPosition;

        if (localPosition.z > 0.0255f)
            flag = true;
        else
            flag = false;
    }
}
