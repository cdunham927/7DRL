using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item : MonoBehaviour
{
    //Name of item
    public string itemName;
    //Use item
    public virtual void Use() { }

    bool canPickup = false;

    //Rarity of item
    public enum Rarity { legendary = 5, rare = 15, uncommon = 30, common = 100 }
    public Rarity rarity = Rarity.common;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPickup)
        {
            PickupItem();
        }
    }

    public virtual void PickupItem()
    {
        //Pickup item
        //gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canPickup = false;
        }
    }
}
