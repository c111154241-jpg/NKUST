using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class URL : MonoBehaviour
{
    [Header("Input Group")]
    [SerializeField] InputField Domain_Input;
    [SerializeField] InputField Port_Input;
    static public string Domain = "";
    static public string Port = "";

    [Header("畫面切換")]
    [SerializeField] private GameObject URLConfirm;
    [SerializeField] private GameObject MenuConfirm;
    [Header("保存")]
    public static string tempFilePath;


    void Start()
    {
        LoadTempData();
    }

    public void Leave()
    {
        URLConfirm.SetActive(false);
        MenuConfirm.SetActive(true);
        LoadTempData();
    }


    public void Check()
    {
        Domain = Domain_Input.text.Trim();
        Port = Port_Input.text.Trim();

        Debug.Log($"Domain: {Domain}, Port: {Port}");

        // 可以在此加入基本格式檢查或顯示錯誤訊息
        if (string.IsNullOrEmpty(Domain) || string.IsNullOrEmpty(Port))
        {
            Debug.LogWarning("請輸入有效的網域與埠號！");
            return;
        }
        SaveTempData();
    }
    public void Reset()
    {
        LoadTempData();
    }
    public void LoadTempData()
    {
        tempFilePath = Path.Combine(Application.persistentDataPath, "URL.txt");
        Debug.Log("文件保存路径: " + tempFilePath);

        if (File.Exists(tempFilePath))
        {
            string lastLine = File.ReadAllLines(tempFilePath)[^1];
            string[] parts = lastLine.Split(' ');
            if (parts.Length >= 2)
            {
                Domain = parts[0];
                Port = parts[1];
                Domain_Input.text = Domain;
                Port_Input.text = Port;
            }
        }
        else
        {
            Domain_Input.text = "";
            Port_Input.text = "";
            Domain = "";
            Port = "";
        }
    }
    public void SaveTempData()
    {
        // tempFilePath = Path.Combine(Application.persistentDataPath, "URL.txt");
        // Debug.Log("文件保存路径: " + tempFilePath);

        if (!Directory.Exists(Application.persistentDataPath))
        {
            Debug.LogError("文件夹路径不存在！");
            return;
        }

        string dataLine = $"{Domain} {Port}";
        try
        {
            File.AppendAllText(tempFilePath, dataLine + Environment.NewLine);
            Debug.Log("成功儲存資料至暫存檔！");
        }
        catch (Exception e)
        {
            Debug.LogError("儲存資料失敗: " + e.Message);
        }
    }
    static public string LoadURL(string php)
    {
        string URL = "";
        if (File.Exists(tempFilePath))
        {
            string[] lines = File.ReadAllLines(tempFilePath);
            if (lines.Length > 0)
            {
                string lastLine = lines[^1];  // 取最後一行
                string[] parts = lastLine.Split(' ');
                if (parts.Length >= 2)
                {
                    Domain = parts[0];
                    Port = parts[1];
                    URL = $"http://{Domain}:{Port}/hpds_anne/{php}";
                }
                else
                {
                    Debug.LogError("檔案最後一行格式錯誤，無法取得 Domain 和 Port");
                }
            }
            else
            {
                Debug.LogError("檔案為空");
            }
        }
        else
        {
            Debug.LogError("保存網域Port的txt不在");
        }
        return URL;
    }

}
