﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Item : MonoBehaviour
{
    //Name of item
    public string itemName;
    public string itemDescription;
    //Use item
    public virtual void Use() { }
    public TextMeshProUGUI itemText;
    public Image textImg;

    bool canPickup = false;

    //Rarity of item
    public enum Rarity { legendary = 5, rare = 15, uncommon = 30, common = 100 }
    public Rarity rarity = Rarity.common;

    //For buying items from the shop
    public bool buying = false;
    public float cost = 1;
    //Text to show
    protected string tString;

    private void Awake()
    {
        itemText = GetComponentInChildren<TextMeshProUGUI>();
        textImg = GetComponentInChildren<Image>();
        SetText();
        if (itemText != null) itemText.text = tString;
        switch(rarity)
        {
            case (Rarity.common):
                textImg.color = new Color(1, 0, 0, 0.25f);
                break;
            case (Rarity.uncommon):
                textImg.color = new Color(0, 1, 0, 0.25f);
                break;
            case (Rarity.rare):
                textImg.color = new Color(0, 0, 1, 0.25f);
                break;
            case (Rarity.legendary):
                textImg.color = new Color(1, 1, 0, 0.25f);
                break;
        }
    }

    public virtual void SetText() { }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canPickup)
        {
            PickupItem();
        }

        itemText.transform.parent.gameObject.SetActive(canPickup);
    }

    public virtual void PickupItem() { }

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