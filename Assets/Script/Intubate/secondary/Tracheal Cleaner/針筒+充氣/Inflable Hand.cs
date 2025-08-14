using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflableHand : MonoBehaviour
{
    [SerializeField] private InflatableAnimation inflatableAnimation;
    public bool flag;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Hand") && inflatableAnimation.handFlag)
            flag = true;
    }
}

