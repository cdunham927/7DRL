using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Weapon curWeapon;
    public float dmgModifier;
    Animator anim;
    Animator playerAnim;
    [SerializeField]
    float cools = 0f;
    public float potencyMod;
    public float atkMod = 0.20f;

    ItemHolder itemHolder;
    [HideInInspector]
    public bool attacking = false;

    public GameObject arrow;

    //Sprite renderer
    SpriteRenderer weaponRend;
    SpriteRenderer weapR;

    //Sounds
    AudioSource src;
    public AudioClip swing;
    public AudioClip stab;
    public AudioClip shoot;

    bool canDamage = true;

    void Awake()
    {
        weaponRend = GetComponent<SpriteRenderer>();
        playerAnim = transform.parent.GetComponent<Animator>();
        anim = GetComponent<Animator>();
        src = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && cools <= 0f)
        {
            cools = curWeapon.cooldown;
            //Debug.Log("Attacking");
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

        if (curWeapon != null) anim.SetInteger("Weapon", (int)curWeapon.type);

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
        weapR = curWeapon.GetComponent<SpriteRenderer>();
        weaponRend.color = weapR.color;

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
        src.volume = 0.45f;
        src.PlayOneShot(swing);
        anim.Play("Sword");
        Invoke("ResetAttacking", 0.25f);
    }

    void Dagger()
    {
        src.volume = 0.6f;
        src.PlayOneShot(stab);
        anim.Play("Dagger");
        Invoke("ResetAttacking", 0.25f);
    }

    void Spear()
    {
        src.volume = 0.8f;
        src.PlayOneShot(stab);
        anim.Play("Spear");
        Invoke("ResetAttacking", 0.25f);
    }

    void Axe()
    {

    }

    void Bow()
    {
        src.volume = 0.6f;
        src.PlayOneShot(shoot);
        //anim.Play("Bow");
        //Set variables
        ProjectileController arr = Instantiate(arrow, transform.position, transform.rotation).GetComponent<ProjectileController>();
        arr.GetComponent<SpriteRenderer>().color = weaponRend.color;
        arr.effect = curWeapon.effect;
        arr.pot = curWeapon.potency;
        arr.atkMod = atkMod;
        arr.atk = curWeapon.atk;
        arr.dmgModifier = dmgModifier;

        Invoke("ResetAttacking", 0.25f);
    }

    private void CanDamage()
    {
        canDamage = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            //col.GetComponent<EnemyController>().TakeDamage(curWeapon.effect, curWeapon.potency, curWeapon.atk + (curWeapon.atk * atkMod), dmgModifier);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.GetComponent<EnemyController>().TakeDamage(curWeapon.effect, curWeapon.potency, curWeapon.atk + (curWeapon.atk * atkMod), dmgModifier);
        }
    }
}
