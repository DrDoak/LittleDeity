using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

[System.Serializable]
public class AutoAttackSettings {
	public float RefreshTime = 0.5f;
	public List<string> AttackAnimations;
	public float AttackAnimationTime = 0.2f;
}

public class AtkAutoSlash : AtkStance {

	public AutoAttackSettings m_autoAttack;
	public WeaponPosition m_weaponPos;
	HitboxMulti DetectionBox;

	private float m_sinceLastAutoAttack = 0.0f;
	private Hitbox m_detectionBox;
	private float m_last_attackTime = 0.0f;

	private string m_animation = "none";

	protected override void OnAttack()
	{
		HitboxInfo hi = m_HitboxInfo [0];
		if (m_HitboxInfo.Count > 0)
			DetectionBox = m_hitboxMaker.CreateHitboxMulti (hi.HitboxScale,hi.HitboxOffset,hi.Damage,hi.Stun,m_AttackAnimInfo.AttackTime,
				hi.Knockback,hi.FixedKnockback,true,hi.Element,m_autoAttack.RefreshTime);
		base.OnAttack ();
	}

	public override void OnHitConfirm(GameObject other, Hitbox hb, HitResult hr) {
		m_last_attackTime = Time.timeSinceLevelLoad;
		m_animation = m_autoAttack.AttackAnimations [Random.Range (0, m_autoAttack.AttackAnimations.Count)];
		FindObjectOfType<WeaponFloating> ().StartSlashFX (m_weaponPos);
	}

	protected override void AttackTick ()
	{
		m_time_in_stance += Time.deltaTime;
		if ((!m_stanceInfo.HasMaxTime || (m_stanceInfo.HasMaxTime && m_time_in_stance < m_stanceInfo.MaxTime)) &&
			(InputManager.GetButton ("Ability1") || InputManager.GetButton ("Ability2") ||
				InputManager.GetButton ("Ability3"))) {
			DelayCurrentAttack (Time.deltaTime);
			DetectionBox.Duration += Time.deltaTime;
		}
		if (Time.timeSinceLevelLoad - m_last_attackTime < m_autoAttack.AttackAnimationTime) {
			GetComponent<AnimatorSprite> ().Play (m_animation);
		} else {
			GetComponent<Fighter> ().ProgressWalkOrIdleAnimation ();
		}
	}
}
