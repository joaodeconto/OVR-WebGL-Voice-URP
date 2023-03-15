using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine.Events;

public class GPT3 : MonoBehaviour
{
    public static GPT3 Instance;
    public UnityAction OnResponseReceived;
    public string LastResponse
    {
        get { return lastResponse; }
    }

    // Variables
    private string apiUrl = "https://api.openai.com/v1/engines/davinci-codex/completions";
    private string apiKey = "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";
    private string prompt = "did the integration worked?";
    private string lastResponse;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void SendPrompt(string input)
    {
        prompt = input;
        StartCoroutine(GetGPT3Response());
    }

    IEnumerator GetGPT3Response()
    {
        // Prepare the request
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
        request.Method = "POST";
        request.Headers.Add("Authorization", "Bearer " + apiKey);
        request.ContentType = "application/json";

        // Prepare the data
        JObject data = new JObject();
        data.Add("prompt", prompt);
        data.Add("max_tokens", 50);

        // Convert the data to a byte array
        byte[] byteData = Encoding.UTF8.GetBytes(data.ToString());

        // Write the data to the request stream
        using (Stream requestStream = request.GetRequestStream())
        {
            requestStream.Write(byteData, 0, byteData.Length);
        }

        // Get the response
        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            using (Stream responseStream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    // Read the response data
                    string responseJson = reader.ReadToEnd();
                    JObject responseData = JObject.Parse(responseJson);

                    // Get the completed text
                    string completedText = responseData["choices"][0]["text"].ToString();

                    // Use the completed text in your project
                    lastResponse = completedText;
                    Debug.Log("GPT-3 Response: " + completedText);
                }
                OnResponseReceived();
            }
        }

        yield return null;
    }
}