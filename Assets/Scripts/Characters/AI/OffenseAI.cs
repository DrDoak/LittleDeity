using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Fighter))]
[RequireComponent (typeof (BasicMovement))]
public class OffenseAI : MonoBehaviour {

	public List<AttackInfo> allAttacks;
	AttackInfo currentAttack;
	public BasicMovement CurrentTarget;
	private Vector3 m_targetOffset;
	private Vector3 TargetPoint;

	public float baseSpacing = 1.0f;
	//public float baseReactionSpeed = 1.0f;
	//public float baseDecisionMaking = 1.0f;
	public float baseAggression = 0.5f;

	float spacing;
	//float reactionSpeed;
	//float decisionMaking;
	float aggression;
	Fighter m_fighter;
	BasicMovement m_movement;

	public string currentAction = "wait";

	void Start () {
		spacing = baseSpacing;
		//reactionSpeed = baseReactionSpeed;
		//decisionMaking = baseDecisionMaking;
		aggression = baseAggression;
		allAttacks = new List<AttackInfo>();
		foreach (AttackInfo ai in GetComponents<AttackInfo> ()) {
			if (ai.name != "sheath" && ai.name != "unsheath") {
				allAttacks.Add (ai);
			}
		}
		m_fighter = GetComponent<Fighter> ();
		m_movement = GetComponent<BasicMovement> ();
	}

	void Update () {
		if (CurrentTarget != null && !m_movement.IsCurrentPlayer) {
			if (currentAction == "wait") {
				decideNextAction ();
			} else if (currentAction == "moveToTarget") {
				if (CurrentTarget.transform.position.x > transform.position.x) {
					TargetPoint = CurrentTarget.transform.position + m_targetOffset;
				} else {
					TargetPoint = CurrentTarget.transform.position + new Vector3(-m_targetOffset.x,m_targetOffset.y,0f);
				}
				m_movement.MoveToPoint (TargetPoint);
				if (Vector3.Distance (transform.position, TargetPoint) < m_movement.m_minDistance) {
					m_movement.FacePoint (CurrentTarget.transform.position);
				}
				decideNextAction ();
			} else if (currentAction == "attack") {
				if (!m_fighter.IsAttacking()) {
					decideNextAction ();
				}
			}
		}
	}

	public void decideNextAction() {
		Vector3 otherPos = CurrentTarget.transform.position;
		float dir = (GetComponent<PhysicsSS> ().FacingLeft) ? -1f : 1f;


		if (Random.value < (aggression * 0.1f)) {
			foreach (AttackInfo ainfo in allAttacks) {
				float xDiff = Mathf.Abs(transform.position.x  + (dir * ainfo.m_AIInfo.AIPredictionOffset.x) - otherPos.x);
				float yDiff = Mathf.Abs(transform.position.y + ainfo.m_AIInfo.AIPredictionOffset.y - otherPos.y);
				if ((ainfo.m_AIInfo.AIPredictionHitbox.x) +
					(ainfo.m_AIInfo.AIPredictionHitbox.x) * Random.Range (0f, 1f - spacing) > xDiff &&
					(ainfo.m_AIInfo.AIPredictionHitbox.y) +
					(ainfo.m_AIInfo.AIPredictionHitbox.y) * Random.Range (0f, 1f - spacing) > yDiff && Random.value > ainfo.m_AIInfo.Frequency) {
					m_fighter.TryAttack (ainfo.AttackName);
					currentAction = "attack";
					allAttacks.Reverse ();
					break;
				}
			}
		}
		currentAction = "moveToTarget";
	}

	public void commitToAction() {}

	public void setTarget(BasicMovement c) {
		setTarget (c, new Vector3 (), 1f);
	}

	public void setTarget(BasicMovement c, Vector3 offset, float tolerance = 1f) {
		m_movement.m_minDistance = tolerance;
		m_targetOffset = new Vector3 (offset.x, offset.y, 0f);
		CurrentTarget = c;
	}
}