using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_ElecTurret : PR_Mechanical {

	private bool m_targeting = false;
	private bool TargetFound = false;
	protected override void OnActive() {
		if (GetComponent<Turret> () != null) {
			m_targeting = true;
			//GetComponent<Turret> ().SetTarget (GameManager.Instance.CurrentPlayer);
		}
		GetComponent<Observer> ().VisibleObjs.Clear ();
	}
	protected override void OnDisable() {
		if (GetComponent<Turret> () != null) {
			GetComponent<Turret> ().SetTarget (null);
			TargetFound = false;
			m_targeting = false;
			GetComponent<Observer> ().VisibleObjs.Clear ();
		}
	} 

	public override void OnUpdate() {
		base.OnUpdate ();
		if (GetComponent<Turret> ().m_target == null) {
			TargetFound = false;
		}
	}
	public override void OnSight(Observable observedObj) {
		Debug.Log ("On sight: " + observedObj.gameObject);
		Debug.Log ("found: " + TargetFound + " targeting: " + m_targeting + " Attack: " + observedObj.GetComponent<Attackable> ());
		if (!TargetFound && m_targeting && observedObj.GetComponent<Attackable> () &&
			GetComponent<Attackable> ().Faction != observedObj.GetComponent<Attackable> ().Faction) {
			GetComponent<Turret> ().SetTarget (observedObj.gameObject);
			TargetFound = true;
		}
	}
}