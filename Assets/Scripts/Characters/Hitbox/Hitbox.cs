using UnityEngine;
using System.Collections.Generic;

public enum HitResult { NONE, HIT,HEAL, BLOCKED, REFLECTED };

public enum ElementType { PHYSICAL, FIRE, BIOLOGICAL, PSYCHIC, LIGHTNING };

public class Hitbox : MonoBehaviour {

	[SerializeField]
	private float m_damage = 10.0f;
	public float Damage { get { return m_damage; } set { m_damage = value; } }

	[SerializeField]
	private float m_duration = 1.0f;
	public float Duration { get { return m_duration; } set { m_duration = value; } }

	[SerializeField]
	protected bool m_hasDuration = true;

	[SerializeField]
	private bool m_isFixedKnockback = false;
	public bool IsFixedKnockback { get { return m_isFixedKnockback; } set { m_isFixedKnockback = value; } }

	[SerializeField]
	private Vector2 m_knockback = new Vector2(0.0f,40.0f);
	public Vector2 Knockback { get { return m_knockback; } set { m_knockback = value; } }

	[SerializeField]
	private float m_stun = 0.0f;
	public float Stun { get { return m_stun; } set { m_stun = value; } }

	[SerializeField]
	private bool m_isRandomKnockback = false;
	public bool IsRandomKnockback { get { return m_isRandomKnockback; } set { m_isRandomKnockback = value; } }


	//private ElementType m_element = ElementType.PHYSICAL;
	[SerializeField]
	private List<ElementType> m_elementList = new List<ElementType> ();
	public List<ElementType> Element { get { return m_elementList; } set { m_elementList = value; } }

	public FactionType Faction = FactionType.HOSTILE;

	[HideInInspector]
	public GameObject Creator { get; set; }

	[SerializeField]
	private GameObject m_followObj;

	[SerializeField]
	private Vector2 m_followOffset;

	[SerializeField]
	private List<Collider2D> m_upRightDownLeftColliders;

	private PhysicsSS m_creatorPhysics;
	private Vector4 m_knockbackRanges;
	public List<Attackable> m_collidedObjs = new List<Attackable> ();
	public List<Attackable> m_overlappingControl = new List<Attackable> (); 

	virtual public void Init()
	{
		m_creatorPhysics = Creator.GetComponent<PhysicsSS>();
		if (m_isRandomKnockback)
			RandomizeKnockback ();
		m_hasDuration = m_duration > 0;
		Tick();
		//Debug.Log ("Hitbox initialized");
	}

	virtual internal void Update()
	{
		Tick();
	}

	protected virtual void Tick()
	{
		//Debug.Log ("Hitbox created");
		if (m_followObj != null)
			FollowObj();
		if (m_creatorPhysics != null) {
			SwitchActiveCollider (m_creatorPhysics.FacingLeft);
		}
		if (m_hasDuration)
			MaintainOrDestroyHitbox();
	}

	public void SetScale(Vector2 scale)
	{
		transform.localScale = scale;
	}

	public void SetFollow(GameObject obj, Vector2 offset)
	{
		m_followObj = obj;
		m_followOffset = offset;
	}

	public void SetKnockbackRanges (float minX, float maxX,float minY, float maxY)
	{
		IsRandomKnockback = true;
		IsFixedKnockback = true;
		m_knockbackRanges = new Vector4 (minX, maxX, minY, maxY);
	}

	private void MaintainOrDestroyHitbox()
	{
		if (m_duration <= 0.0f) {
			//Debug.Log ("Hitbox destroyed!" + m_duration);
			GameObject.Destroy (gameObject);
		}
		Duration -= Time.deltaTime;
	}

	private void FollowObj()
	{
		transform.position = new Vector3(m_followObj.transform.position.x + m_followOffset.x, m_followObj.transform.position.y + m_followOffset.y,0);
	}

	private void RandomizeKnockback ()
	{
		m_knockback.x = Random.Range (m_knockbackRanges.x, m_knockbackRanges.y);
		m_knockback.y = Random.Range (m_knockbackRanges.z, m_knockbackRanges.w);
	}

	protected virtual HitResult OnAttackable(Attackable atkObj)
	{
		if (!canAttack(atkObj))
			return HitResult.NONE;
		if (IsRandomKnockback)
			RandomizeKnockback();
		HitResult r = atkObj.TakeHit(this);
		m_collidedObjs.Add (atkObj);

		if (!m_overlappingControl.Contains (atkObj))
			m_overlappingControl.Add (atkObj);
		if (Creator != null) {
			Creator.GetComponent<HitboxMaker> ().RegisterHit (atkObj.gameObject, this, r);
		}
		CreateHitFX ( atkObj.gameObject, Knockback, r);
		return r;
	}

	protected bool canAttack(Attackable atkObj) {
		return !(!atkObj || (atkObj.gameObject == Creator) || m_collidedObjs.Contains (atkObj) || !atkObj.CanAttack (Faction));
	}
	internal HitResult OnTriggerEnter2D(Collider2D other)
	{
		OnHitObject (other);
		return OnAttackable (other.gameObject.GetComponent<Attackable> ());
	}
	protected virtual void OnHitObject(Collider2D other) {
	}
	internal void OnTriggerExit2D(Collider2D other)
	{
		/*
		 * TODO: Delay removal of collided object to avoid stuttered collisions 
		 */
		/*
		if (other.gameObject.GetComponent<Attackable> () && collidedObjs.Contains(other.gameObject.GetComponent<Attackable>())) {
			collidedObjs.Remove (other.gameObject.GetComponent<Attackable> ());
		}
		*/
		if (other.gameObject.GetComponent<Attackable> () 
			&& m_overlappingControl.Contains(other.gameObject.GetComponent<Attackable>())) {
			m_overlappingControl.Remove (other.gameObject.GetComponent<Attackable> ());
		}
	}

	private void SwitchActiveCollider(bool FacingLeft)
	{
		if (m_upRightDownLeftColliders.Count == 0)
			return;
		var dirIndex = ConvertDirToUpRightDownLeftIndex(FacingLeft);
		// Or'd check on enabled in case collider falls under several categories
		for (var i = 0; i < m_upRightDownLeftColliders.Count; i++)
		{
			m_upRightDownLeftColliders[i].enabled |= (i == dirIndex);
		}
	}

	private int ConvertDirToUpRightDownLeftIndex(bool FacingLeft)
	{
		if (FacingLeft)
			return 3;
		return 1;
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 0, 0, .8f);
		Gizmos.DrawCube (transform.position, transform.lossyScale);
	}

	public void AddElement(ElementType element) {
		m_elementList.Add (element);
	}
	public bool HasElement(ElementType element) {
		foreach (ElementType et in m_elementList) {
			if (et == element)
				return true;
		}
		return false;
	}
	public void ClearElement() {
		m_elementList.Clear ();
	}
	protected void CreateHitFX(GameObject hitObj, Vector2 knockback, HitResult hr) {
		foreach (ElementType et in m_elementList) {
			m_hitFX (et, hitObj, knockback, hr);
			m_playerSound (et, hr);
		}
	}
	private void m_hitFX(ElementType et, GameObject hitObj, Vector2 knockback, HitResult hr) {
		GameObject fx = null;
		if (hr == HitResult.BLOCKED) {
			fx = GameObject.Instantiate (FXHit.Instance.FXHitBlock, hitObj.transform.position, Quaternion.identity);
		} else if (hr == HitResult.HEAL) {
			fx = GameObject.Instantiate (FXHit.Instance.FXHeal, hitObj.transform.position, Quaternion.identity);
		} else if (hr == HitResult.HIT) {
			switch (et) {
			case ElementType.PHYSICAL:
				fx = GameObject.Instantiate (FXHit.Instance.FXHitPhysical, hitObj.transform.position, Quaternion.identity);
				break;
			case ElementType.FIRE:
				fx = GameObject.Instantiate (FXHit.Instance.FXHitFire, hitObj.transform.position, Quaternion.identity);
				break;
			case ElementType.LIGHTNING:
				fx = GameObject.Instantiate (FXHit.Instance.FXHitLightning, hitObj.transform.position, Quaternion.identity);
				break;
			case ElementType.BIOLOGICAL:
				fx = GameObject.Instantiate (FXHit.Instance.FXHitBiological, hitObj.transform.position, Quaternion.identity);
				break;
			case ElementType.PSYCHIC:
				fx = GameObject.Instantiate (FXHit.Instance.FXHitPsychic, hitObj.transform.position, Quaternion.identity);
				break;
			default:
				Debug.Log ("Hit Effect not yet added");
				break;
			}
		}
		if (fx != null) {
			fx.GetComponent<Follow> ().followObj = hitObj;
			float angle = (Mathf.Atan2 (knockback.y, knockback.x) * 180) / Mathf.PI;
			fx.transform.Rotate (new Vector3 (0f, 0f, angle));
		}
	}
	protected void m_playerSound(ElementType et,  HitResult hr) {
		
		if (hr == HitResult.BLOCKED) {
			FindObjectOfType<AudioManager> ().PlayClipAtPos (FXHit.Instance.SFXGuard,transform.position,0.5f,0f,0.25f);
		} else if (hr == HitResult.HEAL) {
			FindObjectOfType<AudioManager> ().PlayClipAtPos (FXHit.Instance.SFXHeal,transform.position,0.5f,0f,0.25f);
		} else if (hr == HitResult.HIT) {
			switch (et) {
			case ElementType.PHYSICAL:
				FindObjectOfType<AudioManager> ().PlayClipAtPos (FXHit.Instance.SFXPhysical,transform.position,0.5f,0f,0.25f);
				break;
			case ElementType.FIRE:
				FindObjectOfType<AudioManager> ().PlayClipAtPos (FXHit.Instance.SFXFire,transform.position,0.5f,0f,0.25f);
				break;
			case ElementType.LIGHTNING:
				FindObjectOfType<AudioManager> ().PlayClipAtPos (FXHit.Instance.SFXElectric,transform.position,0.75f,0f,0.25f);
				break;
			case ElementType.BIOLOGICAL:
				//FindObjectOfType<AudioManager> ().PlayClipAtPos (FXHit.Instance.SFXPhysical,transform.position,0.5f,0f,0.25f);
				break;
			case ElementType.PSYCHIC:
				FindObjectOfType<AudioManager> ().PlayClipAtPos (FXHit.Instance.SFXPsychic,transform.position,0.5f,0f,0.25f);
				break;
			default:
				Debug.Log ("Hit Effect not yet added");
				break;
			}
		}
	}
}