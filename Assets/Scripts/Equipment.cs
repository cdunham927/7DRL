using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public enum equipmentTypes { armor, boots, amulets, hpIncrease, healingIncrease, goldIncrease }
    public equipmentTypes type;
    //Unsure if this is gonna be a percentage amount or a set number(percent would balance better)
    public float modAmt;

    public override void Use()
    {
        switch(type)
        {
            case (equipmentTypes.armor):
                player.defMod = modAmt;
                break;
            case (equipmentTypes.boots):
                player.spdMod = modAmt;
                break;
            case (equipmentTypes.amulets):
                player.GetComponentInChildren<WeaponController>().curWeapon.potency += modAmt;
                break;
            case (equipmentTypes.hpIncrease):
                player.maxHpMod = modAmt;
                break;
            case (equipmentTypes.healingIncrease):
                player.healingMod = modAmt;
                break;
            case (equipmentTypes.goldIncrease):
                player.goldMod = modAmt;
                break;
        }
    }
}
