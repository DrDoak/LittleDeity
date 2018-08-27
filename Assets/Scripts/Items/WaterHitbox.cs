using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WaterHitbox : MonoBehaviour {

	public float SurfaceLevel;
	public float Thickness = 0.1f;
	public float Buoyancy = 10f;
	public List<PhysicsSS> m_overlappingControl = new List<PhysicsSS> (); 

	// Use this for initialization
	void Start () {
		SurfaceLevel = transform.position.y + transform.localScale.y / 2f;
		m_overlappingControl = new List<PhysicsSS> ();
	}
	
	// Update is called once per frame
	void Update () {
		foreach(PhysicsSS p in m_overlappingControl) {
			if (!p.ReactToWater)
				p.AddToVelocity (new Vector2 (0f, 20f * Time.deltaTime));
		}
	}

	internal void OnTriggerEnter2D(Collider2D other)
	{
		if (other.GetComponent<PhysicsSS> ()) {
			PhysicsSS phys = other.GetComponent<PhysicsSS> ();

			if (other.GetComponent<PropertyHolder> ()) {
				other.GetComponent<PropertyHolder> ().SubmergedHitbox = this;
			}
			if (other.GetComponent<BasicMovement> ()) {
				other.GetComponent<BasicMovement> ().SetMoveSpeed( other.GetComponent<BasicMovement>().MoveSpeed / 2f);
				other.GetComponent<BasicMovement> ().Submerged = true;
			}
			float velY = Mathf.Abs (other.GetComponent<PhysicsSS> ().TrueVelocity.y);
			phys.AddToVelocity (new Vector2 (0f, 0.5f * (velY/Time.deltaTime)));
			if (!m_overlappingControl.Contains (phys))
				m_overlappingControl.Add (phys);
			CreateFX (other);
			ExecuteEvents.Execute<ICustomMessageTarget> (other.gameObject, null, (x, y) => x.OnWaterEnter (this));
		}
	}

	internal void OnTriggerExit2D(Collider2D other)
	{
		if (other.GetComponent<PhysicsSS> () && m_overlappingControl.Contains(other.gameObject.GetComponent<PhysicsSS>())) {
			other.GetComponent<PhysicsSS> ();
			//phys.GravityForce *= 1.4f;
			if (other.GetComponent<PropertyHolder> ()) {
				
				other.GetComponent<PropertyHolder> ().SubmergedHitbox = null;
			}
			if (other.GetComponent<BasicMovement> ()) {
				other.GetComponent<BasicMovement> ().SetMoveSpeed( other.GetComponent<BasicMovement>().MoveSpeed * 2f);
				other.GetComponent<BasicMovement> ().Submerged = false;
			}
			m_overlappingControl.Remove (other.gameObject.GetComponent<PhysicsSS> ());
			float velY = Mathf.Abs (other.GetComponent<PhysicsSS> ().TrueVelocity.y);
			if (velY > 0.2f)
				other.GetComponent<PhysicsSS>().AddToVelocity (new Vector2 (0f, -0.5f * velY/Time.deltaTime));
			ExecuteEvents.Execute<ICustomMessageTarget> (other.gameObject, null, (x, y) => x.OnWaterExit (this));
			CreateFX (other);
		}
	}

	void CreateFX(Collider2D other) {
		Vector3 pos = new Vector3 (other.transform.position.x, SurfaceLevel, transform.position.z);
		GameObject waterFX = Instantiate (GameManager.Instance.FXWaterSplash, pos, Quaternion.identity);
		ParticleSystem.ShapeModule s = waterFX.transform.GetChild (0).GetComponent<ParticleSystem>().shape;
		ParticleSystem.MainModule m = waterFX.transform.GetChild (0).GetComponent<ParticleSystem>().main;
		float velY = Mathf.Abs (other.GetComponent<PhysicsSS> ().TrueVelocity.y);

		m.maxParticles = (int)Mathf.Min(10f,(20f * (velY / 0.14f)));
		BoxCollider2D bc = (BoxCollider2D)other;
		Vector3 scale = s.scale;
		scale.x = bc.size.x;
		s.scale = scale;
	}

	public Vector3 BodyScale() {
		Vector3 sc = transform.localScale;
		Vector2 v2 =  GetComponent<BoxCollider2D> ().size;
		return new Vector3(sc.x * v2.x,sc.y *v2.y,1f);
	}
}
