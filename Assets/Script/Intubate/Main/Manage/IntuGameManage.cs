using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Input.Visuals;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
public class IntuGameManage : MonoBehaviour
{
    [Header("GameObject")]
    [SerializeField] GameObject menuGameObject;
    [SerializeField] GameObject scoreGameObject;

    [Header("Speak")]
    [SerializeField] Speak1 Speak1;
    [SerializeField] Speak2 Speak2;
    [SerializeField] Speak3 Speak3;

    [Header("Quest")]
    [SerializeField] Quest1 Quest1;
    [SerializeField] Quest2 Quest2;
    [SerializeField] Quest3 Quest3;
    [SerializeField] Quest4 Quest4;
    [SerializeField] Quest5 Quest5;
    [SerializeField] Quest6 Quest6;
    [SerializeField] Quest7 Quest7;
    [SerializeField] Quest8 Quest8;
    [SerializeField] Quest9 Quest9;
    [SerializeField] Quest9_5 Quest9_5;
    [SerializeField] Quest10 Quest10;
    [SerializeField] Quest11 Quest11;
    [SerializeField] Quest12 Quest12;
    [SerializeField] Quest13 Quest13;
    [SerializeField] Quest14 Quest14;
    [SerializeField] Quest15 Quest15;

    [Header("Text")]
    [SerializeField] Text TimeText;
    [SerializeField] Text scoreText;
    [SerializeField] Text titleText;
    [SerializeField] Text scoreTimeText;

    [Header("傳送資料")]
    [SerializeField] private DataUpload DataUpload;
    [SerializeField] private TempScoreHandler TempScoreHandler;
    [SerializeField] private bool hasSentScore = false;
    public float elapsedTime = 0f; // 累积时间
    public int totalScore = 0; // 总分数，累加所有阶段的分数
    public string errorQuestions = "";

    [Header("Close")]
    private Coroutine countdownCoroutine; // 用于跟踪协程
    [SerializeField] private bool isProcessing = false; // 标志位，用于控制方法的执行
    [SerializeField] private int closeCount = 0;  // 记录 Close 方法的调用次数
    [SerializeField] private int countdownTime = 30; // 倒计时时间（秒）

    [Header("其他")]
    [SerializeField] private GameObject process1;
    [SerializeField] private GameObject process2;
    [SerializeField] private GameObject process3;
    [SerializeField] private GameMenu GameMenu;
    [SerializeField] private Camera mainCamera;

    [Header("結束flag")]
    public bool endFlag1;
    public bool endFlag2;
    public bool endFlag3;

    [Header("保留Gameobject")]
    [SerializeField] private GameObject UI_SpeakGameObject;
    [SerializeField] private GameObject intuGameManageGameObject;
    [SerializeField] private GameObject questsGameObject;

    void Awake()
    {
        DontDestroyOnLoad(UI_SpeakGameObject);
        DontDestroyOnLoad(intuGameManageGameObject);
        DontDestroyOnLoad(questsGameObject);

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "Menu") // 替換成你不希望存在的場景
        {
            Destroy(UI_SpeakGameObject);
            Destroy(intuGameManageGameObject);
            Destroy(questsGameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu") // 當進入 "Menu" 場景
        {
            if (UI_SpeakGameObject != null)
                Destroy(UI_SpeakGameObject);
            if (intuGameManageGameObject != null)
                Destroy(intuGameManageGameObject);
            if (questsGameObject != null)
                Destroy(questsGameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void Start()
    {
        // if (!string.IsNullOrEmpty(OfflineLogin.ID))
        //     TempScoreHandler.SaveTempData();
        // else if (!string.IsNullOrEmpty(Login.ID))
        //     DataUpload.SendData();
    }
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.LTouch) || OVRInput.GetDown(OVRInput.Button.Start, OVRInput.Controller.Hands))
        {
            Input();
            GameMenu.ShowMenu();
        }
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>();
        }
    }
    public void Input()
    {
        float distance = .75f; // 距离
        // 将 B 设置在摄像机前方
        menuGameObject.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;
        // 使 B 面向摄像机
        menuGameObject.transform.LookAt(mainCamera.transform);
        menuGameObject.transform.Rotate(0, 180, 0);
        int minutes = (int)(elapsedTime / 60); // 获取分钟
        int seconds = (int)(elapsedTime % 60); // 获取剩余的秒
        TimeText.text = string.Format("訓練時間 {0}:{1:D2}", minutes, seconds); // 秒部分用两位数表示
        if (!menuGameObject.activeSelf)
            menuGameObject.SetActive(true);
        else
            menuGameObject.SetActive(false);
    }
    public void CalculateScore(string title, (int displayNumber, bool errorFlag)[] questions, GameObject process)
    {
        float distance = .75f;
        scoreGameObject.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;
        scoreGameObject.transform.LookAt(mainCamera.transform);
        scoreGameObject.transform.Rotate(0, 180, 0);

        int score = 0;
        string incorrectQuestions = "";
        scoreGameObject.SetActive(true);

        foreach (var q in questions)
        {
            if (!q.errorFlag)
            {
                score++;
            }
            else
            {
                incorrectQuestions += $"{q.displayNumber}, ";
                errorQuestions += $"{q.displayNumber},";
            }
        }

        if (!string.IsNullOrEmpty(incorrectQuestions))
            incorrectQuestions = incorrectQuestions.TrimEnd(',', ' ');

        scoreText.text = $"答對題數: {score}/{questions.Length}";
        titleText.text = title;

        if (!string.IsNullOrEmpty(incorrectQuestions))
            scoreText.text += $"\n答錯題號: {incorrectQuestions}";

        totalScore += score * 10;

        if (countdownCoroutine == null)
            countdownCoroutine = StartCoroutine(CountdownRoutine());

        process.SetActive(false);
    }
    public void Score1()
    {
        CalculateScore("第一部分:問題回答情況", new[]{
        (1, Quest1.errorFlag),
        (2, Quest2.errorFlag),
        (3, Quest3.errorFlag),
        (4, Quest4.errorFlag),
        (5, Quest5.errorFlag),
        (6, Quest6.errorFlag),
        (7, Quest7.errorFlag),
    }, process1);
    }

    public void Score2()
    {
        CalculateScore("第二部分:問題回答情況", new[]{
        (8, Quest8.errorFlag),
        (9, Quest9.errorFlag),
        (10, Quest9_5.errorFlag),
        (11, Quest10.errorFlag),
    }, process2);
    }

    public void Score3()
    {
        CalculateScore("第三部分:問題回答情況", new[]{
        (12, Quest11.errorFlag),
        (13, Quest12.errorFlag),
        (14, Quest13.errorFlag),
        (15, Quest14.errorFlag),
        (16, Quest15.errorFlag),
    }, process3);
    }


    public void Close()
    {
        if (isProcessing)
        {
            // 如果方法正在处理，直接返回
            Debug.LogWarning("Close method is already being processed. Please wait.");
            return;
        }

        // 标记为处理中
        isProcessing = true;
        StopCountdown();
        // 关闭目标对象
        scoreGameObject.SetActive(false);

        switch (closeCount)
        {
            case 0:
                Speak2.enabled = true;
                endFlag1 = true;
                break;
            case 1:
                Speak3.enabled = true;
                endFlag2 = true;
                break;
            case 2:
                if (!hasSentScore)
                {
                    HandleDataOperations();
                    endFlag3 = true;
                    hasSentScore = true; // 标记成绩已发送
                }
                break;
            // 可以继续添加更多的组件
            default:
                Debug.LogWarning("No more components to enable.");
                break;
        }

        // 增加调用次数
        closeCount++;
        // 恢复标志位，允许下一次调用
        isProcessing = false;
    }
    private void HandleDataOperations()
    {
        if (!string.IsNullOrEmpty(OfflineLogin.ID))
        {
            TempScoreHandler.SaveTempData();
        }
        else if (!string.IsNullOrEmpty(Login.ID))
        {
            DataUpload.SendData();
        }
        else
        {
            Debug.LogWarning("No valid ID found. Data operation skipped.");
        }
    }
    public void StopCountdown()
    {
        if (countdownCoroutine != null) // 确保协程正在运行
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null; // 重置协程引用
            Debug.Log("Countdown has been stopped.");
        }
    }
    private IEnumerator CountdownRoutine()
    {
        int timer = countdownTime; // 初始化计时器
        while (timer > 0)
        {
            scoreTimeText.text = $"繼續 ({timer})";
            yield return new WaitForSeconds(1f); // 等待 1 秒
            timer--; // 减少计时器
        }
        Close();
    }
}

