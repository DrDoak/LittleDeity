using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBlade : Projectile {

	public float MaxPinTime = 10.0f;
	bool pinned = false;
	float TrueStun = 0f;
	float TrueDamage = 0f;
	Vector2 TrueKB = new Vector2(0f,0f);
	float TrueSpeed = 10f;
	bool activated = false;
	// Use this for initialization
	void Start () {
		TrueStun = Stun;
		TrueDamage = Damage;
		TrueKB = Knockback;
		TrueSpeed = ProjectileSpeed;

		ProjectileSpeed = 0.02f;
		Damage = 0f;
		Stun = 0f;
		Knockback = new Vector2 (0f, 0f);
	}
	void OnActivate() {
		if (pinned == false) {
			Damage = TrueDamage;
			Stun = TrueStun;
			ProjectileSpeed = TrueSpeed;
			Knockback = TrueKB;
			Duration = 5f;
			activated = true;
			m_collidedObjs.Clear ();
		}
	}

	new virtual internal void Update()
	{
		Tick();
	}

	protected override void Tick()
	{
		base.Tick ();
		if (Duration < 0.5f && !activated)
			OnActivate ();
		if (Duration < 0.1f)
			GetComponent<Attackable> ().DamageObj (1000f);
	}

	protected override void OnHitObject(Collider2D other) {
		
		if (other.gameObject != Creator && !other.isTrigger && !JumpThruTag (other.gameObject)
			&& other.GetComponent<Attackable> () == null) {
			FindObjectOfType<AudioManager> ().PlayClipAtPos (FXHit.Instance.SFXGuard,transform.position,0.05f,0f,0.25f);
			OnPin ();
		}
	}
	private void OnPin() {
		pinned = true;
		ProjectileSpeed = 0f;
		Damage = 0f;
		Stun = 0f;
		Knockback = new Vector2 (0f, 0f);
		Duration = MaxPinTime;
	}

	private bool JumpThruTag( GameObject obj ) {
		return (obj.CompareTag ("JumpThru") || (obj.transform.parent != null &&
			obj.transform.parent.CompareTag ("JumpThru")));
	}
}
