using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClingPoint {
	public ClingPoint(RaycastHit2D rh) {
		clingPosition = rh.point;
		targetObj = rh.collider.gameObject;
	}
	public Vector3 clingPosition;
	public GameObject targetObj;
}

public class LuaPlatformJump : Property {

	public float MaxHeight = 4;
	public float MinHeight = 1;
	public float Width;
	public int VerticalRayCount = 4;

	private BasicMovement m_movement;
	public PhysicsSS m_physics;
	private RaycastOrigins m_raycastOrigins;
	private LayerMask m_collisionMask;
	private float m_verticalRaySpacing;
	private List<ClingPoint> m_clingPoints;

	// Use this for initialization
	void Start () {
		m_physics = GetComponent<PhysicsSS> ();
		m_collisionMask = m_physics.CollisionMask;

		m_movement = GetComponent<BasicMovement> ();
		m_verticalRaySpacing = Width / (VerticalRayCount - 1);
		m_clingPoints = new List<ClingPoint> ();
	}

	public override void OnJump() {
		if (m_movement.CanBasicJump())
			return;
		m_clingPoints.Clear ();
		FindValidClingPoints ();
		JumpToBestClingTarget ();
	}

	private void FindValidClingPoints() {

		float rayLength = Mathf.Abs (MaxHeight - MinHeight);
		Vector3 pos = transform.position;

		for (int i = 0; i < VerticalRayCount; i ++) {
			Vector2 rayOrigin = new Vector2 ( pos.x - Width/2f, pos.y + MaxHeight );
			rayOrigin += Vector2.right * (m_verticalRaySpacing * i);
			RaycastHit2D [] hitL = Physics2D.RaycastAll(rayOrigin, Vector2.up * -1f, rayLength, m_collisionMask);
			foreach (RaycastHit2D hit in hitL) {
				if (!hit.collider.isTrigger) {
					if (validClingTarget (hit)) {
						m_clingPoints.Add (new ClingPoint (hit));
					}
					break;
				}
			}
		}
	}

	private bool validClingTarget(RaycastHit2D hit) {
		return true;
	}

	private void JumpToBestClingTarget() {
		if (m_clingPoints.Count < 1)
			return;
		ClingPoint bestcp = null;
		float bestScore = 0f;
		foreach (ClingPoint cp in m_clingPoints) {
			float s = ScoreClingTarget (cp);
			if (s > bestScore) {
				bestcp = cp;
				bestScore = s;
			}
		}
		if (bestcp != null) {
			AtkCling ai = (AtkCling)GetComponent<Fighter> ().TryAttack ("cling");
			ai.m_clingInfo.FlyPoint = bestcp.clingPosition + new Vector3 (0f, 2.5f, 0f);
			ai.m_clingInfo.ClingPosition = bestcp.clingPosition;
		}
	}

	private float ScoreClingTarget(ClingPoint hit) {
		float s = (MaxHeight + (Width / 2f)) - Vector3.Distance (hit.clingPosition, transform.position);
		if ((hit.clingPosition.x > transform.position.x && !m_physics.FacingLeft) ||
		    (hit.clingPosition.x < transform.position.x && m_physics.FacingLeft))
			s *= 1.5f;
		return s;
	}
	private bool JumpThruTag( GameObject obj ) {
		return (obj.CompareTag ("JumpThru") || (obj.transform.parent != null &&
			obj.transform.parent.CompareTag ("JumpThru")));
	}
}