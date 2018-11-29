using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

[RequireComponent (typeof (HalberdTargetFinder))]
public class AtkHalberdToss : AttackInfo {

	public float TargetTime;
	private bool OldFloating;
	private GameObject m_target;
	private bool m_targetHit = false;
	private bool m_return = false;

	private float m_time_tossed = 0f;
	private float m_max_time_wait = 2f;

	private Vector2 m_old_halberd_speed;
	private float m_old_warp;

	protected override void OnStartUp() {
		OldFloating = m_physics.Floating;
		ToggleGravity (false);   
		m_targetHit = false;
		m_time_tossed = 0f;
		m_return = false;

	}

	protected override void OnAttack()
	{
		if (GetComponent<HalberdTargetFinder> () != null &&
			GetComponent<HalberdTargetFinder> ().HasTarget())
			m_target = GetComponent<HalberdTargetFinder> ().CurrentTarget;
		ActivateHalberd ();
		GetComponent<PhysicsSS> ().FacingLeft = (m_target.transform.position.x < transform.position.x);
		base.OnAttack ();
	}

	protected override void OnRecovery() {
		EndAttack ();
	}

	public override void OnInterrupt(float stunTime, bool successfulHit, HitInfo hi) {
		EndAttack ();
	}


	public override void OnHitConfirm(GameObject other, HitInfo hb, HitResult hr) {
		if (m_target == other) {
			m_targetHit = true;
			FreezeHalberd ();
		}
	}

	protected override void AttackTick ()
	{
		m_time_tossed += Time.deltaTime;
		if (m_targetHit && m_target != null) {
			if (HoldingAwayTarget (m_target.transform.position)) {
				m_return = true;
			} else if (HoldingTowardsTarget (m_target.transform.position)) {
				GetComponent<Fighter>().EndAttack();
				OnInterrupt (0f,true,new HitInfo());
				GetComponent<Fighter> ().TryAttack ("chase");
			} else if (InputManager.GetButton("Up")) {
				GetComponent<Fighter>().EndAttack();
				OnInterrupt (0f,true,new HitInfo());
				GetComponent<Fighter> ().TryAttack ("chase_up");
			} else if (InputManager.GetButton("Down")) {
				GetComponent<Fighter>().EndAttack();
				OnInterrupt (0f,true,new HitInfo());
				GetComponent<Fighter> ().TryAttack ("chase_down");
			}
		}
		GetComponent<AnimatorSprite> ().Play (m_AttackAnimInfo.RecoveryAnimation);
		if (!m_return && m_target != null && m_time_tossed < m_max_time_wait) {
			DelayCurrentAttack (Time.deltaTime);
		}
	}

	private bool HoldingTowardsTarget(Vector3 target) {
		return ((transform.position.x < target.x && InputManager.GetButton ("Right")) ||
			(transform.position.x > target.x && InputManager.GetButton ("Left")));
	}

	private bool HoldingAwayTarget(Vector3 target) {
		return ((transform.position.x > target.x && InputManager.GetButton ("Right")) ||
			(transform.position.x < target.x && InputManager.GetButton ("Left")));
	}

	private void ToggleGravity(bool toggle) {
		m_physics.CancelVerticalMomentum ();
		m_physics.Floating = !toggle;
	}

	private void EndAttack() {
		m_physics.Floating = OldFloating;
		DeactivateHalberd ();
	}
	private void ActivateHalberd() {
		GameObject h = GetComponent<HalberdTargetFinder> ().m_halberd;
		h.GetComponent<Projectile> ().SetHitboxActive(true);
		h.GetComponent<ChaseTarget>().SetTargetOffset(m_target,new Vector2(0,0f));
		h.GetComponent<WeaponHalberd> ().StartSpinFX (10f, 20f);
		m_old_halberd_speed = h.GetComponent<ChaseTarget> ().MaxSpeed;
		h.GetComponent<ChaseTarget> ().MaxSpeed = new Vector2 (0.4f, 0.4f);
		m_old_warp = h.GetComponent<ChaseTarget> ().WarpDistance;
		h.GetComponent<ChaseTarget> ().WarpDistance = 20f;
	}

	private void FreezeHalberd() {
		GameObject h = GetComponent<HalberdTargetFinder> ().m_halberd;
		h.GetComponent<WeaponHalberd> ().StartSpinFX (3f,0f);
		h.GetComponent<ChaseTarget>().SetTargetOffset(null,new Vector2(0,0f));
	}

	private void DeactivateHalberd() {
		GameObject h = GetComponent<HalberdTargetFinder> ().m_halberd;
		h.GetComponent<Projectile> ().SetHitboxActive(false);
		h.GetComponent<WeaponHalberd> ().StartSpinFX (0f, 3f);
		h.GetComponent<ChaseTarget> ().MaxSpeed = m_old_halberd_speed;
		h.GetComponent<ChaseTarget> ().WarpDistance = m_old_warp;
	}
}