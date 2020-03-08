using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyController : EnemyController
{
    public Weapon.weaponTypes type = Weapon.weaponTypes.sword;
    public EnemyMeleeController weapon;

    private void Awake()
    {
        weapon.anim.SetInteger("Weapon", (int)type);
    }

    public override void Idle()
    {

    }

    public override void Chase()
    {
        float distance = ((transform.position - PlayerController.player.transform.position).sqrMagnitude);
        //Debug.Log("Distance: " + distance.ToString() + " of " + (attackDistance * attackDistance).ToString());

        //Move towards the player
        if (distance > attackDistance * attackDistance)
        {
            //Right
            if (PlayerController.player.transform.position.x > (transform.position.x - 1f))
            {
                anim.transform.rotation = Quaternion.Euler(0, 0, 90);
                bod.AddForce(Vector2.right * curSpd * Time.deltaTime);
            }
            //Left
            else if (PlayerController.player.transform.position.x < (transform.position.x + 1f))
            {
                anim.transform.rotation = Quaternion.Euler(0, 0, -90);
                bod.AddForce(-Vector2.right * curSpd * Time.deltaTime);
            }
            //Down
            if (PlayerController.player.transform.position.y < transform.position.y)
            {
                anim.transform.rotation = Quaternion.Euler(0, 0, 0);
                bod.AddForce(-Vector2.up * curSpd * Time.deltaTime);
            }
            //Up
            else if (PlayerController.player.transform.position.y > transform.position.y)
            {
                anim.transform.rotation = Quaternion.Euler(0, 0, 180);
                bod.AddForce(Vector2.up * curSpd * Time.deltaTime);
            }
        }

        if (Mathf.Abs(Mathf.Abs(PlayerController.player.transform.position.x) - Mathf.Abs(transform.position.x)) < 0.75f)
        {
            if (PlayerController.player.transform.position.y < transform.position.y)
            {
                anim.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else anim.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (Mathf.Abs(Mathf.Abs(PlayerController.player.transform.position.y) - Mathf.Abs(transform.position.y)) < 0.75f)
        {
            if (PlayerController.player.transform.position.x < transform.position.x)
            {
                anim.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else anim.transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        //In range of player
        if (distance < attackDistance * attackDistance && attackCools <= 0f)
        {
            ChangeState(states.attack);
        }
    }
    void ResetAttacking()
    {
        anim.SetBool("attacking", false);
    }

    public override void Attack()
    {

        if (Mathf.Abs(Mathf.Abs(PlayerController.player.transform.position.x) - Mathf.Abs(transform.position.x)) < 1f)
        {
            if (PlayerController.player.transform.position.y < transform.position.y)
            {
                anim.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else anim.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (Mathf.Abs(Mathf.Abs(PlayerController.player.transform.position.y) - Mathf.Abs(transform.position.y)) < 1f)
        {
            if (PlayerController.player.transform.position.x < transform.position.x)
            {
                anim.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else anim.transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        base.Attack();

        anim.SetBool("attacking", true);
        Invoke("ResetAttacking", 0.1f);
        weapon.weaponType = type;
        weapon.effect = effect;
        weapon.atk = atk;
        weapon.pot = potency;



        switch (type)
        {
            case (Weapon.weaponTypes.sword):
                weapon.anim.Play("Sword");
                break;
            case (Weapon.weaponTypes.dagger):
                weapon.anim.Play("Dagger");
                break;
            case (Weapon.weaponTypes.spear):
                weapon.anim.Play("Spear");
                break;
            case (Weapon.weaponTypes.bow):
                weapon.anim.Play("Bow");
                break;
        }

        attackCools = timeBetweenAttacks;
        ChangeState(states.chase);
    }
}
