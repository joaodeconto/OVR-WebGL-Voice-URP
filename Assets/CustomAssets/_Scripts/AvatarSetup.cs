
using BWV.Player;
using ReadyPlayerMe.AvatarLoader;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    public AvatarSettingsSO avatarSettings;

    public void SetupAvatar(GameObject avatar)
    {     
        avatar.transform.localPosition = avatarSettings.position;
        avatar.transform.localRotation = avatarSettings.rotation;

        if (avatarSettings.animator != null)
        {
            Animator animator = avatar.GetComponent<Animator>();
            if (animator != null)
            {
                animator.runtimeAnimatorController = avatarSettings.animator;
            }
        }
    }
}