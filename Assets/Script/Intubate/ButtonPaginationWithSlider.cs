using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Video;

public class ButtonPageController : MonoBehaviour
{
    [Header("UI")]
    public GameObject buttonPrefab;      // 預製按鈕（含 Text 和 Button）
    public Transform buttonContainer;    // 按鈕要放在哪裡（Panel）
    public Slider pageSlider;            // 控制頁數
    public GameObject RawText;
    [Header("按鈕")]
    int totalButtons;       // 總共資料筆數（例如你有20個功能）
    int buttonsPerPage = 5;      // 每頁顯示5個按鈕
    [SerializeField] float posY;
    private List<GameObject> buttonPool = new List<GameObject>(); // 畫面上的5個按鈕
    private List<string> allButtonLabels = new List<string> {
        "退出遊戲",
        "重置視角",
        "開啟選單",
        "重置物件位置",
        "取消排序題答案",
        "插管長度調整",
        "流量計鋼球控制"
    };
    [Header("影片")]
    public VideoPlayer videoPlayer;  // Unity 裡的 VideoPlayer 組件
    public List<VideoClip> videoClips = new List<VideoClip>();  // 跟按鈕一一對應的影片


    void Start()
    {
        totalButtons = allButtonLabels.Count;
        // 創建固定數量的按鈕
        for (int i = 0; i < buttonsPerPage; i++)
        {
            GameObject btn = Instantiate(buttonPrefab, buttonContainer);
            btn.transform.localPosition = new Vector3(0, -i * posY, 0);
            buttonPool.Add(btn);
        }

        // 設定滑條
        pageSlider.minValue = 0;
        pageSlider.maxValue = Mathf.Max(0, Mathf.CeilToInt((float)totalButtons / buttonsPerPage) - 1);
        pageSlider.wholeNumbers = true;
        pageSlider.onValueChanged.AddListener(OnPageChanged);

        // 顯示第一頁
        ShowPage(0);
    }


    void OnPageChanged(float page)
    {
        ShowPage((int)page);
    }

    void ShowPage(int pageIndex)
    {
        int startIndex = pageIndex * buttonsPerPage;

        for (int i = 0; i < buttonsPerPage; i++)
        {
            int dataIndex = startIndex + i;

            if (dataIndex < allButtonLabels.Count)
            {
                GameObject btn = buttonPool[i];
                btn.SetActive(true);

                // 設定按鈕文字
                btn.GetComponentInChildren<Text>().text = allButtonLabels[dataIndex];

                // 清除舊事件再加新事件
                Button buttonComponent = btn.GetComponent<Button>();
                buttonComponent.onClick.RemoveAllListeners();

                int capturedIndex = dataIndex;
                buttonComponent.onClick.AddListener(() => OnButtonClicked(capturedIndex));
            }
            else
            {
                // 如果按鈕不需要顯示（超過資料數量），則隱藏
                buttonPool[i].SetActive(false);
            }
        }

        // 如果最後一頁，按鈕數量不夠，隱藏多餘的按鈕
        if ((pageIndex + 1) * buttonsPerPage > allButtonLabels.Count)
        {
            int extraButtons = (pageIndex + 1) * buttonsPerPage - allButtonLabels.Count;

            // 隱藏超過的按鈕
            for (int i = 0; i < extraButtons; i++)
            {
                buttonPool[buttonsPerPage - 1 - i].SetActive(false);  // 隱藏後面的按鈕
            }
        }
    }

    void OnButtonClicked(int index)
    {
        RawText.SetActive(true);
        if (index < videoClips.Count && videoClips[index] != null)
        {
            videoPlayer.clip = videoClips[index];
            videoPlayer.Play();
        }
        else
        {
            Debug.LogWarning("找不到對應影片");
        }
    }

}
