using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Spike : Property {

	HitboxMulti launchBox;
	Vector2 scl = new Vector2(1.0f, 2.0f);
	Vector2 off2 = new Vector2(0f, 0f);
	float dmg = 20.0f;
	float stun = 0.25f;
	float hd = -0.5f;
	Vector2 kb = new Vector2(10.0f, 0.0f);

	public override void OnAddProperty()
	{
		launchBox = GetComponent<HitboxMaker>().CreateHitboxMulti(scl, off2, dmg, stun, hd, kb,true, true, ElementType.PHYSICAL,0.75f);
	}

	public override void OnRemoveProperty()
	{
		Destroy (launchBox);
	}
}
