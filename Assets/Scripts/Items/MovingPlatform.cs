using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (PhysicsSS))]
public class MovingPlatform : MonoBehaviour {

	public List<Transform> MovingPoints;
	public float Speed;
	public bool IsLoop;
	public Vector3 nextPos;

	private int m_currentWayPoint = 0;
	private const float TOLERANCE = 0.2f;
	private PhysicsSS m_physics;

	private List<Transform> m_collisions;

	// Use this for initialization
	void Start () {
		m_physics = GetComponent<PhysicsSS> ();
		m_collisions = new List<Transform> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		nextPos = getNextPoint ();
		foreach (Transform t in m_collisions)
			if (!MovingTowards (t.position)) {
				t.transform.parent = null;
			}
				
		if (transform.position != nextPos) {
			Vector3 v = Vector3.MoveTowards (transform.position, nextPos, Speed * Time.fixedDeltaTime) - transform.position;
			m_physics.AddArtificialVelocity (new Vector2(v.x,v.y));
		}
	}

	private Vector3 getNextPoint() {
		if (MovingPoints.Count < 1)
			return transform.position;
		Transform t = MovingPoints [m_currentWayPoint];
		int numCycle = 0;
		while (Vector3.Distance (transform.position, t.position) < TOLERANCE) {
			if (m_currentWayPoint < MovingPoints.Count - 1)
				m_currentWayPoint++;
			else {
				if (IsLoop)
					m_currentWayPoint = 0;
				else
					return transform.position;
			}
			t = MovingPoints [m_currentWayPoint];
			numCycle++;
			if (numCycle > MovingPoints.Count)
				return transform.position;
		}
		return t.position;
	}


	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.GetComponent<PhysicsSS> () &&
		    col.gameObject.transform.parent == null &&
		    MovingTowards (col.transform.position)) {
			m_collisions.Add (col.transform);
			col.gameObject.transform.parent = transform;
		}
	}

	void OnTriggerExit2D(Collider2D col) {
		if (col.gameObject.GetComponent<PhysicsSS> () &&
		    col.gameObject.transform.parent == transform) {
			col.gameObject.transform.parent = null;
			if (m_collisions.Contains (col.transform)) 
				m_collisions.Remove (col.transform);
		}
	}

	bool MovingTowards(Vector3 pos) {
		Vector3 mPos = transform.position;
		Vector3 diff = pos - mPos;
		return (diff.y > 0.1f ||
		(Mathf.Sign (diff.x) == Mathf.Sign (m_physics.TrueVelocity.x)));
	}
}