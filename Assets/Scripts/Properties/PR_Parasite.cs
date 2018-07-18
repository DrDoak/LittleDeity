using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Parasite : Property {

    float time_tracker = 0.0f;
    float parasite_damage = 3.0f;
    float parasite_period = 0.1f;


    public override void OnHitConfirm(Hitbox myHitbox, GameObject objectHit, HitResult hr)
    {
        objectHit.GetComponent<Attackable>().DamageObj(myHitbox.Damage);  // double damage
    }

    public override void OnUpdate()
    {
        if (Time.time > time_tracker)
        {
            time_tracker += parasite_period;
            GetComponent<Attackable>().DamageObj(parasite_damage);
        }
    }
}
