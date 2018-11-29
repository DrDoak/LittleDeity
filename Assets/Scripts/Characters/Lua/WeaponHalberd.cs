using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHalberd : WeaponFloating {


	float m_stop_spin = 0f;
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad < m_stop_spin) {
		} else if (!m_slashing) {
			GetComponent<ChaseTarget> ().SetAutoRotate (0f);
			standardSprite ();
			updateStandardSprite ();
		} else {
			GetComponent<ChaseTarget> ().SetAutoRotate (0f);
			ExecuteFX ();
		}
	}
	public void StartSpinFX(float duration,float spinSpeed) {
		m_stop_spin = Time.timeSinceLevelLoad + duration;
		GetComponent<ChaseTarget> ().SetAutoRotate (spinSpeed);
	}
}