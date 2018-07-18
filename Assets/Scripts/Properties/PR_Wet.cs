using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Wet : Property {

    Resistence fireResist;
    Resistence lightningWeakness;
	GameObject fx;

    public override void OnAddProperty()
    {
        fireResist = GetComponent<Attackable>().AddResistence(ElementType.FIRE, 25.0f);
        lightningWeakness = GetComponent<Attackable>().AddResistence(ElementType.LIGHTNING, -25.0f);
		fx = GetComponent<PropertyHolder> ().AddBodyEffect (FXBody.Instance.FXWet);
    }

	public override void OnUpdate() { 
		GetComponent<PropertyHolder> ().RequestRemoveProperty ("Flaming");
	}

    public override void OnRemoveProperty()
    {
        GetComponent<Attackable>().RemoveResistence(fireResist);
        GetComponent<Attackable>().RemoveResistence(lightningWeakness);
		GetComponent<PropertyHolder> ().RemoveBodyEffect (fx);
    }
	public override void OnHitboxCreate (Hitbox hitboxCreated) {
		if (hitboxCreated.HasElement(ElementType.FIRE)) {
			hitboxCreated.Damage = 0f;
		}
	}
}
