﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    //Weapon type and effect
    public enum weaponTypes { sword, dagger, spear, axe, bow }
    public enum weaponEffect { none, poison, burn, slow, paralyze }
    public weaponTypes type = weaponTypes.sword;
    public weaponEffect effect = weaponEffect.none;

    //Cooldown for weapon
    public float cooldown;

    //Rarity of weapon
    public enum Rarity { legendary = 5, rare = 15, uncommon = 30, common = 100 }
    public Rarity rarity = Rarity.common;

    //Changes how affective the weapons status effect is
    //Ie. burning sword with potency 3 inflicts burn slower than burning sword with potency 10
    public float potency;

    //Weapons base attack
    public float atk;

    public override void Use() { }
}