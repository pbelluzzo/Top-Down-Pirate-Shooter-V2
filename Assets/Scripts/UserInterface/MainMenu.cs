using UnityEngine;
using Core;

public class MainMenu : MonoBehaviour
{
    [Header("Start Game Definitions")]
    [SerializeField] int startGameSceneIndex = 1;

    [Header("Menu Navigation Definitions")]
    [SerializeField] GameObject optionsMenuGameObject;

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
