using System;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public bool canSpawn = true;
    public float spawnCooldown = 1.0f;
    public float spawnTimer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!canSpawn)
        {
            spawnTimer = Math.Clamp(spawnTimer + Time.deltaTime, 0.0f, spawnCooldown);

            if (spawnTimer >= spawnCooldown)
            {
                canSpawn = true;
            }

        }
    }

    public bool SpawnEnemy(GameObject enemyPrefab)
    {
        if (!canSpawn) return false;

        Instantiate(enemyPrefab, transform.position, Quaternion.identity);

        canSpawn = false;
        spawnTimer = 0.0f;
        return true;
    }
}
