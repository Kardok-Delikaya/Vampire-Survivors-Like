using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Area")]
    [SerializeField] private Vector2 spawnArea;

    [Header("Enemy Spawn")]
    public int enemyCount;
    [SerializeField] private GameObject[] enemies;
    private float[] enemySpawnTimer;
    [SerializeField] private float[] enemySpawnCoolDown;
    [SerializeField] private int[] enemySpawnStartLevel;
    [SerializeField] private int[] enemySpawnMaxLevel;

    [Header("Boss Spawn")]
    [SerializeField] private GameObject[] bosses;

    private void Start()
    {
        enemySpawnTimer = new float[enemies.Length];
    }

    private void Update()
    {
        if (enemyCount >= 100) return;
        
        for (var i = 0; i < enemies.Length; i++)
        {
            if (enemySpawnStartLevel[i] > GameManager.Instance.level ||
                enemySpawnMaxLevel[i] <= GameManager.Instance.level) continue;
            
            if (enemySpawnTimer[i] <= 0f)
            {
                SpawnEnemy(i);
                enemySpawnTimer[i] = enemySpawnCoolDown[i];
            }
            else
            {
                enemySpawnTimer[i] -= Time.deltaTime;
            }
        }
    }

    private void SpawnEnemy(int i)
    {
        var position = GenerateRandomPosition();
        position += GameManager.Instance.player.transform.position;
        var newEnemy = Instantiate(enemies[i]);
        newEnemy.transform.position = position;
        enemyCount++;
    }

    public void TryToSpawnBoss()
    {
        if (GameManager.Instance.level % 5 != 0) return;
        
        var bossNumber = GameManager.Instance.level / 5;
        var position = GenerateRandomPosition();
        position += GameManager.Instance.player.transform.position;
            
        if (bossNumber < bosses.Length)
        {
            var newEnemy = Instantiate(bosses[bossNumber-1]);
            newEnemy.transform.position = position;
        }
        else
        {
            var newEnemy = Instantiate(bosses[bosses.Length-1]);
            newEnemy.transform.position = position;
        }
    }

    private Vector3 GenerateRandomPosition()
    {
        var position = new Vector3();

        var f = UnityEngine.Random.value > .5f ? -1f : 1f;
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

    public void HandleKill()
    {
        enemyCount--;
    }
}