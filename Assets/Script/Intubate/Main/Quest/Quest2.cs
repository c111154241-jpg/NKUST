using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest2 : MonoBehaviour
{
    [SerializeField] private EKG EKGRed;
    [SerializeField] private EKG EKGWhite;
    [SerializeField] private EKG EKGBlack;
    [SerializeField] private GameObject quest2Hint;
    [SerializeField] private GameObject quest2True;

    public bool flag;
    public bool errorFlag;
    void Start()
    {

    }

    void Update()
    {
        if (!flag && EKGRed.pasteFlag && EKGWhite.pasteFlag && EKGBlack.pasteFlag)
        {
            if (EKGRed.flag && EKGWhite.flag && EKGBlack.flag)
            {
                quest2Hint.SetActive(false);
                quest2True.SetActive(true);
            }
            else if (EKGRed.errorFlag || EKGWhite.errorFlag || EKGBlack.errorFlag)
            {
                quest2Hint.SetActive(true);
                errorFlag = true;
            }
        }
        else
        {
            quest2Hint.SetActive(false);
            quest2True.SetActive(false);
        }
    }
    public void Leave()
    {
        flag = true;
        quest2True.SetActive(false);
    }
}
