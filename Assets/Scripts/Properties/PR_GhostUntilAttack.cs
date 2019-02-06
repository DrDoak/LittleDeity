using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_GhostUntilAttack : Property
{

	private Fighter m_fight;
	private Attackable m_attack;
	private bool m_oldCanTarget;

	Resistence physResist;
	//Resistence fireResist;
	Resistence lightningResist;
    // Start is called before the first frame update

	public override void OnUpdate() { 
		if (m_attack != null && m_fight != null &&
			!m_fight.IsAttacking() && m_fight.StunTime <= 0f) {
			if (physResist == null) {
				physResist = m_attack.AddResistence(ElementType.PHYSICAL, 100.0f, false, false,0f, 100.0f, 100.0f);
				lightningResist = m_attack.AddResistence(ElementType.LIGHTNING, 100.0f, false, false,0f, 100.0f, 100.0f);
				m_attack.CanTarget = false;
			}
		}
	}

	public override void OnAttack(AttackInfo ai) { 
		m_attack.RemoveResistence (physResist);
		m_attack.RemoveResistence (lightningResist);
		physResist = null;
		m_attack.CanTarget = true;
	}

	public override void OnAddProperty() {
		m_attack = GetComponent<Attackable> ();
		m_fight = GetComponent<Fighter> ();
		Debug.Log ("Setting Faction Type");
		physResist = m_attack.AddResistence(ElementType.PHYSICAL, 100.0f, false, false,0f, 100.0f, 100.0f);
		lightningResist = m_attack.AddResistence(ElementType.LIGHTNING, 100.0f, false, false,0f, 100.0f, 100.0f);
		m_oldCanTarget = m_attack.CanTarget;
		m_attack.CanTarget = false;
	}
	public override void OnRemoveProperty() {
		m_attack.RemoveResistence (physResist);
		m_attack.RemoveResistence (lightningResist);
		m_attack.CanTarget = m_oldCanTarget;
	}
}
