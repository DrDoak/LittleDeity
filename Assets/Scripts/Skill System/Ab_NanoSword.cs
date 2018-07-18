using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_NanoSword : Ability {

    private bool selected;
    private int percentage = 10;

    new void Awake()
    {
        base.Awake();
        AbilityClassification = AbilityType.COMBAT;
        selected = false;
    }

    public override void UseAbility()
    {
        int r = Random.Range(0, 100);
        if (r > percentage) return;
        Player.GetComponent<HitboxMaker>().CreateHitbox(Player.GetComponent<AttackInfo>().m_HitboxInfo.HitboxScale, Player.GetComponent<AttackInfo>().m_HitboxInfo.HitboxScale, 
            Player.GetComponent<AttackInfo>().m_HitboxInfo.Damage, Player.GetComponent<AttackInfo>().m_HitboxInfo.Stun, 
            Player.GetComponent<AttackInfo>().m_HitboxInfo.HitboxDuration, Player.GetComponent<AttackInfo>().m_HitboxInfo.Knockback);
    }

    public override void Select()
    {
        if (!selected)
            EventManager.MeleeSpecialEvent += UseAbility;
        else
            EventManager.MeleeSpecialEvent -= UseAbility;

        selected = !selected;
    }
}
