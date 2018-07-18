using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskIdleSearch : FighterTask {

	//private int m_lastObservable;
	//private Observer m_observer;
	private Attackable m_attackable;
	private AIFighter m_fighter;

	public override void Init(Fighter player, AIFighter fighter, FighterRoutine routine) {
		base.Init (player, fighter, routine);
		//m_lastObservable = 0;
		//m_observer = Fighter.gameObject.GetComponent<Observer> ();
		m_attackable = Fighter.GetComponent<Attackable> ();
	}

	override public void Advance() 
	{
		if (Fighter.CurrentTarget != null) {
			//Debug.Log (Fighter.CurrentTarget);
			NextTask ();
			return;
		}
	}

	override public void OnSight(Observable o) {
		if (o.GetComponent<Attackable> () && m_attackable.CanAttack (o.GetComponent<Attackable> ().Faction)) {
			Fighter.CurrentTarget = o.GetComponent<Attackable> ();
			NextTask ();
			return;
		}
	}

	override public void OnHit(Hitbox hb) {
		if (hb.Knockback.x != 0f) {
			Fighter.GetComponent<PhysicsSS> ().SetDirection (hb.Knockback.x > 0f);
		} else {
			Fighter.GetComponent<PhysicsSS> ().SetDirection (hb.transform.position.x < transform.position.x);
		}
	}
}
