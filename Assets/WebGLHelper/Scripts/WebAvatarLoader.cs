using ReadyPlayerMe.AvatarLoader;
using ReadyPlayerMe.Core;
using ReadyPlayerMe.Core.Data;
using UnityEngine;

public class WebAvatarLoader : MonoBehaviour
{
    //private const string TAG = nameof(WebAvatarLoader);

    static public WebAvatarLoader Instance;
    public GameObject AvatarParent { get; set; }
    private GameObject avatar;
    private AvatarObjectLoader avatarLoader;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
            CoreSettings partner = Resources.Load<CoreSettings>("CoreSettings");
        
            WebInterface.SetupRpmFrame(partner.Subdomain);
#endif
    }    

    public void OnWebViewAvatarGenerated(string generatedUrl)
    {
        avatarLoader = new AvatarObjectLoader();
        avatarLoader.OnCompleted += OnAvatarLoadCompleted;
        avatarLoader.OnFailed += OnAvatarLoadFailed;
        avatarLoader.LoadAvatar(generatedUrl);
    }

    private void OnAvatarLoadCompleted(object sender, CompletionEventArgs args)
    {
        if (this.avatar) Destroy(avatar);
        this.avatar = args.Avatar;
        this.avatar.transform.parent = AvatarParent.transform;
        this.avatar.transform.localRotation = Quaternion.identity;
        this.avatar.transform.localPosition = Vector3.zero;
        //AvatarAnimatorHelper.SetupAnimator(args.Metadata.BodyType, avatar);
        if (args.Metadata.BodyType == BodyType.HalfBody)
        {
            avatar.transform.position = new Vector3(0, 1, 0);
        }

        avatarLoader.OnCompleted -= OnAvatarLoadCompleted;
        avatarLoader.OnFailed -= OnAvatarLoadFailed;
    }

    private void OnAvatarLoadFailed(object sender, FailureEventArgs args)
    {
        Debug.LogError($"Avatar Load failed with error: {args.Message}");
        //SDKLogger.Log(TAG, $"Avatar Load failed with error: {args.Message}");
        avatarLoader.OnCompleted -= OnAvatarLoadCompleted;
        avatarLoader.OnFailed -= OnAvatarLoadFailed;
    }
}
