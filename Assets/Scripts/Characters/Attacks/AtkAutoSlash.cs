using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

[System.Serializable]
public class AutoAttackSettings {
	public float RefreshTime = 0.5f;
	public List<string> AttackAnimations;
	public bool EnableUpDown = true;
	public List<string> UpAttackAnimations;
	public List<string> DownAttackAnimations;
	public float AttackAnimationTime = 0.2f;
	public Vector2 MaxTargetSpeed = new Vector2(0.6f,0.6f);
	public float Decceleration = -1f;
	public WeaponPosition up_WeaponPos;
	public WeaponPosition down_WeaponPos;
}

public class AtkAutoSlash : AtkStance {

	public AutoAttackSettings m_autoAttack;
	public WeaponPosition m_weaponPos;
	HitboxMulti DetectionBox;

	private float m_sinceLastAutoAttack = 0.0f;
	private float m_last_attackTime = 0.0f;

	private string m_animation = "none";

	private Vector2 FollowPoint;
	private Vector2 m_chaseSpeed;
	private float FollowTime;
	private bool OldFloating;
	private string m_attackDirection = "side";
	private bool lastFacingLeft;

	protected override void OnAttack()
	{
		HitboxInfo hi = m_HitboxInfo [0];
		FollowTime = 0f;
		if (m_HitboxInfo.Count > 0) {
			DetectionBox = m_hitboxMaker.CreateHitboxMulti (hi.HitboxScale, hi.HitboxOffset, hi.Damage, hi.Stun, m_AttackAnimInfo.AttackTime,
				hi.Knockback, hi.FixedKnockback, true, hi.Element, m_autoAttack.RefreshTime);
			DetectionBox.IsResetKnockback = hi.ResetKnockback;
		}
		OldFloating = m_physics.Floating;
		lastFacingLeft = GetComponent<PhysicsSS> ().FacingLeft;
		DirectionOrient ();
		base.OnAttack ();
	}

	public override void OnInterrupt (float stunTime, bool successfulHit, HitInfo hi)
	{
		base.OnInterrupt (stunTime, successfulHit, hi);
		VerticalMomentumCancel ();
		m_physics.Floating = OldFloating;
	}

	protected override void OnConclude ()
	{
		base.OnConclude ();
		VerticalMomentumCancel ();
		m_physics.Floating = OldFloating;
	}

	public override void OnHitConfirm(GameObject other, HitInfo hb, HitResult hr) {
		m_last_attackTime = Time.timeSinceLevelLoad;

		string newAnim = m_animation;
		List<string> animArray = new List<string> ();

		if (m_attackDirection == "down") {
			SetFollowPoint (new Vector3 (0f, -3f, 0f), other.transform.position,0.4f);
			animArray = m_autoAttack.DownAttackAnimations;
			FindObjectOfType<WeaponFloating> ().StartSlashFX (m_autoAttack.down_WeaponPos);
		} else if (m_attackDirection == "up") {
			if (!GetComponent<PhysicsSS>().OnGround)
				SetFollowPoint (new Vector3 (0f, 3f, 0f), other.transform.position,0.4f,true);
			animArray = m_autoAttack.UpAttackAnimations;
			FindObjectOfType<WeaponFloating> ().StartSlashFX (m_autoAttack.up_WeaponPos);
		} else if (HoldingTowardsTarget (other.transform.position)) {
			SetFollowPoint (new Vector3 (4f, 0.2f, 0f), other.transform.position,0.4f,!GetComponent<PhysicsSS>().OnGround);
			animArray = m_autoAttack.AttackAnimations;
			FindObjectOfType<WeaponFloating> ().StartSlashFX (m_weaponPos);
		} else {
			SetFollowPoint (new Vector3 (-1.5f, 0.75f, 0f), other.transform.position, 0.25f,!GetComponent<PhysicsSS>().OnGround);
			animArray = m_autoAttack.AttackAnimations;
			FindObjectOfType<WeaponFloating> ().StartSlashFX (m_weaponPos);
		}
		if (animArray.Count > 1) {
			while (newAnim == m_animation) {
				newAnim = animArray [Random.Range (0, animArray.Count)];
			}
		} else {
			newAnim = animArray [0];
		}
		m_animation = newAnim;

		if (GetComponent<BasicMovement> ().CurrentAirJumps == 0)
			GetComponent<BasicMovement> ().CurrentAirJumps += 1;
		m_chaseSpeed = m_autoAttack.MaxTargetSpeed;
		if (GetComponent<PhysicsSS> ().OnGround)
			m_chaseSpeed.y = 0f;

	}

	protected override void AttackTick ()
	{
		m_time_in_stance += Time.deltaTime;
		if (FollowTime > 0.0f || (!m_stanceInfo.HasMaxTime || (m_stanceInfo.HasMaxTime && m_time_in_stance < m_stanceInfo.MaxTime)) &&
			(InputManager.GetButton ("Ability1") || InputManager.GetButton ("Ability2") ||
				InputManager.GetButton ("Ability3"))) {
			DelayCurrentAttack (Time.deltaTime);
			if ((InputManager.GetButton ("Ability1") || InputManager.GetButton ("Ability2") ||
				InputManager.GetButton ("Ability3")))
				DetectionBox.Duration += Time.deltaTime;
		}
		if (Time.timeSinceLevelLoad - m_last_attackTime < m_autoAttack.AttackAnimationTime) {
			GetComponent<AnimatorSprite> ().Play (m_animation);
		} else {
			GetComponent<Fighter> ().ProgressWalkOrIdleAnimation ();
		}

		if (FollowTime > 0.0f) {
			FollowTime -= Time.deltaTime;
			chaseTarget ();
		} else { 
			m_physics.Floating = OldFloating;
		}

		DirectionOrient ();
	}

	private void DirectionOrient() {
		if (InputManager.GetButton ("Down") && !m_physics.OnGround && m_autoAttack.EnableUpDown) {
			DetectionBox.Knockback = GetComponent<PhysicsSS>().OrientVectorToDirection(new Vector2 (m_HitboxInfo [0].Knockback.y, -1f * m_HitboxInfo [0].Knockback.x));
			if (m_attackDirection != "down") {
				DetectionBox.SetFollow (gameObject, new Vector2 (0f, -m_HitboxInfo [0].HitboxOffset.x));
			}
			m_attackDirection = "down";
		} else if (InputManager.GetButton ("Up") && m_autoAttack.EnableUpDown) {
			DetectionBox.Knockback = GetComponent<PhysicsSS>().OrientVectorToDirection(new Vector2 (m_HitboxInfo [0].Knockback.y, 7.5f * m_HitboxInfo [0].Knockback.x));
			if (m_attackDirection != "up") {
				DetectionBox.SetFollow (gameObject, new Vector2 (0f, m_HitboxInfo [0].HitboxOffset.x));
			}
			m_attackDirection = "up";
		} else {
			DetectionBox.Knockback = GetComponent<PhysicsSS>().OrientVectorToDirection(new Vector2 (m_HitboxInfo [0].Knockback.x, m_HitboxInfo [0].Knockback.y));
			if (m_attackDirection != "side" || GetComponent<PhysicsSS> ().FacingLeft != lastFacingLeft) {
				DetectionBox.SetFollow (gameObject, m_HitboxInfo [0].HitboxOffset);
			}
			lastFacingLeft = GetComponent<PhysicsSS> ().FacingLeft;
			m_attackDirection = "side";
		}
	}

	private void SetFollowPoint(Vector3 offset, Vector3 target, float time,bool floating = true) {
		Vector3 o = offset;
		if (target.x < transform.position.x)
			o.x *= -1f;
		FollowPoint = target + o;
		FollowTime = time;
		m_physics.Floating = floating;
		VerticalMomentumCancel ();
	}

	private void chaseTarget() {
		Vector2 m_speed = new Vector2 ();
		if (Mathf.Abs (FollowPoint.x - transform.position.x) < m_chaseSpeed.x)
			m_speed.x = (FollowPoint.x - transform.position.x);
		else
			m_speed.x = Mathf.Sign (FollowPoint.x - transform.position.x) * m_chaseSpeed.x;

		if (Mathf.Abs (FollowPoint.y - transform.position.y) < m_chaseSpeed.y)
			m_speed.y = FollowPoint.y - transform.position.y;
		else
			m_speed.y = Mathf.Sign (FollowPoint.y - transform.position.y) * m_chaseSpeed.y;

		//m_physics.AddSelfForce(m_speed, 0.01f);
		m_speed -= ( Time.deltaTime * (1f - m_autoAttack.Decceleration) * m_speed);
		m_physics.AddArtificialVelocity (m_speed);
	}

	private bool HoldingTowardsTarget(Vector3 target) {
		return ((transform.position.x < target.x && InputManager.GetButton ("Right")) ||
		(transform.position.x > target.x && InputManager.GetButton ("Left")));
	}

	private void VerticalMomentumCancel() {
		m_physics.CancelVerticalMomentum ();
	}
}
