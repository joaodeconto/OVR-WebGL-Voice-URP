using UnityEngine;

[CreateAssetMenu(fileName = "AvatarSettings", menuName = "ScriptableObjects/AvatarSettings")]
public class AvatarSettingsSO : ScriptableObject
{
    public Vector3 position = Vector3.zero;
    public Quaternion rotation = Quaternion.identity;
    public RuntimeAnimatorController animator = null;
    public string avatarUrl;
}