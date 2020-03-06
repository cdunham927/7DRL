using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    //Enemy stats
    [Header("Enemy stats")]
    //public string enemyName;
    public float maxHp;
    float hp;
    //Enemy attack
    protected float atk;
    public float regAtk;
    public float lowAtk;
    //Enemy defense
    public float def;
    //Enemy speed
    public float regSpd;
    protected float curSpd;
    public float slowSpd;
    //Gold drop
    public float goldDropAmt;
    public GameObject gold;
    //Attack type(status effect)
    public Weapon.weaponEffect effect = Weapon.weaponEffect.none;
    public float potency = 0;

    //Enemy states and AI
    public enum states { idle, chase, attack }
    [Header("Enemy states and AI")]
    public states curState = states.chase;
    public float attackDistance = 2f;
    protected float attackCools;
    public float timeBetweenAttacks = 0.7f;

    //Animation
    protected Animator anim;

    //Resistances to various status effects
    [Header("Enemy resistances")]
    [Range(0, 100)]
    public float poisonResistance;
    float curPoison = 0;
    public float poisonTime = 8f;
    [Range(0, 100)]
    public float burnResistance;
    float curBurn = 0;
    public float burnTime = 6f;
    [Range(0, 100)]
    public float slowResistance;
    float curSlow = 0;
    public float slowTime = 7f;
    [Range(0, 100)]
    public float paralyzeResistance;
    float curParalyze = 0;
    public float paralyzeTime = 5f;
    public float timeBetweenPoisonDamage = 0.5f;
    public float timeBetweenBurnDamage = 0.5f;

    //Enemy damage cooldown(iframes)
    public float iframes = 0.15f;
    float cools = 0f;

    //Cooldowns for status effects
    float poisonCools = 0f;
    float poisonDmgCools = 0f;
    float burnCools = 0f;
    float burnDmgCools = 0f;
    float slowCools = 0f;
    float paralyzeCools = 0f;

    //Damage done by poison and burn effects
    [Header("Damage taken via status effects")]
    public float poisonDamage = 3f;
    public float burnDamage = 2f;

    //UI
    [Header("UI things")]
    public Image hpImg;
    public float lerpSpd;
    public Image[] statusImages;

    //For buffing enemy stats between floors
    [Header("Buffs between floors")]
    public float hpBuff = 0.1f;
    public float atkBuff = 0.1f;
    public float defBuff = 0.1f;
    public float resBuff = 0.05f;
    public float spdBuff = 0.05f;
    public float goldBuff = 0.2f;

    //For despawning in enemy rooms
    AddRoom room;

    //Components
    protected Rigidbody2D bod;

    void OnEnable()
    {
        anim = GetComponent<Animator>();
        bod = GetComponent<Rigidbody2D>();
        SetStats();
    }

    public void SetRoom(AddRoom rm)
    {
        room = rm;
    }

    void SetStats()
    {
        //Get current floor
        //Modify enemy stats based on current floor
        maxHp += hpBuff * 0f;
        regAtk += atkBuff * 0f;
        lowAtk += atkBuff * 0f;
        def += defBuff * 0f;
        regSpd += spdBuff * 0f;
        slowSpd += spdBuff * 0f;
        goldDropAmt += goldBuff * 0f;
        poisonResistance += resBuff * 0f;
        burnResistance += resBuff * 0f;
        slowResistance += resBuff * 0f;
        paralyzeResistance += resBuff * 0f;

        //Reset the enemies stats
        hp = maxHp;
        atk = regAtk;
        curSpd = regSpd;
    }

    void Update()
    {
        if (cools > 0) cools -= Time.deltaTime;

        //Do the status effects
        //Poison hurts the enemy over time
        if (poisonCools > 0)
        {
            poisonCools -= Time.deltaTime;
            poisonDmgCools -= Time.deltaTime;
            if (poisonDmgCools <= 0f)
            {
                poisonDmgCools = timeBetweenPoisonDamage;
                hp -= poisonDamage;
                if (hp <= 0) Die();
            }
        }
        //Burn hurts the enemy over time and lowers their attack
        if (burnCools > 0)
        {
            burnCools -= Time.deltaTime;
            burnDmgCools -= Time.deltaTime;
            atk = lowAtk;
            if (burnDmgCools <= 0f)
            {
                burnDmgCools = timeBetweenBurnDamage;
                hp -= burnDamage;
                if (hp <= 0) Die();
            }
        }
        //Slow slows down the enemy
        if (slowCools > 0)
        {
            slowCools -= Time.deltaTime;
            curSpd = slowSpd;
        }
        //Paralyze stops the enemy and prevents them from attacking
        if (paralyzeCools > 0)
        {
            paralyzeCools -= Time.deltaTime;
            curSpd = 0f;
        }

        //Reset status effects
        if (burnCools <= 0f) atk = regAtk;
        if (slowCools <= 0f) curSpd = regSpd;
        if (paralyzeCools <= 0f)
        {
            if (slowCools <= 0) curSpd = regSpd;
            else curSpd = slowSpd;
        }

        //Decrement attack cooldown if the enemy isn't paralyzed
        if (attackCools > 0f && paralyzeCools <= 0f) attackCools -= Time.deltaTime;

        //Update UI
        hpImg.fillAmount = Mathf.Lerp(hpImg.fillAmount, hp / maxHp, lerpSpd * Time.deltaTime);
        statusImages[0].gameObject.SetActive((poisonCools > 0) ? true : false);
        statusImages[1].gameObject.SetActive((burnCools > 0) ? true : false);
        statusImages[2].gameObject.SetActive((slowCools > 0) ? true : false);
        statusImages[3].gameObject.SetActive((paralyzeCools > 0) ? true : false);

        //Enemy states
        switch(curState)
        {
            case (states.idle):
                Idle();
                break;
            case (states.chase):
                Chase();
                break;
            case (states.attack):
                Attack();
                break;
        }

        //Face left
        //anim.SetInteger("Dir", 0);

        //For testing in the editor
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.Alpha7)) TakeDamage(Weapon.weaponEffect.poison, 10, 3, 0);
            if (Input.GetKeyDown(KeyCode.Alpha8)) TakeDamage(Weapon.weaponEffect.burn, 10, 3, 0);
            if (Input.GetKeyDown(KeyCode.Alpha9)) TakeDamage(Weapon.weaponEffect.slow, 10, 3, 0);
            if (Input.GetKeyDown(KeyCode.Alpha0)) TakeDamage(Weapon.weaponEffect.paralyze, 10, 3, 0);
        }
    }

    public void TakeDamage(Weapon.weaponEffect effect, float potency, float dmg, float mod)
    {
        if (cools <= 0) {
            switch (effect)
            {
                case (Weapon.weaponEffect.none):
                    hp -= CalculateDamage(dmg, mod);
                    if (hp <= 0) Die();
                    cools = iframes;
                    break;
                case (Weapon.weaponEffect.poison):
                    hp -= CalculateDamage(dmg, mod);
                    if (hp <= 0) Die();
                    curPoison += potency;
                    if (curPoison >= poisonResistance && poisonCools <= 0)
                    {
                        poisonDmgCools = timeBetweenPoisonDamage;
                        poisonCools = poisonTime;
                    }
                    cools = iframes;
                    break;
                case (Weapon.weaponEffect.burn):
                    hp -= CalculateDamage(dmg, mod);
                    if (hp <= 0) Die();
                    curBurn += potency;
                    Debug.Log(curBurn);
                    if (curBurn >= burnResistance && burnCools <= 0)
                    {
                        burnDmgCools = timeBetweenBurnDamage;
                        burnCools = burnTime;
                    }
                    cools = iframes;
                    break;
                case (Weapon.weaponEffect.slow):
                    hp -= CalculateDamage(dmg, mod);
                    if (hp <= 0) Die();
                    curSlow += potency;
                    if (curSlow >= slowResistance && slowCools <= 0) slowCools = slowTime;
                    cools = iframes;
                    break;
                case (Weapon.weaponEffect.paralyze):
                    hp -= CalculateDamage(dmg, mod);
                    if (hp <= 0) Die();
                    curParalyze += potency;
                    if (curParalyze >= paralyzeResistance && paralyzeCools <= 0) paralyzeCools = paralyzeTime;
                    cools = iframes;
                    break;
            }
        }
    }

    public float CalculateDamage(float atk, float mod)
    {
        float tot = ((atk + mod) - def);
        if (tot > 0) return tot;
        return 1;
    }

    public virtual void Idle() { }

    public virtual void Chase() { }

    public virtual void Attack() { }

    public void ChangeState(states newState)
    {
        curState = newState;
    }

    public void Die()
    {
        //Drop gold
        GameObject obj = Instantiate(gold, transform.position, transform.rotation);
        obj.GetComponent<GoldController>().amt = goldDropAmt;

        //Die
        room.RemoveFromRoom(this);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.player.TakeDamage(atk, effect, potency);
        }
    }

    private void OnTriggeStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController.player.TakeDamage(atk, effect, potency);
        }
    }
}
