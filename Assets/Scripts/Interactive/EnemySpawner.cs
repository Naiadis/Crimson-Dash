using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject enemyPrefab;
        public float spawnWeight = 1f;     
        public float spawnHeight = 0f;     
    }

    [Header("Spawn Settings")]
    public EnemyType[] enemies;            
    public float minSpawnInterval = 2f;     
    public float maxSpawnInterval = 5f;     
    public int maxEnemiesAtOnce = 5;       
    
    [Header("Spawn Area")]
    public float spawnDistanceFromPlayer = 15f;  
    public Transform player;                     

    private readonly List<GameObject> activeEnemies = new List<GameObject>();
    private float nextSpawnTime;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        SetNextSpawnTime();
        StartCoroutine(SpawnRoutine());
    }

    void SetNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            CleanupDestroyedEnemies();

            if (Time.time >= nextSpawnTime && activeEnemies.Count < maxEnemiesAtOnce)
            {
                SpawnEnemy();
                SetNextSpawnTime();
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
        if (player == null || enemies.Length == 0) return;

        // Calculate total weight
        float totalWeight = 0;
        foreach (var enemy in enemies)
        {
            totalWeight += enemy.spawnWeight;
        }

        // Select random enemy based on weight
        float randomValue = Random.Range(0, totalWeight);
        float weightSum = 0;
        EnemyType selectedEnemy = enemies[0];

        foreach (var enemy in enemies)
        {
            weightSum += enemy.spawnWeight;
            if (randomValue <= weightSum)
            {
                selectedEnemy = enemy;
                break;
            }
        }

float playerDirection = player.localScale.x;
Vector3 spawnPos = new Vector3(
    player.position.x + spawnDistanceFromPlayer * playerDirection, 
    selectedEnemy.spawnHeight,
    0);

        // Check if spawn position is visible
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(spawnPos);
        if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1)
        {
            spawnPos.x += playerDirection * 5f;
        }

        GameObject newEnemy = Instantiate(selectedEnemy.enemyPrefab, spawnPos, Quaternion.identity);
        activeEnemies.Add(newEnemy);
    }

    void CleanupDestroyedEnemies()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    void Update()
    {
        if (player == null) return;

        float despawnDistance = spawnDistanceFromPlayer * 2f;
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null) continue;

            float distance = Mathf.Abs(activeEnemies[i].transform.position.x - player.position.x);
            if (distance > despawnDistance)
            {
                Destroy(activeEnemies[i]);
                activeEnemies.RemoveAt(i);
            }
        }
    }
}