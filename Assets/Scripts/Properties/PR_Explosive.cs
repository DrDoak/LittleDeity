using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Explosive : Property {

    Vector2 scl = new Vector2(3.5f, 3.5f);
    Vector2 off = new Vector2(0f, 0f);
    float dmg = 50.0f;
    float stun = 1.0f;
    float hd = 0.5f;
    Vector2 kb = new Vector2(20.0f, 10.0f);
	float oldDeathTime = 0.0f;
	//Resistence fireVulnerability;
	GameObject fx;
		
	public override void OnAddProperty()
	{
		oldDeathTime = GetComponent<Attackable> ().DeathTime;
		//fireVulnerability = GetComponent<Attackable>().AddResistence(ElementType.FIRE, -100.0f, false, false);

		GetComponent<Attackable>().DeathTime = 0.0f;
		fx = GetComponent<PropertyHolder> ().AddBodyEffect (FXBody.Instance.FXExplosive);
	}

	public override void OnRemoveProperty()
	{
		GetComponent<Attackable>().DeathTime = oldDeathTime;
		//	GetComponent<Attackable>().RemoveResistence(fireVulnerability);
		GetComponent<PropertyHolder> ().RemoveBodyEffect (fx);
	}
    public override void OnDeath()
    {
		AudioSource.PlayClipAtPoint(FXHit.Instance.SFXExplosive, transform.position);
		GetComponent<HitboxMaker>().CreateHitbox(scl, off, dmg, stun, hd, kb, false,false,ElementType.FIRE);
		Instantiate(FXHit.Instance.FXExplosion, transform.position, transform.rotation);
    }
}
