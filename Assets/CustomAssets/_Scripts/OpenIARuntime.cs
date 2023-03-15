using UnityEngine;
using TMPro;
using OpenAi.Unity.V1;



public class OpenIARuntime : MonoBehaviour
{
    //public InputField Input;
    public TMP_Text Output;

    public void DoApiCompletion(string input)
    {
        string text = input;

        if (string.IsNullOrEmpty(text))
        {
            Debug.LogError("Example requires input in input field");
            return;
        }

        Debug.Log("Performing Completion in Play Mode");

        Output.text = "Perform Completion...";
        OpenAiCompleterV1.Instance.Complete(
            text,
            s => Output.text = s,
            e => Output.text = $"ERROR: StatusCode: {e.responseCode} - {e.error}"
        );
    }
}