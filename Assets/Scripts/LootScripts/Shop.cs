using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public GameObject[] itemSpawns;
    ItemHolder items;
    Item it;

    private void Awake()
    {
        items = FindObjectOfType<ItemHolder>();
        Invoke("GetItems", 0.25f);
    }

    public void GetItems()
    {
        for (int i = 0; i < itemSpawns.Length; i++)
        {
            int x = Random.Range(0, 100);
            //Get item with random rarity
            if (x < (int)Item.Rarity.legendary)
            {
                it = Instantiate(items.GetItem(Item.Rarity.legendary), itemSpawns[i].transform.position, Quaternion.identity);
                it.buying = true;
                it.transform.SetParent(transform);
            }
            else if (x < (int)Item.Rarity.rare)
            {
                it = Instantiate(items.GetItem(Item.Rarity.rare), itemSpawns[i].transform.position, Quaternion.identity);
                it.buying = true;
                it.transform.SetParent(transform);
            }
            else if (x < (int)Item.Rarity.uncommon)
            {
                it = Instantiate(items.GetItem(Item.Rarity.uncommon), itemSpawns[i].transform.position, Quaternion.identity);
                it.buying = true;
                it.transform.SetParent(transform);
            }
            else
            {
                it = Instantiate(items.GetItem(Item.Rarity.common), itemSpawns[i].transform.position, Quaternion.identity);
                it.buying = true;
                it.transform.SetParent(transform);
            }
        }
    }
}
