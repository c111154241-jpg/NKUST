using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Meta.XR.MRUtilityKit;
using System;
using System.Threading;

public class Leader : MonoBehaviour
{
    //獲得線上分數
    // private string apiUrl = "http://fgserver2.ddns.net:2486/hpds_anne/get.php";

    [Header("Text")]
    [SerializeField] private Text[] RankText = new Text[8];
    [SerializeField] private Text[] TimeText = new Text[8];
    [SerializeField] private Text[] ScoreText = new Text[8];
    [SerializeField] private Text[] DateText = new Text[8];
    [SerializeField] private Text[] QuestionsText = new Text[8];
    [SerializeField] private Text LoginText;

    [Header("拉條")]
    [SerializeField] private Slider pageSlider;  // 拖入 UI Slider
    [SerializeField] private Text pageLabel;     // 顯示第幾頁（可選）
    private int totalPages = 0;  // 總頁數
    private bool isSliderUpdating = false; // 防止互相觸發

    [Header("Confirm")]
    [SerializeField] private GameObject LeaderConfirm;
    [SerializeField] private GameObject MenuConfirm;
    private StudentDataList dataList;
    private bool isDataLoaded = false;
    private int count = 0;
    void Start()
    {
        // 初始化子物件
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            For(child);
        }

        // 初始化 RankText, TimeText, ScoreText, DateText
        for (int i = 0; i < RankText.Length; i++)
        {
            RankText[i].text = "None";
            TimeText[i].text = "None";
            ScoreText[i].text = "None";
            DateText[i].text = "None";
            QuestionsText[i].text = "None";
        }
    }
    void Update()
    {
        if (!isDataLoaded)
        {
            if (!string.IsNullOrEmpty(Login.ID))
            {
                // dataList = null;
                LoginText.text = "";
                StartCoroutine(SendLeaderRequest());
                isDataLoaded = true; // 確保資料只載入一次
            }
            else
            {
                LoginText.text = "請登入帳號";
            }
        }
    }

    private void For(Transform child)
    {
        for (int i = 0; i < child.childCount; i++)
        {
            Transform childchild = child.GetChild(i);
            if (child.name == "Rank")
                RankText[i] = childchild.GetComponent<Text>();
            else if (child.name == "Time")
                TimeText[i] = childchild.GetComponent<Text>();
            else if (child.name == "Score")
                ScoreText[i] = childchild.GetComponent<Text>();
            else if (child.name == "Date")
                DateText[i] = childchild.GetComponent<Text>();
            else if (child.name == "Questions")
                QuestionsText[i] = childchild.GetComponent<Text>();
        }
    }
    public void Leave()
    {
        LeaderConfirm.SetActive(false);
        MenuConfirm.SetActive(true);
        isDataLoaded = false;
        for (int i = 0; i < RankText.Length; i++)
        {
            RankText[i].text = "None";
            TimeText[i].text = "None";
            ScoreText[i].text = "None";
            DateText[i].text = "None";
            QuestionsText[i].text = "None";
        }
        pageSlider.value = 0;
        if (pageLabel != null)
            pageLabel.text = "";
    }

    private IEnumerator SendLeaderRequest()
    {
        // 创建请求的参数
        WWWForm form = new WWWForm();
        // form.AddField("student_id", "C111154240");
        form.AddField("student_id", Login.ID);
        form.AddField("password", Login.Pass);

        // 发送 POST 请求
        string php = URL.LoadURL("get.php");
        Debug.Log(php);
        UnityWebRequest www = UnityWebRequest.Post(php, form);
        www.timeout = 10; // 設置超時（以秒為單位）
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            LoginText.text = "連線錯誤：" + www.error;
            Debug.LogError("連線錯誤：" + www.error);
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            LoginText.text = "伺服器回應錯誤：" + www.error;
            Debug.LogError("伺服器回應錯誤：" + www.error);
        }
        else if (www.result != UnityWebRequest.Result.Success)
        {
            LoginText.text = "其他錯誤：" + www.error;
            Debug.LogError("其他錯誤：" + www.error);
        }
        else
        {
            // 处理服务器响应
            string responseText = www.downloadHandler.text;
            Debug.LogError(responseText);
            // 根据服务器的响应更新 UI 或做其他处理
            ParseAndDisplayData(responseText);
        }
    }

    [System.Serializable]
    public class StudentData
    {
        public string student_id;
        public string name;
        public string gender;
        public string score;
        public string answer_time;
        public string answer_date;
        public string incorrect_questions;
    }

    [System.Serializable]
    public class StudentDataList
    {
        public List<StudentData> data;
    }
    void ParseAndDisplayData(string jsonData)
    {
        // 将 JSON 字符串转换为 StudentDataList 对象
        dataList = JsonUtility.FromJson<StudentDataList>(jsonData);
        count = 0; // 初始化为第一页
        DisplayCurrentPageData();
    }

    void DisplayCurrentPageData()
    {
        // 計算總頁數
        totalPages = Mathf.CeilToInt((float)dataList.data.Count / 8);
        if (totalPages <= 0) totalPages = 1;

        // 更新 Slider 範圍與值（避免滑動時觸發 ValueChanged）
        isSliderUpdating = true;
        pageSlider.maxValue = totalPages - 1;
        pageSlider.value = count / 8;
        isSliderUpdating = false;

        if (pageLabel != null)
            pageLabel.text = $"第 {count / 8 + 1} / {totalPages} 頁";

        // 清空
        for (int i = 0; i < RankText.Length; i++)
        {
            RankText[i].text = "None";
            TimeText[i].text = "None";
            ScoreText[i].text = "None";
            DateText[i].text = "None";
            QuestionsText[i].text = "None";
        }

        // 顯示當前頁
        for (int i = count; i < count + 8 && i < dataList.data.Count; i++)
        {
            StudentData student = dataList.data[i];
            RankText[i % 8].text = (i + 1).ToString();
            TimeText[i % 8].text = student.answer_time;
            ScoreText[i % 8].text = student.score;
            DateText[i % 8].text = student.answer_date;
            QuestionsText[i % 8].text = student.incorrect_questions;
        }
    }
    public void up()
    {
        if (count >= 8)
        {
            count -= 8; // 后退到上一页
            DisplayCurrentPageData();
        }
    }

    public void down()
    {
        if (count + 8 < dataList.data.Count)
        {
            count += 8; // 前进到下一页
            DisplayCurrentPageData();
        }
    }
    public void OnPageSliderChanged()
    {
        if (isSliderUpdating) return;
        count = Mathf.RoundToInt(pageSlider.value) * 8;
        DisplayCurrentPageData();
    }

}
