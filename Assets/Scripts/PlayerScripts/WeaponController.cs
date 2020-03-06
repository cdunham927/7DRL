using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon curWeapon;
    public float dmgModifier;
    Animator anim;
    Animator playerAnim;
    float cools = 0f;
    public float potencyMod;

    ItemHolder itemHolder;
    [HideInInspector]
    public bool attacking = false;

    public GameObject arrow;

    //Sounds
    AudioSource src;
    public AudioClip swing;
    public AudioClip stab;

    void Awake()
    {
        playerAnim = PlayerController.player.GetComponent<Animator>();
        anim = GetComponent<Animator>();
        src = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        PlayerController.player.ResetEverything();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && cools <= 0f)
        {
            switch (curWeapon.type)
            {
                case (Weapon.weaponTypes.sword):
                    attacking = true;
                    playerAnim.SetBool("Attacking", true);
                    Sword();
                    break;
                case (Weapon.weaponTypes.dagger):
                    attacking = true;
                    playerAnim.SetBool("Attacking", true);
                    Dagger();
                    break;
                case (Weapon.weaponTypes.spear):
                    attacking = true;
                    playerAnim.SetBool("Attacking", true);
                    Spear();
                    break;
                case (Weapon.weaponTypes.axe):
                    attacking = true;
                    playerAnim.SetBool("Attacking", true);
                    Axe();
                    break;
                case (Weapon.weaponTypes.bow):
                    attacking = true;
                    playerAnim.SetBool("Attacking", true);
                    Bow();
                    break;
            }
        }

        if (cools > 0) cools -= Time.deltaTime;

        if (cools <= 0) attacking = false;

        if (Application.isEditor)
        {

        }
    }

    public void SwitchWeapon(Weapon weap)
    {
        if (curWeapon != null)
        {
            //Put my previous weapon where I picked the other one up
            if (PlayerController.player.curEquip != null)
            {
                if (PlayerController.player.curEquip.type == Equipment.equipmentTypes.amulets)
                {
                    if (PlayerController.player.curEquip.effect == curWeapon.effect) curWeapon.potency -= PlayerController.player.curEquip.modAmt;
                }
            }
            curWeapon.gameObject.SetActive(true);
            curWeapon.transform.position = weap.transform.position;
        }
        //Equip new weapon
        curWeapon = weap;
        curWeapon.gameObject.SetActive(false);
        PlayerController.player.UpdateUI(0);
        if (PlayerController.player.curEquip != null)
        {
            if (PlayerController.player.curEquip.type == Equipment.equipmentTypes.amulets)
            {
                PlayerController.player.curEquip.Use();
            }
        }
    }
    void ResetAttacking()
    {
        playerAnim.SetBool("Attacking", false);
    }

    void Sword()
    {
        src.PlayOneShot(swing);
        anim.Play("Sword");
        cools = curWeapon.cooldown;
        Invoke("ResetAttacking", 0.25f);
    }

    void Dagger()
    {
        src.PlayOneShot(stab);
        cools = curWeapon.cooldown;
    }

    void Spear()
    {

    }

    void Axe()
    {

    }

    void Bow()
    {
        //anim.Play("Bow");
        Vector3 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        ProjectileController arr = Instantiate(arrow, transform.position, transform.rotation).GetComponent<ProjectileController>();
        cools = curWeapon.cooldown;
        Invoke("ResetAttacking", 0.25f);
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
