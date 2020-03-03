﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomTemplates : MonoBehaviour
{
    //Initial spawn
    public GameObject initialSpawn;
    public bool randomStart = false;
    GameObject obj;
    public GameObject[] allRooms;

    //Rooms to link other rooms together
    public GameObject[] bottomRooms;
    public GameObject[] topRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;
    //Rooms with only 1 exit
    public GameObject bottomEndRoom;
    public GameObject topEndRoom;
    public GameObject leftEndRoom;
    public GameObject rightEndRoom;

    [Range(4, 30)]
    public int maxRooms;

    //To prevent doors to nowhere bug
    public GameObject closedRoom;

    public List<GameObject> rooms;

    //Wait for the camera to change
    public float waitTime = 4f;
    public bool spawnedBoss;
    public GameObject boss;

    public GameObject player;
    public GameObject playerBg;
    CameraController cam;


    //For showing which room is which
    public GameObject[] roomSprites;
    [Range(0, 1)]
    public float[] chancePerRoom;

    //Need to put in a certain number of rooms(1 shop per floor, at least 1 loot room, 1 bosskey room)
    public int[] maxOfRoomtype;
    //  0 - enemy     1 - loot    2 - shop      3 - bosskey     4 - boss    5 - player
    int[] numOfRoomtype;

    public AddRoom bossRoom;

    private void Awake()
    {
        numOfRoomtype = new int[(int)AddRoom.RoomTypes.len];
        cam = FindObjectOfType<CameraController>();
        if (FindObjectOfType<PlayerController>() == null) Invoke("SpawnPlayer", 0.5f);
        Invoke("SpawnBoss", waitTime);
        Invoke("SpawnBosskey", waitTime);

        //Spawn the starting room
        if (!randomStart) obj = Instantiate(initialSpawn, Vector3.zero, Quaternion.identity);
        else obj = Instantiate(allRooms[Random.Range(0, allRooms.Length)], transform.position, Quaternion.identity);

        //Get reference to current room spawned
        AddRoom curRoom = obj.GetComponent<AddRoom>();
        curRoom.roomType = AddRoom.RoomTypes.player;
        Debug.Log("Room type number: " + (int)curRoom.roomType);
        //Add to current number of rooms of that type
        AddRoomType(curRoom.roomType, obj);
        //Instantiate the rooms background for debugging
        curRoom.thisRoomsSprite = Instantiate(roomSprites[(int)curRoom.roomType], transform.position, Quaternion.identity);
    }

    public int NumOfRooms(int type)
    {
        return numOfRoomtype[type];
    }

    public void RemoveRoomType(AddRoom.RoomTypes type)
    {
        //numOfRoomtype[(int)type]--;
    }

    public void AddRoomType(AddRoom.RoomTypes type, GameObject obj)
    {
        numOfRoomtype[(int)type]++;
        rooms.Add(obj);
    }

    void SpawnPlayer()
    {
        GameObject obj = Instantiate(player, rooms[0].transform.position, Quaternion.identity);
        //Instantiate(playerBg, rooms[0].transform.position, Quaternion.identity);
        //rooms[0].GetComponent<AddRoom>().ChangeRoom(AddRoom.RoomTypes.player);
    }

    void SpawnBoss()
    {
        //Instantiate(boss, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
        //Instantiate(bossBG, rooms[rooms.Count - 1].transform.position, Quaternion.identity);
        rooms[rooms.Count - 1].GetComponent<AddRoom>().ChangeRoom(AddRoom.RoomTypes.boss);
        bossRoom = rooms[rooms.Count - 1].GetComponent<AddRoom>();
        Invoke("CameraChange", 0.5f);
    }

    void SpawnBosskey()
    {
        int x = Random.Range(1, rooms.Count - 2);
        rooms[x].GetComponent<AddRoom>().ChangeRoom(AddRoom.RoomTypes.bosskey);
    }

    void CameraChange()
    {
        cam.StartFollowingPlayer();
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            //Reload the current scene
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                bossRoom.Unlock();
            }
        }
    }
}
