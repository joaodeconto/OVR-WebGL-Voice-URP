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
        [Header("To Destroy if not Owned by Player")]

        [SerializeField]
        private Recorder recorder;
        [SerializeField] 
        private List<Component> m_Behaviour = new();
        [SerializeField]
        private List<GameObject> m_Object = new();

        public TMP_Text m_PlayerName;
        public string[] m_AvatarUrl;

        void Awake()
        {
            if (!photonView.IsMine)
            {
                m_PlayerName.text = photonView.Owner.UserId;
                foreach (Component c in m_Behaviour)
                {
                    Destroy(c);
                }
                foreach (GameObject g in m_Object)
                {
                    Destroy(g);
                }
                Destroy(recorder);
            }            
        }
        void Start()
        {
        }
        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            info.Sender.TagObject = this.gameObject;
        }

        public int WrapIndex(int index, int bufferLength)
        {
            return (index % bufferLength + bufferLength) % bufferLength;
        }
    }
}
