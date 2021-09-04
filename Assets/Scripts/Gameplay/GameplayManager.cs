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

        bool gameIsOver = false;
        GameObject player;
        ObjectPooler objectPooler;
        static GameplayManager instance;

        public static GameplayManager GetInstance() => instance;
        public int GetScore() => score;
        public Transform GetPlayerTransform() => player.transform;
        public bool GetGameOver() => gameIsOver;

        public void AddScore(int value) => score += value;

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            instance = this;

            SpawnPlayerShip();
            objectPooler = ObjectPooler.GetInstance();
        }

        private void Start()
        {
            StartCoroutine(EnemySpawn());
            StartCoroutine(SessionTimeControl());
        }

        private void SpawnPlayerShip()
        {
            player = Instantiate(playerPrefab);
        }

        private IEnumerator EnemySpawn()
        {
            while (gameIsOver == false)
            {
                PoolType randomEnemy = enemies[Random.Range(0, (enemies.Length))];
                Transform randomSpawnArea = enemySpawnAreas[Random.Range(0, (enemySpawnAreas.Length - 1))].transform;
                GameObject enemy = objectPooler.SpawnFromPool(randomEnemy, randomSpawnArea);

                yield return new WaitForSeconds(PlayerPrefs.GetInt("EnemySpawnTime"));
            }
        }

        private IEnumerator SessionTimeControl()
        {
            float sessionTime = 60 * PlayerPrefs.GetInt("GameSessionTime");
            yield return new WaitForSeconds(sessionTime);
            GameOver();
        }

        public void GameOver()
        {
            StopAllCoroutines();
            StartCoroutine(ShowGameOver());
            gameIsOver = true;
        }

        IEnumerator ShowGameOver()
        {
            yield return new WaitForSeconds(1f);

            gameOverCanvas.SetActive(true);
        }
    }
}

