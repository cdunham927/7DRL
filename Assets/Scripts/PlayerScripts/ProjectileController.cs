using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float spd;
    Rigidbody2D bod;

    public float atk = 1;
    public Weapon.weaponEffect effect;
    [HideInInspector]
    public float pot;

    [HideInInspector]
    public float atkMod = 0;
    [HideInInspector]
    public float dmgModifier = 0;

    private void Awake()
    {
        bod = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        bod.AddForce(-transform.up * spd);
        Invoke("Disable", 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyController>().TakeDamage(effect, pot, atk + (atk * atkMod), dmgModifier);
            Invoke("Disable", 0.001f);
        }

        if (collision.CompareTag("Wall"))
        {
            Invoke("Disable", 0.001f);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyController>().TakeDamage(effect, pot, atk + (atk * atkMod), dmgModifier);
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
