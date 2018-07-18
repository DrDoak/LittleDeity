using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Burner : PR_Mechanical {

	Vector2 scl = new Vector2(6f, 1.0f);
	Vector2 off = new Vector2(3.5f, 0f);
	float dmg = 20.0f;
	float stun = 0.0f;
	float hd = -0.5f;
	Vector2 kb = new Vector2(25.0f, 0.0f);
	HitboxDoT dotBox;
	GameObject fx;

	List<ElementType> fireOnly;

	protected override void OnActive() {
		fireOnly = new List<ElementType> ();
		fireOnly.Add (ElementType.FIRE);

		dotBox = GetComponent<HitboxMaker>().CreateHitboxDoT(scl, off, dmg, stun, hd, kb,true, true, ElementType.FIRE);

		fx = GetComponent<PropertyHolder> ().AddBodyEffect (FXBody.Instance.FXBurner);
		GetComponent<PropertyHolder> ().AddAmbient (FXBody.Instance.SFXFlaming);
	}
	protected override void OnDisable() {
		if (dotBox != null) {
			Destroy (dotBox);
			GetComponent<PropertyHolder> ().RemoveBodyEffect (fx);
			GetComponent<PropertyHolder> ().RemoveAmbient (FXBody.Instance.SFXFlaming);
		}
	}
}
