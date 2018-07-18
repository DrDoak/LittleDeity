using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Melee : Ability {
    
    public float Damage { get; protected set; }
    public float Speed { get; protected set; }

    private float damageUpgrade = 15f;
    private float speedUpgrade = 1f;

	public override void Awake()
	{
        base.Awake();
		AbilityClassification = AbilityType.COMBAT;
        if (Player)
        {
            Damage = Player.GetComponent<AttackInfo>().m_HitboxInfo.Damage;
            Speed = Player.GetComponent<AttackInfo>().m_AttackAnimInfo.AnimSpeed;
        }
	}

	public override void UseAbility()
	{
		Player.GetComponent<Fighter> ().TryAttack ("melee");
        EventManager.TriggerEvent(EventManager.MELEE_SPECIAL);
	}

    public void UpgradeDamage()
    {
        Damage += damageUpgrade;
        UpdateFighter();
    }

    public void UpgradeAttackRate()
    {
        Speed += speedUpgrade;
        UpdateFighter();
    }


    private void UpdateFighter()
    {
        AttackInfo[] attacks = Player.GetComponents<AttackInfo>();
        foreach(AttackInfo a in attacks)
        {
            a.m_HitboxInfo.Damage = Damage;
            a.m_AttackAnimInfo.AnimSpeed = Speed;
        }
    }
}
