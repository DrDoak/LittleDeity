using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class ParticleFXInfo {
	public GameObject ParticlePrefab;
	public Vector3 Offset;
	public bool OrientX = true;

	public void GenerateEffect(GameObject parent = null) {
		if (parent != null) {
		} else {
		}
	}
}
public class CollisionParticles : MonoBehaviour {

	public ParticleFXInfo JumpParticle;
	public ParticleFXInfo HitGroundParticle;
	public ParticleFXInfo HitCeilingParticle;

	private PhysicsSS m_physics;
	private BasicMovement m_movement;
	private Vector2 m_oldVelocity;
	private bool m_oldHitGround;
	private bool m_oldHitCeiling; 

	// Use this for initialization
	void Start () {
		m_physics = GetComponent< PhysicsSS > ();
		m_movement = GetComponent<BasicMovement> ();	

		m_oldVelocity = new Vector2 ();

	}
	
	// Update is called once per frame
	void Update () {
		
		if (m_physics.TimeCollided [Direction.DOWN] != 0f) {
			if (!m_oldHitGround) {
				Vector3 offset = (HitGroundParticle.OrientX) ? GetComponent<PhysicsSS> ().OrientVectorToDirection (HitGroundParticle.Offset) : HitGroundParticle.Offset;
				Instantiate (HitGroundParticle.ParticlePrefab, transform.position + offset, Quaternion.identity);
			}
			m_oldHitGround = true;
		} else {
			if (!m_oldHitGround) {
				Vector3 offset = (JumpParticle.OrientX) ? GetComponent<PhysicsSS> ().OrientVectorToDirection (JumpParticle.Offset) : JumpParticle.Offset;
				Instantiate (JumpParticle.ParticlePrefab, transform.position + offset, Quaternion.identity);
			}
			m_oldHitGround = false;
		}
		if (m_physics.TimeCollided [Direction.UP] != 0f) {
			if (!m_oldHitGround) {
				Vector3 offset = (HitCeilingParticle.OrientX) ? GetComponent<PhysicsSS> ().OrientVectorToDirection (HitCeilingParticle.Offset) : HitCeilingParticle.Offset;
				Instantiate (HitCeilingParticle.ParticlePrefab, transform.position + offset, Quaternion.identity);
			}
			m_oldHitCeiling = true;
		} else {
			m_oldHitCeiling = false;
		}
		m_oldVelocity = m_physics.TrueVelocity;
	}
}
