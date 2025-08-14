
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{

    [Header("Confirm")]
    [SerializeField] private GameObject LoginConfirm;
    [SerializeField] private GameObject MenuConfirm;
    [SerializeField] private GameObject LeaderConfirm;
    [SerializeField] private GameObject URLConfirm;
    [SerializeField] private GameObject OfflineLeaderConfirm;
    [SerializeField] private GameObject OfflineLoginConfirm;

    [Header("Button")]
    [SerializeField] private Button LoginButton;
    [SerializeField] private Button LeaderButton;
    private Text LoginButtonText;
    private Text LeaderButtonText;

    [Header("離線")]
    [SerializeField] private Toggle Offline;
    [SerializeField] static bool flagOffline = false;

    [Header("其他")]
    [SerializeField] private Button start;
    [SerializeField] private Text LoginUserText;
    [SerializeField] private GameObject LoginInButton;
    [SerializeField] bool flag = true;
    [SerializeField] bool intubateFlag = false;
    void Start()
    {
        flag = true;
        LoginButtonText = LoginButton.GetComponentInChildren<Text>();
        LeaderButtonText = LeaderButton.GetComponentInChildren<Text>();
        TimeZoneInfo localTimeZone = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time"); // 更換為你所在地的時區ID
        DateTime localTime = TimeZoneInfo.ConvertTime(DateTime.Now, localTimeZone);
        Debug.Log(OfflineLogin.ID);
        if (!string.IsNullOrEmpty(OfflineLogin.ID))
            flagOffline = true;
        else
            flagOffline = false;
    }
    void Update()
    {
        if ((!string.IsNullOrEmpty(Login.ID) || !string.IsNullOrEmpty(OfflineLogin.ID)) && flag)
        {
            flag = false;
            // 根据 Login.ID 或 OfflineLogin.ID 设置文本
            LoginUserText.text = !string.IsNullOrEmpty(Login.ID) ? Login.ID : OfflineLogin.ID;
            // 隐藏登录按钮
            LoginInButton.SetActive(false);
        }

        // 使用相同条件来设置 start 按钮的交互性
        bool isLoginAvailable = !string.IsNullOrEmpty(Login.ID) || !string.IsNullOrEmpty(OfflineLogin.ID);
        start.interactable = isLoginAvailable;
        if (!string.IsNullOrEmpty(Login.ID))
        {
            flagOffline = false;
            Offline.interactable = false;
        }
        else
        {
            Offline.interactable = true;
        }
        if (flagOffline)
        {
            Offline.isOn = true;
            LoginButtonText.text = "登入(離線)";
            LoginButtonText.fontSize = 48;
            LeaderButtonText.text = "成績查詢(離線)";
            LoginButton.onClick.AddListener(Button_OfflineLoginIn);
            LoginButton.onClick.RemoveListener(Button_LoginIn);
            LeaderButton.onClick.AddListener(Button_OfflineLeader);
            LeaderButton.onClick.RemoveListener(Button_Leader);
        }
        else
        {
            Offline.isOn = false;
            LoginButtonText.text = "登入";
            LoginButtonText.fontSize = 96;
            LeaderButtonText.text = "成績查詢";
            LoginButton.onClick.AddListener(Button_LoginIn);
            LoginButton.onClick.RemoveListener(Button_OfflineLoginIn);
            LeaderButton.onClick.AddListener(Button_Leader);
            LeaderButton.onClick.RemoveListener(Button_OfflineLeader);
        }
    }
    public void Button_Start()
    {
        if (!intubateFlag)
        {
            intubateFlag = true;
            StartCoroutine(LoadSceneAsync("Intubate"));
        }
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while (!operation.isDone)
        {
            yield return null; // 等待下一帧
        }
    }


    public void Button_LoginIn()
    {
        MenuConfirm.SetActive(false);
        LoginConfirm.SetActive(true);
    }
    public void Button_OfflineLoginIn()
    {
        MenuConfirm.SetActive(false);
        OfflineLoginConfirm.SetActive(true);
    }
    public void Button_Leader()
    {
        MenuConfirm.SetActive(false);
        LeaderConfirm.SetActive(true);
    }
    public void Button_OfflineLeader()
    {
        MenuConfirm.SetActive(false);
        OfflineLeaderConfirm.SetActive(true);
    }
    public void Button_URL()
    {
        MenuConfirm.SetActive(false);
        URLConfirm.SetActive(true);
    }
    public void Button_LoginOut()
    {
        LoginUserText.text = "用戶:";
        LoginInButton.SetActive(true);
        Login.ID = "";
        Login.Pass = "";
        OfflineLogin.ID = "";
        OfflineLogin.Pass = "";
        flag = true;
    }
    public void CheckToggle()
    {

        if (Offline != null && Offline.isOn)
        {
            flagOffline = true;
        }
        else if (Offline != null && !Offline.isOn)
        {
            Button_LoginOut();
            flagOffline = false;
        }
    }

}
