using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TKAggressiveAttack : Task {

	public Vector2 TargetPositionOffset = new Vector2 ();
	public float TargetPositionTolerance = 0f;

	// Use this for initialization
	void Start () {
		Init ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Target != null) {
			MasterAI.GetComponent<OffenseAI> ().setTarget (Target.GetComponent<BasicMovement> (),
				TargetPositionOffset,TargetPositionTolerance);
		}
	}
}
