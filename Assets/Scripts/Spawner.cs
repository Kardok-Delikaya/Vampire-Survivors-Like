using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VSLike
{
    public class Spawner : MonoBehaviour
    {
        Transform player;

        [Header("Enemy Spawn")]
        public int enemyCount;
        [SerializeField] GameObject[] enemy;
        [SerializeField] float[] enemyTime;
        [SerializeField] float[] enemyMaxTime;
        [SerializeField] int[] enemyLevel;
        [SerializeField] int[] enemyMaxLevel;

        [Header("Boss Spawn")]
        [SerializeField] GameObject[] boss;
        [SerializeField] int[] bossLevel;
        [SerializeField] Vector2 spawnArea;
        public int level;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        void FixedUpdate()
        {
            if (enemyCount < 100)
            {
                for (int i = 0; i < enemy.Length; i++)
                {
                    if (enemyLevel[i] <= level && enemyMaxLevel[i] > level)
                    {
                        if (enemyTime[i] <= 0f)
                        {
                            SpawnEnemy(i);
                            enemyTime[i] = enemyMaxTime[i];
                        }
                        else
                        {
                            enemyTime[i] -= Time.fixedDeltaTime;
                        }
                    }
                }
            }
        }
        private void SpawnEnemy(int i)
        {
            Vector3 position = GenerateRandomPosition();
            position += player.position;
            GameObject newEnemy = Instantiate(enemy[i]);
            newEnemy.transform.position = position;
            enemyCount++;
        }
        public void BossSpawn()
        {
            Vector3 position = GenerateRandomPosition();
            position += player.position;

            for (int i = 0; i < boss.Length; i++)
            {
                if (level == bossLevel[i])
                {
                    GameObject newEnemy = Instantiate(boss[i]);
                    newEnemy.transform.position = position;
                    break;
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