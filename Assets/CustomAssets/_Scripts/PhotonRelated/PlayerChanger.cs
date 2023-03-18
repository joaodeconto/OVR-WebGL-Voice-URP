using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using ReadyPlayerMe.AvatarLoader;
using BWV.Player;

public class PlayerChanger : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public static PlayerChanger Instance { get; private set; }

        private RaiseEventOptions raiseEventOptions;
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
        public void RandomAvatar()
        {
            ChangePlayerAvatar(GameManager.AvatarUrlSO.GetRandomAvatar());
        }
        public void ChangePlayerName(string newName)
        {
            // Set the new name for the local player
            PhotonNetwork.NickName = newName;
            // Send a custom event to inform other players of the name change
            object[] data = new object[] { PhotonNetwork.LocalPlayer.ActorNumber, newName };
            raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(3, data, raiseEventOptions, SendOptions.SendReliable);
            Debug.LogError("Raised Event Change Player Name");
        }

        public void RaiseEventPlayerAvatar(string avatarUrl)
        {
            object[] data = new object[] { PhotonNetwork.LocalPlayer.ActorNumber, avatarUrl };
            raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(2, data, raiseEventOptions, SendOptions.SendReliable);
            Debug.Log("Raised Event Change Player Avatar");
        }

        public void ChangePlayerAvatar(string avatarUrl)
        {
            GameManager.MyPlayer.transform.GetComponent<WebAvatarLoader>().LoadAvatar(avatarUrl);
            object[] data = new object[] { PhotonNetwork.LocalPlayer.ActorNumber, avatarUrl };
            raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(2, data, raiseEventOptions, SendOptions.SendReliable);
            Debug.Log(GameManager.MyPlayer.transform.name + PhotonNetwork.LocalPlayer.ActorNumber + avatarUrl);
            Debug.Log("ChangePlayerAvatar and Raised Event Change Player Avatar");
        }

        public void OnEvent(EventData photonEvent)
        {
            if (photonEvent.Code == 3)
            {
                object[] data = (object[])photonEvent.CustomData;

                // Get the actor number and new name from the event data
                int actorNumber = (int)data[0];
                string newName = (string)data[1];

                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    if (p.ActorNumber == actorNumber)
                    {
                        GameObject playerObject = p.TagObject as GameObject;
                        if (playerObject != null)
                        {
                            playerObject.GetComponent<PlayerController>().PlayerName = newName;
                        }
                        else Debug.LogError("Null Gameobject");
                        break;
                    }
                }
            }
            else if (photonEvent.Code == 2)
            {
                object[] data = (object[])photonEvent.CustomData;

                // Get the actor number and new name from the event data
                int actorNumber = (int)data[0];
                string newUrl = (string)data[1];

                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    if (p.ActorNumber == actorNumber)
                    {
                        GameObject playerObject = (GameObject)p.TagObject;
                        if (playerObject != null)
                        {
                            Debug.Log(playerObject.name + PhotonNetwork.LocalPlayer.ActorNumber + p.ActorNumber + newUrl);
                            playerObject.GetComponent<WebAvatarLoader>().LoadAvatar(newUrl);
                        }
                        else Debug.LogError("Null Gameobject");
                        break;
                    }
                }

            }
        }
    }