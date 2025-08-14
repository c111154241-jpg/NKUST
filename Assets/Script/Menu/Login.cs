using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Meta.XR.MRUtilityKit;
using System;
using Newtonsoft.Json;
public class Login : MonoBehaviour
{
    public Text LoginUserText;
    public Text errorText;
    public InputField ID_input, Pass_input;
    [SerializeField] private GameObject LoginConfirm;
    [SerializeField] private GameObject MenuConfirm;
    static public string Name;
    static public string ID;
    static public string Pass;


    void Start()
    {

    }
    public void Button_Quit()
    {
        ID_input.text = "";
        Pass_input.text = "";
        LoginConfirm.SetActive(false);
        MenuConfirm.SetActive(true);
    }
    public void Button_Login()
    {
        ID_input.text = "";
        Pass_input.text = "";
        errorText.text = "";
        LoginConfirm.SetActive(false);
        MenuConfirm.SetActive(true);
    }
    public void Loginenter()
    {
        if (!string.IsNullOrEmpty(ID_input.text) && !string.IsNullOrEmpty(Pass_input.text))
        {
            StartCoroutine(LoginAndFetchData());
            // StartCoroutine(SendDataCoroutine(ID_input.text, Pass_input.text));
            errorText.text = "等待中";
        }
        else
            errorText.text = "請輸入帳號";
    }
    IEnumerator LoginAndFetchData()
    {
        //為了獲得Session的teacher或TA權限，用PHP沒愈時問題
        // 登入表單
        WWWForm loginForm = new WWWForm();
        loginForm.AddField("student_id", "C111154240");
        loginForm.AddField("password", "12345678");

        // 發送登入請求
        string php = URL.LoadURL("getData.php");
        Debug.Log(php);
        UnityWebRequest www = UnityWebRequest.Post(php, loginForm);
        www.timeout = 10; // 設置超時（以秒為單位）
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError)
        {
            errorText.text = "連線錯誤：" + www.error;
            Debug.LogError("連線錯誤：" + www.error);
        }
        else if (www.result == UnityWebRequest.Result.ProtocolError)
        {
            errorText.text = "伺服器回應錯誤：" + www.error;
            Debug.LogError("伺服器回應錯誤：" + www.error);
        }
        else if (www.result != UnityWebRequest.Result.Success)
        {
            errorText.text = "其他錯誤：" + www.error;
            Debug.LogError("其他錯誤：" + www.error);
        }
        else
        {
            Debug.Log("登入成功: " + www.downloadHandler.text);
            StartCoroutine(SendDataCoroutine(ID_input.text, Pass_input.text));
        }
    }
    [System.Serializable]
    public class ResponseData
    {
        public bool success;
        public string message;
        public List<StudentData> data;
    }

    [System.Serializable]
    public class StudentData
    {
        public string student_id;
        public string name;
        public string password;
        public string gender;
        public string permission;
    }

    public IEnumerator SendDataCoroutine(string username, string password)
    {
        string action = "fetch";
        WWWForm form = new WWWForm();
        form.AddField("action", action);
        string php = URL.LoadURL("manage.php");
        Debug.Log(php);
        using (UnityWebRequest www = UnityWebRequest.Post(php, form))
        {
            www.timeout = 10; // 設置超時（以秒為單位）
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError)
            {
                errorText.text = "連線錯誤：" + www.error;
                Debug.LogError("連線錯誤：" + www.error);
            }
            else if (www.result == UnityWebRequest.Result.ProtocolError)
            {
                errorText.text = "伺服器回應錯誤：" + www.error;
                Debug.LogError("伺服器回應錯誤：" + www.error);
            }
            else if (www.result != UnityWebRequest.Result.Success)
            {
                errorText.text = "其他錯誤：" + www.error;
                Debug.LogError("其他錯誤：" + www.error);
            }
            else
            {
                Debug.Log("資料發送成功: " + www.downloadHandler.text);

                // 嘗試解析 JSON
                try
                {
                    ResponseData parsedData = JsonConvert.DeserializeObject<ResponseData>(www.downloadHandler.text);
                    if (parsedData.success)
                    {
                        Debug.Log("解析成功，学生数据数量: " + parsedData.data.Count);

                        // 遍历所有学生数据，查找用户名和密码匹配的学生
                        foreach (var student in parsedData.data)
                        {
                            if (student.student_id == username && student.password == password)
                            {
                                errorText.text = $"找到学生: {student.name}";
                                Debug.Log($"找到学生: {student.name}，学号: {student.student_id}, 性别: {student.gender}, 权限: {student.permission}");
                                // 如果匹配，处理找到的学生信息
                                // 例如显示在 UI 上或进行其他操作
                                ID = username;
                                Pass = password;
                                errorText.text = "登入成功";
                                LoginUserText.text = ID;
                                Button_Login();
                                // 处理成功登录的逻辑
                                yield break;
                            }
                        }
                        // 如果没有找到匹配的用户名和密码
                        errorText.text = "未找到匹配的用户名和密码";
                        Debug.LogError("未找到匹配的用户名和密码");
                    }
                    else
                    {
                        errorText.text = "服务器返回失败: " + parsedData.message;
                        Debug.LogError("服务器返回失败: " + parsedData.message);
                    }
                }
                catch (System.Exception ex)
                {
                    errorText.text = "JSON 解析失敗: " + ex.Message;
                    Debug.LogError("JSON 解析失敗: " + ex.Message);
                }
            }
        }
    }
}
