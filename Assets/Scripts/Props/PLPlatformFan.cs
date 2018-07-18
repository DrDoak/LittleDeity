using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLPlatformFan : MonoBehaviour {

	bool HasElec = false;

	void Update () {
		if (!HasElec && GetComponent<PropertyHolder> ().HasProperty ("Electrical")) {
			UpdateStatus ();
		} else if (HasElec && !GetComponent<PropertyHolder> ().HasProperty ("Electrical")) {
			UpdateStatus ();
		}
	}

	void UpdateStatus() {
		//Debug.Log ("Updating Status");
		bool hasPower = GetComponent<PropertyHolder> ().HasProperty ("Electrical");
		PropertyHolder[] pList = FindObjectsOfType<PropertyHolder> ();
		if (hasPower) {
			foreach (PropertyHolder h in pList) {
				if (h.HasProperty ("Fan") && !h.HasProperty("Electrical")) {
					h.AddProperty ("PR_Electrical");
					Debug.Log ("Adding a property" + h.HasProperty("Electrical") );
				}
			}
		} else {
			foreach (PropertyHolder h in pList) {
				if (h.HasProperty ("Fan")) {
					Debug.Log ("Requesting remove property");
					h.RequestRemoveProperty ("Electrical");
				}
			}
		}
		HasElec = hasPower;

	}
}
