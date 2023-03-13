using System.Collections.Generic;
using UnityEngine;
using BWV.Player;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerOptionsSO _playerOptionsSO;
    [SerializeField] private AvatarUrlSO _avatarUrlSO;
    public static PlayerOptionsSO PlayerOptions;
    public static AvatarUrlSO AvatarUrlSO;
    public static PlayerController MyPlayer { get; private set; }
    public static List<PlayerController> OtherPlayers { get; private set; } = new List<PlayerController>();
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        PlayerOptions = _playerOptionsSO;
        AvatarUrlSO = _avatarUrlSO;
    }

    public void SetMyPlayer(PlayerController player)
    {
        MyPlayer = player;
    }

    public void AddOtherPlayer(PlayerController player)
    {
        OtherPlayers.Add(player);
    }

    public void RemoveOtherPlayer(PlayerController player)
    {
        OtherPlayers.Remove(player);
    }

    // Example usage in another script:
    // GameManager.Instance.SetMyPlayer(playerController);
    // GameManager.Instance.AddOtherPlayer(otherPlayerController);
}

