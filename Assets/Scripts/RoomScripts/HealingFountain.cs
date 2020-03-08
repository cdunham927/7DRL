using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingFountain : MonoBehaviour
{
    bool hasHealed = false;
    bool canUseHeal = false;
    public float healAmt = 25;
    public Sprite emptySprite;
    public SpriteRenderer rend;

    AudioSource src;
    public AudioClip clip;

    private void Update()
    {
        if (canUseHeal && Input.GetKeyDown(KeyCode.E) && !hasHealed)
        {
            PlayerController.player.Heal(healAmt);
            hasHealed = true;
            src.PlayOneShot(clip);
            rend.sprite = emptySprite;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canUseHeal = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canUseHeal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canUseHeal = false;
        }
    }
}
