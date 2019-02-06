using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

[System.Serializable]
public class AttackStanceInfo {
	public bool CanChangeDirection = true;
	public bool CanMove = true;
	public bool CanJump = true;
	public string IdleAnimation = "idle";
	public string WalkAnimation = "walk";
	public string AirAnimation = "air";

	public float speedModifier = 0.5f;
	public float jumpModifier = 1.0f;

	public bool Holdable = false;
	public bool HasMaxTime = false;
	public float MaxTime = 10.0f;

	//public bool ForceMovement = false;
	//public Vector2 ForcedInput = new Vector2 (0, 0);
}

public class AtkStance : AttackInfo {

	public AttackStanceInfo m_stanceInfo;

	private string old_idle = "idle";
	private string old_walk = "walk";
	private string old_air = "air";

	private bool old_can_jump = true;
	private bool old_can_move = true;
	protected float m_time_in_stance = 0;

	protected override void OnStartUp() {
		old_can_move = GetComponent<PhysicsSS> ().CanMove;
	}

	protected override void OnRecovery() {
		OnStanceEnd ();
	}

	public override void OnInterrupt(float stunTime, bool successfulHit, HitInfo hi) {
		OnStanceEnd ();
	}

	protected override void OnAttack ()
	{
		OnStanceStart ();
	}

	protected override void AttackTick ()
	{
		m_time_in_stance += Time.deltaTime;
		if ((!m_stanceInfo.HasMaxTime || (m_stanceInfo.HasMaxTime && m_time_in_stance < m_stanceInfo.MaxTime)) &&
			(InputManager.GetButton ("Ability1") || InputManager.GetButton ("Ability2") ||
				InputManager.GetButton ("Ability3")))
			DelayCurrentAttack (Time.deltaTime);
		GetComponent<Fighter> ().ProgressWalkOrIdleAnimation ();
	}

	protected virtual void OnStanceStart() {
		Fighter f = GetComponent<Fighter> ();

		old_air = f.AirAnimation;
		old_walk = f.WalkAnimation;
		old_idle = f.IdleAnimation;
			
		f.AirAnimation = m_stanceInfo.AirAnimation;
		f.WalkAnimation = m_stanceInfo.WalkAnimation;
		f.IdleAnimation = m_stanceInfo.IdleAnimation;

		BasicMovement bm = GetComponent<BasicMovement> ();
		old_can_jump = bm.CanJump;

		bm.SetMoveSpeed (bm.MoveSpeed * m_stanceInfo.speedModifier);
		bm.SetJumpHeight (bm.JumpHeight * m_stanceInfo.jumpModifier);
		bm.CanJump = m_stanceInfo.CanJump;


		GetComponent<PhysicsSS> ().CanMove = m_stanceInfo.CanMove;
		m_time_in_stance = Time.deltaTime;
	}

	protected virtual void OnStanceEnd() {
		if (m_time_in_stance > 0f) {
			Fighter f = GetComponent<Fighter> ();
			f.AirAnimation = old_air;
			f.WalkAnimation = old_walk;
			f.IdleAnimation = old_idle;

			BasicMovement bm = GetComponent<BasicMovement> ();
			bm.SetMoveSpeed (bm.MoveSpeed / m_stanceInfo.speedModifier);
			bm.SetJumpHeight (bm.JumpHeight / m_stanceInfo.jumpModifier);
			bm.CanJump = old_can_jump;

			GetComponent<PhysicsSS> ().CanMove = old_can_move;
		}
		m_time_in_stance = 0f;
	}
}
