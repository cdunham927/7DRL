using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    //Enemy stats
    public float maxHp;
    float hp;
    //Resistances to various status effects
    [Range(0, 100)]
    public float poisonResistance;
    float curPoison = 0;
    bool poisoned = false;
    [Range(0, 100)]
    public float burnResistance;
    float curBurn = 0;
    bool burned = false;
    [Range(0, 100)]
    public float slowResistance;
    float curSlow = 0;
    bool slowed = false;
    [Range(0, 100)]
    public float paralyzeResistance;
    float curParalyze = 0;
    bool paralyzed = false;
    //Enemy attack
    public float atk;
    public float lowAtk;
    //Enemy defense
    public float def;
    //Enemy speed
    public float regSpd;
    float curSpd;
    public float slowSpd;
    //Gold drop
    public float gold;
    //public GameObject gold;

    //Enemy damage cooldown(iframes)
    float cools = 0f;

    void OnEnable()
    {
        hp = maxHp;
        curSpd = regSpd;
    }

    void Update()
    {
        if (cools > 0) cools -= Time.deltaTime;

        //Do the status effects
        if (poisoned)
        {

        }
        if (burned)
        {

        }
        if (slowed)
        {

        }
        if (paralyzed)
        {

        }

        //Remove the status effects
        if (curPoison <= 0) poisoned = false;
        if (curBurn <= 0) burned = false;
        if (curSlow <= 0) slowed = false;
        if (curParalyze <= 0) paralyzed = false;
    }

    public void TakeDamage(Weapon.weaponEffect effect, float potency, float dmg, float mod)
    {
        if (cools <= 0) {
            switch (effect)
            {
                case (Weapon.weaponEffect.none):
                    hp -= CalculateDamage(dmg, mod);
                    if (hp <= 0) Die();
                    break;
                case (Weapon.weaponEffect.poison):
                    hp -= CalculateDamage(dmg, mod);
                    if (hp <= 0) Die();
                    curPoison += potency;
                    if (curPoison >= poisonResistance) poisoned = true;
                    cools = 0.1f;
                    break;
                case (Weapon.weaponEffect.burn):
                    hp -= CalculateDamage(dmg, mod);
                    if (hp <= 0) Die();
                    curBurn += potency;
                    if (curBurn >= burnResistance) burned = true;
                    cools = 0.1f;
                    break;
                case (Weapon.weaponEffect.slow):
                    hp -= CalculateDamage(dmg, mod);
                    if (hp <= 0) Die();
                    curSlow += potency;
                    if (curSlow >= slowResistance) slowed = true;
                    cools = 0.1f;
                    break;
                case (Weapon.weaponEffect.paralyze):
                    hp -= CalculateDamage(dmg, mod);
                    if (hp <= 0) Die();
                    curParalyze += potency;
                    if (curParalyze >= paralyzeResistance) paralyzed = true;
                    cools = 0.1f;
                    break;
            }
        }
    }

    public float CalculateDamage(float atk, float mod)
    {
        return ((atk + mod) - def);
    }

    public void Die()
    {

    }
}
