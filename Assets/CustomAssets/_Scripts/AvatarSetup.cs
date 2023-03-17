using Photon.Pun;
using ReadyPlayerMe.AvatarLoader;
using UnityEngine;

namespace BWV.Player
{
    public class AvatarSetup : MonoBehaviour
    {
        public AvatarSettingsSO avatarSettings;
        private GameObject avatar;
        private Animator animator;
        //private PhotonAnimatorView photonAnimator;
        public void SetupAvatar(GameObject avatarObj, bool isPlayer)
        {
            avatar = avatarObj;
            
            avatar.transform.localPosition = avatarSettings.position;
            avatar.transform.localRotation = avatarSettings.rotation;
            animator = gameObject.GetComponent<Animator>();
            animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            animator.applyRootMotion = false;
            avatar.AddComponent<EyeAnimationHandler>();
            //avatar.AddComponent<VoiceHandler>();

            if (avatarSettings.animator != null)
            {
                animator = avatar.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.runtimeAnimatorController = avatarSettings.animator;
                }
            }

            if(isPlayer)SetupSynchronizeParameters();

        }
        void SetupSynchronizeParameters()
        {
            //PhotonView photonView = gameObject.AddComponent<PhotonView>();
            PhotonAnimatorView photonAnimator = avatar.AddComponent<PhotonAnimatorView>();
            //photonAnimator.SetLayerSynchronized(0, PhotonAnimatorView.SynchronizeType.Discrete);
            //photonAnimator.SetParameterSynchronized("Forward", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Continuous);
            //photonAnimator.SetParameterSynchronized("Strafe", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);
            //photonAnimator.SetParameterSynchronized("Speed", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);
            foreach (var a in photonAnimator.GetSynchronizedParameters())
            {
                Debug.LogError(a.SynchronizeType + "    " + a.Name);
                //a.SynchronizeType = PhotonAnimatorView.SynchronizeType.Discrete;
            }
        }
    }
}