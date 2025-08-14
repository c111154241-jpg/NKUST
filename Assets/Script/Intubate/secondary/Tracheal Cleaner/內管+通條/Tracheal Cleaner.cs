using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//只需保留OnTriggerStay(Collider other)
public class TrachealCleaner : MonoBehaviour
{
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private GameObject Model;//指插管動畫的MODEL
    [SerializeField] private GameObject CarModel;
    [SerializeField] private GameObject Tracheal75;//非7.5不要放
    [SerializeField] private GameObject Intubation75;//非7.5不要放
    [SerializeField] private GameObject Car;//非7.5不要放
    [SerializeField] private Vector3 Distance;//非7.5不要放
    [SerializeField] private Speak2 Speak2;//非7.5不要放
    [SerializeField] private Text showText;//在QUEST8 非7.5不要放
    [SerializeField] private GameObject button;//在QUEST8 非7.5不要放
    [SerializeField] private GameObject syringe;
    [SerializeField] private bool GrabBool;
    public bool flag;
    Dictionary<string, string> validPairs = new Dictionary<string, string>
{
    { "插管導管-鐵絲 7", "插管導管(本體)-7" },
    { "插管導管-鐵絲 7.5 (觸發)", "插管導管(本體)-7.5" },
    { "插管導管-鐵絲 8", "插管導管(本體)-8" }
};
    void Start()
    {

    }


    void Update()
    {
        // if (flag || GrabBool)
        // {
        //     Rigidbody.isKinematic = true;
        // }
        // else
        // {
        //     Rigidbody.isKinematic = false;
        // }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ironwire"))
        {
            string otherName = other.gameObject.name;
            string thisName = transform.name;

            // 判斷是否為正確匹配
            if (validPairs.TryGetValue(otherName, out string expectedName))
            {
                if (thisName == expectedName)
                {
                    if (thisName == "插管導管(本體)-7.5" && Speak2.count >= 5)
                    {

                        flag = true;
                        Model.transform.position = Car.transform.position + Distance;
                        Model.SetActive(true);
                        Tracheal75.SetActive(false);
                        Intubation75.SetActive(false);
                        showText.text = "Q8\n\n請確認通條放置位置，確認後按下按鈕回答";
                        button.SetActive(true);
                        Syringe Syringe = syringe.GetComponent<Syringe>();
                        Syringe.flag = false;
                        Rigidbody rb = syringe.GetComponent<Rigidbody>();
                        rb.isKinematic = false;
                        Debug.Log("大小匹配正確");
                    }
                    else
                    {
                        flag = false;
                        Debug.Log("大小不對要7.5號");
                    }
                }
                else
                {
                    flag = false;
                    Debug.Log("大小不對：" + thisName + " 不匹配 " + otherName);
                }
            }
            else
            {
                flag = false;
                Debug.Log("未知的鐵絲名稱：" + otherName);
            }
        }

        // if (other.gameObject.layer == LayerMask.NameToLayer("Drawer") && other.gameObject.name == "Drawer4")
        // {
        //     Rigidbody.isKinematic = true;
        //     transform.position = CarModel.transform.position;
        //     transform.rotation = CarModel.transform.rotation;
        // }
    }
    void OnTriggerExit(Collider other)
    {
        // if (other.gameObject.layer == LayerMask.NameToLayer("Drawer") && other.gameObject.name == "Drawer4")
        // {
        //     Rigidbody.isKinematic = false;
        // }
    }
    public void Grab()
    {
        GrabBool = true;
    }
    public void UnGrab()
    {
        GrabBool = false;
    }
}
