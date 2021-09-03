using UnityEngine;
using Core;

namespace UserInterface
{
    public class MainMenu : MonoBehaviour
    {
        [Header("Start Game Definitions")]
        [SerializeField] int startGameSceneIndex = 1;

        [Header("Menu Navigation Definitions")]
        [SerializeField] GameObject optionsMenuGameObject;

        private void Awake()
        {
            optionsMenuGameObject.SetActive(false);
        }

        public void NavigateToOptionsMenu()
        {
            optionsMenuGameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void StartGame()
        {
            SceneLoader.instance.LoadScene(startGameSceneIndex);
        }
    }
}

