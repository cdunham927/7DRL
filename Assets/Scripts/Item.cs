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

    //Rarity of item
    public enum Rarity { legendary = 5, rare = 15, uncommon = 30, common = 100 }
    public Rarity rarity = Rarity.common;
}
