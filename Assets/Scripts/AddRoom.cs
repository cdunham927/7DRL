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

    //For enemies in enemy rooms
    public List<EnemyController> enemiesInRoom;

    [Header("Shop rooms")]
    public GameObject shopParent;
    Item[] shopChildren;

    [Header("Boss and bosskey rooms")]
    public GameObject bossSpawn;
    
    private void Awake()
    {
        templates = FindObjectOfType<RoomTemplates>();
        doorAnim = doorParent.GetComponentsInChildren<Animator>();
    }

    public void RemoveFromRoom(EnemyController enem)
    {
        enemiesInRoom.Remove(enem);
        if (enemiesInRoom.Count <= 0) ClearRoom();
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
        if (roomType == RoomTypes.loot)
        {
            int x = Random.Range(0, 100);
            if (x < (int)Item.Rarity.legendary) Instantiate(templates.chests[0], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            else if (x < (int)Item.Rarity.rare) Instantiate(templates.chests[1], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            else if (x < (int)Item.Rarity.uncommon) Instantiate(templates.chests[2], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
            else Instantiate(templates.chests[3], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
        if (roomType == RoomTypes.shop) shopParent.SetActive(true);
        
        //Spawn the carpet and change the banners
        //For if we want random carpet in the rooms
        //int choice = Random.Range(0, 3);

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
                        GameObject a = Instantiate(templates.enemy[Random.Range(0, templates.enemy.Length)], enemySpawnParent.transform.GetChild(i).transform.position, Quaternion.identity);
                        EnemyController b = a.GetComponent<EnemyController>();
                        b.SetRoom(this);
                        enemiesInRoom.Add(b);
                    }
                }
                spawned = true;
                break;
            case (RoomTypes.loot):
                break;
            case (RoomTypes.bosskey):
                foreach (Animator anim in doorAnim) anim.Play("Close");
                //Spawn miniboss
                GameObject c = Instantiate(templates.enemy[Random.Range(0, templates.enemy.Length)], bossSpawn.transform.position, Quaternion.identity);
                EnemyController d = c.GetComponent<EnemyController>();
                d.SetRoom(this);
                enemiesInRoom.Add(d);
                break;
            case (RoomTypes.shop):
                break;
            case (RoomTypes.boss):
                foreach (Animator anim in doorAnim) anim.Play("Close");
                //Spawn boss
                GameObject e = Instantiate(templates.enemy[Random.Range(0, templates.enemy.Length)], bossSpawn.transform.position, Quaternion.identity);
                EnemyController f = e.GetComponent<EnemyController>();
                f.SetRoom(this);
                enemiesInRoom.Add(f);
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
            case (RoomTypes.bosskey):
                templates.bossRoom.Unlock();
                break;
            case (RoomTypes.boss):
                //Boss rooms should spawn exit to next floor
                //Should probably have a gameobject that tells it where to spawn
                Instantiate(templates.exit, transform.position + new Vector3(0, 2f, 0), transform.rotation);
                //Spawn chest 
                int x = Random.Range(0, 100);
                if (x < (int)Item.Rarity.legendary) Instantiate(templates.chests[0], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                else if (x < (int)Item.Rarity.rare) Instantiate(templates.chests[1], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                else if (x < (int)Item.Rarity.uncommon) Instantiate(templates.chests[2], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                else Instantiate(templates.chests[3], transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);

                break;
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
