using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Meta.XR.MRUtilityKit;
using System;
using Newtonsoft.Json;
using System.IO;

public class OfflineLogin : MonoBehaviour
{
    public Text LoginUserText;
    public Text errorText;
    public InputField ID_input, Pass_input;
    [SerializeField] private GameObject OfflineLoginConfirm;
    [SerializeField] private GameObject MenuConfirm;
    static public string Name;
    static public string ID;
    static public string Pass;
    private string databaseFilePath;

    private void Start()
    {
        // 指定 DatabaseData.txt 的路徑
        databaseFilePath = Path.Combine(Application.persistentDataPath, "DatabaseData.txt");
    }
    public void Button_Quit()
    {
        ID_input.text = "";
        Pass_input.text = "";
        OfflineLoginConfirm.SetActive(false);
        MenuConfirm.SetActive(true);
    }
    public void Button_Login()
    {
        ID_input.text = "";
        Pass_input.text = "";
        errorText.text = "";
        OfflineLoginConfirm.SetActive(false);
        MenuConfirm.SetActive(true);
    }
    public void Loginenter()
    {
        if (!string.IsNullOrEmpty(ID_input.text) && !string.IsNullOrEmpty(Pass_input.text))
        {
            errorText.text = "等待中";
            ValidateLogin();
        }
        else
            errorText.text = "請輸入帳號";
    }

    private IEnumerator SendLoginRequest(string userID, string password)
    {
        ID = userID;
        Pass = password;
        LoginUserText.text = ID;
        Button_Login();
        yield return null;

    }

    public void ValidateLogin()
    {
        string studentId = ID_input.text.Trim();
        string password = Pass_input.text.Trim();

        if (string.IsNullOrEmpty(studentId))
        {
            errorText.text = "請輸入學號！";
            return;
        }

        if (!File.Exists(databaseFilePath))
        {
            errorText.text = "DatabaseData.txt 檔案不存在！";
            return;
        }

        bool studentIdFound = false;
        bool passwordCorrect = false;

        // 讀取 DatabaseData.txt 並檢查內容
        foreach (string line in File.ReadAllLines(databaseFilePath))
        {
            // 分割每一行的資料
            string[] fields = line.Split(' ');
            if (fields.Length < 5) continue;

            string id = fields[0];       // 學號
            string storedPassword = fields[2]; // 密碼

            if (id == studentId)
            {
                studentIdFound = true;
                if (storedPassword == password)
                {
                    StartCoroutine(SendLoginRequest(id, storedPassword));
                    passwordCorrect = true;
                }
                break;
            }
        }

        // 根據檢查結果更新文字
        if (!studentIdFound)
        {
            Debug.LogError("無此學號");
            errorText.text = "無此學號";
        }
        else if (passwordCorrect)
        {
            Debug.LogError("成功登入");
            errorText.text = "成功登入";
        }
        else
        {
            Debug.LogError("密碼錯誤");
            errorText.text = "密碼錯誤";
        }
    }

}
