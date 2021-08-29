using System.Collections;
using UnityEngine;

namespace Gameplay
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] GameObject[] enemySpawnAreas;
        [SerializeField] PoolType[] enemies;
        [SerializeField] GameObject playerPrefab;
        [SerializeField] GameObject gameOverCanvas;
        [SerializeField] int score;

        GameObject player;
        static GameplayManager instance;

        public static GameplayManager GetInstance() => instance;

        public int GetScore() => score;

        public void AddScore(int value) => score += value;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            instance = this;
        }

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
                PoolType randomEnemy = enemies[Random.Range(0, (enemies.Length))];
                Transform randomSpawnArea = enemySpawnAreas[Random.Range(0, (enemySpawnAreas.Length - 1))].transform;
                //GameObject enemy = Instantiate(randomEnemy, randomSpawnArea.position, randomSpawnArea.rotation);
                //enemy.AddComponent<EnemyInitializer>();
                yield return new WaitForSeconds(PlayerPrefs.GetInt("EnemyRespawnTime"));
            }
        }

        private IEnumerator SessionTimeControl()
        {
            Debug.Log("StartSessionControl");
            float sessionTime = 60 * PlayerPrefs.GetInt("GameSessionTime");
            yield return new WaitForSeconds(sessionTime);
            GameOver();
        }

        private void GameOver()
        {
            StopCoroutine(SessionTimeControl());
           // StopCoroutine(EnemySpawn());
            gameOverCanvas.SetActive(true);
        }
    }
}

