using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

namespace BWV.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] 
        private List<Component> m_Behaviour = new List<Component>();
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
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
