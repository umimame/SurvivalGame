using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject ApplePrefab;
    public GameObject GrapePrefab;
    public GameObject OrangePrefab;
    public int totalItems = 50;
    public float spawnInterval = 30f;
    public Vector2 spawnArea = new Vector2(10f, 5f);

    private void Start()
    {
        InvokeRepeating("SpawnItems", 0f, spawnInterval);
    }

    void SpawnItems()
    {
        for (int i = 0; i < totalItems; i++)
        {
            SpawnItem(OrangePrefab);
            SpawnItem(GrapePrefab);
            SpawnItem(ApplePrefab);
        }
    }

    void SpawnItem(GameObject prefab)
    {
        float randomX = Random.Range(-spawnArea.x / 2, spawnArea.x / 2);
        float randomZ = Random.Range(-spawnArea.y / 2, spawnArea.y / 2);

        Vector3 spawnPosition = new Vector3(randomX, 10f, randomZ);
        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}