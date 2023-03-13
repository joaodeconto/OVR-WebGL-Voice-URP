using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

namespace BWV.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header ("To Destroy if not Owned by Player")]

        [SerializeField] 
        private List<Component> m_Behaviour = new();
        [SerializeField]
        private List<GameObject> m_Object = new();

        public TMP_Text m_PlayerName;
        private PhotonView photonView;

        void Awake()
        {
            photonView = GetComponent<PhotonView>();
            if (!photonView.IsMine)
            {
                m_PlayerName.text = photonView.Owner.NickName;
                foreach (Component c in m_Behaviour)
                {
                    Destroy(c);
                }
                foreach (GameObject g in m_Object)
                {
                    Destroy(g);
                }
            }
        }
    }
}
