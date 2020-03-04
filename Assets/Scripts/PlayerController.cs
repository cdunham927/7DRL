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
    [HideInInspector]
    public float spdMod;
    float gold = 0;
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

    void Awake()
    {
        if (player == null)
        {
            player = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        hp = maxHp;
        bod = GetComponent<Rigidbody2D>();
    }

    public void ResetEverything()
    {

    }
    
    void FixedUpdate()
    {
        Vector2 inp = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (inp.magnitude != 0)
        {
            bod.AddForce(new Vector2(inp.x * spd * Time.deltaTime, inp.y * spd * Time.deltaTime));
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(20);
        }
        if (hpBar != null) hpBar.fillAmount = Mathf.Lerp(hpBar.fillAmount, hp / maxHp, lerpSpd * Time.deltaTime);
        if (floorText != null) floorText.text = "Floor: " + floor.ToString();
        if (goldText != null) goldText.text = gold.ToString();

        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.M))
            {
                gold += 1;
            }
        }
    }

    public void SwitchEquipment(Equipment equip)
    {
        if (curEquip != null)
        {

        }
        curEquip = equip;
        curEquip.Use();
    }

    public void Heal()
    {

    }

    public void PickupGold(float amt)
    {
        gold += (amt + (amt * goldMod));
    }

    void TakeDamage(float dmg)
    {
        hp -= dmg;
    }
}
