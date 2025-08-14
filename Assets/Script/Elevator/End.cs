using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class End : MonoBehaviour
{
    [SerializeField] IntuGameManage intuGameManage;
    [SerializeField] Text text;
    void Update()
    {
        if (intuGameManage == null)
        {
            intuGameManage = FindObjectOfType<IntuGameManage>();
        }
    }
    public void Quest()
    {
        if (string.IsNullOrEmpty(intuGameManage.errorQuestions))
        {
            text.text = "題目全對";
        }
        else
        {
            text.text = "答錯題號: " + intuGameManage.errorQuestions;
        }
    }
    public void Return()
    {
        SceneManager.LoadScene("Menu");
    }
}
