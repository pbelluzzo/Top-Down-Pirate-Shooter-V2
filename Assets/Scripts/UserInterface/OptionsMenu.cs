using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class OptionsMenu : MonoBehaviour
    {
        [Header("Player Prefs Definitions")]
        [SerializeField] Slider gameSessionSlider;
        [SerializeField] Slider enemySpawnSlider;

        [Header("Menu Navigation Definitions")]
        [SerializeField] GameObject mainMenuGameObject;

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
            SetSlidersValues();
        }

        private void SetSlidersValues()
        {
            if (PlayerPrefs.HasKey("GameSessionTime"))
            {
                gameSessionSlider.value = PlayerPrefs.GetInt("GameSessionTime");
            }

            if (PlayerPrefs.HasKey("EnemySpawnTime"))
            {
                enemySpawnSlider.value = PlayerPrefs.GetInt("EnemySpawnTime");
            }
        }
    }
}

