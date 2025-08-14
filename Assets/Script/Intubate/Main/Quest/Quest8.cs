using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest8 : MonoBehaviour
{
    [SerializeField] private IntubationAnimation Intubation;
    [SerializeField] private Text showText;
    public bool flag;
    public bool errorFlag;
    void Start()
    {

    }
    public void Answer()
    {
        if (Intubation.loneFlag)
        {
            errorFlag = true;
            showText.text = "Q8\n\n請確認通條放置位置，確認後按下按鈕回答\n\n放置位置太長，請在重新操作一次";
        }
        else if (Intubation.shortFlag)
        {
            errorFlag = true;
            showText.text = "Q8\n\n請確認通條放置位置，確認後按下按鈕回答\n\n放置位置太短，請在重新操作一次";
        }
        else if (Intubation.flag)
        {
            flag = true;
        }
    }
}
