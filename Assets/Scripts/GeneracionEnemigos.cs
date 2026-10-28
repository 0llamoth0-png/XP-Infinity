using UnityEngine;

public class GeneracionEnemigos : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnInterval = 3f;
    [SerializeField] Vector3 spawnArea = new Vector3(10f, 0f, 10f);
    [SerializeField] int maxEnemies = 5;

    float nextSpawnTime;

    void Awake()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("GeneracionEnemigos requires an enemy prefab.", this);
            return;
        }

        if (spawnInterval <= 0f)
        {
            Debug.LogError("GeneracionEnemigos requires a spawn interval greater than zero.", this);
        }

        if (maxEnemies <= 0)
        {
            Debug.LogError("GeneracionEnemigos requires maxEnemies to be greater than zero.", this);
        }

        nextSpawnTime = Time.time + Mathf.Max(0f, spawnInterval);
    }

    public bool SpawnEnemy()
    {
        if (enemyPrefab == null || maxEnemies <= 0)
        {
            return false;
        }

        int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (enemyCount >= maxEnemies)
        {
            return false;
        }

        Vector3 offset = new Vector3(
            Random.Range(-Mathf.Abs(spawnArea.x), Mathf.Abs(spawnArea.x)),
            Random.Range(-Mathf.Abs(spawnArea.y), Mathf.Abs(spawnArea.y)),
            Random.Range(-Mathf.Abs(spawnArea.z), Mathf.Abs(spawnArea.z)));

        Instantiate(enemyPrefab, transform.position + offset, Quaternion.identity);
        return true;
    }

    void Update()
    {
        if (enemyPrefab == null || spawnInterval <= 0f || maxEnemies <= 0)
        {
            return;
        }

        if (Time.time >= nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnInterval;
            SpawnEnemy();
        }
    }
}
