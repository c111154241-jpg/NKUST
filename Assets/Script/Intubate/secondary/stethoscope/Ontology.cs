using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ontology : MonoBehaviour
{
    [SerializeField] GameObject quest3GameObject;
    public GameObject handStethoscope;
    public GameObject controlStethoscope;
    [SerializeField] Speak1 speak;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand") && speak.count >= 10)
        {
            transform.gameObject.SetActive(false);
            handStethoscope.SetActive(true);
            controlStethoscope.SetActive(true);
        }
    }
}
