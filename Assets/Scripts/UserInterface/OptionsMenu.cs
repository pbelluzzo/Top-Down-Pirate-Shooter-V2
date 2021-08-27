using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UserInterface
{
    public class OptionsMenu : MonoBehaviour
    {
        [Header("Player Prefs Definitions")]
        [SerializeField] Slider gameSessionSlider;
        [SerializeField] TextMeshProUGUI gameSessionValueTMPro;
        [SerializeField] Slider enemySpawnSlider;
        [SerializeField] TextMeshProUGUI enemySpawnValueTMPro;

        [Header("Menu Navigation Definitions")]
        [SerializeField] GameObject mainMenuGameObject;

        public void SetGameSessionValueText() => gameSessionValueTMPro.text = gameSessionSlider.value.ToString();
        public void SetEnemySpawnValueText() => enemySpawnValueTMPro.text = enemySpawnSlider.value.ToString();

        public void SaveAndReturn()
        {
            PlayerPrefs.SetInt("GameSessionTime", Mathf.RoundToInt(gameSessionSlider.value));
            PlayerPrefs.SetInt("EnemySpawnTime", Mathf.RoundToInt(enemySpawnSlider.value));

            PlayerPrefs.Save();

            mainMenuGameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            SetUIValues();
        }

        private void SetUIValues()
        {
            if (PlayerPrefs.HasKey("GameSessionTime"))
            {
                gameSessionSlider.value = PlayerPrefs.GetInt("GameSessionTime");
                SetGameSessionValueText();
            }

            if (PlayerPrefs.HasKey("EnemySpawnTime"))
            {
                enemySpawnSlider.value = PlayerPrefs.GetInt("EnemySpawnTime");
                SetEnemySpawnValueText();
            }
        }
    }
}

