using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NRM : MonoBehaviour
{
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private Speak1 speak;
    [SerializeField] private GameObject Model;
    [SerializeField] private GameObject CarModel;
    public bool GrabBool;
    public bool flag;

    void Start()
    {

    }


    void FixedUpdate()
    {
        if (flag || GrabBool)
        {
            Rigidbody.isKinematic = true;
        }
        else
        {
            Rigidbody.isKinematic = false;
        }
        if (speak.NRMFlag)
        {
            transform.position = Model.transform.position;
            transform.rotation = Model.transform.rotation;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (!GrabBool && other.gameObject.layer == LayerMask.NameToLayer("NRMCollider"))
        {
            Rigidbody.isKinematic = true;
            transform.position = Model.transform.position;
            transform.rotation = Model.transform.rotation;
            flag = true;
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
        if (other.gameObject.layer == LayerMask.NameToLayer("NRMCollider"))
        {
            Rigidbody.isKinematic = false;
            flag = false;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Drawer") && other.gameObject.name == "Drawer5")
        {
            Rigidbody.isKinematic = false;
        }
    }
    public void Grab()
    {
        GrabBool = true;
    }
    public void UnGrab()
    {
        // Rigidbody.isKinematic = false;
        GrabBool = false;
    }
}
