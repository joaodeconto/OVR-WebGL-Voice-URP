
using UnityEngine;
using TMPro;

namespace BWV.Player
{
    public class PlayerController : MonoBehaviour
    {
         [SerializeField] private TMP_Text m_PlayerName;

        public string PlayerName { get { return m_PlayerName.text; } set { m_PlayerName.text = value; } }
        void Start()
        {
        }
    }
}
