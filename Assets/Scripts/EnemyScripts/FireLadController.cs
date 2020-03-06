using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLadController : EnemyController
{
    public GameObject projectile;

    private Vector3 targetPoint;
    private Quaternion targetRotation;

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
            if (PlayerController.player.transform.position.x > transform.position.x) bod.AddForce(Vector2.right * curSpd * Time.deltaTime);
            else if (PlayerController.player.transform.position.x < transform.position.x) bod.AddForce(-Vector2.right * curSpd * Time.deltaTime);
            else if (PlayerController.player.transform.position.y < transform.position.y) bod.AddForce(-Vector2.up * curSpd * Time.deltaTime);
            else if (PlayerController.player.transform.position.y > transform.position.y) bod.AddForce(Vector2.up * curSpd * Time.deltaTime);
        }

        //In range of player
        if (distance < attackDistance * attackDistance && attackCools <= 0f)
        {
            ChangeState(states.attack);
        }
    }

    public override void Attack()
    {
        //anim.SetBool("attacking", true);
        Vector3 dir = PlayerController.player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);

        //Shoot thing
        EnemyProjectileController proj = Instantiate(projectile, transform.position, rot).GetComponent<EnemyProjectileController>();
        proj.effect = effect;
        proj.atk = atk;

        attackCools = timeBetweenAttacks;
        ChangeState(states.chase);
    }
}
