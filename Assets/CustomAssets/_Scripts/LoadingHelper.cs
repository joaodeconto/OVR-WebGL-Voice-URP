using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace BWV
{
    public class LoadingHelper : MonoBehaviour
    {
        public static LoadingHelper Instance;

        [SerializeField] private LoadingHelperSO loadingSO;
        [SerializeField] private GameObject loadingCanvas;
        [SerializeField] GameObject spinner;
        [SerializeField] TMP_Text loadingText;
        [SerializeField] Slider slider;
        [SerializeField] Image fade;

        private bool isLoading = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            loadingCanvas.SetActive(false);
            slider.gameObject.SetActive(false);
        }

        public void ShowLoadingScreen(string message = "Loading...")
        {
            isLoading = true;
            loadingCanvas.SetActive(true);
            loadingText.text = message;

            // Start spinning texture animation
            StartCoroutine(SpinLoadingIcon());
        }

        public void HideLoadingScreen()
        {
            isLoading = false;
            loadingCanvas.SetActive(false);

            // Stop spinning texture animation
            StopAllCoroutines();
        }

        private IEnumerator SpinLoadingIcon()
        {
            float angle = 0f;
            while (isLoading)
            {
                angle -= Time.deltaTime * 180f;
                spinner.transform.rotation = Quaternion.Euler(0f, 0f, angle);
                yield return null;
            }
        }

        public void UpdateSliderValue(float value)
        {
            slider.value = value;
        }

        public void UpdateLoadingText(string message)
        {
            loadingText.text = message;
        }

        public void FadeInLoadingScreen(float duration)
        {
            StartCoroutine(FadeIn(duration));
        }

        public void FadeOutLoadingScreen(float duration)
        {
            StartCoroutine(FadeOut(duration));
        }

        private IEnumerator FadeIn(float duration)
        {
            fade.gameObject.SetActive(true);
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);

            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, time / duration);
                fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, alpha);
                yield return null;
            }

            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
        }

        private IEnumerator FadeOut(float duration)
        {
            fade.gameObject.SetActive(true);
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);

            float time = 0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, time / duration);
                fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, alpha);
                yield return null;
            }

            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);
            fade.gameObject.SetActive(false);
        }
    }
}