using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Voice;
using TMPro;
using Photon.Voice.Unity;

namespace BWV.Player
{
    public class PlayerController : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {

        void Start()
        {
        }
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            info.Sender.TagObject = this.gameObject;
        }
    }
}
