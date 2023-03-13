using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using BWV.Player;

public class PlayerNameChanger : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public void ChangePlayerName(string newName)
    {
        // Set the new name for the local player
        PhotonNetwork.NickName = newName;
        GameManager.MyPlayer.m_PlayerName.text = newName;
        // Send a custom event to inform other players of the name change
        object[] data = new object[] { PhotonNetwork.LocalPlayer.ActorNumber, newName, PhotonNetwork.LocalPlayer.TagObject};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(1, data, raiseEventOptions, SendOptions.SendReliable);
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == 1)
        {
            object[] data = (object[])photonEvent.CustomData;

            // Get the actor number and new name from the event data
            int actorNumber = (int)data[0];
            string newName = (string)data[1];

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.ActorNumber == actorNumber)
                {
                    GameObject playerObject = PhotonView.Find(p.ActorNumber).gameObject;
                    if (playerObject != null)
                    {
                        playerObject.GetComponent<PlayerController>().m_PlayerName.text = newName;
                    }
                    break;
                }
            }
            
        }
    }
}
