using UnityEngine;

[CreateAssetMenu(fileName = "AvatarUrlList", menuName = "ScriptableObjects/AvatarUrls")]
public class AvatarUrlSO : ScriptableObject
{
    [SerializeField]
    private string[] m_AvatarUrls;

    public string CurrentUrl;

    public string GetRandomAvatar()
    {
        return GetAvatarUrl(Random.Range(0, m_AvatarUrls.Length));
    }
    public string GetAvatarUrl(int index)
    {
        if (index >= 0 && index < m_AvatarUrls.Length)
        {
            Debug.LogWarning("avatar URL index: " + index);
            return m_AvatarUrls[index];
        }
        else
        {
            Debug.LogWarning("Invalid avatar URL index: " + index);
            return null;
        }
    }
    public void AddAvatarUrl(string url)
    {
        if (url != null && url != "")
        {
            // Check if the URL already exists in the array
            if (System.Array.IndexOf(m_AvatarUrls, url) != -1)
            {
                Debug.LogWarning("Avatar URL already exists: " + url);
                return;
            }

            // Resize the array and add the new URL
            int oldLength = m_AvatarUrls.Length;
            System.Array.Resize(ref m_AvatarUrls, oldLength + 1);
            m_AvatarUrls[oldLength] = url;
        }
        else
        {
            Debug.LogWarning("Invalid avatar URL: " + url);
        }
    }

}

