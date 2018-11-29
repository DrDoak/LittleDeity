using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PhysicsSS))]
[RequireComponent (typeof (BasicMovement))]
[RequireComponent (typeof (Fighter))]
public class Fighter_CollisionDamage : MonoBehaviour {

	private Vector2 m_oldVelocity;
	private PhysicsSS m_physics;
	private BasicMovement m_movement;
	// Use this for initialization
	void Start () {
		m_oldVelocity = new Vector2 ();
		m_physics = GetComponent< PhysicsSS > ();
		m_movement = GetComponent<BasicMovement> ();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < 4; i++) {
			if (m_physics.TimeCollided [(Direction)i] == 0f) {
				
			}				
		}
		m_oldVelocity = m_physics.TrueVelocity;
	}
}
