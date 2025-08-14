using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatheterTip : MonoBehaviour
{
    [Header("導管配置")]
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private CatheterTip catheterTip;
    [SerializeField] private GameObject MRNModel;
    [SerializeField] private GameObject OxygenModel;
    [SerializeField] private int fixedUpdateCounter = 0; // 計數器
    [SerializeField] private int count = 2; // 計數器
    [SerializeField] private bool GrabBool;
    public bool MRNflag;
    public bool Oxygenflag;

    [Header("繩子配置")]
    public float segmentLength = 0.5f; // 繩子的固定長度
    private ConfigurableJoint joint;

    void Start()
    {
        // 初始化 Rigidbody 狀態
        Rigidbody.isKinematic = false;
    }

    void FixedUpdate()
    {
        // 確保 Rigidbody 狀態根據標誌切換
        Rigidbody.isKinematic = GrabBool || Oxygenflag || MRNflag;

        fixedUpdateCounter++;
        if (fixedUpdateCounter >= count)
        {
            HandleConstraintsAndPosition();
            fixedUpdateCounter = 0; // 重置計數器
        }
    }

    private void SetupRopeConnection(GameObject target)
    {
        // 確保只創建一個 Joint
        if (joint != null) return;

        Rigidbody targetRb = target.GetComponent<Rigidbody>();
        if (targetRb == null)
        {
            targetRb = target.AddComponent<Rigidbody>();
            targetRb.isKinematic = true;
        }

        joint = gameObject.AddComponent<ConfigurableJoint>();
        joint.connectedBody = targetRb;

        // 設置繩子的長度
        joint.xMotion = ConfigurableJointMotion.Limited;
        joint.yMotion = ConfigurableJointMotion.Limited;
        joint.zMotion = ConfigurableJointMotion.Limited;

        SoftJointLimit limit = new SoftJointLimit
        {
            limit = segmentLength,
            contactDistance = 0.05f
        };
        joint.linearLimit = limit;

        JointDrive drive = new JointDrive
        {
            positionSpring = 0f,
            positionDamper = 50f,
            maximumForce = Mathf.Infinity
        };
        joint.xDrive = drive;
        joint.yDrive = drive;
        joint.zDrive = drive;

        joint.angularXMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Locked;
        joint.angularZMotion = ConfigurableJointMotion.Locked;

        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;
    }
    private void DestroyRopeConnection()
    {
        if (joint != null)
        {
            Destroy(joint);
            joint = null;
        }
    }
    private void HandleConstraintsAndPosition()
    {
        if (Oxygenflag)
        {
            ApplyConstraintAndAlign(OxygenModel);
        }
        else if (MRNflag)
        {
            ApplyConstraintAndAlign(MRNModel);
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
        if (!GrabBool && other.gameObject.layer == LayerMask.NameToLayer("OxygenTrigger") && !catheterTip.Oxygenflag)
        {
            Oxygenflag = true;
            SetupRopeConnection(OxygenModel);
        }
        else if (!GrabBool && other.gameObject.layer == LayerMask.NameToLayer("NRMTrigger") && !catheterTip.MRNflag)
        {
            MRNflag = true;
            SetupRopeConnection(MRNModel);
        }
    }

    public void Grab()
    {
        GrabBool = true;
    }

    public void UnGrab()
    {
        Oxygenflag = false;
        MRNflag = false;
        GrabBool = false;
        DestroyRopeConnection();
    }
}
