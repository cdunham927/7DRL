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
}
