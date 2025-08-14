using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class OfflineLeader : MonoBehaviour
{
    //獲得本地分數
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
    [SerializeField] private GameObject OfflineLeaderConfirm;
    [SerializeField] private GameObject MenuConfirm;
    private StudentDataList dataList = new StudentDataList();
    private int count = 0;
    private bool isDataLoaded = false;

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
            if (!string.IsNullOrEmpty(OfflineLogin.ID))
            {
                LoginText.text = "";
                LoadData();
                isDataLoaded = true; // 確保資料只載入一次
            }
            else
            {
                LoginText.text = "請登入帳號";
            }
        }
    }

    void LoadData()
    {
        dataList.data.Clear(); // <<<<<< 加這行，清除舊資料
        // 檔案路徑
        string tempFilePath = Path.Combine(Application.persistentDataPath, "temp.txt");

        // 檢查檔案是否存在
        if (File.Exists(tempFilePath))
        {
            // 讀取檔案的所有行
            string[] lines = File.ReadAllLines(tempFilePath);

            // 解析每行數據並存入列表
            foreach (string line in lines)
            {
                string[] parts = line.Split(' ');

                if (parts.Length >= 5)
                {
                    try
                    {
                        string studentId = parts[0];

                        // 僅處理 student_id 與 OfflineLogin.ID 一樣的數據
                        if (studentId == OfflineLogin.ID)
                        {
                            StudentData student = new StudentData
                            {
                                student_id = studentId,
                                score = parts[1],
                                answer_time = parts[2],
                                answer_date = parts[3],
                                incorrect_questions = parts[4],
                            };
                            student.answer_date = student.answer_date.Replace('T', ' ');
                            dataList.data.Add(student);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogWarning($"解析行出錯: {line}, 錯誤: {ex.Message}");
                    }
                }
                else
                {
                    Debug.LogWarning($"行格式不正確: {line}");
                }
            }

            // 顯示第一頁數據
            DisplayCurrentPageData();
        }
        else
        {
            Debug.LogWarning("temp.txt 文件不存在！");
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
        OfflineLeaderConfirm.SetActive(false);
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

    [System.Serializable]
    public class StudentData
    {
        public string student_id;
        public string score;
        public string answer_time;
        public string answer_date;
        public string incorrect_questions;
    }

    public class StudentDataList
    {
        public List<StudentData> data = new List<StudentData>();
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
            count -= 8; // 回到上一頁
            DisplayCurrentPageData();
        }
    }

    public void down()
    {
        if (count + 8 < dataList.data.Count)
        {
            count += 8; // 前進到下一頁
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
