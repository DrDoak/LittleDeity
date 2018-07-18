using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Ultimate_Carbon_Copy : Ability
{
    private bool selected;

    new void Awake()
    {
        base.Awake();
        Ultimate = true;
        selected = false;
        AbilityClassification = AbilityType.COMBAT;
    }

    public override void UseAbility()
    {
        Debug.Log("Carbon Copy triggered!");
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
