using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TKAggressiveAttack : Task {

	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Target != null) {
			Debug.Log (MasterAI);
			MasterAI.GetComponent<OffenseAI> ().setTarget (Target.GetComponent<BasicMovement> ());
		}
	}
}
