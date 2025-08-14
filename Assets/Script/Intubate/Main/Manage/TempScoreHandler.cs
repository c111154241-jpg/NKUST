using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TempScoreHandler : MonoBehaviour
{
    //使用離線模式，把站存資料更新上資料庫，有flag才做
    //使用離線模式，把站存資料存在本地
    [SerializeField] private IntuGameManage IntuGameManage;
    [SerializeField] bool flag;//使用離線模式，把站存資料更新上資料庫
    private string tempFilePath;

    void Start()
    {
        // tempFilePath = Path.Combine(Application.persistentDataPath, "temp.txt");
        // Debug.Log("文件保存路径: " + tempFilePath);
    }


    public void SaveTempData()
    {
        tempFilePath = Path.Combine(Application.persistentDataPath, "temp.txt");
        Debug.Log("文件保存路径: " + tempFilePath);
        if (!Directory.Exists(Application.persistentDataPath))
            Debug.LogError("文件夹路径不存在！");
        // 检查文件是否存在
        if (File.Exists(tempFilePath))
            Debug.Log("文件已存在");
        else
            Debug.Log("文件不存在，将会创建文件");

        string studentId;
        if (!string.IsNullOrEmpty(OfflineLogin.ID))
            studentId = OfflineLogin.ID;
        else
            studentId = "NONE";
        string score = IntuGameManage.totalScore.ToString();
        string answerTime = FormatAnswerTime(((int)IntuGameManage.elapsedTime).ToString());
        string currentTime = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
        string incorrect_questions = IntuGameManage.errorQuestions;
        if (string.IsNullOrEmpty(studentId) || string.IsNullOrEmpty(score) || string.IsNullOrEmpty(answerTime))
        {
            Debug.LogError("請填寫所有欄位！");
            return;
        }

        string dataLine = $"{studentId} {score} {answerTime} {currentTime} {incorrect_questions}";
        try
        {
            File.AppendAllText(tempFilePath, dataLine + Environment.NewLine);
            Debug.Log("成功儲存資料至暫存檔！");
            if (flag)
                SendTempData();
        }
        catch (Exception e)
        {
            Debug.LogError("儲存資料失敗: " + e.Message);
        }
    }

    string FormatAnswerTime(string input)
    {
        try
        {
            string[] parts = input.Split(':');

            if (parts.Length == 1)
            {
                // 單純的秒數
                int seconds = int.Parse(parts[0]);
                return string.Format("{0}:{1:00}", seconds / 60, seconds % 60);
            }
            else if (parts.Length == 2)
            {
                // 分鐘:秒數
                int minutes = int.Parse(parts[0]);
                int seconds = int.Parse(parts[1]);
                return string.Format("{0}:{1:00}", minutes, seconds);
            }
            else if (parts.Length == 3)
            {
                // 小時:分鐘:秒數
                int hours = int.Parse(parts[0]);
                int minutes = int.Parse(parts[1]);
                int seconds = int.Parse(parts[2]);
                return string.Format("{0}:{1:00}:{2:00}", hours, minutes, seconds);
            }
            else
            {
                Debug.LogError("格式錯誤: " + input);
                return "0:00";
            }
        }
        catch (Exception e)
        {
            Debug.LogError("格式化用時失敗: " + e.Message);
            return "0:00";
        }
    }

    void SendTempData()
    {
        if (!File.Exists(tempFilePath))
        {
            Debug.LogError("暫存檔不存在！");
            return;
        }

        string tempData = File.ReadAllText(tempFilePath);

        if (string.IsNullOrEmpty(tempData))
        {
            Debug.LogError("暫存檔為空！");
            return;
        }

        StartCoroutine(SendDataCoroutine(tempData));
    }

    IEnumerator SendDataCoroutine(string tempData)
    {
        string[] lines = tempData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] parts = line.Split(' ');

            if (parts.Length < 5)
            {
                Debug.LogError("資料格式錯誤: " + line);
                continue;
            }

            string studentId = parts[0];
            string score = parts[1];
            string answerTime = parts[2];
            string answerDate = parts[3];
            string answerText = parts[4];

            WWWForm form = new WWWForm();
            form.AddField("student_id", studentId);
            form.AddField("score", score);
            form.AddField("answer_time", answerTime);
            form.AddField("answer_date", answerDate);
            form.AddField("incorrect_questions", answerText);
            string php = URL.LoadURL("your_php_file.php");
            Debug.Log(php);
            using (UnityWebRequest www = UnityWebRequest.Post(php, form))
            {
                www.timeout = 10; // 設置超時（以秒為單位）
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("資料發送失敗: " + www.error);
                }
                else
                {
                    Debug.Log("資料發送成功: " + www.downloadHandler.text);
                }
            }
        }

        // 清空暫存檔
        try
        {
            File.WriteAllText(tempFilePath, string.Empty);
            Debug.Log("暫存檔已清空！");
        }
        catch (Exception e)
        {
            Debug.LogError("清空暫存檔失敗: " + e.Message);
        }
    }
}
