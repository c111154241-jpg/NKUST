using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Laryngoscope : MonoBehaviour
{

    [SerializeField] private Rigidbody Rigidbody;
    [SerializeField] private GameObject HeadModel;
    [SerializeField] private GameObject HeadModelA;
    [SerializeField] private GameObject CarModel;
    [SerializeField] private bool GrabBool;
    public bool flag;

    void Start()
    {

    }


    void FixedUpdate()
    {
        if (GrabBool)
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
        if ((transform.name == "無菌袋(head)" || transform.name == "無菌袋(head) (A)") && other.gameObject.layer == LayerMask.NameToLayer("Drawer") && other.gameObject.name == "Drawer3")
        {
            Rigidbody.isKinematic = true;
            transform.position = CarModel.transform.position;
            transform.rotation = CarModel.transform.rotation;
        }
        else if (transform.name == "handle" && other.gameObject.layer == LayerMask.NameToLayer("Drawer") && other.gameObject.name == "Drawer3")
        {
            Rigidbody.isKinematic = true;
            transform.position = CarModel.transform.position;
            transform.rotation = CarModel.transform.rotation;
        }
        if (other.name == "head (觸發)" && GrabBool)
        {
            flag = true;
            other.gameObject.SetActive(false);
            HeadModel.gameObject.SetActive(true);
        }
        if (other.name == "無菌袋(head) (A)" && GrabBool)
        {
            flag = true;
            other.gameObject.SetActive(false);
            HeadModelA.gameObject.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (transform.name == "無菌袋(head)" && other.gameObject.layer == LayerMask.NameToLayer("Drawer") && other.gameObject.name == "Drawer3")
        {
            Rigidbody.isKinematic = false;
        }
        else if (transform.name == "handle" && other.gameObject.layer == LayerMask.NameToLayer("Drawer") && other.gameObject.name == "Drawer3")
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
