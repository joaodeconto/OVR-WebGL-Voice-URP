
    using UnityEngine;
    using UnityEngine.UI;
    using Photon.Voice.PUN;

//PhotonCode
namespace BWV.Photon
{
    [RequireComponent(typeof(Canvas))]
    public class CustomHighlighter : MonoBehaviour
    {
        private Canvas canvas;

        [SerializeField]
        private PhotonVoiceView photonVoiceView;

        [SerializeField]
        private Image recorderSprite;

        [SerializeField]
        private Image speakerSprite;

        [SerializeField]
        private Text bufferLagText;

        private bool showSpeakerLag;

        private void Awake()
        {
            this.canvas = this.GetComponent<Canvas>();
            //if (this.canvas != null && this.canvas.worldCamera == null) { this.canvas.worldCamera = Camera.main; }
            //this.photonVoiceView = this.GetComponentInParent<PhotonVoiceView>();
        }


        // Update is called once per frame
        private void Update()
        {
            this.recorderSprite.enabled = this.photonVoiceView.IsRecording;
            this.speakerSprite.enabled = this.photonVoiceView.IsSpeaking;
            this.bufferLagText.enabled = this.showSpeakerLag && this.photonVoiceView.IsSpeaking;
            this.bufferLagText.text = string.Format("{0}", this.photonVoiceView.SpeakerInUse.Lag);
        }

        private void LateUpdate()
        {
            //if (this.canvas == null || this.canvas.worldCamera == null) { return; } // should not happen, throw error
            //this.transform.rotation = Quaternion.Euler(0f, this.canvas.worldCamera.transform.eulerAngles.y, 0f); //canvas.worldCamera.transform.rotation;
        }
    }
}
