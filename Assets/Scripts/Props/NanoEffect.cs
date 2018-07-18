using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NanoEffect : MonoBehaviour {

	public GameObject OnDestroyFX;
	// Use this for initialization
	void OnDestroy() {
		if (GetComponent<ChaseTarget>().Target != null) {
			Instantiate (OnDestroyFX, GetComponent<ChaseTarget> ().Target.transform);
		}
	}
}
