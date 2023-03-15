using BWV;
using BWV.Player;
using Photon.Pun;
using ReadyPlayerMe.AvatarLoader;
using ReadyPlayerMe.Core.Data;
using UnityEngine;


public class WebAvatarLoader : MonoBehaviourPunCallbacks
{
    public AvatarSettingsSO avatarSettings;

    private const string TAG = nameof(WebAvatarLoader);
    [SerializeField]
    private GameObject avatarParent;
    [SerializeField]
    private CharacterAnimatorController animatorController;
    private GameObject avatar;
    private AvatarObjectLoader avatarLoader;

    private void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        CoreSettings partner = Resources.Load<CoreSettings>("CoreSettings");
        
        WebInterface.SetupRpmFrame(partner.Subdomain);
#endif

        if (photonView.IsMine) PlayerAvatarGenerated(GameManager.AvatarUrlSO.CurrentUrl);
        else RemoteAvatarGenerated(GetAvatarUrlFromInstantiate());
    }

    string GetAvatarUrlFromInstantiate()
    {
        object[] instantiationData = photonView.InstantiationData;
        string avatarUrl = instantiationData[0].ToString();
        return avatarUrl;
    }

    public void PlayerAvatarGenerated(string generatedUrl)
    {
        GameManager.AvatarUrlSO.AddAvatarUrl(generatedUrl);
        PlayerChanger.Instance.RaiseEventPlayerAvatar(generatedUrl);

        if (avatar != null) GameObject.DestroyImmediate(avatar);

        avatarLoader = new AvatarObjectLoader();
        LoadingHelper.Instance.ShowLoadingScreen();
        avatarLoader.OnCompleted += (_, args) =>
        {
            this.avatar = args.Avatar;            
            SetupPlayerAvatar();
        };
        LoadingHelper.Instance.HideLoadingScreen();
        avatarLoader.LoadAvatar(generatedUrl);
        
    }

    public void RemoteAvatarGenerated(string generatedUrl)
    {
        if (avatar != null) GameObject.DestroyImmediate(avatar);

        avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += (_, args) =>
        {
            this.avatar = args.Avatar;
            SetupRemoteAvatar();
        };
        avatarLoader.LoadAvatar(generatedUrl);
    }

    private void SetupRemoteAvatar()
    {
        avatar.transform.parent = avatarParent.transform;
        AvatarSetup avatarSetup = avatar.AddComponent<AvatarSetup>();
        avatarSetup.avatarSettings = avatarSettings;
        avatarSetup.SetupAvatar(avatar);
    }

    private void SetupPlayerAvatar()
    {
        avatar.transform.parent = avatarParent.transform;
        AvatarSetup avatarSetup = avatar.AddComponent<AvatarSetup>();
        avatarSetup.avatarSettings = avatarSettings;
        avatarSetup.SetupAvatar(avatar);
        Animator anim = avatar.gameObject.GetComponent<Animator>();
        anim.applyRootMotion = false;
        animatorController.animator = avatar.gameObject.GetComponent<Animator>();
        avatar.AddComponent<PhotonAnimatorView>();
        avatar.AddComponent<EyeAnimationHandler>();
        avatar.AddComponent<VoiceHandler>();
    }
}
