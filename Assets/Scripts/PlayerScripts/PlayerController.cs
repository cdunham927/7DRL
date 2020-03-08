using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //Player has to be a singleton
    //Doesnt get destroyed between scenes
    //Upon death, remove all equipment and give a basic starter weapon
    //Reset position
    //Then load room 1
    public static PlayerController player;
    Animator anim;

    //Components
    Rigidbody2D bod;
    
    //Currently equipment
    public Equipment curEquip;

    //Start player stats
    //Health
    public float maxHp = 100;
    [HideInInspector]
    public float maxHpMod;
    [HideInInspector]
    public float healingMod;
    float hp;
    //Defense
    float def = 0;
    [HideInInspector]
    public float defMod = 0;
    //Spd
    public float spd;
    float curSpd;
    public float slowSpd;
    [HideInInspector]
    public float spdMod;
    [HideInInspector]
    public float gold = 0;
    [HideInInspector]
    public float goldMod;

    //Health Image
    public Image hpBar;
    public float lerpSpd = 10f;

    //Floor Level
    public int floor = 1;
    public Text floorText;

    //Gold text
    public Text goldText;

    //Items UI
    public Image weaponImg;
    public Image equipImg;
    RectTransform weapRect;
    RectTransform equipRect;
    //public Image consumeImg;
    public Image weaponTypeImg;
    public Image equipTypeImg;

    //Status effect UI
    public Image[] statusImages;

    //Status sprites
    public Sprite[] effectSprites;

    //Current weapon
    [HideInInspector]
    public WeaponController weapon;
    ItemHolder itemHolder;
    public Vector3[] weaponPositions;
    SpriteRenderer weaponRend;

    //Taking damage cooldown(iframes)
    public float iframes = 0.2f;
    float cools = 0f;

    //Resistances to various status effects
    [Header("Player resistances")]
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
    public float potencyRemoval = 1f;

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

    //Movement
    bool moving = false;

    //Sounds
    AudioSource src;
    public AudioClip hit;

    //Animations
    public enum character { knight, bandit, lizard, rogue, wolf, len }
    [SerializeField]
    int ch = 0;

    void Awake()
    {
        if (player == null)
        {
            player = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        anim = GetComponent<Animator>();
        src = GetComponent<AudioSource>();

        //Get UI references
        weapRect = weaponImg.GetComponent<RectTransform>();
        equipRect = equipImg.GetComponent<RectTransform>();

        hp = maxHp;
        curSpd = spd;
        bod = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<WeaponController>();
        weaponRend = weapon.GetComponent<SpriteRenderer>();
        Invoke("ResetEverything", 0.01f);
    }

    public void ResetEverything()
    {
        //Reset items
        itemHolder = FindObjectOfType<ItemHolder>();
        Weapon weap = Instantiate((Weapon)itemHolder.GetWeapon(Item.Rarity.common), transform.position, Quaternion.identity);
        weapon.SwitchWeapon(weap);
        PlayerController.player.UpdateUI(0);

        ch = Random.Range(0, (int)character.len - 1);

        anim.SetInteger("Char", ch);
    }
    
    void FixedUpdate()
    {
        Vector2 inp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (inp.magnitude != 0)
        {
            bod.AddForce(new Vector2(inp.x * (spd + (spd * spdMod)) * Time.deltaTime, inp.y * spd * Time.deltaTime));
        }

        if (!weapon.attacking)
        {
            if (inp.y != 0)
            {
                moving = true;
                transform.rotation = Quaternion.Euler(0, 0, (inp.y > 0) ? 180 : 0);
                //bod.AddForce(new Vector2(0f, inp.y * curSpd * Time.deltaTime));
            }
            if (inp.x != 0)
            {
                moving = true;
                transform.rotation = Quaternion.Euler(0, 0, (inp.x > 0) ? 90 : -90);
                //bod.AddForce(new Vector2(inp.x * curSpd * Time.deltaTime, 0f));
            }
            //else moving = false;
        }

        if (weapon.curWeapon != null) anim.SetInteger("Weapon", (int)weapon.curWeapon.type);

        if (hpBar != null) hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, hp / maxHp, lerpSpd * Time.deltaTime);
        if (floorText != null) floorText.text = "Floor: " + floor.ToString();
        if (goldText != null) goldText.text = gold.ToString();

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
            weapon.atkMod = 2;
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
        if (burnCools <= 0f) weapon.atkMod = 0;
        if (slowCools <= 0f) curSpd = spd;
        if (paralyzeCools <= 0f)
        {
            if (slowCools <= 0) curSpd = spd;
            else curSpd = slowSpd;
        }

        statusImages[0].gameObject.SetActive((poisonCools > 0) ? true : false);
        statusImages[1].gameObject.SetActive((burnCools > 0) ? true : false);
        statusImages[2].gameObject.SetActive((slowCools > 0) ? true : false);
        statusImages[3].gameObject.SetActive((paralyzeCools > 0) ? true : false);

        //Decrement status effects
        if (curPoison > 0) curPoison -= Time.deltaTime * potencyRemoval;
        if (curBurn > 0) curBurn -= Time.deltaTime * potencyRemoval;
        if (curSlow > 0) curSlow -= Time.deltaTime * potencyRemoval;
        if (curParalyze > 0) curParalyze -= Time.deltaTime * potencyRemoval;

        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.M))
            {
                gold += 1;
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                if (ch != 4) ch++;
                else ch = 0;

                anim.SetInteger("Char", ch);
            }
        }
    }

    public void UpdateUI(int n)
    {
        if (n == 0)
        {
            weaponImg.sprite = weapon.curWeapon.GetComponent<SpriteRenderer>().sprite;
            //Change size of weapon UI box
            switch (weapon.curWeapon.type)
            {
                case (Weapon.weaponTypes.sword):
                    weapRect.sizeDelta = new Vector2(15f, 35f);
                    break;
                case (Weapon.weaponTypes.dagger):
                    weapRect.sizeDelta = new Vector2(10f, 30f);
                    break;
                case (Weapon.weaponTypes.bow):
                    weapRect.sizeDelta = new Vector2(17.5f, 32.5f);
                    break;
            }

            //Set weapon effect UI sprite
            switch (weapon.curWeapon.effect)
            {
                case (Weapon.weaponEffect.none):
                    weaponTypeImg.sprite = effectSprites[0];
                    break;
                case (Weapon.weaponEffect.poison):
                    weaponTypeImg.sprite = effectSprites[1];
                    break;
                case (Weapon.weaponEffect.burn):
                    weaponTypeImg.sprite = effectSprites[2];
                    break;
                case (Weapon.weaponEffect.slow):
                    weaponTypeImg.sprite = effectSprites[3];
                    break;
                case (Weapon.weaponEffect.paralyze):
                    weaponTypeImg.sprite = effectSprites[4];
                    break;
            }
        }
        if (n == 1)
        {
            equipImg.sprite = curEquip.GetComponent<SpriteRenderer>().sprite;

            //Change size of equipment UI box
            switch (curEquip.type)
            {
                case (Equipment.equipmentTypes.amulets):
                    equipRect.sizeDelta = new Vector2(50, 50);
                    break;
                case (Equipment.equipmentTypes.boots):
                    equipRect.sizeDelta = new Vector2(40, 40);
                    break;
                case (Equipment.equipmentTypes.armor):
                    equipRect.sizeDelta = new Vector2(40, 40);
                    break;
            }

            //Set weapon effect UI sprite
            switch (curEquip.effect)
            {
                case (Weapon.weaponEffect.none):
                    equipTypeImg.sprite = effectSprites[0];
                    break;
                case (Weapon.weaponEffect.poison):
                    equipTypeImg.sprite = effectSprites[1];
                    break;
                case (Weapon.weaponEffect.burn):
                    equipTypeImg.sprite = effectSprites[2];
                    break;
                case (Weapon.weaponEffect.slow):
                    equipTypeImg.sprite = effectSprites[3];
                    break;
                case (Weapon.weaponEffect.paralyze):
                    equipTypeImg.sprite = effectSprites[4];
                    break;
            }
        }
    }

    public void SwitchEquipment(Equipment equip)
    {
        if (curEquip != null)
        {
            switch(curEquip.type)
            {
                case (Equipment.equipmentTypes.armor):
                    PlayerController.player.defMod = 0;
                    //Put my previous equipment where I picked the other one up
                    curEquip.gameObject.SetActive(true);
                    curEquip.transform.position = equip.transform.position;
                    break;
                case (Equipment.equipmentTypes.boots):
                    PlayerController.player.spdMod = 0;
                    break;
                case (Equipment.equipmentTypes.amulets):
                    if (curEquip.effect == weapon.curWeapon.effect) weapon.curWeapon.potency -= curEquip.modAmt;
                    //Put my previous equipment where I picked the other one up
                    curEquip.gameObject.SetActive(true);
                    curEquip.transform.position = equip.transform.position;
                    break;
                case (Equipment.equipmentTypes.hpIncrease):
                    PlayerController.player.maxHpMod = 0;
                    //Put my previous equipment where I picked the other one up
                    curEquip.gameObject.SetActive(true);
                    curEquip.transform.position = equip.transform.position;
                    break;
                case (Equipment.equipmentTypes.healingIncrease):
                    PlayerController.player.healingMod = 0;
                    //Put my previous equipment where I picked the other one up
                    curEquip.gameObject.SetActive(true);
                    curEquip.transform.position = equip.transform.position;
                    break;
                case (Equipment.equipmentTypes.goldIncrease):
                    PlayerController.player.goldMod = 0;
                    //Put my previous equipment where I picked the other one up
                    curEquip.gameObject.SetActive(true);
                    curEquip.transform.position = equip.transform.position;
                    break;
            }
        }
        curEquip = equip;
        UpdateUI(1);
        curEquip.gameObject.SetActive(false);
        curEquip.Use();
    }

    public void Heal(float amt)
    {
        hp += (amt + (amt * healingMod));
    }

    public void PickupGold(float amt)
    {
        gold += (amt + (amt * goldMod));
    }

    public void TakeDamage(float dmg, Weapon.weaponEffect eff, float pot = 0)
    {
        if (cools <= 0)
        {
            hp -= dmg;
            cools = iframes;

            switch(eff) {
                case (Weapon.weaponEffect.none):
                hp -= CalculateDamage(dmg, defMod);
                if (hp <= 0) Die();
                break;
            case (Weapon.weaponEffect.poison):
                hp -= CalculateDamage(dmg, defMod);
                if (hp <= 0) Die();
                curPoison += pot;
                if (curPoison >= poisonResistance && poisonCools <= 0)
                {
                    poisonDmgCools = timeBetweenPoisonDamage;
                    poisonCools = poisonTime;
                }
                break;
            case (Weapon.weaponEffect.burn):
                //Debug.Log("Taking fire damage");
                hp -= CalculateDamage(dmg, defMod);
                if (hp <= 0) Die();
                curBurn += pot;
                //Debug.Log(curBurn);
                if (curBurn >= burnResistance && burnCools <= 0)
                {
                    burnDmgCools = timeBetweenBurnDamage;
                    burnCools = burnTime;
                }
                break;
            case (Weapon.weaponEffect.slow):
                hp -= CalculateDamage(dmg, defMod);
                if (hp <= 0) Die();
                curSlow += pot;
                if (curSlow >= slowResistance && slowCools <= 0) slowCools = slowTime;
                break;
            case (Weapon.weaponEffect.paralyze):
                hp -= CalculateDamage(dmg, defMod);
                if (hp <= 0) Die();
                curParalyze += pot;
                if (curParalyze >= paralyzeResistance && paralyzeCools <= 0) paralyzeCools = paralyzeTime;
                break;
            }
        }
    }

    public void Die()
    {

    }

    public float CalculateDamage(float atk, float mod)
    {
        float tot = ((atk + mod) - (def + def * defMod));
        if (tot > 0) return tot;
        return 1;
    }
}
