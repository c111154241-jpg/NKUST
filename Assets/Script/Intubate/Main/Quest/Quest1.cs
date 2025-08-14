using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest1 : MonoBehaviour
{
    [SerializeField] private string ansLetter;
    [SerializeField] private string answer;
    [SerializeField] private Text A;
    [SerializeField] private Text B;
    [SerializeField] private Text C;
    [SerializeField] private Text D;
    public bool flag;
    public bool errorFlag;
    void Start()
    {
        answer = "A";
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
            switch (ansLetter)
            {
                case "B":
                    B.color = Color.red;
                    break;
                case "C":
                    C.color = Color.red;
                    break;
                case "D":
                    D.color = Color.red;
                    break;
            }
            // 更改文本的颜色
            A.color = Color.green; // 设置为綠色，您可以使用任何颜色
        }
    }
}
