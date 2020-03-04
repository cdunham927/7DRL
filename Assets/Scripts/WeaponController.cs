using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon curWeapon;
    public float dmgModifier;
    Animator anim;
    float cools = 0f;

    ItemHolder itemHolder;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        itemHolder = FindObjectOfType<ItemHolder>();
        if (curWeapon == null)
        {
            curWeapon = (Weapon)itemHolder.GetWeapon(Item.Rarity.common);
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && cools <= 0f)
        {
            switch (curWeapon.type)
            {
                case (Weapon.weaponTypes.sword):
                    Sword();
                    break;
                case (Weapon.weaponTypes.dagger):
                    Dagger();
                    break;
                case (Weapon.weaponTypes.spear):
                    Spear();
                    break;
                case (Weapon.weaponTypes.axe):
                    Axe();
                    break;
                case (Weapon.weaponTypes.bow):
                    Bow();
                    break;
            }
        }

        if (cools > 0) cools -= Time.deltaTime;

        if (Application.isEditor)
        {

        }
    }

    public void SwitchWeapon(Weapon weap)
    {
        if (curWeapon != null)
        {
            //Put my previous weapon where I picked the other one up

        }
        //Equip new weapon
        curWeapon = weap;
    }

    void Sword()
    {
        anim.Play("Sword");
        cools = curWeapon.cooldown;
    }

    void Dagger()
    {

    }

    void Spear()
    {

    }

    void Axe()
    {

    }

    void Bow()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyController>().TakeDamage(curWeapon.effect, curWeapon.potency, curWeapon.atk, dmgModifier);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyController>().TakeDamage(curWeapon.effect, curWeapon.potency, curWeapon.atk, dmgModifier);
        }
    }
}
