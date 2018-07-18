using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Generator : Property {

	float SinceLastCheck = 0.0f;
	float INTERVAL_CHECK = 3.0f;

	// Update is called once per frame
	public override void OnUpdate () {
		if (SinceLastCheck > INTERVAL_CHECK) {
			RefreshElectricity ();
		}
		SinceLastCheck += Time.deltaTime;
	}

	void RefreshElectricity() {
		if (!GetComponent<PropertyHolder> ().HasProperty ("Electrical")) {
			GetComponent<PropertyHolder> ().AddProperty ("PR_Electrical");
			SinceLastCheck = 0f;
		}
	}
}
