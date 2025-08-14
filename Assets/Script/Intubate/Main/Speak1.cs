using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class Speak1 : MonoBehaviour
{
    [SerializeField] private bool testFlag;
    [SerializeField] IntuGameManage intuGameManage;
    [SerializeField] private Text showText;
    [SerializeField] private Text hiteText;
    [SerializeField] private GameObject hiteTextGameObject;

    [Header("Hunman")]
    [SerializeField] private GameObject nudeHunman;
    [SerializeField] private GameObject mainHunmanA;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    public List<AudioClip> audioClips;
    public int count;

    [Header("Quest Gameobject")]
    [SerializeField] private GameObject quest1GameObject;
    [SerializeField] private GameObject quest2GameObject;
    [SerializeField] private GameObject quest3GameObject;
    [SerializeField] private GameObject quest4GameObject;
    [SerializeField] private GameObject quest5GameObject;
    [SerializeField] private GameObject quest6GameObject;
    [SerializeField] private GameObject quest7GameObject;

    [Header("Quest")]
    [SerializeField] private Quest1 quest1;
    [SerializeField] private Quest2 quest2;
    [SerializeField] private Quest3 quest3;
    [SerializeField] private Quest4 quest4;
    [SerializeField] private Quest5 quest5;
    [SerializeField] private Quest6 quest6;
    [SerializeField] private Quest7 quest7;


    [Header("Other Quest")]
    [SerializeField] private Ontology ontology;
    [SerializeField] private NRMAnimator NRMAnimator;
    [SerializeField] private Bracelet bracelet;
    [SerializeField] private HunmanA hunmanA;
    [SerializeField] private Phone phone;
    [SerializeField] private Stethoscope stethoscopeHand;
    [SerializeField] private Stethoscope stethoscopeControl;

    [Space(10)]
    public bool EKGFlag;
    public bool NRMFlag;
    public bool OxygenFlag;

    private List<string> dialogues = new List<string>
    {
        "護理師: 請問你叫什麼名字",//0
        "病人: 我叫黃美麗",//1
        "護理師: 請問你的出生年月日",//2
        "病人: 民國61年",//3
        "護理師: 請問你哪裡不舒服",//4
        "病人: 我好喘喔，好像吸不到氣",//5
        "護理師: 我來偵測生命跡象",//6
        "護理師: 呼吸喘是甚麼時候開始的",//7
        "病人: 3個小時前發生",//8
        "護理師: 我來幫你用聽診器聽診",//9
        "護理師: 醫師，我是病房護理師{Login.Name}",//10
        "護理師: 黃美麗小姐，51歲，昨天因車禍入院",//11
        "護理師: 現在主訴喘不舒服，呼吸次數為每分鐘40次 ，SpO2: 84 %",//12
        "護理師: 可以請你出來評估嗎?",//13
        "醫師: 好，請你先準備NRM O2: full，我立刻過去",//14
        "護理師: 你還好嗎",//15
        "病人: 我還是不舒服",//16
        "護理師: 醫師，我是病房護理師{Login.Name}",//17
        "護理師: 剛剛黃美麗小姐，51歲",//18
        "護理師: 在NRM O2: full，10分鐘，呼吸次數為每分鐘42次，SpO2: 82 %",//19
        "護理師: 病人呼吸喘的情況沒有改善",//20
        "醫師: 推急救車，準備插管"//21
    };

    void Start()
    {
        if (!string.IsNullOrWhiteSpace(Login.Name))
        {
            dialogues = dialogues.Select(d => d.Replace("{Login.Name}", Login.Name)).ToList();
            //Debug.LogError(dialogues[10]);
        }
        else
        {
            dialogues = dialogues.Select(d => d.Replace("{Login.Name}", "")).ToList();
            //Debug.LogError(dialogues[10]);
        }

        audioSource.Stop();
        StartCoroutine(DisplayDialogue());
    }

    private IEnumerator DisplayDialogue()
    {
        if (!testFlag)
        {
            for (int i = 0; i < dialogues.Count; i++)
            {
                count = i;

                if (i == 0)
                {
                    quest1GameObject.SetActive(true);
                    showText.text = "請先回答題目";
                    yield return new WaitUntil(() => quest1.flag);
                    quest1GameObject.SetActive(false);
                }
                else if (i == 4)
                {
                    showText.text = "等待病人舉起手後觸摸手環查看訊息";
                    yield return new WaitUntil(() => bracelet.flag);
                }
                else if (i == 7)
                {
                    quest2GameObject.SetActive(true);
                    showText.text = "請先貼上EKG貼片";
                    // mainHunman.SetActive(false);
                    mainHunmanA.SetActive(false);
                    nudeHunman.SetActive(true);
                    yield return new WaitUntil(() => quest2.flag);
                    quest2GameObject.SetActive(false);
                    EKGFlag = true;
                    // mainHunman.SetActive(true);
                    mainHunmanA.SetActive(true);
                    nudeHunman.SetActive(false);
                }
                else if (i == 10)
                {
                    // quest3GameObject.SetActive(true); 到(Ontology)聽診器拿起後才出現
                    showText.text = "請拿起聽診器聽診，聽診器位於左方桌子上";
                    yield return new WaitUntil(() => quest3.flag);
                    quest3GameObject.SetActive(false);
                    quest4GameObject.SetActive(true);
                    hiteTextGameObject.SetActive(true);
                    ontology.handStethoscope.SetActive(false);
                    ontology.controlStethoscope.SetActive(false);
                    stethoscopeHand.enabled = false;
                    stethoscopeControl.enabled = false;
                    showText.text = "請撥打電話，電話位於左方桌子上";
                    hiteText.text = "請先回答題目後撥打電話";
                    yield return new WaitUntil(() => quest4.flag);
                    hiteText.text = "請拿起話筒";
                    quest4GameObject.SetActive(false);
                    phone.flag = false;
                    yield return new WaitUntil(() => phone.flag);
                }
                else if (i == 15)
                {
                    quest5GameObject.SetActive(true);
                    hiteText.text = "請轉身回答題目";
                    showText.text = "請先回答題目";
                    yield return new WaitUntil(() => quest5.flag);
                    showText.text = "";
                    quest5GameObject.SetActive(false);
                    quest6GameObject.SetActive(true);
                    yield return new WaitUntil(() => quest6.flag);
                    NRMFlag = true;
                    OxygenFlag = true;
                    quest6GameObject.SetActive(false);
                    yield return new WaitForSeconds(2f); //，默認等待2秒
                }
                else if (i == 17)
                {
                    quest7GameObject.SetActive(true);
                    showText.text = "請撥打電話，電話位於左方桌子上";
                    hiteText.text = "請先回答題目後撥打電話";
                    yield return new WaitUntil(() => quest7.flag);
                    hiteText.text = "請拿起話筒";
                    quest7GameObject.SetActive(false);
                    phone.flag = false;
                    yield return new WaitUntil(() => phone.flag);
                }
                if (i < audioClips.Count)
                {
                    if (i >= 10 && i <= 14)
                    {
                        hiteText.text = dialogues[i];
                        showText.text = "";
                    }
                    else if (i >= 17 && i <= 21)
                    {
                        hiteText.text = dialogues[i];
                        showText.text = "";
                    }
                    else
                    {
                        hiteText.text = "";
                        showText.text = dialogues[i];
                    }
                    yield return new WaitForSeconds(.5f);
                    // yield return new WaitUntil(() => !hunman.flag);
                    yield return new WaitUntil(() => !hunmanA.flag);
                    audioSource.clip = audioClips[i];
                    audioSource.Play();
                    yield return WaitForAudioToFinish(audioSource);
                    yield return new WaitForSeconds(.5f); //，默認等待2秒
                }
                else
                {
                    yield return new WaitForSeconds(2f); // 若無音效，默認等待2秒
                }
            }
            showText.text = ""; // 清空對話文本 
            hiteText.text = "";
            intuGameManage.Score1();
        }
        else
        {
            intuGameManage.Score1();
        }
    }
    private IEnumerator WaitForAudioToFinish(AudioSource audioSource)
    {
        // 等待直到音效完全播放結束，包括處理暫停狀態
        while (audioSource.isPlaying || audioSource.time > 0)
        {
            if (!audioSource.isPlaying && audioSource.time > 0) // 如果暫停，等待下一幀
                yield return null;
            else
                yield return null;  // 繼續等音效播放結束
        }
    }

}
