using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;
public class DatabaseFetcher : MonoBehaviour
{
    //把本地資料更新上資料庫
    //把資料庫帳號更新到本地
    private string tempFilePath;
    [SerializeField] private Text errorText;
    public void FetchDataFromDatabase()
    {
        StartCoroutine(LoginAndFetchData());
    }
    IEnumerator LoginAndFetchData()

    {   //為了獲得Session的teacher或TA權限，用PHP沒愈時問題
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
            StartCoroutine(GetDatabaseData());
        }
    }
    private IEnumerator GetDatabaseData()
    {
        WWWForm form = new WWWForm();
        form.AddField("action", "fetch");
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
                Debug.Log("Data fetched successfully");
                Debug.Log("Received JSON: " + www.downloadHandler.text);
                SaveDataToAssets(www.downloadHandler.text);
            }
        }
    }
    private void SaveDataToAssets(string jsonData)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "DatabaseData.txt");
        if (!Directory.Exists(Application.persistentDataPath))
        {
            Debug.LogError("文件夹路径不存在！");
            errorText.text = "文件夹路径不存在！";
        }
        // 检查文件是否存在
        if (File.Exists(filePath))
        {
            Debug.Log("文件已存在");
            // errorText.text = "文件已存在";
            // errorText.text = File.ReadAllText(filePath);
        }
        else
        {
            Debug.Log("文件不存在，将会创建文件");
            errorText.text = "文件不存在，将会创建文件";
        }
        try
        {
            // 解析 JSON 為 Root 物件
            var root = JsonUtility.FromJson<Root>(jsonData);

            if (root == null || !root.success || root.data == null || root.data.Length == 0)
            {
                Debug.LogError("Error: Parsed data is null, unsuccessful, or empty.");
                errorText.text = "Error: Parsed data is null, unsuccessful, or empty.";
                return;
            }

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var data in root.data)
                {
                    string line = $"{data.student_id} {data.name} {data.password} {data.permission} {data.gender}";
                    writer.WriteLine(line);
                }
            }
            // errorText.text = $"Data saved to: {filePath}";
            Debug.Log($"Data saved to: {filePath}");
        }
        catch (System.Exception e)
        {
            errorText.text = "Error saving data: " + e.Message;
            Debug.LogError("Error saving data: " + e.Message);
        }
    }


    public void SendTempData()
    {
        tempFilePath = Path.Combine(Application.persistentDataPath, "temp.txt");
        Debug.Log("文件保存路径: " + tempFilePath);
        if (!File.Exists(tempFilePath))
        {
            Debug.LogError("暫存檔不存在！");
            return;
        }

        string tempData = File.ReadAllText(tempFilePath);

        if (string.IsNullOrEmpty(tempData))
        {
            Debug.LogError("暫存檔為空！");
            return;
        }
        StartCoroutine(SendDataCoroutine(tempData));
    }

    IEnumerator SendDataCoroutine(string tempData)
    {
        string[] lines = tempData.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            string[] parts = line.Split(' ');

            if (parts.Length < 5)
            {
                Debug.LogError("資料格式錯誤: " + line);
                continue;
            }

            string studentId = parts[0];
            string score = parts[1];
            string answerTime = parts[2];
            string answerDate = parts[3];
            string answerText = parts[4];

            WWWForm form = new WWWForm();
            form.AddField("student_id", studentId);
            form.AddField("score", score);
            form.AddField("answer_time", answerTime);
            form.AddField("answer_date", answerDate);
            form.AddField("incorrect_questions", answerText);
            string php = URL.LoadURL("your_php_file.php");
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
                }
            }
        }

        try
        {
            File.WriteAllText(tempFilePath, string.Empty);
            Debug.Log("暫存檔已清空！");
        }
        catch (Exception e)
        {
            Debug.LogError("清空暫存檔失敗: " + e.Message);
        }
    }
}

[System.Serializable]
public class Data
{
    public string id;
    public string student_id;
    public string password;
    public string name;
    public string permission;
    public string gender;
}

[System.Serializable]
public class Root
{
    public bool success;
    public Data[] data;
}

