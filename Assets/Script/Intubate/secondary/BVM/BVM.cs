using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BVM : MonoBehaviour
{
    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private GameObject Model;
    [SerializeField] private GameObject CarModel;
    [SerializeField] private bool GrabBool;
    public bool flag;

    void Start()
    {

    }


    void Update()
    {
        if (flag || GrabBool)
        {
            Rigidbody.isKinematic = true;
        }
        else
        {
            Rigidbody.isKinematic = false;
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (!GrabBool && other.gameObject.layer == LayerMask.NameToLayer("BVMCollider"))
        {
            transform.position = Model.transform.position;
            transform.rotation = Model.transform.rotation;
            Rigidbody.isKinematic = true;
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
        if (other.gameObject.layer == LayerMask.NameToLayer("BVMCollider"))
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
        GrabBool = false;
    }
}
