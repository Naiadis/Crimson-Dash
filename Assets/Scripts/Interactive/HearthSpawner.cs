using UnityEngine;

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

    void Start()
    {
        nextSpawnTime = Time.time + spawnRate;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnHearth();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnHearth()
    {
        if (player == null) return;

        Vector3 spawnPosition = new Vector3(
            player.position.x + spawnDistanceFromPlayer,
            Random.Range(minHeight, maxHeight),
            0f
        );

        Instantiate(hearthPrefab, spawnPosition, Quaternion.identity);
    }
}