using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Speak3 : MonoBehaviour
{
    [SerializeField] private bool testFlag;
    [SerializeField] IntuGameManage intuGameManage;
    [SerializeField] private Text showText;
    [SerializeField] private GameObject hiteTextGameObject;
    [SerializeField] private Text hiteText;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    public List<AudioClip> audioClips;
    public int count;

    [Header("Quest Gameobject")]
    [SerializeField] private GameObject quest11GameObject;
    [SerializeField] private GameObject quest12GameObject;
    [SerializeField] private GameObject quest13GameObject;
    [SerializeField] private GameObject quest14GameObject;
    [SerializeField] private GameObject quest15GameObject;

    [Header("Quest")]
    [SerializeField] private Quest11 quest11;
    [SerializeField] private Quest12 quest12;
    [SerializeField] private Quest13 quest13;
    [SerializeField] private Quest14 quest14;
    [SerializeField] private Quest15 quest15;

    [Header("淡出")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject fadeGameObject;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 3f;
    [SerializeField] private float distance = .5f;

    [Header("Other Quest")]
    [SerializeField] private Phone phone;
    [SerializeField] private Phone phoneElevator;
    [SerializeField] private oxygencylinder oxygencylinderA;
    [SerializeField] private oxygencylinder oxygencylinderB;

    [Header("Other UI GameObject")]
    [SerializeField] private GameObject level;

    private List<string> dialogues = new List<string>
    {
        "醫師: 我已經聯絡好加護病房，請準備病患轉送",//0
        "護理師: 請確認病人輸送等級",//1
        "醫師: 輸送等級應為第三級，使用呼吸器",//2
        "護理師: 輸送等級確認三級，使用呼吸器",//3
        "醫師: 正確，請遵照此標準進行輸送",//4
        "護理師: 電話聯繫輸送人員",//5
        "護理師: 準備完成，確認護送人員安排",//6
        "備卡提示：請準備足夠的氧氣",//7
        "緊急情境：輸送過程中發現氧氣不足。(指針再一半不動，沒有氧氣-因為指針壞了) ",//8
        "護理師: 已聯絡臨近單位，請求支援，請立刻協助補充氧氣",//9
        "臨近單位醫護人員： 收到，馬上支援" //10
    };

    void Start()
    {
        audioSource.Stop();
        StartCoroutine(DisplayDialogue());
    }
    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (mainCamera == null)
        {
            mainCamera = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>();
        }
        if (sceneName == "Elevator" && phoneElevator == null)
        {
            phoneElevator = GameObject.Find("按鈕").GetComponent<Phone>();
        }
    }
    private IEnumerator DisplayDialogue()
    {
        if (!testFlag)
        {
            for (int i = 0; i < dialogues.Count; i++)
            {
                count = i;
                if (i == 2)
                {
                    quest11GameObject.SetActive(true);
                    level.SetActive(true);
                    showText.text = "請查看身旁的輸送等級表後回答題目";
                    yield return new WaitUntil(() => quest11.flag);
                    quest11GameObject.SetActive(false);
                    level.SetActive(false);
                    // yield return StartCoroutine(FadeOutAndSwitchScene("Elevator")); //測試
                }
                else if (i == 4)
                {
                    quest12GameObject.SetActive(true);
                    showText.text = "請先回答題目";
                    yield return new WaitUntil(() => quest12.flag);
                    quest12GameObject.SetActive(false);
                }
                else if (i == 6)
                {
                    showText.text = "請撥打電話，電話位於左方桌子上";
                    hiteText.text = "請拿起話筒";
                    hiteTextGameObject.SetActive(true);
                    phone.flag = false;
                    yield return new WaitUntil(() => phone.flag);
                }
                else if (i == 8)
                {
                    quest13GameObject.SetActive(true);
                    showText.text = "請先回答題目";
                    hiteText.text = "請先回答題目";
                    yield return new WaitUntil(() => quest13.flag);
                    quest13GameObject.SetActive(false);
                    showText.text = "請去急救車旁拿取氧氣鋼瓶，並懸掛在床尾";
                    hiteText.text = "請去急救車旁拿取氧氣鋼瓶，並懸掛在床尾";
                    yield return new WaitUntil(() => oxygencylinderA.flag && oxygencylinderB.flag);
                    yield return StartCoroutine(FadeOutAndSwitchScene("Elevator"));
                    hiteTextGameObject.SetActive(false);
                }
                else if (i == 9)
                {
                    quest14GameObject.SetActive(true);
                    yield return new WaitUntil(() => quest14.flag);
                    quest14GameObject.SetActive(false);
                    quest15GameObject.SetActive(true);
                    yield return new WaitUntil(() => quest15.flag);
                    quest15GameObject.SetActive(false);
                    hiteTextGameObject.SetActive(true);
                    showText.text = "請使用電梯對講機，聯繫臨近單位";
                    hiteText.text = "請按壓對講機按鈕";
                    phoneElevator.flag = false;
                    yield return new WaitUntil(() => phoneElevator.flag);
                }
                if (i < audioClips.Count)
                {
                    if (i >= 6 && i <= 7)
                    {
                        showText.text = dialogues[i];
                        hiteText.text = dialogues[i];
                    }
                    else if (i <= 8)
                    {
                        showText.text = dialogues[i];
                        hiteText.text = "";
                    }
                    else
                    {
                        showText.text = "";
                        hiteText.text = dialogues[i];
                    }

                    yield return new WaitForSeconds(.5f);
                    audioSource.clip = audioClips[i];
                    audioSource.Play();
                    yield return new WaitForSeconds(audioClips[i].length); // 等待音效播放完成
                    yield return new WaitForSeconds(.5f); //，默認等待2秒
                }
                else
                {
                    yield return new WaitForSeconds(2f); // 若無音效，默認等待2秒
                }
            }
            showText.text = "等待電梯門打開，把病床拉到急救室"; // 清空對話文本
            intuGameManage.Score3();
        }
        else
        {
            intuGameManage.Score3();
        }
    }
    private IEnumerator FadeOutAndSwitchScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        float distance = .4f;
        fadeGameObject.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;
        fadeGameObject.transform.LookAt(mainCamera.transform);
        fadeGameObject.transform.Rotate(0, 180, 0);
        float time = 0;
        Color color = fadeImage.color;
        fadeGameObject.SetActive(true);
        while (time < fadeDuration)
        {
            fadeGameObject.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;
            fadeGameObject.transform.LookAt(mainCamera.transform);
            fadeGameObject.transform.Rotate(0, 180, 0);
            time += Time.deltaTime;
            color.a = Mathf.Lerp(0, 1, time / fadeDuration);
            fadeImage.color = color;
            if (color.a > 0.97f && !asyncOperation.allowSceneActivation)
            {
                asyncOperation.allowSceneActivation = true; // 啟動場景
            }
            yield return null;
        }
        color.a = 1;
        fadeImage.color = color;

        // 若場景仍未啟動，強制啟動
        if (!asyncOperation.allowSceneActivation)
        {
            asyncOperation.allowSceneActivation = true; // 確保場景啟動
        }
        while (!asyncOperation.isDone)
        {
            yield return null;  // 等待直到場景完全加載並啟動
        }
        fadeGameObject.SetActive(false);
    }
}
