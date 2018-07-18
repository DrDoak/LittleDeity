using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxMulti : Hitbox {

	public float refreshTime = 0.1f;
	public float timeSinceLast = 0.0f;

	new void Update () {
		timeSinceLast += Time.deltaTime;
		if (timeSinceLast > refreshTime) {
			timeSinceLast = 0.0f;
			m_collidedObjs.Clear ();
			foreach (Attackable cont in m_overlappingControl) {
				OnAttackable (cont);
			}
		}		
		base.Tick ();
	}

}
