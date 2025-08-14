using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oxygencylinder : MonoBehaviour
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
        if (!GrabBool && other.gameObject.layer == LayerMask.NameToLayer("OxygenCylinderCollider"))
        {
            transform.position = Model.transform.position;
            transform.rotation = Model.transform.rotation;
            Rigidbody.isKinematic = true;
            flag = true;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("側面"))
        {
            Rigidbody.isKinematic = true;
            transform.position = CarModel.transform.position;
            transform.rotation = CarModel.transform.rotation;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("OxygenCylinderCollider"))
        {
            Rigidbody.isKinematic = false;
            flag = false;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("側面"))
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
