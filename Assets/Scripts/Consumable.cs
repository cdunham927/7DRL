using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    public enum consumeType { potion }
    public consumeType type;

    public override void Use()
    {
        switch(type)
        {

        }
    }
}
