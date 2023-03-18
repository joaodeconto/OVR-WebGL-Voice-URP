using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ReadyPlayerMe.AvatarLoader;
using ReadyPlayerMe.Core.Data;

namespace BWV.Player
{
    /// <summary>
    /// Handles the loading and setup of avatars from Ready Player Me.
    /// </summary>
    public class WebAvatarLoader : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
        public AvatarSettingsSO avatarSettings;
        [SerializeField] private GameObject avatarContainer; // Parent transform for the avatar game object.

        private GameObject avatar;
        private AvatarObjectLoader avatarLoader;
        private bool isLoading;

        public event Action OnAvatarLoaded; // Invoked when the avatar has finished loading.

        private void Awake()
        {
            if (!avatarContainer)
            {
                Debug.LogError("Avatar container is not set.");
                enabled = false;
                return;
            }            

#if !UNITY_EDITOR && UNITY_WEBGL
            CoreSettings partner = Resources.Load<CoreSettings>("CoreSettings");

            // Set up the Ready Player Me frame for WebGL builds.
            WebInterface.SetupRpmFrame(partner.Subdomain);
#endif
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            info.Sender.TagObject = this.gameObject;
            Debug.Log("info MSG INSTANTIATE" + info.Sender.TagObject.ToString());

            // Load the avatar if the URL is available.
            string avatarUrl = GetAvatarUrlFromInstantiate();
            if (!string.IsNullOrEmpty(avatarUrl))
            {
                LoadAvatar(avatarUrl);
            }
        }

        private string GetAvatarUrlFromInstantiate()
        {
            object[] instantiationData = photonView.InstantiationData;
            if (instantiationData.Length >= 5)
            {
                return instantiationData[4]?.ToString();
            }
            return null;
        }

        #region Avatar Loading

        public void LoadAvatar(string avatarUrl)
        {
            foreach (Transform t in avatarContainer.transform)
            {
                Destroy(t.gameObject);
            }

            if (isLoading)
            {
                Debug.LogWarning("An avatar is already being loaded.");
                return;
            }
            isLoading = true;

            // Load the avatar using the AvatarObjectLoader class.
            avatarLoader = new AvatarObjectLoader();
            avatarLoader.OnCompleted += (_, args) =>
            {
                if (args.Avatar != null)
                {
                    this.avatar = args.Avatar;
                    SetupAvatar(this.photonView.IsMine);
                    OnAvatarLoaded?.Invoke();
                }
                else
                {
                    Debug.LogError("Failed to load avatar from URL: " + avatarUrl);
                }
                isLoading = false;
            };
            avatarLoader.LoadAvatar(avatarUrl);
        }

        #endregion

        #region Avatar Setup

        private void SetupAvatar(bool isPlayer)
        {
            Debug.Log("ou no seutp");
            if (!avatar)
            {
                Debug.LogError("Avatar game object is not set.");
                return;
            }            

            avatar.transform.SetParent(avatarContainer.transform, false);

            
            if(!avatarContainer.TryGetComponent<AvatarSetup>(out var avatarSetup))
                 avatarSetup = avatar.AddComponent<AvatarSetup>();


            avatarSetup.avatarSettings = avatarSettings;
            avatarSetup.SetupAvatar(avatar, isPlayer);
        }

        #endregion
    }
}
