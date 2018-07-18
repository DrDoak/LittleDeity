using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Wooden : Property {

	public float m_flameDamage = 0f;
	const float FLAME_THREASHOLD = 8.0f;
	public bool inWater = false;

	public override void OnAddProperty()
	{
		m_flameDamage = 0f;
		if (GetComponent<BasicMovement> () != null) {
			//GetComponent<BasicMovement> ().SetJumpData (GetComponent<BasicMovement> ().JumpHeight, GetComponent<BasicMovement> ().TimeToJumpApex * 1.4f);
		}
	}
	public override void OnRemoveProperty()
	{
		m_flameDamage = 0f;
		if (GetComponent<BasicMovement> () != null) {
			//GetComponent<BasicMovement> ().SetJumpData (GetComponent<BasicMovement> ().JumpHeight, GetComponent<BasicMovement> ().TimeToJumpApex / 1.4f);
		}
	}
	public override void OnUpdate() {
		if (m_flameDamage > 0f)
			m_flameDamage -= (0.2f * Time.deltaTime);
		floatPhysics ();
	}
	void floatPhysics() {
		if (GetComponent<PhysicsSS> () == null)
			return;
		if (GetComponent<PropertyHolder> ().SubmergedHitbox != null) {
			float surfaceLevel = GetComponent<PropertyHolder> ().SubmergedHitbox.SurfaceLevel;
			float diff = surfaceLevel - 0.2f - transform.position.y;
			GetComponent<PhysicsSS> ().AddSelfForce (new Vector2 (0f, -0.2f * GetComponent<PhysicsSS> ().TrueVelocity.y), Time.fixedDeltaTime);
			if (Mathf.Abs(diff) > 0.3f) {
				float scale = (diff/4f) * (diff/4f) * Mathf.Sign(diff);
				GetComponent<PhysicsSS> ().BuoyancyScale = scale;
			} else {
				GetComponent<PhysicsSS> ().BuoyancyScale = 0f;
			}
		} else {
			GetComponent<PhysicsSS> ().BuoyancyScale = -1f;
		}
	}

	public override void OnHit(Hitbox hb, GameObject attacker) { 
		if (!GetComponent<PropertyHolder> ().HasProperty ("Flaming")) {
			//Debug.Log ("Does not have flaming");
			if (hb.HasElement(ElementType.FIRE)) {
				//Debug.Log ("has flaming");
				HitboxDoT hd = hb as HitboxDoT;
				if (hd != null) {
					m_flameDamage += (Time.deltaTime * hb.Damage);
				} else {
					m_flameDamage += hb.Damage;
				}
				if (m_flameDamage >= FLAME_THREASHOLD) {
					GetComponent<PropertyHolder> ().AddProperty ("PR_Flaming");
					m_flameDamage = 0f;
				}
			}
		}
	}
	public override void OnWaterEnter(WaterHitbox waterCollided)  {
		GetComponent<PhysicsSS> ().ReactToWater = true;
	}
	public override void OnWaterExit(WaterHitbox waterCollided) {
		GetComponent<PhysicsSS> ().ReactToWater = false;
	}
}
