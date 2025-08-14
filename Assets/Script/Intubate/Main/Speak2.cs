using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Video;
using Unity.VisualScripting.Antlr3.Runtime;
public class Speak2 : MonoBehaviour
{
    [SerializeField] private bool testFlag;
    [SerializeField] IntuGameManage intuGameManage;
    [SerializeField] private GameObject showTextGameObject;
    [SerializeField] private Text showText;
    [SerializeField] private GameObject showTextGameObjectQ10;
    [SerializeField] private Text showTextQ10;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    public List<AudioClip> audioClips;
    public int count;

    [Header("Quest Gameobject")]
    [SerializeField] private GameObject quest8GameObject;
    [SerializeField] private GameObject quest9GameObject;
    [SerializeField] private GameObject quest9_5GameObject;
    [SerializeField] private GameObject quest10GameObject;


    [Header("Quest")]
    [SerializeField] private Quest8 quest8;
    [SerializeField] private Quest9 quest9;
    [SerializeField] private Quest9_5 quest9_5;
    [SerializeField] private Quest10 quest10;

    [Header("Other Quest")]
    [SerializeField] private Laryngoscope laryngoscope;
    [SerializeField] private InflatableAnimation InflatableAnimation;//插管充氣


    [Header("Video(喉頭)")]
    [SerializeField] private List<VideoClip> videoClips;  // 存放所有影片
    [SerializeField] private VideoPlayer videoPlayer;  // VideoPlayer 物件
    [SerializeField] private GameObject videoScreen;
    [SerializeField] private Video videoControl;

    [Header("Video(插管)")]
    [SerializeField] private GameObject intubationVideoGameObject;
    [SerializeField] private VideoPlayer intubationVideoPlayer;

    [Header("車上的氣管")]
    [SerializeField] private GameObject inflatable;
    [SerializeField] private GameObject Cube;//懶得改ISDK_HandGrabInteraction的參數，所以用3個，之後的人把下面3個放在同1個父物件裡
    [SerializeField] private GameObject StartCube;
    [SerializeField] private GameObject EndCube;

    [Header("放入的氣管")]
    [SerializeField] private GameObject pullInflatable;

    [Header("Other GameObject")]

    [SerializeField] private GameObject laryngoscopeHandle;
    [SerializeField] private GameObject NRM;
    [SerializeField] private GameObject catheterTip;

    [Header("淡出")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject fadeGameObject;
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 3f;


    private List<string> dialogues = new List<string>
    {
        "護理師: 我要準備插管器材",//0
        "護理師: 選擇合適的插管尺寸",//1
        "醫師: 7.5號",//2
        "護理師: 我要測試Endo cuff是否漏氣",//3
        "護理師: 將通條插入氣管內管",//4
        "護理師: 確認喉頭葉長度及亮度",//5
        "醫師: 7.5號，請正確備妥器材",//6
        "護理師: 是，準備完成",//7
        "醫師: 我on好了",//8
        "護理師: 我拔除通條",//9
        "醫師: 我來確認位置，位置正確，固定在22公分",//10
        "護理師: 我來固定氣管內管，醫師你幫我壓ambu",//11
    };

    void Start()
    {
        audioSource.Stop();
        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        if (!testFlag)
        {
            showTextGameObject.SetActive(true);
            for (int i = 0; i < dialogues.Count; i++)
            {
                count = i;
                if (i == 4)
                {
                    quest8GameObject.SetActive(true);
                    showText.text = "拿起空針打氣至cuff鼓起來，並用手壓cuff，檢查漏氣";
                    yield return new WaitUntil(() => InflatableAnimation.flag);
                }
                else if (i == 5)
                {
                    showText.text = "請將通條從無菌袋裡拉出來，並將通條放置氣管內管";
                    yield return new WaitUntil(() => quest8.flag);
                    quest8GameObject.SetActive(false);
                    Cube.SetActive(false);
                    StartCube.SetActive(false);
                    EndCube.SetActive(false);
                }
                else if (i == 6)
                {
                    showText.text = "組裝喉頭葉，喉頭葉在第3層";
                    yield return new WaitUntil(() => laryngoscope.flag);

                    // 隨機選擇並播放影片
                    int token = Random.Range(0, videoClips.Count);
                    quest9.SelectedClip(token);
                    Debug.Log(token);
                    VideoClip selectedClip = videoClips[token];
                    videoPlayer.clip = selectedClip;
                    videoScreen.SetActive(true); // 啟動隨機選中的影片
                    videoControl.movie = videoScreen;
                    quest9.video = videoScreen;
                    showText.text = "觀看影片，選擇喉頭葉的狀況";
                    yield return new WaitUntil(() => quest9.flag);
                    videoScreen.SetActive(false); // 關閉影片
                    quest9GameObject.SetActive(false);
                }
                else if (i == 8)
                {
                    pullInflatable.SetActive(true);
                    NRM.SetActive(false);
                    catheterTip.SetActive(false);
                    showText.text = "";
                    inflatable.SetActive(false);
                    laryngoscopeHandle.SetActive(false);
                    showText.text = "轉身觀看影片，排序插管流程";
                    yield return StartCoroutine(OnVideoFinished());
                    quest9_5GameObject.SetActive(true);
                    yield return new WaitUntil(() => quest9_5.flag);
                    quest9_5GameObject.SetActive(false);

                    // pullInflatable.SetActive(true);
                    // NRM.SetActive(false);
                    // catheterTip.SetActive(false);
                    // showText.text = "";
                    // inflatable.SetActive(false);
                    // laryngoscopeHandle.SetActive(false);
                }
                else if (i == 10)
                {
                    quest10GameObject.SetActive(true);
                    showTextQ10.text = "請回答問題";
                    yield return new WaitUntil(() => quest10.flag);
                    quest10GameObject.SetActive(false);
                }
                if (i < audioClips.Count)
                {
                    if (i >= 8)
                    {
                        showTextQ10.text = dialogues[i];
                        showText.text = "請到病床邊";
                    }
                    else
                    {
                        showText.text = dialogues[i];
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
            showText.text = ""; // 清空對話文本
            showTextQ10.text = ""; // 清空對話文本
            intuGameManage.Score2();
        }
        else
        {
            intuGameManage.Score2();
        }
    }
    private IEnumerator OnVideoFinished()
    {
        intubationVideoGameObject.SetActive(true);      // 顯示影片畫面
        intubationVideoPlayer.Play();                   // 播放影片

        // 等待影片播放完成
        yield return new WaitUntil(() =>
        !intubationVideoPlayer.isPlaying && intubationVideoPlayer.time > 0
        );

        intubationVideoGameObject.SetActive(false);     // 關閉影片畫面
    }

    // private IEnumerator FadeOut()
    // {
    //     float distance = .4f;
    //     fadeGameObject.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;
    //     fadeGameObject.transform.LookAt(mainCamera.transform);
    //     fadeGameObject.transform.Rotate(0, 180, 0);
    //     float time = 0;
    //     Color color = fadeImage.color;
    //     fadeGameObject.SetActive(true);
    //     while (time < fadeDuration)
    //     {
    //         fadeGameObject.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;
    //         fadeGameObject.transform.LookAt(mainCamera.transform);
    //         fadeGameObject.transform.Rotate(0, 180, 0);
    //         time += Time.deltaTime;
    //         color.a = Mathf.Lerp(0, 1, time / fadeDuration);
    //         fadeImage.color = color;
    //         yield return null;
    //     }
    //     color.a = 1;
    //     fadeImage.color = color;
    //     fadeGameObject.SetActive(false);
    // }
}
