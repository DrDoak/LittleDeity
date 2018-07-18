using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_CriticalStrike : Ability
{

    private bool selected;

    new void Awake()
    {
        base.Awake();
        selected = false;
        AbilityClassification = AbilityType.COMBAT;
    }

    public override void UseAbility()
    {
        //Debug.Log("Critical Strike triggered!");
    }

    public override void Select()
    {
        if (!selected)
            EventManager.MeleeSpecialEvent += UseAbility;
        else
            EventManager.MeleeSpecialEvent -= UseAbility;

        selected = !selected;
    }
}
