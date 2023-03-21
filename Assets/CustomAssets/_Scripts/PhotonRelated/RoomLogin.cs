
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using BWV.Player;

public class RoomLogin : MonoBehaviourPunCallbacks
{
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Photon server");

        // Join a random room
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join a room, creating new room");

        // Create a new room
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom()
    {
        //InstantiateAvatar();
    }
    public override void OnPlayerEnteredRoom(Player otherPlayer)
    {
        //UIManager.Instance.SystemText(otherPlayer + " Entrou");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //UIManager.Instance.SystemText(otherPlayer + " Entrou");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("-> JV: OnPlayerLeft is MasterClient? {0}", PhotonNetwork.IsMasterClient);
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void SetPlayerName(string playerName)
    {
        PhotonNetwork.NickName = playerName;
    }

    public int WrapIndex(int index, int bufferLength)
    {
        return (index % bufferLength + bufferLength) % bufferLength;
    }

}