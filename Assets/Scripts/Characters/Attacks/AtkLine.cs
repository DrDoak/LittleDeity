using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackLineInfo {
	public float range = 0f;
	public Vector2 direction = new Vector2 (0f, 0f);
	public Vector2 HitboxOffset = new Vector2(0f,0f);
	public float Damage = 10.0f;
	public float Stun = 0.3f;
	public float HitboxDuration = 0.5f;
	public Vector2 Knockback = new Vector2(10.0f,10.0f);
	public ElementType Element = ElementType.PHYSICAL;
}


public class AtkLine : AttackInfo {

	public AttackLineInfo m_lineInfo;


	protected override void OnAttack() {
		base.OnAttack ();
		LineHitbox lbox = GetComponent<HitboxMaker>().createLineHB(m_lineInfo.range, m_lineInfo.direction, m_lineInfo.HitboxOffset, m_lineInfo.Damage, 
			m_lineInfo.Stun, m_lineInfo.HitboxDuration, m_lineInfo.Knockback, true,m_lineInfo.Element);
		lbox.Stun = m_lineInfo.Stun;
	}
}
