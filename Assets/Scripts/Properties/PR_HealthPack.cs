using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_HealthPack : Property {

	public override void OnAddProperty()
	{
		if (GetComponent<ExperienceHolder>() != null) {
			m_giveHealth ();
		}
	}
		

	void m_giveHealth() {
		if (GetComponent<PropertyHolder> ().HasProperty ("Biological")) {
			GetComponent<Attackable> ().DamageObj (-value * 2f);
		} else {
			GetComponent<Attackable> ().DamageObj (-value);
		}
		GetComponent<PropertyHolder> ().RequestRemoveProperty ("HealthPack");
	}
}