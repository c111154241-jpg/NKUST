using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [Header("車")]
    [SerializeField] private GameObject Car;
    [SerializeField] private Vector3 distanceCarTrachealCleanerGameobject;
    [SerializeField] private Vector3 distanceCarLaryngoscopeGameobject;
    [Header("抽屜位置")]//上往下
    [SerializeField] private GameObject Drawer1;
    private Vector3 Drawer1Positions = new Vector3();
    [SerializeField] private GameObject Drawer2;
    private Vector3 Drawer2Positions = new Vector3();
    [SerializeField] private GameObject Drawer3;
    private Vector3 Drawer3Positions = new Vector3();
    [SerializeField] private GameObject Drawer4;
    private Vector3 Drawer4Positions = new Vector3();
    [SerializeField] private GameObject Drawer5;
    private Vector3 Drawer5Positions = new Vector3();

    [Header("距離")]
    [SerializeField] private Vector3 distanceOxygen;
    [SerializeField] private Vector3 distanceNRM;
    [SerializeField] private Vector3 distanceBVM;
    [SerializeField] private Vector3 distanceSyringe;
    [SerializeField] private List<Vector3> distanceTrachealCleaner;
    [SerializeField] private List<Vector3> distanceCatheter;
    [SerializeField] private List<Vector3> distanceLaryngoscope;
    [SerializeField] private List<Vector3> distanceOxygenCylinder;

    [Header("EKG")]
    [SerializeField] private List<GameObject> EKG; // 用於存放EKG物件的列表
    [SerializeField] private List<Vector3> EKGPositions = new List<Vector3>(); // 用於紀錄EKG物件的位置
    [SerializeField] private List<Vector3> EKGEulerAngles = new List<Vector3>();
    [SerializeField] private List<GameObject> linePerfab;
    [SerializeField] private List<Transform> linePerfabKeys = new List<Transform>(); // 對應字典的鍵
    [SerializeField] private List<Vector3> linePerfabValues = new List<Vector3>();   // 對應字典的值
    [SerializeField] private Dictionary<Transform, Vector3> linePerfabPositions = new Dictionary<Transform, Vector3>(); // 紀錄每個子物件的初始位置

    [Header("Oxygen")]
    [SerializeField] private GameObject oxygen;
    [SerializeField] private Vector3 oxygenPositions = new Vector3();
    [SerializeField] private Vector3 oxygenEulerAngles = new Vector3();

    [Header("NRM")]
    [SerializeField] private GameObject NRM;
    [SerializeField] private Vector3 NRMPositions = new Vector3();
    [SerializeField] private Vector3 NRMEulerAngles = new Vector3();

    [Header("氧氣導管(Catheter)")]
    [SerializeField] private List<GameObject> catheter;
    [SerializeField] private List<Vector3> catheterPositions = new List<Vector3>();
    [SerializeField] private List<Vector3> catheterEulerAngles = new List<Vector3>();
    [SerializeField] private List<GameObject> catheterline;
    [SerializeField] private List<Transform> catheterlineKeys = new List<Transform>(); // 對應字典的鍵
    [SerializeField] private List<Vector3> catheterlineValues = new List<Vector3>();   // 對應字典的值
    [SerializeField] private Dictionary<Transform, Vector3> catheterlinePositions = new Dictionary<Transform, Vector3>(); // 紀錄每個子物件的初始位置
    [SerializeField] private LineConnectWithFixedLength LineConnect;
    public bool resetLineConnectFlag;

    [Header("BVM")]
    [SerializeField] private GameObject BVM;
    [SerializeField] private Vector3 BVMPositions = new Vector3();
    [SerializeField] private Vector3 BVMEulerAngles = new Vector3();

    [Header("插管內管及通條(TrachealCleaner)")]
    [SerializeField] private List<GameObject> TrachealCleaner;
    [SerializeField] private List<Vector3> TrachealCleanerPositions = new List<Vector3>();
    [SerializeField] private List<Vector3> TrachealCleanerEulerAngles;
    [SerializeField] private List<GameObject> carTrachealCleanerGameobject;
    // [SerializeField] private GameObject TrachealCleanerLocal;
    // [SerializeField] private Vector3 TrachealCleanerLocalPositions;
    // [SerializeField] private Vector3 TrachealCleanerLocalEulerAngles;


    [Header("針筒(Syringe)")]
    [SerializeField] private GameObject Syringe;
    [SerializeField] private Vector3 SyringePositions = new Vector3();
    [SerializeField] private Vector3 SyringeEulerAngles = new Vector3();

    [Header("喉頭鏡(laryngoscope)")]
    [SerializeField] private List<GameObject> Laryngoscope;
    [SerializeField] private List<Vector3> LaryngoscopePositions = new List<Vector3>();
    [SerializeField] private List<Vector3> LaryngoscopeEulerAngles;
    [SerializeField] private List<GameObject> carLaryngoscopeGameobject;
    // [SerializeField] private GameObject LaryngoscopeLocal;
    // [SerializeField] private Vector3 LaryngoscopeLocalPositions;
    // [SerializeField] private Vector3 LaryngoscopeLocalEulerAngles;

    [Header("氧氣瓶(oxygen cylinder)")]
    [SerializeField] private List<GameObject> oxygenCylinder;
    [SerializeField] private List<Vector3> oxygenCylinderPositions = new List<Vector3>();
    [SerializeField] private List<Vector3> oxygenCylinderEulerAngles;

    void Start()
    {
        StartCoroutine(RecordDrawerPositions());
        StartCoroutine(RecordEKGPositions());
        StartCoroutine(RecordlinePerfabPositions());
        StartCoroutine(RecordoxygenPositions());
        StartCoroutine(RecordNRMPositions());
        StartCoroutine(RecordcatheterPositions());
        StartCoroutine(RecordcatheterlinePositions());
        StartCoroutine(RecordBVMPositions());
        StartCoroutine(RecordTrachealCleanerPositions());
        StartCoroutine(RecordSyringePositions());
        StartCoroutine(RecordLaryngoscopePositions());
        StartCoroutine(RecordOxygenCylinderPositions());
    }

    /// <summary>
    /// 紀錄所有物件的初始位置
    /// </summary>
    public IEnumerator RecordDrawerPositions()
    {
        Drawer1Positions = Drawer1.transform.position;
        Drawer2Positions = Drawer2.transform.position;
        Drawer3Positions = Drawer3.transform.position;
        Drawer4Positions = Drawer4.transform.position;
        Drawer5Positions = Drawer5.transform.position;
        yield return null;
    }
    public IEnumerator RecordEKGPositions()
    {
        // 紀錄每個EKG物件的初始位置
        foreach (GameObject segment in EKG)
        {
            if (segment != null) // 確保物件不為空
            {
                EKGEulerAngles.Add(segment.transform.eulerAngles);
                EKGPositions.Add(segment.transform.position);
            }
        }
        yield return null;
    }
    public IEnumerator RecordlinePerfabPositions()
    {
        foreach (GameObject segments in linePerfab)
        {
            if (segments != null) // 確保segments不為空
            {
                // 等待直到有子物件
                while (segments.transform.childCount == 0)
                {
                    // 沒有子物件的話等待一幀，繼續檢查
                    yield return null;
                }

                // 當有子物件時，處理它們
                for (int i = 0; i < segments.transform.childCount; i++)
                {
                    Transform child = segments.transform.GetChild(i);
                    if (child != null) // 確保子物件不為空
                    {
                        // 儲存到字典
                        linePerfabPositions[child] = child.position;

                        // 同時更新序列化用的列表
                        linePerfabKeys.Add(child);
                        linePerfabValues.Add(child.position);
                    }
                }
            }
        }
    }
    public IEnumerator RecordoxygenPositions()
    {
        distanceOxygen = Drawer5Positions - oxygen.transform.position;
        oxygenPositions = oxygen.transform.position;
        oxygenEulerAngles = oxygen.transform.eulerAngles;
        yield return null;
    }
    public IEnumerator RecordNRMPositions()
    {
        distanceNRM = Drawer5Positions - NRM.transform.position;
        NRMPositions = NRM.transform.position;
        NRMEulerAngles = NRM.transform.eulerAngles;
        yield return null;
    }
    public IEnumerator RecordcatheterPositions()
    {
        foreach (GameObject segment in catheter)
        {
            if (segment != null) // 確保物件不為空
            {
                catheterEulerAngles.Add(segment.transform.eulerAngles);
                distanceCatheter.Add(Drawer5Positions - segment.transform.position);
                catheterPositions.Add(segment.transform.position);
            }
        }
        yield return null;
    }
    public IEnumerator RecordcatheterlinePositions()
    {
        foreach (GameObject segments in catheterline)
        {
            if (segments != null) // 確保segments不為空
            {
                // 等待直到有子物件
                while (segments.transform.childCount == 0)
                {
                    // 沒有子物件的話等待一幀，繼續檢查
                    yield return null;
                }

                // 當有子物件時，處理它們
                for (int i = 0; i < segments.transform.childCount; i++)
                {
                    Transform child = segments.transform.GetChild(i);
                    if (child != null) // 確保子物件不為空
                    {
                        // 儲存到字典
                        catheterlinePositions[child] = Drawer5Positions - child.position;

                        // 同時更新序列化用的列表
                        catheterlineKeys.Add(child);
                        catheterlineValues.Add(Drawer5Positions - child.position);
                    }
                }
            }
        }
    }
    public IEnumerator RecordBVMPositions()
    {
        distanceBVM = Drawer5Positions - BVM.transform.position;
        BVMPositions = BVM.transform.position;
        BVMEulerAngles = BVM.transform.eulerAngles;
        yield return null;
    }
    public IEnumerator RecordTrachealCleanerPositions()
    {
        foreach (GameObject segments in TrachealCleaner)
        {
            distanceTrachealCleaner.Add(Drawer4Positions - segments.transform.position);
            TrachealCleanerPositions.Add(segments.transform.position);
            TrachealCleanerEulerAngles.Add(segments.transform.eulerAngles);
        }
        // TrachealCleanerLocalPositions = TrachealCleanerLocal.transform.localPosition;
        // TrachealCleanerLocalEulerAngles = TrachealCleanerLocal.transform.localEulerAngles;
        yield return null;
    }
    public IEnumerator RecordSyringePositions()
    {
        distanceSyringe = Drawer3Positions - Syringe.transform.position;
        SyringePositions = Syringe.transform.position;
        SyringeEulerAngles = Syringe.transform.eulerAngles;
        yield return null;
    }
    public IEnumerator RecordLaryngoscopePositions()
    {
        foreach (GameObject segment in Laryngoscope)
        {
            if (segment != null) // 確保物件不為空
            {
                LaryngoscopeEulerAngles.Add(segment.transform.eulerAngles);
                distanceLaryngoscope.Add(Drawer3Positions - segment.transform.position);
                LaryngoscopePositions.Add(segment.transform.position);
            }
        }
        // LaryngoscopeLocalPositions = LaryngoscopeLocal.transform.localPosition;
        // LaryngoscopeLocalEulerAngles = LaryngoscopeLocal.transform.localEulerAngles;
        yield return null;
    }
    public IEnumerator RecordOxygenCylinderPositions()
    {
        foreach (GameObject segment in oxygenCylinder)
        {
            if (segment != null) // 確保物件不為空
            {
                oxygenCylinderEulerAngles.Add(segment.transform.eulerAngles);
                distanceOxygenCylinder.Add(Drawer3Positions - segment.transform.position);
                oxygenCylinderPositions.Add(segment.transform.position);
            }
        }
        yield return null;
    }




    public void ResetEKGPositions()
    {
        // 重置EKG物件的位置
        for (int i = 0; i < EKG.Count; i++)
        {
            if (EKG[i] != null && i < EKGPositions.Count)
            {
                Rigidbody rb = EKG[i].GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
                EKG[i].transform.rotation = Quaternion.Euler(EKGEulerAngles[i]);
                EKG[i].transform.position = EKGPositions[i];
                if (rb != null) rb.isKinematic = false;
            }
        }
    }
    public void ResetlinePerfabPositions()
    {
        foreach (GameObject segments in linePerfab)
        {
            for (int i = 0; i < segments.transform.childCount; i++)
            {
                Transform child = segments.transform.GetChild(i);
                if (child != null) // 確保子物件不為空
                {
                    child.gameObject.SetActive(false); // 設置子物件為不可見
                }
            }
        }
        ResetEKGPositions();
        // 重置linePerfab子物件的位置
        foreach (KeyValuePair<Transform, Vector3> entry in linePerfabPositions)
        {
            if (entry.Key != null)
            {
                entry.Key.position = entry.Value;
            }
        }
        foreach (GameObject segments in linePerfab)
        {
            for (int i = 0; i < segments.transform.childCount; i++)
            {
                Transform child = segments.transform.GetChild(i);
                if (child != null) // 確保子物件不為空
                {
                    child.gameObject.SetActive(true); // 設置子物件為不可見
                }
            }
        }
    }
    public void ResetoxygenPositions()
    {
        if (oxygen != null)
        {
            Rigidbody rb = oxygen.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            oxygen.transform.position = Drawer5.transform.position - distanceOxygen;
            oxygen.transform.rotation = Quaternion.Euler(oxygenEulerAngles);
            rb.isKinematic = false;
        }
    }
    public void ResetNRMPositions()
    {
        if (NRM != null)
        {
            Rigidbody rb = NRM.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            NRM.transform.position = Drawer5.transform.position - distanceNRM;
            NRM.transform.rotation = Quaternion.Euler(NRMEulerAngles);
            rb.isKinematic = false;
        }
    }
    public void ResetcatheterPositions()
    { // 重置EKG物件的位置
        for (int i = 0; i < catheter.Count; i++)
        {
            if (catheter[i] != null && i < catheterPositions.Count)
            {
                catheter[i].transform.position = Drawer5.transform.position - distanceCatheter[i];
                catheter[i].transform.rotation = Quaternion.Euler(catheterEulerAngles[i]);
            }
        }
    }
    public void ResetcatheterlinePositions()
    {
        LineConnect.DeleteAllSegments();
        ResetcatheterPositions();
        LineConnect.Line();
        resetLineConnectFlag = true;

        // 啟動協程來延遲設為 false
        StartCoroutine(ResetFlagAfterDelay());
    }

    private IEnumerator ResetFlagAfterDelay()
    {
        yield return new WaitForSeconds(5f); // 延遲 5 秒
        resetLineConnectFlag = false;
    }

    public void ResetBVMPositions()
    {
        if (BVM != null)
        {
            Rigidbody rb = BVM.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            BVM.transform.position = Drawer5.transform.position - distanceBVM;
            BVM.transform.rotation = Quaternion.Euler(BVMEulerAngles);
            rb.isKinematic = false;
        }
    }
    public void ResetTrachealCleanerPositions()
    {
        Rigidbody rb;
        for (int i = 0; i < TrachealCleaner.Count; i++)
        {
            if (TrachealCleaner[i] != null)
            {
                rb = TrachealCleaner[i].GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
                TrachealCleaner[i].transform.position = Drawer4.transform.position - distanceTrachealCleaner[i];
                TrachealCleaner[i].transform.rotation = Quaternion.Euler(TrachealCleanerEulerAngles[i]);
                if (rb != null) rb.isKinematic = false;
            }
        }
        for (int i = 0; i < carTrachealCleanerGameobject.Count; i++)
        {
            if (carTrachealCleanerGameobject[i] != null)
            {
                rb = carTrachealCleanerGameobject[i].GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
                carTrachealCleanerGameobject[i].transform.position = Car.transform.position + distanceCarTrachealCleanerGameobject;
                if (rb != null) rb.isKinematic = false;
            }
        }
        // rb = TrachealCleanerLocal.GetComponent<Rigidbody>();
        // if (rb != null) rb.isKinematic = true;
        // TrachealCleanerLocal.transform.localPosition = TrachealCleanerLocalPositions;
        // TrachealCleanerLocal.transform.localRotation = Quaternion.Euler(TrachealCleanerLocalEulerAngles);
        // if (rb != null) rb.isKinematic = false;
    }
    public void ResetSyringePositions()
    {
        if (Syringe != null)
        {
            Rigidbody rb = Syringe.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            Syringe.transform.position = Drawer3.transform.position - distanceSyringe;
            Syringe.transform.rotation = Quaternion.Euler(SyringeEulerAngles);
            rb.isKinematic = false;
        }
    }
    public void ResetLaryngoscopePositions()
    {
        Rigidbody rb;
        for (int i = 0; i < Laryngoscope.Count; i++)
        {
            if (Laryngoscope[i] != null)
            {
                rb = Laryngoscope[i].GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
                Laryngoscope[i].transform.position = Drawer3.transform.position - distanceLaryngoscope[i];
                Laryngoscope[i].transform.rotation = Quaternion.Euler(LaryngoscopeEulerAngles[i]);
                if (rb != null) rb.isKinematic = false;
            }
        }
        for (int i = 0; i < carLaryngoscopeGameobject.Count; i++)
        {
            if (carLaryngoscopeGameobject[i] != null)
            {
                rb = carLaryngoscopeGameobject[i].GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
                carLaryngoscopeGameobject[i].transform.position = Car.transform.position + distanceCarLaryngoscopeGameobject;
                if (rb != null) rb.isKinematic = false;
            }
        }
        // rb = LaryngoscopeLocal.GetComponent<Rigidbody>();
        // if (rb != null) rb.isKinematic = true;
        // LaryngoscopeLocal.transform.localPosition = LaryngoscopeLocalPositions;
        // LaryngoscopeLocal.transform.localRotation = Quaternion.Euler(LaryngoscopeLocalEulerAngles);
        // if (rb != null) rb.isKinematic = true;
    }
    public void ResetOxygenCylinderPositions()
    {
        for (int i = 0; i < oxygenCylinder.Count; i++)
        {
            if (oxygenCylinder[i] != null)
            {
                Rigidbody rb = oxygenCylinder[i].GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = true;
                oxygenCylinder[i].transform.position = Drawer3.transform.position - distanceOxygenCylinder[i];
                oxygenCylinder[i].transform.rotation = Quaternion.Euler(oxygenCylinderEulerAngles[i]);
                if (rb != null) rb.isKinematic = false;
            }
        }
    }
}

