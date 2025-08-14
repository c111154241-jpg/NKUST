using UnityEngine;
using UnityEngine.UI;
using Meta.WitAi;
using Meta.WitAi.Events;
using Oculus.Voice;
using Meta.WitAi.Lib;
using System;

public class VoiceRecognition : MonoBehaviour
{
    public AppVoiceExperience appVoiceExperience;
    public Text transcriptionText; // 用于显示转录文本的UI文本组件
    private bool voiceCommandReady;
    public Button voiceButton;
    public UnityAndGeminiV2 UnityAndGeminiV2;
    [SerializeField] private Mic mic;

    private void Start()
    {
        // 初始化麦克风
        InitializeMicrophone();
        voiceCommandReady = true;
        // 订阅语音事件
        appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener(OnPartialTranscription);
        appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(OnFullTranscription);
        appVoiceExperience.VoiceEvents.OnStartListening.AddListener(OnStartListening);
        appVoiceExperience.VoiceEvents.OnStoppedListening.AddListener(OnStoppedListening);
        GameObject targetObject = GameObject.Find("AudioBuffer");
        mic = targetObject.GetComponent<Mic>();
        mic.ChangeMicDevice(0);
    }

    private void InitializeMicrophone()
    {
        if (Microphone.devices.Length > 0)
        {
            Debug.LogWarning("Number of microphones: " + Microphone.devices.Length);
            string selectedMic = Microphone.devices[0]; // 选择第一个麦克风
            Debug.LogWarning("Using microphone: " + selectedMic);
            transcriptionText.text = "";
            foreach (string device in Microphone.devices)
            {
                transcriptionText.text += device + "\n";
            }

            appVoiceExperience.Activate();
        }
        else
        {
            Debug.LogWarning("No microphone found.");
        }
    }

    private void OnPartialTranscription(string transcription)
    {
        if (!voiceCommandReady) return;
        transcriptionText.text = transcription; // 显示部分转录文本
    }

    private void OnFullTranscription(string transcription)
    {
        if (!voiceCommandReady) return;
        transcriptionText.text = transcription; // 显示完整转录文本
        voiceButton.interactable = true;
        UnityAndGeminiV2.OnSubmit(transcriptionText);
    }
    private void OnStartListening()
    {
        voiceButton.interactable = false;
    }
    private void OnStoppedListening()
    {
        voiceButton.interactable = true;
    }
    private void OnDestroy()
    {
        // 取消订阅事件
        appVoiceExperience.VoiceEvents.OnPartialTranscription.RemoveListener(OnPartialTranscription);
        appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(OnFullTranscription);
    }

    public void OnUserClickStopButton()
    {
        appVoiceExperience.Activate();// 開始语音识别
        voiceButton.interactable = false;
    }

    public void StopVoiceRecognition()
    {
        appVoiceExperience.Deactivate(); // 停止语音识别
        voiceButton.interactable = true;
    }
}