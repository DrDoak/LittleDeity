using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Decaying : Property {

    float time_tracker = 0.0f;
    float decay_period = 0.1f;
    float decay_damage = 1.0f;

    public override void OnCreation()
    {
		PropertyName = "Decay";
		Description = "Lose health over time";
    }

    public override void OnUpdate()
    {
        if (Time.time > time_tracker)
        {
            time_tracker += decay_period;
            GetComponent<Attackable>().DamageObj(decay_damage);
        }
    }

}
