using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stethoscope : MonoBehaviour
{
    [SerializeField] private Transform hand;
    [SerializeField] private IntuGameManage GameManage;
    [SerializeField] private ShowHide showHide;
    [SerializeField] private Hand handScript;
    [SerializeField] private GameObject Model;
    [SerializeField] private bool GrabBool;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] GameObject quest3GameObject;

    public AudioClip audioClip;
    public bool frist = true;
    public bool flag;

    void Start()
    {

    }

    void Update()
    {
        if (!flag)// frist或GrabBool
        {
            transform.position = hand.position;
        }
        if (handScript == null && showHide.handScript)
            handScript = showHide.handScript;
    }
    void OnTriggerStay(Collider other)
    {
        // Debug.Log("持续停留在触发器内: " + other.gameObject.layer);
        if (handScript.flag && !GrabBool && other.gameObject.layer == LayerMask.NameToLayer("StethoscopeCollider"))
        {
            quest3GameObject.SetActive(true);
            transform.position = Model.transform.position;
            transform.rotation = Model.transform.rotation;
            if (frist)
            {
                frist = false;
                audioSource.clip = audioClip;
                audioSource.Play();
            }
            flag = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("StethoscopeCollider"))
        {
            audioSource.Stop();
            flag = false;
            frist = true;
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
