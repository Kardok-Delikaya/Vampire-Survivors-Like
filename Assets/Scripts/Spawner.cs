using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class Spawner : MonoBehaviour
    {
        Transform player;
        GameManager gameManager;

        [Header("Spawn Area")]
        [SerializeField] Vector2 spawnArea;

        [Header("Enemy Spawn")]
        public int enemyCount;
        [SerializeField] GameObject[] enemies;
        float[] enemySpawnTimer;
        [SerializeField] float[] enemySpawnCoolDown;
        [SerializeField] int[] enemySpawnStartLevel;
        [SerializeField] int[] enemySpawnMaxLevel;

        [Header("Boss Spawn")]
        [SerializeField] GameObject[] bosses;

        void Start()
        {
            enemySpawnTimer = new float[enemies.Length];
            gameManager=GetComponent<GameManager>();
            player = FindAnyObjectByType<PlayerManager>().transform;
        }

        void FixedUpdate()
        {
            if (enemyCount < 100)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (enemySpawnStartLevel[i] <= gameManager.level && enemySpawnMaxLevel[i] > gameManager.level)
                    {
                        if (enemySpawnTimer[i] <= 0f)
                        {
                            SpawnEnemy(i);
                            enemySpawnTimer[i] = enemySpawnCoolDown[i];
                        }
                        else
                        {
                            enemySpawnTimer[i] -= Time.fixedDeltaTime;
                        }
                    }
                }
            }
        }

        private void SpawnEnemy(int i)
        {
            Vector3 position = GenerateRandomPosition();
            position += player.position;
            GameObject newEnemy = Instantiate(enemies[i]);
            newEnemy.transform.position = position;
            enemyCount++;
        }

        public void TryToSpawnBoss()
        {
            Vector3 position = GenerateRandomPosition();
            position += player.position;

            if (gameManager.level % 5 == 0)
            {
                int bossNumber = gameManager.level / 5;

                if (bossNumber < bosses.Length)
                {
                    GameObject newEnemy = Instantiate(bosses[bossNumber-1]);
                    newEnemy.transform.position = position;
                }
                else
                {
                    GameObject newEnemy = Instantiate(bosses[bosses.Length-1]);
                    newEnemy.transform.position = position;
                }
            }
        }

        private Vector3 GenerateRandomPosition()
        {
            Vector3 position = new Vector3();

            float f = UnityEngine.Random.value > .5f ? -1f : 1f;
            if (UnityEngine.Random.value > .5f)
            {
                position.x = UnityEngine.Random.Range(-spawnArea.x, spawnArea.x);
                position.y = spawnArea.y * f;
            }
            else
            {
                position.y = UnityEngine.Random.Range(-spawnArea.y, spawnArea.y);
                position.x = spawnArea.x * f;
            }
            position.z = 0;
            return position;
        }
    }
}