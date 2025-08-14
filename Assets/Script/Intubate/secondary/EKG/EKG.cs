using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EKG : MonoBehaviour
{
    //這跟人物的EKG Collider有關
    [SerializeField] private IntuGameManage GameManage;
    [SerializeField] private bool GrabBool;
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private Collider Collider;
    [SerializeField] private Speak1 speak;
    [SerializeField] private Transform EKGRed;
    [SerializeField] private Transform EKGWhite;
    [SerializeField] private Transform EKGBlack;
    [SerializeField] private List<GameObject> linePerfab;


    public bool flag;
    public bool errorFlag;
    public bool pasteFlag;
    public LayerMask layerMask; // 在 Inspector 中設定
    void Start()
    {

    }


    void Update()
    {
        if (GrabBool)
        {
            Rigidbody.isKinematic = true;
        }
        else
        {
            Rigidbody.isKinematic = false;
        }

        if (speak.EKGFlag)
        {
            StartCoroutine(LinePerfabMask());
            if (transform.name == "EKG Red")
            {
                transform.position = EKGRed.transform.position;
                transform.rotation = EKGRed.transform.rotation;
            }
            else if (transform.name == "EKG White")
            {
                transform.position = EKGWhite.transform.position;
                transform.rotation = EKGWhite.transform.rotation;
            }
            else if (transform.name == "EKG Black")
            {
                transform.position = EKGBlack.transform.position;
                transform.rotation = EKGBlack.transform.rotation;
            }
        }
    }
    void OnTriggerStay(Collider other)
    {
        // Debug.Log("持续停留在触发器内: " + other.gameObject.layer);
        if (!GrabBool && other.gameObject.layer == LayerMask.NameToLayer("EKGCollider"))
        {   //因為不知道考生貼哪裡所以在EKG Collider裡取名都叫EKG
            Transform childTransform = other.transform.Find("EKG");
            transform.position = childTransform.transform.position;
            transform.rotation = childTransform.transform.rotation;
            Rigidbody.isKinematic = true;
            //因為不知道考生貼正不正確所以要找第1個來判斷
            pasteFlag = true;
            if (other.transform.GetChild(1).name == gameObject.name)
            {
                flag = true;
            }
            else
            {
                errorFlag = true;
            }
            layerMask = (1 << 6) | (1 << 19);
            Collider.excludeLayers = layerMask;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("EKGCollider"))
        {
            flag = false;
            errorFlag = false;
            pasteFlag = false;
            layerMask &= ~(1 << 19);
            Collider.excludeLayers = layerMask;
        }
    }
    public void Grab()
    {
        GrabBool = true;
    }
    public void UnGrab()
    {
        //Rigidbody.isKinematic = false;
        //這樣沒用，雖然Rigidbody.isKinematic變成false但不知為何他又會自動變為true
        //在updata裡用這取代
        // if (GrabBool)
        // Rigidbody.isKinematic = true;
        // else
        // Rigidbody.isKinematic = false;

        GrabBool = false;
    }

    private IEnumerator LinePerfabMask()
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
                        Rigidbody rb = child.GetComponent<Rigidbody>();
                        rb.excludeLayers = layerMask;
                    }
                }
            }
        }
    }
}
