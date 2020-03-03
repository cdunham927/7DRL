using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Add to players key count or just unlock the boss door
            RoomTemplates temp = FindObjectOfType<RoomTemplates>();
            temp.bossRoom.Unlock();
            Destroy(gameObject);
        }
    }
}
