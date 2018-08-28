using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_HealthPack : Property {

	public int HealthValue = 25;

	public override void OnAddProperty()
	{
		if (GetComponent<ExperienceHolder>() != null) {
			m_giveHealth ();
		}
	}

	void m_giveHealth() {
		if (GetComponent<PropertyHolder> ().HasProperty ("Biological")) {
			GetComponent<Attackable> ().DamageObj (-HealthValue * 2f);
		} else {
			GetComponent<Attackable> ().DamageObj (-HealthValue);
		}
		GetComponent<PropertyHolder> ().RequestRemoveProperty ("HealthPack");
	}
}