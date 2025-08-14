using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class UnityAndGeminiKey
{
    public string key;
}

[System.Serializable]
public class Response
{
    public Candidate[] candidates;
}

[System.Serializable]
public class Candidate
{
    public Content content;
}

[System.Serializable]
public class Content
{
    public Part[] parts;
}

[System.Serializable]
public class Part
{
    public string text;
}

public class UnityAndGeminiV2 : MonoBehaviour
{
    public TextAsset jsonApi;      // JSON file containing the API key
    private string apiKey = "";
    private string apiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent";  // API endpoint
    public Text outputText;        // Reference to the UI Text to display the response

    void Start()
    {
        UnityAndGeminiKey jsonApiKey = JsonUtility.FromJson<UnityAndGeminiKey>(jsonApi.text);
        apiKey = jsonApiKey.key;  // Load the API key from the JSON file
    }

    // Method to handle the button click and send the prompt to Gemini
    public void OnSubmit(Text inputText)
    {
        string prompt = inputText.text;  // Get the text from the input field
        StartCoroutine(SendRequestToGemini(prompt));  // Send the request
    }

    // Coroutine to send the request to the Gemini API
    public IEnumerator SendRequestToGemini(string promptText)
    {
        // Construct the request URL
        string url = $"{apiEndpoint}?key={apiKey}";

        // Create JSON data for the request body
        string jsonData = "{\"contents\": [{\"parts\": [{\"text\": \"" + promptText + "\"}]}]}";
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        // Create a UnityWebRequest with the JSON data
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
                outputText.text = "Error: " + www.error;  // Display error message
            }
            else
            {
                Debug.Log("Request complete!");
                Response response = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                if (response.candidates.Length > 0 && response.candidates[0].content.parts.Length > 0)
                {
                    string text = response.candidates[0].content.parts[0].text;
                    Debug.Log(text);
                    outputText.text = text;  // Display the response text
                }
                else
                {
                    outputText.text = "No text found.";  // Handle empty response
                }
            }
        }
    }
}
