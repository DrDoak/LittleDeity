using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPursueToRange : FighterTask {

	public float TargetRange = 2.0f;
	public float AttackAggression = 1.0f;
	private AIFighter m_fighter;
	private Observer m_observer;

	public override void Init(Fighter player, AIFighter fighter, FighterRoutine routine) {
		base.Init (player, fighter, routine);
		m_fighter = fighter.GetComponent<AIFighter> ();
		m_observer = fighter.GetComponent<Observer> ();
	}

	override public void Advance() 
	{
		if (Fighter.CurrentTarget == null || !m_observer.IsVisible (m_fighter.CurrentTarget.GetComponent<Observable>())) {
			Debug.Log (m_fighter.CurrentTarget);
			NextTask ();
			return;
		}
		Vector2 target = Fighter.CurrentTarget.transform.position;
		if (Random.value < (AttackAggression * 0.1f)) {
			List<string> mf = m_fighter.AvailableAttacks (m_fighter.CurrentTarget.GetComponent<BasicMovement>());
			if (mf.Count > 0) {
				//Debug.Log ("Trying attack: " + mf [Random.Range (0, mf.Count)]);
				Fighter.Fighter.TryAttack (mf [Random.Range (0, mf.Count)]);
				NextTask ();
				return;
			}
		}
		float dist = Vector2.Distance ((Vector2)Fighter.BasicMove.transform.position, target);
		if (dist < TargetRange)
			return;
		Fighter.BasicMove.MoveToPoint((Vector3)target);
	}
}
