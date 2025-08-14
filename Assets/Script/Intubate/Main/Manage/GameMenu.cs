using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] IntuGameManage IntuGameManage;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject check;
    [SerializeField] GameObject reset1;
    [SerializeField] GameObject reset2;
    [SerializeField] GameObject resetButton;
    void Update()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Elevator")
            resetButton.SetActive(false); // 在 Elevator 場景時隱藏按鈕
        else
            resetButton.SetActive(true);  // 其他場景顯示按鈕
    }
    public void ShowMenu()
    {
        menu.SetActive(true);
        reset1.SetActive(false);
        reset2.SetActive(false);
        check.SetActive(false);
    }
    public void ShowReset()
    {
        menu.SetActive(false);
        reset1.SetActive(true);
        reset2.SetActive(false);
        check.SetActive(false);
    }
    public void ShowCheck()
    {
        menu.SetActive(false);
        reset1.SetActive(false);
        reset2.SetActive(false);
        check.SetActive(true);
    }
    public void Close()
    {
        IntuGameManage.Input();
    }
    public void Return()
    {
        SceneManager.LoadScene("Menu");
    }
    public void Up()
    {
        reset1.SetActive(true);
        reset2.SetActive(false);
    }
    public void Down()
    {
        reset1.SetActive(false);
        reset2.SetActive(true);
    }
}
