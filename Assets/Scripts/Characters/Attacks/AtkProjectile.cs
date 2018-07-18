using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class ProjectileInfo {
	public GameObject Projectile = null;
	public Vector2 ProjectileCreatePos = new Vector2 (1.0f, 0f);
	public Vector2 ProjectileAimDirection = new Vector2 (1.0f, 0f);
	public float ProjectileSpeed = 10.0f;
	public int PenetrativePower = 1;
	public float Damage = 10.0f;
	public float Stun = 0.3f;
	public float HitboxDuration = 0.5f;
	public Vector2 Knockback = new Vector2(10.0f,10.0f);
	public ElementType Element = ElementType.PHYSICAL;
}


public class AtkProjectile : AtkDash {
	
	[SerializeField]
	private ProjectileInfo m_ProjectileData;

	protected override void OnAttack()
	{
		base.OnAttack();
		Projectile p = GetComponent<HitboxMaker> ().CreateProjectile (m_ProjectileData.Projectile, m_ProjectileData.ProjectileCreatePos, 
			m_ProjectileData.ProjectileAimDirection, m_ProjectileData.ProjectileSpeed,
			m_ProjectileData.Damage,m_ProjectileData.Stun,m_ProjectileData.HitboxDuration,m_ProjectileData.Knockback,true,
			m_ProjectileData.Element);
		p.PenetrativePower = m_ProjectileData.PenetrativePower;
	}
}
