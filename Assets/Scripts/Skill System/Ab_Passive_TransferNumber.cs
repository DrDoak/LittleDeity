using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Passive_TransferNumber : Ability
{
    Ab_Transfer parent;

    new void Awake()
    {
        base.Awake();
        parent = FindObjectOfType<Ab_Transfer>();
        Passive = true;
        Tiered = true;
        Maxed = false;
        _mtier = 1;
    }

    public override void UseAbility()
    {
        if (_mtier < MAX_TIER)
        {
            _mtier++;
            parent.UpgradeNumTransfers();
        }
        else
            Maxed = true;
    }
}
