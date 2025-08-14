using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phone : MonoBehaviour
{
    [SerializeField] private IntuGameManage intuGameManage;
    [SerializeField] private Speak1 speak;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private bool GrabBool;
    private Vector3 initialPosition; // 紀錄物件的初始位置
    private Quaternion initialRotation;
    public bool flag;
    void Start()
    {
        // 紀錄當前物件的位置
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        intuGameManage = FindObjectOfType<IntuGameManage>();
        speak = FindObjectOfType<Speak1>();
    }
    void Update()
    {
        if (!GrabBool)
        {
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            if (!intuGameManage.endFlag1 && ((speak.count >= 10 && speak.count <= 14) || (speak.count >= 17 && speak.count <= 21)))
                audioSource.Pause();

        }
        else
        {
            if (!intuGameManage.endFlag1 && ((speak.count >= 10 && speak.count <= 14) || (speak.count >= 17 && speak.count <= 21)))
                audioSource.UnPause();
        }
    }
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand") && GrabBool)
        {
            flag = true;
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
    public void Flag()
    {
        flag = true;
    }
}
