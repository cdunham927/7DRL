using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedRoomController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rooms"))
        {
            Debug.Log("Destroyed closed room");
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Rooms"))
        {
            Debug.Log("Destroyed closed room");
            Destroy(gameObject);
        }
    }
}
