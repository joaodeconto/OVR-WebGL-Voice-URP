using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Toggle flyPlayerToggle;
    public Toggle flyForwardToggle;
    public Toggle gravityToggle;
    public static TMP_Text gptResponse;

    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }
    private void Start()
    {
        flyPlayerToggle.isOn = GameManager.PlayerOptions.Levitate;
        flyForwardToggle.isOn = GameManager.PlayerOptions.FlyForward;
        gravityToggle.isOn = GameManager.PlayerOptions.GravityOn;
    }
    public void OnCreateAvatar()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        WebInterface.SetIFrameVisibility(true);
#endif
    }
    private void RefreshResponse(string response)
    {
        gptResponse.text = response;
    }
}