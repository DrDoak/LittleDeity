using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {
	public float GravityInWater = 0.0f;
	public float GravityOutWater = -0.5f;
	public float MoveSpeedOutWater = 0.0f;
	public float MoveSpeedInWater = 3.0f;

	private bool JustOutWater = false;

	BasicMovement m_movt;
	PhysicsSS m_physics;
	// Use this for initialization
	void Start () {
		m_movt = GetComponent<BasicMovement> ();
		m_physics = GetComponent<PhysicsSS> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_movt.Submerged) {
			m_physics.SetGravityScale (GravityInWater);
			m_physics.Floating = true;
			m_movt.SetMoveSpeed( MoveSpeedInWater );
			JustOutWater = true;
		} else {
			m_physics.SetGravityScale (GravityOutWater);
			m_physics.Floating = false;
			m_movt.SetMoveSpeed( MoveSpeedOutWater );
			if (JustOutWater) {
				m_physics.AddSelfForce (new Vector2 (0, -5f), 0.25f);
				JustOutWater = false;
			}
		}
	}
}
