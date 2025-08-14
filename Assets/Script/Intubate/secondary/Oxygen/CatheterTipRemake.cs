using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatheterTipRemake : MonoBehaviour
{
    [SerializeField] private Speak1 speak;

    [Header("導管配置")]
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private GameObject OxygenModel;
    [SerializeField] private GameObject CarModel;
    [SerializeField] private int fixedUpdateCounter = 0; // 計數器
    [SerializeField] private int count = 2; // 計數器
    public bool GrabBool;
    public bool Oxygenflag;
    [Header("線配置")]
    [SerializeField] private GameObject linePerfab;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private LineConnectWithFixedLength LineConnect;




    void Start()
    {
        // 初始化 Rigidbody 狀態
        Rigidbody.isKinematic = false;
    }

    void FixedUpdate()
    {
        // 確保 Rigidbody 狀態根據標誌切換
        Rigidbody.isKinematic = GrabBool || Oxygenflag;

        fixedUpdateCounter++;
        if (fixedUpdateCounter >= count)
        {
            HandleConstraintsAndPosition();
            fixedUpdateCounter = 0; // 重置計數器
        }
        if (speak.OxygenFlag)
        {
            transform.position = OxygenModel.transform.position;
            transform.rotation = OxygenModel.transform.rotation;
        }
    }

    private void HandleConstraintsAndPosition()
    {
        if (Oxygenflag)
        {
            ApplyConstraintAndAlign(OxygenModel);
        }
        else
        {
            Rigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    private void ApplyConstraintAndAlign(GameObject targetModel)
    {
        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        transform.position = targetModel.transform.position;
        transform.rotation = targetModel.transform.rotation;
    }

    void OnTriggerStay(Collider other)
    {
        if (!GrabBool && other.gameObject.layer == LayerMask.NameToLayer("OxygenTrigger"))
        {
            Oxygenflag = true;

        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Drawer") && other.gameObject.name == "Drawer5")
        {
            Rigidbody.isKinematic = true;
            transform.position = CarModel.transform.position;
            transform.rotation = CarModel.transform.rotation;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("OxygenTrigger"))
        {
            Oxygenflag = false;

        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Drawer") && other.gameObject.name == "Drawer5")
        {
            Rigidbody.isKinematic = false;
        }
    }

    public void Grab()
    {
        LineGrab();
        GrabBool = true;
    }

    public void UnGrab()
    {
        LineUnGrab();
        Oxygenflag = false;
        GrabBool = false;
    }
    private void LineGrab()
    {
        foreach (Transform child in linePerfab.transform)
        {
            // 嘗試獲取子物件上的 Rigidbody 組件
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.excludeLayers = layerMask;
            }
        }
    }
    private void LineUnGrab()
    {
        foreach (Transform child in linePerfab.transform)
        {
            // 嘗試獲取子物件上的 Rigidbody 組件
            Rigidbody rb = child.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.excludeLayers = LineConnect.layerMask;
            }
        }
    }
}

