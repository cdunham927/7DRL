using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    //List of all the different items
    public List<Item> allItemsList = new List<Item>();
    public List<Item> commonItemsList;
    public List<Item> uncommonItemsList;
    public List<Item> rareItemsList;
    public List<Item> legendaryItemsList;
    //Should have more lists for certain item types(Ex. list of all poison items)
    public List<Item> commonWeaponsList;
    public List<Item> uncommonWeaponsList;
    public List<Item> rareWeaponsList;
    public List<Item> legendaryWeaponsList;

    private void Awake()
    {
        commonItemsList = new List<Item>();
        uncommonItemsList = new List<Item>();
        rareItemsList = new List<Item>();
        legendaryItemsList = new List<Item>();
        commonWeaponsList = new List<Item>();
        uncommonWeaponsList = new List<Item>();
        rareWeaponsList = new List<Item>();
        legendaryWeaponsList = new List<Item>();

        //Add the items to lists respective to their rarity
        foreach (Item it in allItemsList)
        {
            if (it.rarity == Item.Rarity.common) commonItemsList.Add(it);
            else if (it.rarity == Item.Rarity.uncommon) uncommonItemsList.Add(it);
            else if (it.rarity == Item.Rarity.rare) rareItemsList.Add(it);
            else if (it.rarity == Item.Rarity.legendary) legendaryItemsList.Add(it);

            if (it.GetComponent<Weapon>() != null)
            {
                if (it.rarity == Item.Rarity.common) commonWeaponsList.Add(it);
                else if (it.rarity == Item.Rarity.uncommon) uncommonWeaponsList.Add(it);
                else if (it.rarity == Item.Rarity.rare) rareWeaponsList.Add(it);
                else if (it.rarity == Item.Rarity.legendary) legendaryWeaponsList.Add(it);
            }
        }
    }

    public Item GetItem()
    {
        int ind = Random.Range(0, allItemsList.Count);
        return allItemsList[ind];
    }

    public Item GetItem(Item.Rarity rarity)
    {
        int ind;
        if (rarity == Item.Rarity.common)
        {
            ind = Random.Range(0, commonItemsList.Count - 1);
            return commonItemsList[ind];
        }
        else if (rarity == Item.Rarity.uncommon)
        {
            ind = Random.Range(0, uncommonItemsList.Count - 1);
            return uncommonItemsList[ind];
        }
        else if (rarity == Item.Rarity.rare)
        {
            ind = Random.Range(0, rareItemsList.Count - 1);
            return rareItemsList[ind];
        }
        ind = Random.Range(0, legendaryItemsList.Count - 1);
        return legendaryItemsList[ind];
    }

    public Item GetWeapon(Item.Rarity rarity)
    {
        int ind;
        if (rarity == Item.Rarity.common)
        {
            ind = Random.Range(0, commonItemsList.Count - 1);
            return commonWeaponsList[ind];
        }
        else if (rarity == Item.Rarity.uncommon)
        {
            ind = Random.Range(0, uncommonItemsList.Count - 1);
            return uncommonWeaponsList[ind];
        }
        else if (rarity == Item.Rarity.rare)
        {
            ind = Random.Range(0, rareItemsList.Count - 1);
            return rareWeaponsList[ind];
        }
        ind = Random.Range(0, legendaryItemsList.Count - 1);
        return legendaryWeaponsList[ind];
    }

    public void RemoveItem(Item it)
    {
        if (it.rarity == Item.Rarity.common)
        {
            allItemsList.Remove(it);
            commonItemsList.Remove(it);
        }
        else if (it.rarity == Item.Rarity.uncommon)
        {
            allItemsList.Remove(it);
            uncommonItemsList.Remove(it);
        }
        else if (it.rarity == Item.Rarity.rare)
        {
            allItemsList.Remove(it);
            rareItemsList.Remove(it);
        }
        else if (it.rarity == Item.Rarity.legendary)
        {
            allItemsList.Remove(it);
            legendaryItemsList.Remove(it);
        }
    }
}
