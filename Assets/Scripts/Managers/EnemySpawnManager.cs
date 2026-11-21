using UnityEngine;
using System.Collections.Generic;

public class Room
{
    public bool enemyStatus;
    public EnemySpawner[] spawners;
    public int index;
}

public class EnemySpawnManager : MonoBehaviour
{
    
    private int[] enemyStatus;

    private GameObject[] roomPrefabs;
    private Room[] rooms;
    private List<int> selectedRooms = new List<int>();
    RoomManager rm;

    void Awake()
    {
        rm = (GameObject.Find("RoomManager")).GetComponent<RoomManager>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int current = rm.GetRoomNum();
        rooms[current].enemyStatus = RoomEnemyStatus(current);
    }

    public void AddRooms()
    {
        for(int i = 0; i < roomPrefabs.Length; i++)
        {
            rooms[i] = new Room();
            rooms[i].index = i;
            rooms[i].enemyStatus = true;
            //GameObject temp = roomPrefabs[i].Find("EnemySpawnPoints");
            rooms[i].spawners = roomPrefabs[i].GetComponentsInChildren<EnemySpawner>();
        }
    }

    public void RoomSetup(GameObject[] prefabs)
    {
        roomPrefabs = prefabs;
        rooms = new Room[prefabs.Length];
        Debug.Log("Done Setting Up");
    }

    public bool RoomEnemyStatus(int currentRoom)
    {
        EnemySpawner[] temp = rooms[currentRoom].spawners;
        for (int j = 0; j < temp.Length; j++)
        {
            if (temp[j].GetStatus() == true)
            {
                return true;
            }
        }

        return false;
    }

    public void Spawn(int currentRoom)
    {
        Room r = rooms[currentRoom];
        if (r.enemyStatus == true)
        {
            for(int k = 0; k < r.spawners.Length; k++)
            {
                r.spawners[k].SpawnEnemy();
            }
        }
    }
}
