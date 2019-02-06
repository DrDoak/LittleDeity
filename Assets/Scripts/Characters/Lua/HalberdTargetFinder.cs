using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

public class HalberdTargetFinder : MonoBehaviour {

	public float MaxRange = 5.0f;
	private const float m_scanInterval = 0.05f;

	private float m_nextScan;
	public GameObject CurrentTarget;

	public GameObject m_halberd;
	private GameObject m_target;
	private bool FacingLeft;
	private PhysicsSS m_physics;

	// Use this for initialization
	void Start () {
		m_nextScan = UnityEngine.Random.Range (Time.timeSinceLevelLoad,Time.timeSinceLevelLoad + m_scanInterval);
		FacingLeft = GetComponent<PhysicsSS> ().FacingLeft;
		m_halberd = Instantiate (ListLua.Instance.Halberd);
		m_halberd.GetComponent<WeaponFloating> ().m_target = gameObject;
		m_halberd.GetComponent<ChaseTarget> ().Target = gameObject;
		m_target = Instantiate (ListLua.Instance.HalberdTargeter);
		m_halberd.GetComponent<Projectile> ().SetHitboxActive (false);
		m_halberd.GetComponent<Projectile> ().Creator = gameObject;
		m_physics = GetComponent<PhysicsSS> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.timeSinceLevelLoad > m_nextScan) {
			updateTargets ();
		}
		if (CurrentTarget == null) {
			updateTargets ();
		}
		if (FacingLeft != m_physics.FacingLeft) {
			updateTargets ();
			FacingLeft = m_physics.FacingLeft;
		}
	}

	void updateTargets() {
		Attackable[] m_attackables = FindObjectsOfType<Attackable> ();
		float maxScoreFound = 0;
		GameObject bestTarget = null;
		foreach (Attackable a in m_attackables) {
			if (!GetComponent<Attackable> ().CanAttack (a.Faction) || !a.Alive || !a.CanTarget)
				continue;
			float dist = Vector3.Distance (transform.position, a.transform.position);
			if (dist > MaxRange)
				continue;
			float score = 0;
			if (GetComponent<Fighter> ().AttackHistory.ContainsKey (a)) {
				if (GetComponent<Fighter> ().AttackHistory [a].TimeSinceLastHit () < 2.0f) {
					score += 20f;
					score += (2.0f - GetComponent<Fighter> ().AttackHistory [a].TimeSinceLastHit ()) * 5f;
				}
			}
			if (FacingTowardsTarget(a.transform.position)) {
				score += 20f;
			}
			if (HoldingTowardsTarget(a.transform.position)) {
				score += 15f;
			}
			score += Mathf.Min (10f, (MaxRange - dist) * ((10f/MaxRange) + 1f));
			if (score > maxScoreFound) {
				bestTarget = a.gameObject;
				maxScoreFound = score;
			}
		}
		SetTarget (bestTarget);
	}

	private void SetTarget(GameObject go) {
		if (go != null) {
			CurrentTarget = go;
			m_target.GetComponent<ChaseTarget>().SetTargetOffset(go,new Vector2());
		} else {
			CurrentTarget = null;
			m_target.GetComponent<ChaseTarget>().SetTargetOffset(gameObject,new Vector2(0f,-100f));
		}
	}

	private bool HoldingTowardsTarget(Vector3 target) {
		return ((transform.position.x < target.x && InputManager.GetButton ("Right")) ||
			(transform.position.x > target.x && InputManager.GetButton ("Left")));
	}

	private bool FacingTowardsTarget(Vector3 target) {
		return ((transform.position.x < target.x && !GetComponent<PhysicsSS>().FacingLeft) ||
			(transform.position.x > target.x && GetComponent<PhysicsSS>().FacingLeft));
	}

	public bool HasTarget() {
		return (CurrentTarget != null);
	}
	public Vector3 GetTarget() {
		return m_target.transform.position;
	}
}
