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

    private int totalEnemies = 10;
    private int enemiesCleared = 0;
    private List<Spawner> spawners;
    public bool isCleared;
    public bool isActive;
    private Collider2D roomTrigger;
    private List<Door> doors;

    [SerializeField]
    public RoomType roomType;


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

    //emergency
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MusicManager.Instance.SetRoomType(1.0f);
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
                isActive = false;
                MusicManager.Instance.SetRoomType(1.0f);
            }

            //spawning enemy logic


        }
    }
}
