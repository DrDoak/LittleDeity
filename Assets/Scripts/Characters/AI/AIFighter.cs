using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Fighter))]
[RequireComponent (typeof (BasicMovement))]
public class AIFighter : MonoBehaviour {
	
	public Fighter Fighter { get; set; }
	public BasicMovement BasicMove { get; set; }
	public Attackable Attackable { get; set; }
	public FighterRoutine Routine { get; set; }

	private List<FighterRoutine> m_routines;
	public List<AttackInfo> allAttacks;

	public Attackable CurrentTarget;

	float spacing = 1.0f;

	void Start() {
		BasicMove = GetComponent<BasicMovement>();
		Fighter = GetComponent<Fighter>();
		Attackable = GetComponent<Attackable>();
		Routine = GetComponentInChildren<FighterRoutine>();
		m_routines = new List<FighterRoutine> (GetComponentsInChildren<FighterRoutine>());
		foreach (FighterRoutine fr in m_routines) {
			fr.Init (Fighter, this);
		}
		allAttacks = new List<AttackInfo>();

		foreach (AttackInfo ai in GetComponents<AttackInfo> ()) {
			if (ai.name != "sheath" && ai.name != "unsheath") {
				allAttacks.Add (ai);
			}
		}
	}

	void Update () {
		if (!BasicMove.IsCurrentPlayer) {
			foreach (FighterRoutine fr in m_routines) {
				fr.Advance ();
			}
		}
	}

	public void OnSight(Observable o) {
		foreach (FighterRoutine fr in m_routines) {
			fr.OnSight (o);
		}
	}

	public void OnHit(Hitbox hb) {
		foreach (FighterRoutine fr in m_routines) {
			fr.OnHit (hb);
		}
	}

	public void StartRoutine() {}

	public void EndRoutine() {}

	public List<string> AvailableAttacks(BasicMovement target) {
		Vector3 otherPos = target.transform.position;
		float dir = (GetComponent<PhysicsSS> ().FacingLeft) ? -1f : 1f;
		List<string> atks = new List<string> ();
		foreach (AttackInfo ainfo in allAttacks) {
			Vector3 offPos = otherPos + (target.GetComponent<PhysicsSS>().TrueVelocity/Time.deltaTime) * ainfo.m_AttackAnimInfo.StartUpTime * 0.5f;
			float xDiff = Mathf.Abs(transform.position.x  + (dir * ainfo.m_AIInfo.AIPredictionOffset.x) - offPos.x);
			float yDiff = Mathf.Abs(transform.position.y + ainfo.m_AIInfo.AIPredictionOffset.y - offPos.y);
			if ((ainfo.m_AIInfo.AIPredictionHitbox.x) +
				(ainfo.m_AIInfo.AIPredictionHitbox.x) * Random.Range (0f, 1f - spacing) > xDiff &&
				(ainfo.m_AIInfo.AIPredictionHitbox.y) +
				(ainfo.m_AIInfo.AIPredictionHitbox.y) * Random.Range (0f, 1f - spacing) > yDiff && Random.value > ainfo.m_AIInfo.Frequency) {
				atks.Add (ainfo.AttackName);
			}
		}
		return atks;
	}
}