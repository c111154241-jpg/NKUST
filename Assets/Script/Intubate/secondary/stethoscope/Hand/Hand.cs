using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    //用來控制聽診器，因為聽診器有手的與遙控器的
    public Stethoscope stethoscope;
    public bool flag;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("StethoscopeCollider"))
        {
            flag = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("StethoscopeCollider"))
        {
            // Debug.LogError("持续停留在触发器内: " + other.gameObject.layer);
            stethoscope.flag = false;
            stethoscope.frist = true;
            flag = false;
        }
    }
}
