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
    Animator[] doorAnim;
    [SerializeField]
    bool cleared = false;
    [SerializeField]
    bool spawned = false;
    //public Text unlockText;
    public SpriteRenderer[] banners;

    [Header("Enemy rooms")]
    public GameObject enemySpawnParent;
    int numSpawned;


    private void Awake()
    {
        templates = FindObjectOfType<RoomTemplates>();
        doorAnim = doorParent.GetComponentsInChildren<Animator>();
    }

    public void Unlock()
    {
        foreach (Animator anim in doorAnim) anim.Play("Open");
        //Maybe spawn text above the player to say that the boss door is open now
    }

    public void ChangeRoom(RoomTypes type)
    {
        templates.RemoveRoomType(type);
        roomType = type;
        Destroy(thisRoomsSprite);

        foreach (SpriteRenderer rend in banners)
        {
            rend.sprite = templates.bannerSprites[(int)roomType];
        }
        Invoke("SpawnInitial", 0.5f);
    }

    public void SpawnInitial()
    {
        if (roomType == RoomTypes.boss) foreach (Animator anim in doorAnim) anim.Play("Close");
        //Spawn the carpet and change the banners

        int choice = Random.Range(0, 3);

        foreach (SpriteRenderer rend in banners)
        {
            rend.sprite = templates.bannerSprites[(int)roomType];
        }
        if (thisRoomsSprite != null)
        {
            Destroy(thisRoomsSprite);
            thisRoomsSprite = Instantiate(templates.roomSprites[(int)roomType], transform.position, Quaternion.identity);
        }
        else thisRoomsSprite = Instantiate(templates.roomSprites[(int)roomType], transform.position, Quaternion.identity);
    }

    public void SpawnThisRoomsThing()
    {
        switch(roomType)
        {
            case (RoomTypes.enemy):
                foreach (Animator anim in doorAnim) anim.Play("Close");
                numSpawned = 0;
                for (int i = 0; i < enemySpawnParent.transform.childCount; i++)
                {
                    if (numSpawned < templates.maxEnemiesPerRoom && Random.value < templates.enemySpawnChance)
                    {
                        numSpawned++;
                        GameObject obj = Instantiate(templates.enemy, enemySpawnParent.transform.GetChild(i).transform.position, Quaternion.identity);
                    }
                }
                spawned = true;
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
                //PlayerController.player.transform.position = transform.position;
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
        foreach (Animator anim in doorAnim) anim.Play("Open");
        cleared = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!spawned && collision.CompareTag("Player"))
        {
            SpawnThisRoomsThing();
        }
    }
}
