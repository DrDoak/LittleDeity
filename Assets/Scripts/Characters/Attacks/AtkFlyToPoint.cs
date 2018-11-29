using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackFlyInfo {
	public Vector3 Target;
	public float AttackRange = 2.0f;
	public Vector2 MaxTargetSpeed = new Vector2(0.6f,0.6f);
	public Vector2 AttackDash = new Vector2 ();
	public float AttackDashDuration = 0.5f;
	public float Decceleration = -1f;
	public bool FlyToHalberd = true;
	public string FlyAnim = "jump";
}

public class AtkFlyToPoint : AtkStance {

	private bool OldFloating;
	private float m_floatingTime;
	public AttackFlyInfo m_flyInfo;

	protected override void OnStartUp() {
		OldFloating = m_physics.Floating;
		ToggleGravity (false);
		m_stanceInfo.HasMaxTime = true;
	}

	protected override void OnAttack()
	{
		if (m_flyInfo.FlyToHalberd && GetComponent<HalberdTargetFinder> () != null &&
			GetComponent<HalberdTargetFinder> ().HasTarget())
			m_flyInfo.Target = GetComponent<HalberdTargetFinder> ().GetTarget ();
		base.OnAttack ();
	}

	protected override void OnRecovery() {
		m_physics.Floating = OldFloating;
		createHitboxes ();
		m_physics.CancelVerticalMomentum ();

		if (m_flyInfo.AttackDash.y != 0f)
			m_physics.DisableGravity (m_flyInfo.AttackDashDuration);
		m_physics.AddSelfForce(m_physics.OrientVectorToDirection(m_flyInfo.AttackDash), m_flyInfo.AttackDashDuration);
		OnStanceEnd ();
	}

	protected override void AttackTick ()
	{
		m_time_in_stance += Time.deltaTime;
		if (Vector3.Distance (transform.position, m_flyInfo.Target) > m_flyInfo.AttackRange) {
			if (m_time_in_stance < m_stanceInfo.MaxTime){
					DelayCurrentAttack (Time.deltaTime);
			}
		} else if (GetComponent<BasicMovement> ().CurrentAirJumps == 0) {
			GetComponent<BasicMovement> ().CurrentAirJumps += 1;
		}
		GetComponent<AnimatorSprite> ().Play (m_flyInfo.FlyAnim);
		chaseTarget ();
	}

	public override void OnInterrupt (float stunTime, bool successfulHit, HitInfo hi)
	{
		base.OnInterrupt (stunTime, successfulHit, hi);
		m_physics.Floating = OldFloating;
	}

	protected override void OnConclude ()
	{
		base.OnConclude ();
		m_physics.Floating = OldFloating;
	}

	private void ToggleGravity(bool toggle) {
		m_physics.CancelVerticalMomentum ();
		m_physics.Floating = !toggle;
	}

	private void chaseTarget() {
		Vector2 m_speed = new Vector2 ();
		if (Mathf.Abs (m_flyInfo.Target.x - transform.position.x) < m_flyInfo.MaxTargetSpeed.x)
			m_speed.x = (m_flyInfo.Target.x - transform.position.x);
		else
			m_speed.x = Mathf.Sign (m_flyInfo.Target.x - transform.position.x) * m_flyInfo.MaxTargetSpeed.x;

		if (Mathf.Abs (m_flyInfo.Target.y - transform.position.y) < m_flyInfo.MaxTargetSpeed.y)
			m_speed.y = m_flyInfo.Target.y - transform.position.y;
		else
			m_speed.y = Mathf.Sign (m_flyInfo.Target.y - transform.position.y) * m_flyInfo.MaxTargetSpeed.y;
		m_speed -= ( Time.deltaTime * (1f - m_flyInfo.Decceleration) * m_speed);
		m_physics.AddArtificialVelocity (m_speed);
	}
}
