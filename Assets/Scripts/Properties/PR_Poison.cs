using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Poison : Property {

	Vector2 off = new Vector2(0f, 0f);
	float dmg = 2.0f;
	float stun = 0.0f;
	float hd = -0.5f;
	Vector2 kb = new Vector2(0.0f, 0.0f);
	List<ElementType> bioOnly;
	HitboxMulti bioSurround;
	private float time_tracker = 0f;
	private float bio_period = 2f;

	float eliminatedHealth = 0f;
	float squashedHealth = 0f;
	float oldMaxHealth = 0f;

	public override void OnAddProperty()
	{
		Vector3 sc = GetComponent<PropertyHolder> ().BodyScale ();
		sc *= 1.2f;
		if (GetComponent<HitboxMaker> () != null) {
			bioSurround = GetComponent<HitboxMaker> ().CreateHitboxMulti (sc, off, dmg, stun, hd, kb, true, true, ElementType.BIOLOGICAL, 0.5f);
			bioSurround.GetComponent<Hitbox> ().Faction = FactionType.HOSTILE;
			//fx = GetComponent<PropertyHolder> ().AddBodyEffect (FXBody.Instance.FXFlame);
			bioOnly = new List<ElementType> ();
			bioOnly.Add (ElementType.BIOLOGICAL);
			eliminatedHealth = GetComponent<Attackable> ().MaxHealth / 2f;
			squashedHealth = Mathf.Max (0f, GetComponent<Attackable> ().Health - eliminatedHealth);
			GetComponent<Attackable> ().DamageObj (squashedHealth);
		}
	}
	public override void OnRemoveProperty() {
		if (bioSurround != null) 
			Destroy (bioSurround);
		GetComponent<Attackable> ().DamageObj (-squashedHealth);
	}

	public override void OnUpdate()
	{
		if (bioSurround != null) {
			List<ElementType> oldEle = bioSurround.Element;
			bioSurround.Element = bioOnly;
			if (GetComponent<Attackable> ().Health > eliminatedHealth) {
				GetComponent<Attackable> ().DamageObj (GetComponent<Attackable> ().Health - eliminatedHealth);
			}
			if (Time.timeSinceLevelLoad > time_tracker) {
				time_tracker = Time.timeSinceLevelLoad + bio_period;
				GetComponent<Attackable> ().TakeHit (bioSurround);
				GameObject.Instantiate (FXHit.Instance.FXHitBiological, transform.position, Quaternion.identity);
			}
			bioSurround.Element = oldEle;
		}
	}

	public override void OnHitboxCreate(Hitbox hitboxCreated) {
		hitboxCreated.AddElement(ElementType.BIOLOGICAL);
	}
}
