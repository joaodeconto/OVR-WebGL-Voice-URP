using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Toggle flyPlayerToggle;
    public Toggle flyForwardToggle;

    private void Start()
    {
        flyPlayerToggle.isOn = GameManager.PlayerOptions.flyPlayer;
        flyForwardToggle.isOn = GameManager.PlayerOptions.flyForward;
    }
    public void OnCreateAvatar()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        WebInterface.SetIFrameVisibility(true);
#endif
    }
}