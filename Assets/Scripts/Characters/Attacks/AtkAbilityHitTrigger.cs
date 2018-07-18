using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtkAbilityHitTrigger : AtkDash {

	public Ability mAbility;
	bool hitFirst = false;
	// Use this for initialization
	protected override void OnStartUp() {
		base.OnStartUp();
		hitFirst = false;
	}
	public override void OnHitConfirm(GameObject other, Hitbox hb, HitResult hr) {
		if (!hitFirst) {
			if (mAbility != null) {
				mAbility.SetTarget (other);
				mAbility.UseAbility ();
			} else {
				Debug.Log ("Ability not assigned to " + GetType ());
			}
			hitFirst = true;
		}
	}
}
