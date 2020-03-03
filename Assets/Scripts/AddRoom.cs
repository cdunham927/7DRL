using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    RoomTemplates templates;
    public enum RoomTypes { enemy, loot, shop, bosskey, boss, player, len }
    [Header("Type of room")]
    public RoomTypes roomType = RoomTypes.enemy;

    [HideInInspector]
    public GameObject thisRoomsSprite;

    //Door to close when the player enters the room
    [Header("For locking and clearing rooms")]
    public GameObject doorParent;
    [SerializeField]
    bool cleared = false;
    public GameObject lockParent;
    //public Text unlockText;

    [Header("Enemy rooms")]
    public GameObject enemySpawnParent;


    private void Awake()
    {
        templates = FindObjectOfType<RoomTemplates>();
    }

    public void Unlock()
    {
        lockParent.SetActive(false);
        //Maybe spawn text above the player to say that the boss door is open now
    }

    public void ChangeRoom(RoomTypes type)
    {
        templates.RemoveRoomType(type);
        roomType = type;
        Destroy(thisRoomsSprite);
        if (roomType == RoomTypes.boss) lockParent.SetActive(true);
        thisRoomsSprite = Instantiate(templates.roomSprites[(int)roomType], transform.position, Quaternion.identity);
    }

    public void SpawnThisRoomsThing()
    {
        switch(roomType)
        {
            case (RoomTypes.enemy):
                doorParent.SetActive(true);
                break;
            case (RoomTypes.loot):

                break;
            case (RoomTypes.bosskey):
                doorParent.SetActive(true);
                break;
            case (RoomTypes.shop):

                break;
            case (RoomTypes.boss):
                doorParent.SetActive(true);
                break;
            //case (RoomTypes.challenge):
                //doorParent.SetActive(true);
                //break;
            //case (RoomTypes.healing):
                //doorParent.SetActive(true);
                //break;
            case (RoomTypes.player):
                PlayerController.player.transform.position = transform.position;
                break;
        }
    }

    private void Update()
    {
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                ClearRoom();
            }
        }
    }

    public void ClearRoom()
    {
        //Big switch case in here for what to do upon clearing certain rooms
        switch(roomType)
        {

        }
        doorParent.SetActive(false);
        cleared = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!cleared && collision.CompareTag("Player"))
        {
            SpawnThisRoomsThing();
        }
    }
}
