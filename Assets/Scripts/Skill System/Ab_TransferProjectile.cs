using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_TransferProjectile : Ab_Transfer {
	
    new private void Awake()
    {
        base.Awake();
        Ultimate = true;
    }

    public override void UseAbility()
    {

        //Change to a projectile
        //Should be mostly the same as the base, but different hitbox/animation
        base.UseAbility();
    }
}