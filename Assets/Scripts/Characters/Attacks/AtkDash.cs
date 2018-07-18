using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackDashInfo {
	public Vector2 StartUpDash = new Vector2 (0.0f, 0f);
	public float StartUpDuration = 0.0f;
	public Vector2 AttackDash = new Vector2 (0.0f, 0f);
	public float AttackDashDuration = 0.0f;
	public Vector2 ConclusionDash = new Vector2 (0.0f, 0f);
	public float ConclusionDuration = 0.0f;
}
public class AtkDash : AttackInfo
{
	public AttackDashInfo m_dashInfo;

	protected override void OnStartUp()
	{
		base.OnStartUp();
		m_physics = GetComponent<PhysicsSS>();
		if (m_dashInfo.StartUpDash.y != 0f)
			VerticalMomentumCancel (m_AttackAnimInfo.StartUpTime);
		m_physics.AddSelfForce(m_physics.OrientVectorToDirection(m_dashInfo.StartUpDash), m_dashInfo.StartUpDuration);
	}

	protected override void OnAttack()
	{
		base.OnAttack();
		if (m_dashInfo.AttackDash.y != 0f)
			VerticalMomentumCancel (m_AttackAnimInfo.RecoveryTime);
		m_physics.AddSelfForce(m_physics.OrientVectorToDirection(m_dashInfo.AttackDash), m_dashInfo.AttackDashDuration);
	}

	protected override void OnConclude()
	{
		base.OnConclude();
		if (m_dashInfo.ConclusionDash.y != 0f)
			VerticalMomentumCancel (m_dashInfo.ConclusionDuration);
		m_physics.AddSelfForce(m_physics.OrientVectorToDirection(m_dashInfo.ConclusionDash), m_dashInfo.ConclusionDuration);
	}

	public virtual void OnInterrupt(float stunTime, bool successfulHit, Hitbox hb)
	{
		m_physics.DisableGravity (0f);
	}
	private void VerticalMomentumCancel(float time) {
		m_physics.CancelVerticalMomentum ();
		m_physics.DisableGravity (m_AttackAnimInfo.StartUpTime);
	}
}
