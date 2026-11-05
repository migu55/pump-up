using System;
using System.Collections.Generic;
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
        Debug.Log("Room Trigger: " + roomTrigger);
    }

    private void LockDoors()
    {

    }

    private void UnlockDoors()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log("Trigger Active");

        if (!isCleared)
        {
            if (collision.CompareTag("Player"))
            {

                Debug.Log("Activating Room");

                isActive = true;
                LockDoors();

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

                MusicManager.Instance.SetRoomType(1.0f);
            }

            //spawning enemy logic


        }
    }
}
