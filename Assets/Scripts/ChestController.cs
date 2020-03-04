using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    bool canOpen = false;
    public Item.Rarity itRarity = Item.Rarity.common;
    public Item it;
    Animator anim;
    ItemHolder items;
    bool opened = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        items = FindObjectOfType<ItemHolder>();
        it = Instantiate(items.GetItem(itRarity), transform.position, Quaternion.identity);
        it.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canOpen && !opened)
        {
            anim.Play("Open");
            Invoke("ActivateItem", 0.5f);
            opened = true;
        }
    }

    void ActivateItem()
    {
        it.gameObject.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canOpen = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canOpen = false;
        }
    }
}
