using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public enum equipmentTypes { armor, boots, amulets, hpIncrease, healingIncrease, goldIncrease, familiar }
    public equipmentTypes type;
    //Unsure if this is gonna be a percentage amount or a set number(percent would balance better)
    public float modAmt;
    [Header("Only change this for amulets")]
    public Weapon.weaponEffect effect = Weapon.weaponEffect.none;

    Weapon weap;

    public override void SetText() { tString = (itemName + "\n" + itemDescription + "\n" + "Cost - " + cost.ToString()); }

    public override void Use()
    {
        switch(type)
        {
            case (equipmentTypes.armor):
                PlayerController.player.defMod = modAmt;
                break;
            case (equipmentTypes.boots):
                PlayerController.player.spdMod = modAmt;
                break;
            case (equipmentTypes.amulets):
                if (effect == PlayerController.player.weapon.curWeapon.effect) PlayerController.player.weapon.curWeapon.potency += modAmt;
                break;
            case (equipmentTypes.hpIncrease):
                PlayerController.player.maxHpMod = modAmt;
                break;
            case (equipmentTypes.healingIncrease):
                PlayerController.player.healingMod = modAmt;
                break;
            case (equipmentTypes.goldIncrease):
                PlayerController.player.goldMod = modAmt;
                break;
            case (equipmentTypes.familiar):

                break;
        }
    }

    public override void PickupItem()
    {
        PlayerController.player.SwitchEquipment(this);
    }
}
