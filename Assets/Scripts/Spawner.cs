using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs = new List<GameObject>();

    public bool spawnerActive = false;
    public bool spawnerReady = true;
    public float spawnCooldown = 1.0f;
    public float spawnTimer = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnerActive)
        {
            if (spawnerReady)
            {
                if (GameManager.Instance.CurrentRoomReference != null && GameManager.Instance.CurrentRoomReference.SpawnEnemyCheck())
                {
                    SpawnEnemy();
                    spawnTimer = 0.0f;
                    spawnerReady = false;
                }
            } else
            {
                spawnTimer = Math.Clamp(spawnTimer + Time.deltaTime, 0.0f, spawnCooldown);

                if (spawnTimer >= spawnCooldown)
                {
                    spawnerReady = true;
                }
            }
        }
    }

    public void SpawnEnemy()
    {
        Debug.Log("Spawner > Spawn Enemy");

        int randomIndex = UnityEngine.Random.Range(0, enemyPrefabs.Count);
        GameObject enemyPrefab = enemyPrefabs[randomIndex];

        if (enemyPrefab != null)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        }
       

    }
}
