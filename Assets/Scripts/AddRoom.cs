using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    RoomTemplates templates;
    public enum RoomTypes { enemy, loot, shop, bosskey, boss, player, len }
    public RoomTypes roomType = RoomTypes.enemy;

    [HideInInspector]
    public GameObject thisRoomsSprite;

    private void Awake()
    {
        templates = FindObjectOfType<RoomTemplates>();
    }

    public void ChangeRoom(RoomTypes type)
    {
        templates.RemoveRoomType(type);
        roomType = type;
        Destroy(thisRoomsSprite);
        thisRoomsSprite = Instantiate(templates.roomSprites[(int)roomType], transform.position, Quaternion.identity);
    }

    public void SpawnThisRoomsThing()
    {

    }
}
