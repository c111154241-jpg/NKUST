using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Quest9 : MonoBehaviour
{
    [SerializeField] private GameObject quest9;
    public GameObject video;
    [SerializeField] private string ansLetter;
    [SerializeField] private string answer;
    [SerializeField] private Text A;
    [SerializeField] private Text B;
    [SerializeField] private Text C;

    public bool flag;
    public bool errorFlag;


    public void SelectedClip(int token)
    {
        switch (token)
        {
            case 0:
                answer = "A";
                break;
            case 1:
                answer = "B";
                break;
            case 2:
                answer = "C";
                break;
        }
    }

    public void InputLetter(string letter)
    {
        ansLetter = letter;
        if (ansLetter == answer)
        {
            flag = true;
        }
        else
        {
            errorFlag = true;

            // 將錯誤答案標紅
            if (ansLetter == "A") A.color = Color.red;
            if (ansLetter == "B") B.color = Color.red;
            if (ansLetter == "C") C.color = Color.red;

            // 將正確答案標綠
            if (answer == "A") A.color = Color.green;
            if (answer == "B") B.color = Color.green;
            if (answer == "C") C.color = Color.green;
        }
    }
    public void Return()
    {
        video.SetActive(true);
        quest9.SetActive(false);
    }
}
