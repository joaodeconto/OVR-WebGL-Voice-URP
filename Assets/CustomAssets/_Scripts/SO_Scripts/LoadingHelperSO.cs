using UnityEngine;

namespace BWV
{
    [CreateAssetMenu(fileName = "LoadingHelper", menuName = "ScriptableObjects/LoadingHelper")]
    public class LoadingHelperSO : ScriptableObject
    {
        [Header("Loading Settings")]

        public GameObject loadingScreenPrefab;
        public GameObject spinner;
        public TMPro.TMP_Text loadingText;
        public UnityEngine.UI.Slider slider;
        public GameObject fade;

        [SerializeField]
        private string[] jokes = {
            "Why do programmers prefer dark mode? Because light attracts bugs.",
            "Why don't scientists trust atoms? Because they make up everything.",
            "What did one wall say to the other wall? I'll meet you at the corner!",
            "Why did the scarecrow win an award? Because he was outstanding in his field.",
            "Why don't skeletons fight each other? They don't have the guts.",
            "What did the grape say when it got stepped on? Nothing, it just let out a little wine.",
            "Why don't seagulls fly by the bay? Because then they'd be bay-gulls!",
            "Why did the tomato turn red? Because it saw the salad dressing!",
            "Why did the hipster burn his tongue? He drank his coffee before it was cool.",
            "What do you call a fake noodle? An impasta."
        };
        public string[] Jokes { get { return jokes; } }
    }
}