using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum RoomType
    {
        Combat,
        Shop,
        Boss
    }

    private int totalEnemies;
    private int enemiesCleared;
    private List<Spawner> spawners;
    public bool isCleared;
    public bool isActive;
    private Collider2D roomTrigger;
    private List<Door> doors;
    private RoomType roomType;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        roomTrigger = GetComponent<Collider2D>();
    }

    private void LockDoors()
    {

    }

    private void UnlockDoors()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isCleared)
        {
            if (collision.CompareTag("Player"))
            {
                isActive = true;
                LockDoors();
            }
        }

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
            }

            //spawning enemy logic


        }
    }
}
