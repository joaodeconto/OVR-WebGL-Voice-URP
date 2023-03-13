using Photon.Pun;
using ReadyPlayerMe.AvatarLoader;
using ReadyPlayerMe.Core;
using ReadyPlayerMe.Core.Data;
using System;
using UnityEngine;

public class WebAvatarLoader : MonoBehaviourPunCallbacks
{
    private const string TAG = nameof(WebAvatarLoader);
    [SerializeField]
    private GameObject avatarParent;
    private GameObject avatar;
    [SerializeField]
    public string[] m_AvatarUrl;
    private AvatarObjectLoader avatarLoader;

    private void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        CoreSettings partner = Resources.Load<CoreSettings>("CoreSettings");
        
        WebInterface.SetupRpmFrame(partner.Subdomain);
#endif

        if (photonView.IsMine) OnWebViewAvatarGenerated(GameManager.AvatarUrlSO.CurrentUrl);
        else OnManualAvatarGenerated(GetAvatarUrlFromInstantiate());
    }

    string GetAvatarUrlFromInstantiate()
    {
        object[] instantiationData = photonView.InstantiationData;
        string avatarUrl = instantiationData[0].ToString();
        return avatarUrl;
    }

    public void OnWebViewAvatarGenerated(string generatedUrl)
    {
        GameManager.AvatarUrlSO.AddAvatarUrl(generatedUrl);
        PlayerChanger.Instance.RaiseEventPlayerAvatar(generatedUrl);
        avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += (_, args) =>
        {
            foreach (Transform t in avatarParent.transform) {
                GameObject.Destroy(t.gameObject);
            }
            this.avatar = args.Avatar;
            this.avatar.transform.parent = avatarParent.transform;
            this.avatar.transform.localRotation = Quaternion.identity;
            this.avatar.transform.localPosition = Vector3.zero;
            //AvatarAnimatorHelper.SetupAnimator(args.Metadata.BodyType, avatar);
        };
        avatarLoader.LoadAvatar(generatedUrl);
    }

    public void OnManualAvatarGenerated(string generatedUrl)
    {
        avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += (_, args) =>
        {
            foreach (Transform t in avatarParent.transform)
            {
                GameObject.Destroy(t.gameObject);
            }
            this.avatar = args.Avatar;
            this.avatar.transform.parent = avatarParent.transform;
            this.avatar.transform.localRotation = Quaternion.identity;
            this.avatar.transform.localPosition = Vector3.zero;
            //AvatarAnimatorHelper.SetupAnimator(args.Metadata.BodyType, avatar);
        };
        avatarLoader.LoadAvatar(generatedUrl);
    }
}
