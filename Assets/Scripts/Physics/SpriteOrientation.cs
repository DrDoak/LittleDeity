using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PhysicsSS))]
[RequireComponent (typeof (SpriteRenderer))]
[ExecuteInEditMode]
public class SpriteOrientation : MonoBehaviour {

	// Tracking m_sprite orientation (flipping if left)...
	private SpriteRenderer m_sprite;
	private PhysicsSS m_physics;
	private bool m_facingLeft = false;
	// Use this for initialization
	internal void Awake () {
		m_sprite = GetComponent<SpriteRenderer>();
		m_physics = GetComponent<PhysicsSS> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_facingLeft != m_physics.FacingLeft) {
			SetDirection (m_physics.FacingLeft);
		}
	}

	public void SetDirection(bool left) {
		m_facingLeft = left;
		if (m_sprite != null && m_sprite.sprite) {
			if (m_facingLeft) {
				m_sprite.flipX = true;
			} else {
				m_sprite.flipX = false;
			}
		}
	}
}
