using BWV;
using BWV.Player;
using Photon.Pun;
using ReadyPlayerMe.Core.Data;
using ReadyPlayerMe.AvatarLoader;
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

        if (photonView.IsMine) PlayerAvatarGeneration(GameManager.AvatarUrlSO.CurrentUrl);
        else RemoteAvatarGeneration(GetAvatarUrlFromInstantiate());
    }

    string GetAvatarUrlFromInstantiate()
    {
        object[] instantiationData = photonView.InstantiationData;
        string avatarUrl = instantiationData[0].ToString();
        return avatarUrl;
    }

    public void PlayerAvatarGeneration(string generatedUrl)
    {
        GameManager.AvatarUrlSO.AddAvatarUrl(generatedUrl);
        PlayerChanger.Instance.RaiseEventPlayerAvatar(generatedUrl);

        if (avatar != null) GameObject.DestroyImmediate(avatar);

        avatarLoader = new AvatarObjectLoader();
        LoadingHelper.Instance.ShowLoadingScreen();
        avatarLoader.OnCompleted += (_, args) =>
        {
            this.avatar = args.Avatar;
            SetupAvatar(true);
        };
        LoadingHelper.Instance.HideLoadingScreen();
        avatarLoader.LoadAvatar(generatedUrl);
        
    }

    public void RemoteAvatarGeneration(string generatedUrl)
    {
        if (avatar != null) GameObject.DestroyImmediate(avatar);

        avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += (_, args) =>
        {
            this.avatar = args.Avatar;
            SetupAvatar(false);
        };
        avatarLoader.LoadAvatar(generatedUrl);
    }

    private void SetupAvatar(bool isPlayer)
    {
        avatar.transform.parent = avatarParent.transform;
        AvatarSetup avatarSetup = avatar.AddComponent<AvatarSetup>();
        avatarSetup.avatarSettings = avatarSettings;
        avatarSetup.SetupAvatar(avatar);
        Animator animator= avatar.gameObject.GetComponent<Animator>();
        if (isPlayer) animatorController.animator = animator;
        animator.applyRootMotion = false;
        animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("AvatarController");
        SetupSynchronizeParameters();
        avatar.AddComponent<EyeAnimationHandler>();
        avatar.AddComponent<VoiceHandler>();
    }

    void SetupSynchronizeParameters()
    {
        PhotonAnimatorView photonAnimator = avatar.AddComponent<PhotonAnimatorView>();
        photonAnimator.SetLayerSynchronized(0, PhotonAnimatorView.SynchronizeType.Discrete);
        foreach (var a in photonAnimator.GetSynchronizedParameters())
        {
            Debug.LogError(a.SynchronizeType + "    " + a.Name);
            //a.SynchronizeType = PhotonAnimatorView.SynchronizeType.Discrete;
        }
        //photonAnimator.SetParameterSynchronized("Forward", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Continuous);
        photonAnimator.SetParameterSynchronized("Strafe", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);
        photonAnimator.SetParameterSynchronized("Speed", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);
        foreach (var a in photonAnimator.GetSynchronizedParameters())
        {
            Debug.LogError(a.SynchronizeType + "    " + a.Name);
            //a.SynchronizeType = PhotonAnimatorView.SynchronizeType.Discrete;
        }
    }
}
