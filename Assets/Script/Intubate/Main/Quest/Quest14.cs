using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class Quest14 : MonoBehaviour
{
    [SerializeField] private string[] ansLetters = new string[4];
    [SerializeField] private string[] answer = new string[4];
    [SerializeField] private GameObject showAnswer;
    [SerializeField] private Text answerText; public bool flag;
    public bool errorFlag;
    void Start()
    {
        answer[0] = "A";
        answer[1] = "B";
        answer[2] = "D";
    }
    void Update()
    {
        // 過濾掉 null 或空字串，並按字母順序排序
        string[] sortedLetters = ansLetters
            .Where(x => !string.IsNullOrEmpty(x)) // 移除空值
            .OrderBy(x => x)                      // 按字母順序排序
            .ToArray();                            // 轉回陣列

        // 根據 ansLetters 的數量來決定顯示格式
        if (sortedLetters.Length > 0)
        {
            string separator = sortedLetters.Length == 1 ? "" : ", "; // 如果只有一個字母，就不加逗號
            answerText.text = "回答: " + string.Join(separator, sortedLetters); // 使用動態決定的分隔符
        }
        else
        {
            answerText.text = "回答: "; // 如果沒有字母，顯示"回答:"
        }
    }
    public void Answer()
    {
        // 取得使用者選擇的有效字母（排除 null）
        List<string> userSelection = ansLetters.Where(x => !string.IsNullOrEmpty(x)).ToList();

        // 取得正確答案的有效字母（排除 null）
        List<string> correctAnswer = answer.Where(x => !string.IsNullOrEmpty(x)).ToList();

        // 檢查兩個陣列是否相同（忽略順序）
        if (userSelection.Count == correctAnswer.Count && !userSelection.Except(correctAnswer).Any())
        {
            flag = true;
            Debug.Log("Correct Answer!");
        }
        else
        {
            errorFlag = true;
            Debug.LogError("Incorrect Answer");

            // 清空使用者選擇
            for (int i = 0; i < ansLetters.Length; i++)
            {
                ansLetters[i] = null;
            }

            // 顯示正確答案
            showAnswer.SetActive(true);
        }
    }
    public void InputLetter(string letter)
    {
        // 檢查是否已經選擇過該字母
        for (int j = 0; j < ansLetters.Length; j++)
        {
            if (ansLetters[j] == letter)
            {
                // 如果已存在，則刪除它並將後面的字母向前移動
                for (int k = j; k < ansLetters.Length - 1; k++)
                {
                    ansLetters[k] = ansLetters[k + 1]; // 後面的字母往前移
                }
                ansLetters[ansLetters.Length - 1] = null; // 最後一個位置清空
                return;
            }
        }

        // 如果字母還沒被選擇，則填入第一個空位
        for (int i = 0; i < ansLetters.Length; i++)
        {
            if (string.IsNullOrEmpty(ansLetters[i]))
            {
                ansLetters[i] = letter;
                break;
            }
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
