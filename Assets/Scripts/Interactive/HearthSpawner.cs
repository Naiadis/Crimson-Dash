using UnityEngine;
using System.Collections.Generic;

public class HearthSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject hearthPrefab;
    public float spawnRate = 5f;
    public float minHeight = -2f;
    public float maxHeight = 2f;
    public float spawnDistanceFromPlayer = 10f;
    public Transform player;

    private float nextSpawnTime;
    private Camera mainCamera;
    private List<GameObject> activeHearts = new List<GameObject>();

    void Start()
    {
        nextSpawnTime = Time.time + spawnRate;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnHearth();
            nextSpawnTime = Time.time + spawnRate;
        }

        CleanupHearts();
    }

    void SpawnHearth()
    {
        if (player == null) return;

        Vector3 spawnPosition = new Vector3(
            player.position.x + spawnDistanceFromPlayer,
            Random.Range(minHeight, maxHeight),
            0f
        );

        GameObject newHeart = Instantiate(hearthPrefab, spawnPosition, Quaternion.identity);
        activeHearts.Add(newHeart);
    }

    void CleanupHearts()
    {
        activeHearts.RemoveAll(heart => heart == null);
        
        if (player == null) return;
        
        float despawnDistance = spawnDistanceFromPlayer * 1.5f;
        for (int i = activeHearts.Count - 1; i >= 0; i--)
        {
            if (activeHearts[i] == null) continue;

            float distance = activeHearts[i].transform.position.x - player.position.x;
            if (distance < -despawnDistance)
            {
                Destroy(activeHearts[i]);
                activeHearts.RemoveAt(i);
            }
        }
    }
}