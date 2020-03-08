using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeController : MonoBehaviour
{
    public Weapon.weaponTypes weaponType = Weapon.weaponTypes.sword;
    public Weapon.weaponEffect effect = Weapon.weaponEffect.none;
    public float atk = 1;
    public float pot = 0;
    public Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.player.TakeDamage(atk, effect, pot);
        }
    }
}
