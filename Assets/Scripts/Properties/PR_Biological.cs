using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Biological : Property
{
	public float m_bioDamage = 0f;
	const float BIO_THREASHOLD = 10.0f;

	Resistence bioVulnerability;

    float time_tracker = 0.0f;
    float damagetime_tracker = 0.0f;
    float damagetime_period = 3.0f;
    float heal_period = 0.1f;
    float heal_amount = 2f;

	public override void OnAddProperty()
	{
		bioVulnerability = GetComponent<Attackable>().AddResistence(ElementType.BIOLOGICAL, -100.0f, false, false, 0.0f, 70.0f, 70.0f);
	}

    public override void OnUpdate()
    {
       /* if(Time.time > time_tracker)
        {
            time_tracker += heal_period;
            if(Time.time > damagetime_tracker + damagetime_period)
            {
				GetComponent<Attackable>().DamageObj(heal_amount * -1.0f * Time.deltaTime);
            }
            
        }*/
    }

	public override void OnRemoveProperty()
    {
		GetComponent<Attackable>().RemoveResistence(bioVulnerability);
    }
		
	public override void OnHit(Hitbox hb, GameObject attacker) { 
		if (!GetComponent<PropertyHolder> ().HasProperty ("Parasite")) {
			if (hb.HasElement(ElementType.BIOLOGICAL)) {
				HitboxDoT hd = hb as HitboxDoT;
				if (hd != null) {
					m_bioDamage += (Time.deltaTime * hb.Damage);
				} else {
					m_bioDamage += hb.Damage;
				}
				if (m_bioDamage >= BIO_THREASHOLD) {
					GetComponent<PropertyHolder> ().AddProperty ("PR_Poison");
					m_bioDamage = 0f;
				}
			}
		}
	}

}