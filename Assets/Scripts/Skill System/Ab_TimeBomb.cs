using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_TimeBomb : Ability {

    public GameObject Bomb;

    public override void Awake()
    {
        base.Awake();
        if (!Bomb)
        {
            //Set Bomb to resource
        }
    }

    public override void UseAbility()
    {
        //Drop bomb at Creator location
        GameObject b = Instantiate(Bomb);
        b.transform.position = Creator.transform.position;
        
    }
}
