using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRoomController : MonoBehaviour {

	private List<AIFighter> m_aiFighters;
	private Fighter m_player;

	void Awake() {
		CollectAndInitializeFighters();
	}

	void CollectAndInitializeFighters() {
		/*
		m_aiFighters = new List<AIFighter>();
		foreach (BasicMovement fighterObj in Object.FindObjectsOfType<BasicMovement>()) {
			// Add Fighters that are not the current player
			if (fighterObj.IsCurrentPlayer) {
				m_player = fighterObj.GetComponent<Fighter>();
				continue;
			}

			// Collect fighter components of relevance
			AIFighter fighterInfo = new AIFighter();
			fighterInfo.BasicMove = fighterObj;
			fighterInfo.Fighter = fighterObj.GetComponent<Fighter>();
			fighterInfo.Attackable = fighterObj.GetComponent<Attackable>();
			fighterInfo.Routine = fighterObj.GetComponentInChildren<FighterRoutine>();
			// Catalog fighter
			m_aiFighters.Add(fighterInfo);
		}

		foreach (AIFighter fighter in m_aiFighters) {
			fighter.Routine.Init(m_player, fighter);
		}
		*/
	}
		
}
