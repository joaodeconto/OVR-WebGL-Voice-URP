
using UnityEngine;
using UnityEngine.UI;
using Photon.Voice.PUN;

//PhotonCode
namespace BWV.Photon
{
    [RequireComponent(typeof(Canvas))]
    public class CustomHighlighter : MonoBehaviour
    {
        [SerializeField]
        private PhotonVoiceView photonVoiceView;

        [SerializeField]
        private Image recorderSprite;

        [SerializeField]
        private Image speakerSprite;

        private void Update()
        {
            this.recorderSprite.enabled = this.photonVoiceView.IsRecording;
            this.speakerSprite.enabled = this.photonVoiceView.IsSpeaking;
        }
    }
}
