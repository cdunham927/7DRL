using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1 --> need bottom door
    // 2 --> need top door
    // 3 --> need left door
    // 4 --> need right door
    public bool spawned = false;

    public float waitTime = 4f;

    RoomTemplates templates;
    public GameObject initialSpawn;

    GameObject obj;
    AddRoom curRoom;

     void Awake()
    {
        //Destroy spawns after they've done their part
        Destroy(gameObject, waitTime);
        templates = FindObjectOfType<RoomTemplates>();
        if (!spawned) Invoke("Spawn", Random.Range(0.05f, 0.275f));
    }

    void Spawn()
    {
        if (spawned == false)
        {
            if (openingDirection == 1)
            {
                if (templates.rooms.Count < templates.maxRooms)
                {
                    //Need to spawn a room with a bottom door
                    obj = Instantiate(templates.bottomRooms[Random.Range(0, templates.bottomRooms.Length)], transform.position, Quaternion.identity);
                    //If the spawnpoints spawn enemies too we can call that function from here
                    //obj.GetComponent<EnemySpawner>().SpawnThings()
                    //That, or the EnemySpawner could be called in OnEnable of whatever gameobject its attached to
                }
                else
                {
                    obj = Instantiate(templates.bottomEndRoom, transform.position, Quaternion.identity);
                }
            }
            else if (openingDirection == 2)
            {
                if (templates.rooms.Count < templates.maxRooms)
                {
                    //Need to spawn a room with a top door
                    obj = Instantiate(templates.topRooms[Random.Range(0, templates.topRooms.Length)], transform.position, Quaternion.identity);
                }
                else
                {
                    obj = Instantiate(templates.topEndRoom, transform.position, Quaternion.identity);
                }
            }
            else if (openingDirection == 3)
            {
                if (templates.rooms.Count < templates.maxRooms)
                {
                    //Need to spawn a room with a left door
                    obj = Instantiate(templates.leftRooms[Random.Range(0, templates.leftRooms.Length)], transform.position, Quaternion.identity);
                }
                else
                {
                    obj = Instantiate(templates.leftEndRoom, transform.position, Quaternion.identity);
                }
            }
            else if (openingDirection == 4)
            {
                if (templates.rooms.Count < templates.maxRooms)
                {
                    //Need to spawn a room with a right door
                    obj = Instantiate(templates.rightRooms[Random.Range(0, templates.rightRooms.Length)], transform.position, Quaternion.identity);
                }
                else
                {
                    obj = Instantiate(templates.rightEndRoom, transform.position, Quaternion.identity);
                }
            }

            templates = FindObjectOfType<RoomTemplates>();

            //Pick a procedural room type
            Debug.Log((int)AddRoom.RoomTypes.len);
            //int n = Random.Range(0, (int)RoomTypes.len - 3);
            float ran = Random.value;
            for (int i = 0; i < (int)AddRoom.RoomTypes.len - 3; i++)
            {
                if (ran < templates.chancePerRoom[i])
                {
                    Debug.Log("Spawning a room: " + i);
                    if (templates.maxOfRoomtype[i] > templates.NumOfRooms(i))
                    {
                        obj.GetComponent<AddRoom>().roomType = (AddRoom.RoomTypes)i;
                        Debug.Log((AddRoom.RoomTypes)i + " Room");
                        break;
                    }
                }
            }
            //Get reference to current room spawned
            curRoom = obj.GetComponent<AddRoom>();
            //Add to current number of rooms of that type
            templates.AddRoomType(curRoom.roomType, obj);
            //Instantiate the rooms background for debugging
            curRoom.thisRoomsSprite = Instantiate(templates.roomSprites[(int)curRoom.roomType], transform.position, Quaternion.identity);
            spawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rooms"))
        {
            spawned = true;
            //Destroy(gameObject);
        }

        if (collision.CompareTag("Spawn"))
        {
            if (collision.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                //Spawn wall blocking off any openings
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity);
                //Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
