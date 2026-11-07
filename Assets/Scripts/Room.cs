using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum RoomType
    {
        Spawn,
        Combat,
        Shop,
        Boss
    }

    public int totalEnemies = 10;
    public int enemiesSpawned = 0;
    public int enemiesCleared = 0;

    public bool isCleared;
    public bool isActive;
    private Gate[] gates;
    private Spawner[] spawners;

    [SerializeField]
    public RoomType roomType;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        if (roomType == RoomType.Shop || roomType == RoomType.Spawn)
        {
            isCleared = true;
        }

        if (roomType == RoomType.Shop || roomType == RoomType.Combat)
        {
            gates = transform.GetChild(1).transform.GetComponentsInChildren<Gate>()
           .Where(gate => gate.AlwaysActive == false).ToArray();
        }

        if (roomType == RoomType.Combat)
        {
            spawners = transform.GetChild(2).transform.GetComponentsInChildren<Spawner>();
        }
    }

    private void LockDoors()
    {
        foreach (var gate in gates)
        {
            gate.gameObject.SetActive(true);
        }
    }

    private void UnlockDoors()
    {
        foreach (var gate in gates)
        {
            gate.gameObject.SetActive(false);
        }
    }

    private void ActivateSpawners()
    {
        foreach (var spwn in spawners)
        {
            spwn.spawnerActive = true;
        }
    }

    private void DeactivateSpawners()
    {
        foreach (var gate in gates)
        {
            spwn.spawnerActive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (!isCleared)
        {
            if (collision.CompareTag("Player"))
            {

                Debug.Log("Activating Room");

                isActive = true;
                LockDoors();
                ActivateSpawners();

                if (roomType == RoomType.Shop)
                {
                    MusicManager.Instance.SetRoomType(0.0f);
                }
                else if (roomType == RoomType.Combat)
                {
                    MusicManager.Instance.SetRoomType(2.0f);
                }
            }
        }

        GameManager.Instance.CurrentRoomReference = this;
    }

    //emergency
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MusicManager.Instance.SetRoomType(1.0f);
        }

        GameManager.Instance.CurrentRoomReference = null;
    }

    public bool SpawnEnemyCheck()
    {
        if (enemiesSpawned <= totalEnemies)
        {
            enemiesSpawned++;
            return true;
        }
        
        return false;
    }


    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (enemiesCleared >= totalEnemies)
            {
                isCleared = true;
                UnlockDoors();
                DeactivateSpawners();
                isActive = false;
                MusicManager.Instance.SetRoomType(1.0f);
            }
        }
    }
}
