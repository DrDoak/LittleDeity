using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterTask : MonoBehaviour {
	[HideInInspector]
	public Fighter Player;

	[HideInInspector]
	public AIFighter Fighter;

	public bool Active { get; private set; }

	private FighterRoutine m_routine;

	public virtual void Init(Fighter player, AIFighter fighter, FighterRoutine routine) {
		this.Player = player;
		this.Fighter = fighter;
		this.m_routine = routine;
	}

	public void Activate() {
		Active = true;
	}

	public void NextTask() {
		m_routine.MoveToNextTask ();
	}

	virtual public void Advance() {}

	virtual public void OnSight(Observable o) {}
	virtual public void OnHit(Hitbox hb) {}
}
