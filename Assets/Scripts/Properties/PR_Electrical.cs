using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Electrical : Property {

	Vector2 off = new Vector2(0f, 0f);
	float dmg = 30.0f;
	float stun = 1.0f;
	float hd = -0.5f;
	Vector2 kb = new Vector2(0.0f, 0.0f);
	List<ElementType> elecOnly;
	HitboxMulti elecBox;
	private float time_tracker = 0f;
	private float bio_period = 2.5f;

	GameObject fx;
	private bool m_inWater;

	public override void OnAddProperty() {
		fx = GetComponent<PropertyHolder> ().AddBodyEffect (FXBody.Instance.FXLightning);
		elecOnly = new List<ElementType> ();
		elecOnly.Add (ElementType.LIGHTNING);
	}
	public override void OnRemoveProperty() {
		GetComponent<PropertyHolder> ().RemoveBodyEffect (fx);
		if (elecBox != null)
			Destroy (elecBox);
	}
	public override void OnHitboxCreate (Hitbox hitboxCreated) {
		hitboxCreated.AddElement( ElementType.LIGHTNING );
		hitboxCreated.Stun = hitboxCreated.Stun * 1.5f;
	}
	public override void OnWaterEnter(WaterHitbox waterCollided)  {
		Vector3 sc = GetComponent<PropertyHolder> ().BodyScale ();
		sc *= 1.2f;
		elecBox = GetComponent<HitboxMaker>().CreateHitboxMulti(sc, off, dmg, stun, hd, kb,true, true, ElementType.LIGHTNING,0.3f);
	}
	public override void OnWaterExit(WaterHitbox waterCollided) {
		if (elecBox != null)
			Destroy (elecBox);
		elecBox = null;
	}
	public override void OnUpdate() {
		if (elecBox != null && Time.timeSinceLevelLoad > time_tracker)
		{
			List<ElementType> oldEle = elecBox.Element;
			elecBox.Element = elecOnly;
			elecBox.Damage *= 1.5f;
			time_tracker = Time.timeSinceLevelLoad + bio_period;
			GetComponent<Attackable>().TakeHit(elecBox);
			GameObject.Instantiate (FXHit.Instance.FXHitLightning, transform.position, Quaternion.identity);
			FindObjectOfType<AudioManager> ().PlayClipAtPos (FXHit.Instance.SFXElectric,transform.position,0.75f,0f,0.25f);
			elecBox.Damage /= 0.5f;
			elecBox.Element = oldEle;
		}
	}
}
