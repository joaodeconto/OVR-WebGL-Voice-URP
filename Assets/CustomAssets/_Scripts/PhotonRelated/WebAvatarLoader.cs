using BWV;
using BWV.Player;
using Photon.Pun;
using ReadyPlayerMe.Core.Data;
using ReadyPlayerMe.AvatarLoader;
using UnityEngine;
using Photon.Realtime;


public class WebAvatarLoader : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
{
    public AvatarSettingsSO avatarSettings;

    private const string TAG = nameof(WebAvatarLoader);
    [SerializeField]
    private GameObject avatarParent;
    [SerializeField]
    private CharacterAnimatorController animatorController;
    private GameObject avatar;
    private GameObject oldAvatar;
    private GameObject newAvatar;
    private AvatarObjectLoader avatarLoader;

    private void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        CoreSettings partner = Resources.Load<CoreSettings>("CoreSettings");
        
        WebInterface.SetupRpmFrame(partner.Subdomain);
#endif
        avatar = transform.GetChild(0).gameObject;
        oldAvatar = avatar;
        Animator animator = avatar.GetComponent<Animator>();
        if (photonView.IsMine) animatorController.animator = animator;

        if (photonView.IsMine) animatorController.animator = animator;  //PlayerAvatarGeneration(GameManager.AvatarUrlSO.CurrentUrl);
        //else RemoteAvatarGeneration(avatarSettings.avatarUrl);        
           
    }

    string GetAvatarUrlFromInstantiate()
    {
        object[] instantiationData = photonView.InstantiationData;
        string avatarUrl = instantiationData[4].ToString();
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
        avatar.transform.parent = this.transform;
        AvatarSetup avatarSetup = avatar.AddComponent<AvatarSetup>();
        avatarSetup.avatarSettings = avatarSettings;
        avatarSetup.SetupAvatar(avatar);
        Animator animator= avatar.GetComponent<Animator>();
        if (isPlayer) animatorController.animator = animator;
        animator.applyRootMotion = false;
        if(animator.runtimeAnimatorController == null) 
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("AvatarController");
        SetupSynchronizeParameters();        
    }

    void SetupSynchronizeParameters()
    {
        PhotonAnimatorView photonAnimator = avatar.AddComponent<PhotonAnimatorView>();
        photonAnimator.SetLayerSynchronized(0, PhotonAnimatorView.SynchronizeType.Disabled);
        photonAnimator.SetParameterSynchronized("Forward", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);
        photonAnimator.SetParameterSynchronized("Strafe", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);
        photonAnimator.SetParameterSynchronized("Speed", PhotonAnimatorView.ParameterType.Float, PhotonAnimatorView.SynchronizeType.Discrete);
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        info.Sender.TagObject = this.gameObject;
    }
}
