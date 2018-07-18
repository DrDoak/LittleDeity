using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Knockdown : Ability {

    private bool selected;
	
	new void Awake()
    {
        base.Awake();
        Ultimate = true;
        selected = false;
    }

    public override void UseAbility()
    {
        Debug.Log("Knockdown triggered!");
    }

    public override void Select()
    {
        if (!selected)
            EventManager.TransferSpecialEvent += UseAbility;
        else
            EventManager.TransferSpecialEvent -= UseAbility;

        selected = !selected;
    }
}