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
}