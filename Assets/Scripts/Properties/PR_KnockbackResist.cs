using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_KnockbackResist : Property {

	Resistence physResist;
	Resistence fireResist;
	Resistence elecResist;
	Resistence bioResist;
	Resistence psyResist;

	// Use this for initialization
	public override void OnAddProperty() {
		physResist = GetComponent<Attackable>().AddResistence(ElementType.PHYSICAL, 0.0f,false,false,0f,0f,100f);
		fireResist = GetComponent<Attackable>().AddResistence(ElementType.FIRE, 0.0f,false,false,0f,0f,100f);
		elecResist = GetComponent<Attackable>().AddResistence(ElementType.LIGHTNING,0.0f,false,false,0f,0f,100f);
		bioResist = GetComponent<Attackable>().AddResistence(ElementType.BIOLOGICAL,0.0f,false,false,0f,0f,100f);
		psyResist = GetComponent<Attackable>().AddResistence(ElementType.PSYCHIC, 0.0f,false,false,0f,0f,100f);
	}

	// Update is called once per frame
	public override void OnRemoveProperty()
	{
		GetComponent<Attackable>().RemoveResistence(physResist);
		GetComponent<Attackable>().RemoveResistence(fireResist);
		GetComponent<Attackable>().RemoveResistence(elecResist);
		GetComponent<Attackable>().RemoveResistence(bioResist);
		GetComponent<Attackable>().RemoveResistence(psyResist);
	}
}