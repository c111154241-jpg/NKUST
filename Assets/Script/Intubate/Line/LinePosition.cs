using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinePosition : MonoBehaviour
{
    [SerializeField] private GameObject carModel;
    [SerializeField] private NRM NRM;
    [SerializeField] private CatheterTipRemake CatheterTip;
    [SerializeField] private Vector3 recordedPosition;
    [SerializeField] private Quaternion recordedRotation;
    [SerializeField] private Vector3 carModelStartPosition; // 記錄 carModel 當時的位置
    [SerializeField] Tool tool;
    bool flag;

    void Start()
    {
        NRM = FindObjectOfType<NRM>(); // 自動尋找場景中的 NRM
        CatheterTip = FindObjectOfType<CatheterTipRemake>(); // 自動尋找 CatheterTipRemake
        carModel = GameObject.Find("drawer05_red001_0");
        RecordPosition();
    }
    void Update()
    {
        if (tool == null)
        {
            tool = FindAnyObjectByType<Tool>();
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (tool != null && !tool.resetLineConnectFlag)
            if (NRM != null && CatheterTip != null && flag)
            {
                if (other.gameObject.layer == LayerMask.NameToLayer("Drawer") && other.gameObject.name == "Drawer5" && !NRM.GrabBool && !CatheterTip.GrabBool)
                {
                    Vector3 carModelDelta = carModel.transform.position - carModelStartPosition; // 計算 carModel 的位移量
                    transform.rotation = recordedRotation;
                    transform.position = recordedPosition + carModelDelta;
                }
            }
    }

    void RecordPosition()
    {
        carModelStartPosition = carModel.transform.position; // 記錄 carModel 當前的位置
        recordedRotation = transform.rotation; // 記錄當前位置
        recordedPosition = transform.position; // 記錄當前位置
        flag = true;
    }
}
