using Photon.Pun;
using ReadyPlayerMe.AvatarLoader;
using UnityEngine;

namespace BWV.Player
{
    public class AvatarSetup : MonoBehaviour
    {
        public AvatarSettingsSO avatarSettings;
        private PhotonAnimatorView photonAnimator;
        private GameObject avatarGo;
        private Animator animator;
        private Avatar avatarType;
        public void SetupAvatar(GameObject avatarFrom, bool isPlayer)
        {
            SwapAvatarParent(avatarFrom);
            avatarGo.transform.localPosition = avatarSettings.position;
            avatarGo.transform.localRotation = avatarSettings.rotation;

            animator = avatarGo.GetComponent<Animator>();
            animator.applyRootMotion = false;
            animator.avatar = avatarType;
            animator.Rebind();
            if (animator.runtimeAnimatorController == null)
                animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("AvatarController");

            if (!gameObject.TryGetComponent<PhotonAnimatorView>(out var photonAnimator))
                SetupSynchronizeParameters();
        }
        void SetupSynchronizeParameters()
        {
            photonAnimator = avatarGo.AddComponent<PhotonAnimatorView>();
            photonAnimator.SetLayerSynchronized(0, PhotonAnimatorView.SynchronizeType.Disabled);
            photonAnimator.SetParameterSynchronized("Forward", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);
            photonAnimator.SetParameterSynchronized("Strafe", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);
            photonAnimator.SetParameterSynchronized("Speed", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);
        }

        void SwapAvatarParent(GameObject go)
        {
            avatarGo = this.gameObject;
            animator = go.GetComponent<Animator>();
            avatarType = animator.avatar;

            Destroy(animator);

            foreach (Transform t in go.transform)
            {
                t.SetParent(this.transform, false);
            }
            //Destroy(go);
        }
    }
}
