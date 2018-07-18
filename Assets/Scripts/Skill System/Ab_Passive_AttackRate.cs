using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Passive_AttackRate : Ability {

    Ab_Melee parent;

    new void Awake()
    {
        base.Awake();
        parent = FindObjectOfType<Ab_Melee>();
        AbilityClassification = AbilityType.COMBAT;
        Passive = true;
        Maxed = false;
        Tiered = true;
        _mtier = 1;
    }

    public override void UseAbility()
    {
        if (_mtier < MAX_TIER)
        {
            _mtier++;
            parent.UpgradeAttackRate();
        }
        else
            Maxed = true;
    }
}
