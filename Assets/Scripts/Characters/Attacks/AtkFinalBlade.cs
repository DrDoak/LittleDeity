using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class BladeInfo {
	public GameObject Projectile = null;
	public Vector2 ProjectileCreatePos = new Vector2 (1.0f, 0f);
	public Vector2 ProjectileAimDirection = new Vector2 (1.0f, 0f);
	public float ProjectileSpeed = 20.0f;
	public int PenetrativePower = 1;
	public float Damage = 20.0f;
	public float Stun = 0.3f;
	public float KnifeDelay = 0.5f;
	public Vector2 Knockback = new Vector2(10.0f,0.0f);
	public ElementType Element = ElementType.PHYSICAL;
}

public class AtkFinalBlade : AttackInfo {

	[SerializeField]
	private List<BladeInfo> m_Blades;

	protected override void OnStartUp()
	{
		base.OnStartUp();
		foreach (BladeInfo proj_info in m_Blades) {
			Projectile p = GetComponent<HitboxMaker> ().CreateProjectile (proj_info.Projectile, proj_info.ProjectileCreatePos, 
				proj_info.ProjectileAimDirection, proj_info.ProjectileSpeed,
				proj_info.Damage, proj_info.Stun, proj_info.KnifeDelay + 0.5f, proj_info.Knockback, true,
				proj_info.Element);
			p.PenetrativePower = proj_info.PenetrativePower;
		}
	}
}
