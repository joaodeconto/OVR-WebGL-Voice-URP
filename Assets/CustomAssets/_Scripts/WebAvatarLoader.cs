using BWV;
using BWV.Player;
using Photon.Pun;
using Photon.Realtime;
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
            animatorController.animator = avatar.GetComponent<Animator>();
            SetupAvatarGameObject(true);
        };
        LoadingHelper.Instance.HideLoadingScreen();
        avatarLoader.LoadAvatar(generatedUrl);
        
    }

    public void RemoteAvatarGeneration(string generatedUrl)
    {
        GameManager.AvatarUrlSO.AddAvatarUrl(generatedUrl);
        if (avatar != null) GameObject.DestroyImmediate(avatar);

        avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += (_, args) =>
        {
            this.avatar = args.Avatar;
            SetupAvatarGameObject(false);
        };
        avatarLoader.LoadAvatar(generatedUrl);
    }

    private void SetupAvatarGameObject(bool isPlayer)
    {        
        avatar.transform.parent = avatarParent.transform;
        AvatarSetup avatarSetup = avatar.AddComponent<AvatarSetup>();        
        avatarSetup.avatarSettings = avatarSettings;
        avatarSetup.SetupAvatar(avatar, isPlayer);        
    }
}
