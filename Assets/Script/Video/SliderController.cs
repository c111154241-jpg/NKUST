using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class SliderController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] private GameObject VideosConfirm;//影片Confirm
    [SerializeField] public VideoPlayer video; //給進度條使用
    Slider slider; //進度條長度
    bool slide = false; //判斷是否有按著


    void Start()
    {
        slider = GetComponent<Slider>();

    }

    public void OnPointerDown(PointerEventData x) //按著進度條
    {
        slide = true;
    }
    public void OnPointerUp(PointerEventData x) //放開進度條
    {
        float frame = (float)slider.value * video.frameCount;//進度條的值*影片的總幀數
        video.frame = (long)frame; //影片的當前幀數
        slide = false; //放開進度條

    }
    void Update()
    {
        if (!slide && video.isPlaying) //如果沒有按著進度條且影片正在播放
        {
            slider.value = (float)video.frame / video.frameCount; //進度條的值=影片的當前幀數/影片的總幀數
        }

    }
    public void Button_Exit()
    { //結束影片
        VideosConfirm.SetActive(false);
    }
}
