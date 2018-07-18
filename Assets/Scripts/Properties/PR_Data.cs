using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Data : Property {

	GameObject fx;

	public override void OnAddProperty()
	{
		if (GetComponent<ExperienceHolder>() != null) {
			m_DropExperience ();
		}
	}

	public override void OnDeath()
	{
		m_DropExperience ();
	}

	void m_DropExperience() {
		int expDropped = 0;
		ExperienceHolder eh = null;
		if (GetComponent<Attackable> ().Killer != null && GetComponent<Attackable> ().Killer.GetComponent<ExperienceHolder> ()) {
			eh = GetComponent<Attackable> ().Killer.GetComponent<ExperienceHolder> ();
		} else {
			eh = FindObjectOfType<ExperienceHolder> ();
		}
		while (expDropped < value) {
			GameObject go = Instantiate (GameManager.Instance.FXExperience, transform.position, Quaternion.identity);
			if (eh != null) {
				go.GetComponent<ChaseTarget> ().Target = eh.GetComponent<PhysicsSS> ();

			}
			go.GetComponent<ChaseTarget> ().StartingVel = new Vector2 (Random.Range (-10f, 10f), 10f);
			expDropped += 50;
		}
		if (eh != null)
			eh.AddExperience ((int)value);
		FindObjectOfType<AudioManager> ().PlayClipAtPos (FXHit.Instance.SFXHeal,transform.position,0.25f,0f,0.25f);
		GetComponent<PropertyHolder> ().RequestRemoveProperty ("Data");
	}
}