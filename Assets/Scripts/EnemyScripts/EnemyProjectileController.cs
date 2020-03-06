using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileController : MonoBehaviour
{
    public float spd;
    Rigidbody2D bod;

    public float atk = 1;
    public Weapon.weaponEffect effect;
    public float pot = 0;

    private void Awake()
    {
        bod = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        bod.AddForce(transform.up * spd);
        Invoke("Disable", 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.player.TakeDamage(1, effect);
            Invoke("Disable", 0.001f);
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
