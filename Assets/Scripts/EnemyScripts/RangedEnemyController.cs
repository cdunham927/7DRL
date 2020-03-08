using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyController : EnemyController
{
    public GameObject projectile;
    public Vector2 boxRayCast;
    public LayerMask mask;
    int direction = 0;
    public GameObject spawn;

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
            if (PlayerController.player.transform.position.x > transform.position.x)
            {
                if (Mathf.Abs(PlayerController.player.transform.position.y - transform.position.y) < Mathf.Abs(PlayerController.player.transform.position.x - transform.position.x))
                {
                    anim.transform.rotation = Quaternion.Euler(0, 0, 90);
                    bod.AddForce(Vector2.right * curSpd * Time.deltaTime);
                }
                else
                {
                    //Down
                    if (PlayerController.player.transform.position.y < transform.position.y)
                    {
                        anim.transform.rotation = Quaternion.Euler(0, 0, 0);
                        bod.AddForce(-Vector2.up * curSpd * Time.deltaTime);
                    }
                    //Up
                    else
                    {
                        anim.transform.rotation = Quaternion.Euler(0, 0, 180);
                        bod.AddForce(Vector2.up * curSpd * Time.deltaTime);
                    }
                }
            }
            //Left
            else if (PlayerController.player.transform.position.x < transform.position.x)
            {
                if (Mathf.Abs(PlayerController.player.transform.position.y - transform.position.y) < Mathf.Abs(PlayerController.player.transform.position.x - transform.position.x))
                {
                    anim.transform.rotation = Quaternion.Euler(0, 0, -90);
                    bod.AddForce(-Vector2.right * curSpd * Time.deltaTime);
                }
                else
                {
                    //Down
                    if (PlayerController.player.transform.position.y < transform.position.y)
                    {
                        anim.transform.rotation = Quaternion.Euler(0, 0, 0);
                        bod.AddForce(-Vector2.up * curSpd * Time.deltaTime);
                    }
                    //Up
                    else
                    {
                        anim.transform.rotation = Quaternion.Euler(0, 0, 180);
                        bod.AddForce(Vector2.up * curSpd * Time.deltaTime);
                    }
                }
            }
        }

        if (Mathf.Abs(Mathf.Abs(PlayerController.player.transform.position.x) - Mathf.Abs(transform.position.x)) < 0.4f)
        {
            if (PlayerController.player.transform.position.y < transform.position.y)
            {
                anim.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else anim.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (Mathf.Abs(Mathf.Abs(PlayerController.player.transform.position.y) - Mathf.Abs(transform.position.y)) < 0.4f)
        {
            if (PlayerController.player.transform.position.x < transform.position.x)
            {
                anim.transform.rotation = Quaternion.Euler(0, 0, -90);
            }
            else anim.transform.rotation = Quaternion.Euler(0, 0, 90);
        }

        if (distance < attackDistance * attackDistance && attackCools <= 0)
        {
            ChangeState(states.attack);
        }

        /*Collider2D hit;
        if (hit = Physics2D.OverlapBox(anim.transform.position, boxRayCast, 0f, mask))
        {
            if (attackCools <= 0) ChangeState(states.attack);
        }*/
    }
    void ResetAttacking()
    {
        anim.SetBool("attacking", false);
    }

    public override void Attack()
    {
        //Debug.Log("X POSITION DIFFERENCE: " + (Mathf.Abs(PlayerController.player.transform.position.x) - Mathf.Abs(transform.position.x)));
        //Debug.Log("Y POSITION DIFFERENCE: " + (Mathf.Abs(PlayerController.player.transform.position.y) - Mathf.Abs(transform.position.y)));

        if (Mathf.Abs(Mathf.Abs(PlayerController.player.transform.position.x) - Mathf.Abs(transform.position.x)) < 0.5f)
        {
            if (PlayerController.player.transform.position.y < transform.position.y)
            {
                anim.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else anim.transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (Mathf.Abs(Mathf.Abs(PlayerController.player.transform.position.y) - Mathf.Abs(transform.position.y)) < 0.5f)
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

        //Shoot thing
        EnemyProjectileController proj = Instantiate(projectile, spawn.transform.position, anim.transform.rotation).GetComponent<EnemyProjectileController>();
        SpriteRenderer rend = proj.GetComponent<SpriteRenderer>();
        switch(effect)
        {
            case (Weapon.weaponEffect.none):
                break;
            case (Weapon.weaponEffect.poison):
                rend.color = new Color(1, 0, 1);
                break;
            case (Weapon.weaponEffect.burn):
                rend.color = new Color(1, 0, 0);
                break;
            case (Weapon.weaponEffect.slow):
                rend.color = new Color(0, 0, 1);
                break;
            case (Weapon.weaponEffect.paralyze):
                rend.color = new Color(1, 1, 0);
                break;
        }

        proj.effect = effect;
        proj.atk = atk;
        proj.pot = potency;

        attackCools = timeBetweenAttacks;
        ChangeState(states.chase);
    }

    private void OnDrawGizmos()
    {
        /*
        Gizmos.color = Color.green;
        var left = anim.transform.position - new Vector3(boxRayCast.x * 0.5f, 0f, 0f);
        var right = anim.transform.position + new Vector3(boxRayCast.x * 0.5f, 0f, 0f);
        var down = -new Vector3(0f, boxRayCast.y, 0f);
        Gizmos.DrawLine(left, left + down);
        Gizmos.DrawLine(right, right + down);
        Gizmos.DrawLine(left, right);
        Gizmos.DrawLine(left + down, right + down);
        */
    }
}
