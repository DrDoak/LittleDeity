using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Cannon : Property {

	public override void OnUpdate() { 
		if (GetComponent<PropertyHolder>().HasProperty("Flaming")) {
			GetComponent<Fighter> ().TryAttack ("fire");
		}
	}

	public override void OnHit(Hitbox hb, GameObject attacker) { 
		if (hb.HasElement(ElementType.FIRE)) {
			GetComponent<Fighter> ().TryAttack ("fire");
		}
	}
}
