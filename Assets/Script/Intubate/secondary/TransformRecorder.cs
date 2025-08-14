using System.Collections.Generic;
using UnityEngine;

public class TransformRecorder : MonoBehaviour
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    void Start()
    {
        RecordTransform(); // 在 Start 中记录初始 Transform
    }
    void Update()
    {
        if (transform.position.y < -2)
        {
            RestoreTransform();
        }
    }
    // 记录当前 Transform
    public void RecordTransform()
    {
        position = transform.position;
        rotation = transform.rotation;
        scale = transform.localScale;
    }

    // 恢复到上一个 Transform
    public void RestoreTransform()
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = scale;
    }
}