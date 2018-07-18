using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ab_Forcepush : Ability {

    private Vector2 hitboxScale = new Vector2(2f,2f);
    private Vector2 offset = new Vector2(1.5f,0f);
    private Vector2 knockback = new Vector2(15f,0f);
    private float duration = 0.5f;
    private float stun = 0f;
    private float damage = 0f;

    private float left = -1f;
    private float right = 1f;
    private float dir = 1f;

    public override void UseAbility()
    {
        if (Creator.GetComponent<PhysicsSS>().FacingLeft)
            dir = left;
        else
            dir = right;

        Creator.GetComponent<HitboxMaker>().CreateHitbox(hitboxScale, offset * dir, damage, stun, duration, knockback);
        //Play audio
        //Play animation
    }
}
