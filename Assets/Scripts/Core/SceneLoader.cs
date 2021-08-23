using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Core
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeTime = 1;

        public static SceneLoader instance;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            instance = this;
        }

        public void LoadScene(int sceneNumber)
        {
            StartCoroutine(HandleLoadScene(sceneNumber));
        }

        private IEnumerator HandleLoadScene(int sceneNumber)
        {
            yield return FadeOut();
            yield return SceneManager.LoadSceneAsync(sceneNumber);
            yield return FadeIn();
        }

        private IEnumerator FadeOut()
        {
            while (fadeImage.color.a < 1)
            {
                float newAlpha = fadeImage.color.a + Time.deltaTime / fadeTime;
                newAlpha = Mathf.Clamp(newAlpha, 0, 1);
                Color color = fadeImage.color;
                color.a = newAlpha;
                fadeImage.color = color;
                yield return null;
            }
        }

        private IEnumerator FadeIn()
        {
            while (fadeImage.color.a > 0)
            {
                float newAlpha = fadeImage.color.a - Time.deltaTime / fadeTime;
                newAlpha = Mathf.Clamp(newAlpha, 0, 1);
                Color color = fadeImage.color;
                color.a = newAlpha;
                fadeImage.color = color;
                yield return null;
            }
        }
    }

}