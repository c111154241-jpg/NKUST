using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syringe : MonoBehaviour
{
    [SerializeField] private GameObject PumpGameObject;
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private GameObject Model70;
    [SerializeField] private GameObject Model75;
    [SerializeField] private GameObject Model80;
    [SerializeField] private GameObject CarModel;
    [SerializeField] private bool GrabBool;
    public bool flag;
    public bool errorFlag;

    [SerializeField] private Pump Pump;
    [SerializeField] private InflatableAnimation InflatableAnimation;
    [SerializeField] private InflableHand InflableHand;

    void Start()
    {

    }


    void Update()
    {
        if (flag || GrabBool || errorFlag)
            Rigidbody.isKinematic = true;
        else
            Rigidbody.isKinematic = false;
        if (InflatableAnimation.handFlag && !InflableHand.flag)
        {
            Pump.transform.localPosition = new Vector3(0f, 0f, 0.03f);
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Drawer") && other.gameObject.name == "Drawer3")
        {
            Rigidbody.isKinematic = true;
            transform.position = CarModel.transform.position;
            transform.rotation = CarModel.transform.rotation;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("inflatable") && other.gameObject.name == "插管導管(充氣)-7")
        {
            errorFlag = true;
            Rigidbody.isKinematic = true;
            transform.position = Model70.transform.position;
            transform.rotation = Model70.transform.rotation;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("inflatable") && other.gameObject.name == "插管導管(充氣)-7.5")
        {
            flag = true;
            Rigidbody.isKinematic = true;
            transform.position = Model75.transform.position;
            transform.rotation = Model75.transform.rotation;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("inflatable") && other.gameObject.name == "插管導管(充氣)-8")
        {
            errorFlag = true;
            Rigidbody.isKinematic = true;
            transform.position = Model80.transform.position;
            transform.rotation = Model80.transform.rotation;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("inflatable"))
        {
            flag = false;
            errorFlag = false;
            if (InflableHand.flag)
            {
                Pump.transform.localPosition = new Vector3(0f, 0f, 0.03f);
            }
            else
            {
                Pump.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Drawer") && other.gameObject.name == "Drawer3")
        {
            Rigidbody.isKinematic = false;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("inflatable"))
        {
            flag = false;
            errorFlag = false;
            if (InflableHand.flag)
            {
                Pump.transform.localPosition = new Vector3(0f, 0f, 0.03f);
            }
            else
            {
                Pump.transform.localPosition = new Vector3(0f, 0f, 0f);
            }
        }
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
