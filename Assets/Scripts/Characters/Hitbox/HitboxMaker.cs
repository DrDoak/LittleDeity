using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HitboxMaker : MonoBehaviour
{

//	public List<ElementType> elementTypes;
	public FactionType Faction;
	PhysicsSS m_physics;
	Fighter m_fighter;

	void Awake () {
		m_physics = GetComponent<PhysicsSS>();
		m_fighter = GetComponent<Fighter>();

	}

	void Start() {
		if (GetComponent<Attackable> ()) {
			Faction = GetComponent<Attackable> ().Faction;
		}
	}
	public LineHitbox createLineHB(float range, Vector2 aimPoint, Vector2 offset,float damage, float stun, float hitboxDuration,
		Vector2 knockback, bool followObj = true,ElementType element = ElementType.PHYSICAL) {
		if (m_physics != null) {
			aimPoint = m_physics.OrientVectorToDirection (aimPoint);
			offset = m_physics.OrientVectorToDirection (offset);
		}
		Vector3 newPos = new Vector3(transform.position.x + offset.x, transform.position.y + offset.y, 0);
		GameObject go = Instantiate(HitboxList.Instance.HitboxLine,newPos,Quaternion.identity) as GameObject; 
		LineHitbox line = go.GetComponent<LineHitbox> ();
		line.setRange (range);
		line.Damage = damage;
		line.setAimPoint (aimPoint);
		line.Duration = hitboxDuration;
		if (m_physics != null)
			line.Knockback = m_physics.OrientVectorToDirection(knockback);
		line.IsFixedKnockback = true;
		line.Creator = gameObject;
		line.Faction = Faction;
		line.AddElement(element);
		line.Stun = stun;
		line.Init();

		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnHitboxCreate(line));
		return line;
	}

	public Hitbox CreateHitbox(Vector2 hitboxScale, Vector2 offset, float damage, float stun, float hitboxDuration, Vector2 knockback, bool fixedKnockback = true,
		bool followObj = true, ElementType element = ElementType.PHYSICAL, bool applyProps = true)
	{
		Vector2 cOff = (m_physics == null) ? offset : m_physics.OrientVectorToDirection(offset);
		Vector3 newPos = transform.position + (Vector3)cOff;
		var go = GameObject.Instantiate(HitboxList.Instance.Hitbox, newPos, Quaternion.identity);

		Hitbox newBox = go.GetComponent<Hitbox>();
		if (followObj) {
			go.transform.SetParent (gameObject.transform);
			newBox.transform.localScale = m_physics.OrientVectorToDirection(new Vector2 (hitboxScale.x / transform.localScale.x, hitboxScale.y / transform.localScale.y), false);
		} else {
			newBox.SetScale ((m_physics == null) ? hitboxScale : m_physics.OrientVectorToDirection(hitboxScale,false));
		}
		newBox.Damage = damage;
		newBox.Duration = hitboxDuration;
		newBox.Knockback = (m_physics == null) ? knockback : m_physics.OrientVectorToDirection(knockback);
		newBox.IsFixedKnockback = fixedKnockback;
		newBox.Stun = stun;
		newBox.AddElement(element);
		newBox.Creator = gameObject;
		newBox.Faction = Faction;
		if (followObj)
			newBox.SetFollow (gameObject,offset);
		if (applyProps)
			ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnHitboxCreate(newBox));
		newBox.Init();
		return newBox;
	}

	public HitboxDoT CreateHitboxDoT(Vector2 hitboxScale, Vector2 offset, float damage, float stun, float hitboxDuration, Vector2 knockback, bool fixedKnockback = true,
		bool followObj = true, ElementType element = ElementType.PHYSICAL)
	{
		Vector2 cOff = (m_physics == null) ? offset : m_physics.OrientVectorToDirection(offset);
		Vector3 newPos = transform.position + (Vector3)cOff;
		var go = GameObject.Instantiate(HitboxList.Instance.HitboxDoT, newPos, Quaternion.identity);

		HitboxDoT newBox = go.GetComponent<HitboxDoT>();
		if (followObj) {
			go.transform.SetParent (gameObject.transform);
			Vector2 ls = new Vector2 (hitboxScale.x / transform.localScale.x, hitboxScale.y / transform.localScale.y);
			newBox.transform.localScale = (m_physics == null) ?  ls : m_physics.OrientVectorToDirection(ls , false);
		} else {
			newBox.SetScale ((m_physics == null) ? hitboxScale : m_physics.OrientVectorToDirection(hitboxScale,false));
		}
		newBox.Damage = damage;
		newBox.Duration = hitboxDuration;
		newBox.Knockback = (m_physics == null) ? knockback : m_physics.OrientVectorToDirection(knockback);
		newBox.IsFixedKnockback = fixedKnockback;
		newBox.Stun = stun;
		newBox.AddElement(element);
		newBox.Creator = gameObject;
		newBox.Faction = Faction;

		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnHitboxCreate(newBox));
		newBox.Init();
		return newBox;
	}

	public HitboxMulti CreateHitboxMulti(Vector2 hitboxScale, Vector2 offset, float damage, float stun, float hitboxDuration, Vector2 knockback, bool fixedKnockback = true,
		bool followObj = true, ElementType element = ElementType.PHYSICAL, float refreshTime = 0.2f)
	{
		Vector2 cOff = (m_physics == null) ? offset : m_physics.OrientVectorToDirection(offset);
		Vector3 newPos = transform.position + (Vector3)cOff;
		var go = GameObject.Instantiate(HitboxList.Instance.HitboxMulti, newPos, Quaternion.identity);
		HitboxMulti newBox = go.GetComponent<HitboxMulti>();

		if (followObj) {
			go.transform.SetParent (gameObject.transform);
			Vector2 ls = new Vector2 (hitboxScale.x / transform.localScale.x, hitboxScale.y / transform.localScale.y);
			newBox.transform.localScale = (m_physics == null) ?  ls : m_physics.OrientVectorToDirection(ls , false);
		} else {
			newBox.SetScale ((m_physics == null) ? hitboxScale : m_physics.OrientVectorToDirection(hitboxScale,false));
		}
		newBox.Damage = damage;
		newBox.Duration = hitboxDuration;
		newBox.Knockback = (m_physics == null) ? knockback : m_physics.OrientVectorToDirection(knockback);
		newBox.IsFixedKnockback = fixedKnockback;
		newBox.Stun = stun;
		newBox.AddElement(element);
		newBox.Creator = gameObject;
		newBox.Faction = Faction;
		newBox.refreshTime = refreshTime;

		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnHitboxCreate(newBox));
		newBox.Init();
		return newBox;
	}
	public GameObject CreateItem( GameObject prefab, Vector2 creationPoint, Vector2 throwPoint,
		float throwSpeed) {
		Vector2 cOff = (m_physics == null) ? creationPoint :m_physics.OrientVectorToDirection(creationPoint);
		Vector3 newPos = transform.position + (Vector3)cOff;
		GameObject go = Instantiate (prefab, newPos, Quaternion.identity);
		if (go.GetComponent<PhysicsSS> () != null) {
			Vector2 throwVec = throwSpeed * throwPoint.normalized;
			go.GetComponent<PhysicsSS> ().AddToVelocity (m_physics.OrientVectorToDirection (throwVec));
		}
		return go;
	}

	public Projectile CreateProjectile(GameObject prefab, Vector2 creationPoint, Vector2 targetPoint,
		float projectileSpeed, float damage, float stun, float projectileDuration, Vector2 knockback, 
		bool fixedKnockback = true, ElementType element = ElementType.PHYSICAL)
	{
		Vector2 cOff = (m_physics == null) ? creationPoint : m_physics.OrientVectorToDirection(creationPoint);
		Vector3 newPos = transform.position + (Vector3)cOff;
		GameObject go;
		if (prefab != null) {
			go = GameObject.Instantiate (prefab, newPos, Quaternion.identity);
		} else {
			go = GameObject.Instantiate (HitboxList.Instance.StandardProjectile, newPos, Quaternion.identity);
		}
		Projectile newProjectile = go.GetComponent<Projectile>();

		newProjectile.Damage = damage;
		newProjectile.Duration = projectileDuration;
		newProjectile.Knockback = (m_physics == null) ? knockback : m_physics.OrientVectorToDirection(knockback);
		newProjectile.IsFixedKnockback = fixedKnockback;
		newProjectile.Stun = stun;
		newProjectile.AddElement(element);
		newProjectile.Creator = gameObject;
		newProjectile.Faction = Faction;
		newProjectile.AimPoint = (m_physics == null) ? targetPoint : m_physics.OrientVectorToDirection(targetPoint);
		newProjectile.ProjectileSpeed = projectileSpeed;

		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnHitboxCreate(newProjectile));
		newProjectile.Init();
		return newProjectile;
	}

	public void ClearHitboxes()
	{
		foreach (Hitbox hb in GetComponentsInChildren<Hitbox>())
			Destroy(hb.gameObject);
	}

	public void RegisterHit(GameObject otherObj, Hitbox hb, HitResult hr)
	{
		if (m_fighter)
			m_fighter.RegisterHit (otherObj,hb,hr);
	}

}
