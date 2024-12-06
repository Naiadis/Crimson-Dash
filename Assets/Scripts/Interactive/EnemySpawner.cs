using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject enemyPrefab;
        public float baseSpawnWeight = 1f;     
        public float spawnHeight = 0f;     
    }

    [Header("Spawn Settings")]
    public EnemyType[] enemies;            
    public float minSpawnInterval = 2f;     
    public float maxSpawnInterval = 5f;     
    public int maxEnemiesAtOnce = 5;       
    
    [Header("Spawn Area")]
    public Transform player;      

    [Header("Gap Scaling")]
    public float initialSpawnDistance = 15f;
    public float minSpawnDistance = 8f;
    public float distanceReductionRate = 0.001f;    

    [Header("Difficulty Scaling")]
    public float distanceInterval = 500f;
    public float weightIncreasePerInterval = 0.2f;
    public float maxWeightMultiplier = 2f;
    public float spawnIntervalReduction = 0.1f;

    private readonly List<GameObject> activeEnemies = new List<GameObject>();
    private float nextSpawnTime;
    private Camera mainCamera;
    private float distanceTraveled;
    private float currentWeightMultiplier = 1f;
    private float currentSpawnDistance;

    void Start()
    {
        mainCamera = Camera.main;
        currentSpawnDistance = initialSpawnDistance;
        SetNextSpawnTime();
        StartCoroutine(SpawnRoutine());
    }

    void SetNextSpawnTime()
    {
        float currentMinInterval = Mathf.Max(minSpawnInterval - (distanceTraveled / distanceInterval) * spawnIntervalReduction, 0.5f);
        float currentMaxInterval = Mathf.Max(maxSpawnInterval - (distanceTraveled / distanceInterval) * spawnIntervalReduction, 1f);
        nextSpawnTime = Time.time + Random.Range(currentMinInterval, currentMaxInterval);
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

        EnemyType selectedEnemy = SelectEnemyByWeight();
        float playerDirection = player.localScale.x;
        Vector3 spawnPos = CalculateSpawnPosition(playerDirection, selectedEnemy.spawnHeight);

        GameObject newEnemy = Instantiate(selectedEnemy.enemyPrefab, spawnPos, Quaternion.identity);
        activeEnemies.Add(newEnemy);
    }

    EnemyType SelectEnemyByWeight()
    {
        float totalWeight = 0;
        foreach (var enemy in enemies)
        {
            totalWeight += enemy.baseSpawnWeight * currentWeightMultiplier;
        }

        float randomValue = Random.Range(0, totalWeight);
        float weightSum = 0;

        foreach (var enemy in enemies)
        {
            weightSum += enemy.baseSpawnWeight * currentWeightMultiplier;
            if (randomValue <= weightSum)
            {
                return enemy;
            }
        }

        return enemies[0];
    }

    Vector3 CalculateSpawnPosition(float playerDirection, float spawnHeight)
    {
        Vector3 spawnPos = new Vector3(
            player.position.x + currentSpawnDistance * playerDirection,
            spawnHeight,
            0
        );

        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(spawnPos);
        if (viewportPoint.x >= 0 && viewportPoint.x <= 1 && viewportPoint.y >= 0 && viewportPoint.y <= 1)
        {
            spawnPos.x += playerDirection * 5f;
        }

        return spawnPos;
    }

    void CleanupDestroyedEnemies()
    {
        activeEnemies.RemoveAll(enemy => enemy == null);
    }

    void Update()
    {
        if (player == null) return;

        UpdateDifficultyScaling();
        CleanupOffscreenEnemies();
    }

    void UpdateDifficultyScaling()
    {
        distanceTraveled = player.position.x;
        
        currentSpawnDistance = Mathf.Max(
            initialSpawnDistance - (distanceTraveled * distanceReductionRate),
            minSpawnDistance
        );

        currentWeightMultiplier = Mathf.Min(
            1f + (Mathf.Floor(distanceTraveled / distanceInterval) * weightIncreasePerInterval),
            maxWeightMultiplier
        );
    }

    void CleanupOffscreenEnemies()
    {
        float despawnDistance = currentSpawnDistance * 1.5f;
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
                continue;
            }

            float distance = activeEnemies[i].transform.position.x - player.position.x;
            if (distance < -despawnDistance)
            {
                Destroy(activeEnemies[i]);
                activeEnemies.RemoveAt(i);
            }
        }
    }
}