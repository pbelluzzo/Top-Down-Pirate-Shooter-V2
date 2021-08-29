using System.Collections;
using UnityEngine;
using Core;
using Gameplay;
using TMPro;

namespace UserInterface
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        public void PlayGame()
        {
            SceneLoader.instance.LoadScene(1);
        }

        public void LoadMenu()
        {
            SceneLoader.instance.LoadScene(0);
        }

        public void SetScore()
        {
            int score = GameplayManager.GetInstance().GetScore();
            StartCoroutine(IncreaseScoreUntilMax(score));
        }

        private IEnumerator IncreaseScoreUntilMax(int maxScore)
        {
            int score = 0;
            while (score < maxScore)
            {
                score++;
                RenderScore(score);
                yield return new WaitForSeconds(0.05f);
            }
        }

        private void RenderScore(int score)
        {
            scoreText.text = score.ToString();
        }

        private void OnEnable()
        {
            SetScore();
        }
    }
}
