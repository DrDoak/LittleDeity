using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTarget : MonoBehaviour {

	public PhysicsSS Target;
	public Vector2 StartingVel = new Vector2();

	public bool Active = true;
	public bool Accelerate = true;
	public Vector2 MaxSpeed = new Vector2 (1.0f, 1.0f);
	public float Decceleration = 0.5f;
	float m_currentDelay = 0.0f;
	Vector3 m_offset = new Vector2 ();
	Vector3 m_currentTarget;
	Vector2 m_speed;
	float m_targetSpeed;

	[SerializeField] float ACCELERATION = 0.5f;
	[SerializeField] float CHASE_TOLERANCE = 0.2f;
	[SerializeField] float PURSUE_DISTANCE = 1000f;

	SpriteRenderer m_sprite;
	//TrailRenderer trail;

	float timeOut = 0f;
	//float targetTime = 0.6f;

	[SerializeField]
	bool m_isOrientToSpeed = false;

	[SerializeField]
	bool m_isDestroyOnProximity = false;
	[SerializeField]
	float m_destroyDistance = 0.5f;
	[SerializeField] private float m_spawnSpeed;

	[SerializeField]
	bool m_isDestroyWhenTargetGone = true;

	[SerializeField]
	float m_Delay = 0.5f;

	void Start () {
		m_sprite = GetComponent<SpriteRenderer> ();
		m_currentTarget = new Vector2 ();
		m_speed = new Vector2 ();

		//trail = GetComponent<TrailRenderer> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (!Active)
			return;
		if (m_currentDelay < m_Delay) {
			m_currentDelay += Time.deltaTime;
			transform.Translate (StartingVel * Time.deltaTime,Space.World);
			return;
		}
		if (Target != null) {
			m_currentTarget = Target.transform.position + m_offset;
			float d = Vector3.Distance (m_currentTarget, transform.position);
			if (d < PURSUE_DISTANCE) {
				chaseTarget ();
			}
			if (m_isDestroyOnProximity && d < m_destroyDistance) {
				Destroy (gameObject);
			}

		} else if (m_isDestroyWhenTargetGone) {
			Destroy (gameObject);
		}
	}

	void chaseTarget() {
		if (!Accelerate) {
			if (Mathf.Abs (m_currentTarget.x - transform.position.x) < MaxSpeed.x)
				m_speed.x = (m_currentTarget.x - transform.position.x);
			else
				m_speed.x = Mathf.Sign (m_currentTarget.x - transform.position.x) * MaxSpeed.x;

			if (Mathf.Abs (m_currentTarget.y - transform.position.y) < MaxSpeed.y)
				m_speed.y = m_currentTarget.y - transform.position.y;
			else
				m_speed.y = Mathf.Sign (m_currentTarget.y - transform.position.y) * MaxSpeed.y;
		} else if (Vector3.Distance (transform.position, m_currentTarget) > CHASE_TOLERANCE) {
			if (Accelerate) {
				float newX = m_speed.x + ACCELERATION * Time.deltaTime * Mathf.Pow (m_currentTarget.x - transform.position.x, 3f);
				if (Mathf.Abs (newX) < MaxSpeed.x)
					m_speed.x = newX;
				else
					m_speed.x = Mathf.Sign (m_currentTarget.x - transform.position.x) * MaxSpeed.x;
				float newY = m_speed.y + ACCELERATION * Time.deltaTime * Mathf.Pow (m_currentTarget.y - transform.position.y, 3f);
				if (Mathf.Abs (newY) < MaxSpeed.y)
					m_speed.y = newY;
				else
					m_speed.y = Mathf.Sign (m_currentTarget.y - transform.position.y) * MaxSpeed.y;
			}

		}
		timeOut += Time.deltaTime;
		m_speed -= ( Time.deltaTime * (1f - Decceleration) * m_speed);
		if (m_isOrientToSpeed) {
			orientToSpeed (m_speed);
		}
		transform.Translate (m_speed,Space.World);
	}

	public void SetTargetOffset(PhysicsSS t, Vector2 offset) {
		Target = t;
		m_offset = offset;
	}
	void orientToSpeed(Vector2 speed) {
		m_sprite.transform.rotation = Quaternion.Euler (new Vector3(0f,0f,Mathf.Rad2Deg * Mathf.Atan2 (speed.y, speed.x)));
	}
}