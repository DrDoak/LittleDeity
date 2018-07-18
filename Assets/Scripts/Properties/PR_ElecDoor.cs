using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_ElecDoor : PR_Mechanical {

	protected override void OnActive() {
		if (GetComponent<Door> () != null) {
			GetComponent<Door> ().SetOpen (true);
		}
	}
	protected override void OnDisable() {
		if (GetComponent<Door> () != null) {
			GetComponent<Door> ().SetOpen (false);
		}
	}
}

