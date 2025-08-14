using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
public class Video : MonoBehaviour
{
    public GameObject movie;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject quest9;
    void Start()
    {
        // 確保 VideoPlayer 存在
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd; // 當影片播完時觸發
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("影片播放完畢，切換回 Quest9");
        movie.SetActive(false); // 關閉影片物件
        quest9.SetActive(true); // 開啟 Quest9 物件
    }
}
