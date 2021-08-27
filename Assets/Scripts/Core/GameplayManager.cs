using System.Collections;
using UnityEngine;

namespace Core
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] enemySpawnAreas;
        [SerializeField] private GameObject[] enemies;

        [SerializeField] private GameObject playerPrefab;
        private GameObject player;

        [SerializeField] private GameObject gameOverMenu;

        [SerializeField] private int score;

        private void Start()
        {
            SpawnPlayerShip();
            //StartCoroutine(EnemySpawn());
            StartCoroutine(SessionTimeControl());
        }

        private void SpawnPlayerShip()
        {
            player = Instantiate(playerPrefab);
        }

        private IEnumerator EnemySpawn()
        {
            while (true)
            {
                GameObject randomEnemy = enemies[Random.Range(0, (enemies.Length))];
                Transform randomSpawnArea = enemySpawnAreas[Random.Range(0, (enemySpawnAreas.Length - 1))].transform;
                GameObject enemy = Instantiate(randomEnemy, randomSpawnArea.position, randomSpawnArea.rotation);
                enemy.AddComponent<EnemyInitializer>();
                yield return new WaitForSeconds(PlayerPrefs.GetFloat("EnemyRespawnTime"));
            }
        }

        private IEnumerator SessionTimeControl()
        {
            float sessionTime = 60 * PlayerPrefs.GetFloat("GameSessionTime");
            yield return new WaitForSeconds(sessionTime);
            GameOver();
        }

        private void GameOver()
        {
            StopCoroutine(SessionTimeControl());
           // StopCoroutine(EnemySpawn());
            //gameOverMenu.SetActive(true);
            //gameOverMenu.GetComponent<GameOverMenu>().SetScore(score);
        }
    }
}

