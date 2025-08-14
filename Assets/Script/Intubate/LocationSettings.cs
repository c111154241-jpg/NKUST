using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocationSettings : MonoBehaviour
{
    [SerializeField] private string Model;
    [SerializeField] private GameObject CarModel;
    [SerializeField] private Vector3 Distance;
    [SerializeField] private Vector3 RotationOffset;
    [SerializeField] private bool carFlag;
    [SerializeField] private bool elevatorFlag;
    [SerializeField] private bool transformFlag;
    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (carFlag && sceneName == "Intubate")
        {
            transform.position = CarModel.transform.position + Distance;
            transform.rotation = Quaternion.Euler(RotationOffset);
        }
        else if (elevatorFlag && sceneName == "Elevator")
        {
            transform.position = Distance;
            transform.rotation = Quaternion.Euler(RotationOffset);
        }
        else if (transformFlag && sceneName == "Elevator")
        {
            // 嘗試尋找 Model 物件
            GameObject foundObject = GameObject.Find(Model);
            if (foundObject != null)
            {
                Transform ModelTransform = foundObject.transform;

                // 設置當前物件的位置、旋轉
                transform.position = ModelTransform.position;
                transform.rotation = ModelTransform.rotation;

                // 計算世界縮放，確保與 Model 大小一致
                Vector3 modelWorldScale = ModelTransform.lossyScale;
                Vector3 parentWorldScale = transform.parent ? transform.parent.lossyScale : Vector3.one;
                transform.localScale = new Vector3(
                    modelWorldScale.x / parentWorldScale.x,
                    modelWorldScale.y / parentWorldScale.y,
                    modelWorldScale.z / parentWorldScale.z
                );
            }
        }
    }
}
