using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest6 : MonoBehaviour
{
    [SerializeField] Text A;
    [SerializeField] Text B;
    [SerializeField] Text C;
    [SerializeField] Text D;
    [SerializeField] CatheterTipRemake CatheterTip;
    [SerializeField] Oxygen Oxygen;
    [SerializeField] NRM NRM;
    [SerializeField] SteelBall SteelBall;
    public bool flag;
    public bool errorFlag;

    void Start()
    {

    }

    void Update()
    {
        if (Oxygen.flag)
            A.color = Color.green;
        else
            A.color = Color.black;

        if (CatheterTip.Oxygenflag)
            B.color = Color.green;
        else
            B.color = Color.black;

        if (NRM.flag)
            C.color = Color.green;
        else
            C.color = Color.black;

        if (SteelBall.flag)
            D.color = Color.green;
        else
            D.color = Color.black;


        if (A.color == Color.green &&
            B.color == Color.green &&
            C.color == Color.green &&
            D.color == Color.green)
        {
            flag = true;
        }
    }
}
