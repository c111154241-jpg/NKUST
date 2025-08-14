using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Quest4 : MonoBehaviour
{
    [SerializeField] private string[] ansLetters = new string[5];
    [SerializeField] private string[] answer = new string[5];
    [SerializeField] private GameObject showAnswer;
    [SerializeField] private Text one, two, three, four, five;

    public bool flag;
    public bool errorFlag;
    void Start()
    {
        answer[0] = "B";
        answer[1] = "A";
        answer[2] = "D";
        answer[3] = "C";
        answer[4] = "E";
    }
    void Update()
    {
        one.text = ansLetters[0];
        two.text = ansLetters[1];
        three.text = ansLetters[2];
        four.text = ansLetters[3];
        five.text = ansLetters[4];
    }
    public void InputLetter(string letter)
    {
        // 先檢查是否已經存在相同的字母
        for (int j = 0; j < ansLetters.Length; j++)
        {
            if (ansLetters[j] == letter)
            {
                ansLetters[j] = null;
                return; // 如果已經存在相同字母，直接返回
            }
        }

        // 沒有重複的字母，才進行填入
        for (int i = 0; i < ansLetters.Length; i++)
        {
            if (string.IsNullOrEmpty(ansLetters[i]))
            {
                // 將新字母填入空位
                ansLetters[i] = letter;
                break;
            }
        }
    }
    public void Answer()
    {
        if (ansLetters.SequenceEqual(answer))
        {
            flag = true;
            Debug.Log("Correct Answer!");
        }
        else
        {
            errorFlag = true;
            Debug.LogError("Incorrect Answer");
            for (int i = 0; i < ansLetters.Length; i++)
            {
                ansLetters[i] = null;
            }
            showAnswer.SetActive(true);
        }
    }
    public void Clear()
    {
        for (int i = 0; i < ansLetters.Length; i++)
        {
            ansLetters[i] = null;
        }
    }
}
