using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceDrop : MonoBehaviour {
	public int Experience = 100;
	void OnDestroy() {
		/*int expDropped = 0;
		ExperienceHolder eh = null;
		if (GetComponent<Attackable> ().Killer != null && GetComponent<Attackable> ().Killer.GetComponent<ExperienceHolder> ()) {
			eh = GetComponent<Attackable> ().Killer.GetComponent<ExperienceHolder> ();
		} else {
			eh = FindObjectOfType<ExperienceHolder> ();
		}
		while (expDropped < Experience) {
			GameObject go = Instantiate (GameManager.Instance.FXExperience, transform.position, Quaternion.identity);
			if (eh != null) {
				go.GetComponent<ChaseTarget> ().Target = eh.GetComponent<PhysicsSS> ();

			}
			go.GetComponent<ChaseTarget> ().StartingVel = new Vector2 (Random.Range (-10f, 10f), 10f);
			expDropped += 50;
		}
		if (eh != null)
			eh.AddExperience (Experience);*/
	}
}
