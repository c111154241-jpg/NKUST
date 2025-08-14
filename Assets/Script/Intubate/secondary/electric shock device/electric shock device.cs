using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入 UI 命名空间
public class electricshockdevice : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private Speak1 speak;
    public AudioClip audioClipsNormal;
    public AudioClip audioClipsAbnormal;

    [Header("Image Settings")]
    [SerializeField] private GameObject image;
    [SerializeField] private Image displayImage; // UI 图片组件
    public Sprite p1s4Sprite;
    public Sprite p1s8Sprite;
    public Sprite p3s13Sprite;
    void Start()
    {
        StartCoroutine(AudioPlay());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator AudioPlay()
    {
        // audioSource.clip = audioClipsAbnormal;
        // audioSource.Play();
        // yield return new WaitUntil(() => speak.count == 8);
        // audioSource.clip = audioClipsNormal;
        // audioSource.Play();
        // yield return new WaitUntil(() => speak.count == 16);
        // audioSource.clip = audioClipsAbnormal;
        // audioSource.Play();
        yield return new WaitUntil(() => speak.count == 8);
        image.SetActive(true);
        displayImage.sprite = p1s4Sprite;
        audioSource.clip = audioClipsAbnormal;
        audioSource.Play();
        yield return new WaitUntil(() => speak.count == 16);
        displayImage.sprite = p1s8Sprite;

    }
}
