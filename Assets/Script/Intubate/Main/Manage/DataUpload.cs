using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class DataUpload : MonoBehaviour
{
    //使用線上模式，把資料上傳資料庫
    [SerializeField] private IntuGameManage IntuGameManage;
    public void SendData()
    {
        StartCoroutine(SendLoginRequest(Login.ID, Login.Pass, IntuGameManage.totalScore, IntuGameManage.errorQuestions, IntuGameManage.elapsedTime.ToString()));
    }
    private IEnumerator SendLoginRequest(string username, string password, int score, string incorrect_questions, string answer_time)
    {
        if (incorrect_questions == null)
        {
            incorrect_questions = "";  // 如果是 null，將其設置為空字符串
        }
        // 创建请求的参数
        WWWForm form = new WWWForm();
        form.AddField("student_id", username);
        form.AddField("password", password);
        form.AddField("score", score);
        form.AddField("incorrect_questions", incorrect_questions);
        form.AddField("answer_time", answer_time);

        // 发送 POST 请求
        string php = URL.LoadURL("getStudentData.php");
        Debug.Log(php);
        UnityWebRequest www = UnityWebRequest.Post(php, form);
        www.timeout = 10; // 設置超時（以秒為單位）
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            // errorText.text = request.error;
            Debug.LogError($"错误: {www.error}");
        }
        else
        {
            // 处理服务器响应
            string responseText = www.downloadHandler.text;
            Debug.LogError(responseText);
            if (responseText.Contains("erro"))
            {
                Debug.LogError("上傳失敗");
            }
            else if (responseText.Contains("success"))
            {
                Debug.LogError("上傳成功");
            }
        }
    }
}