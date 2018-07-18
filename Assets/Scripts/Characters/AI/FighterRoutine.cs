using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterRoutine : MonoBehaviour {
	[SerializeField]
	private List<FighterTask> m_tasks;
	public List<FighterTask> Tasks { get { return m_tasks; } set { m_tasks = value; } }

	protected float TaskTimer = 0;
	protected int ActiveTask = 0;

	public void Init(Fighter player, AIFighter fighter) {
		m_tasks = new List<FighterTask> (GetComponents<FighterTask> ());
		//Debug.Log ("Initializing Routine for fighter: " + fighter);
		foreach (FighterTask task in m_tasks) {
			//Debug.Log ("init: " + task);
			task.Init(player, fighter,this);
		}
		//Debug.Log (m_tasks.Count);
	}

	virtual public void Advance() {
		AdvanceCurrentTask();
		TaskTimer += Time.deltaTime;
	}

	void AdvanceCurrentTask() {
		if (ActiveTask == -1 || Tasks.Count == 0)
			return;
		m_tasks[ActiveTask].Advance();
	}

	public void MoveToNextTask() {
		TaskTimer = 0;
		ActiveTask = (ActiveTask + 1) % Tasks.Count;
		//Debug.Log ("Moving to next task: " + ActiveTask);
		Tasks[ActiveTask].Activate();
	}

	public void OnSight(Observable o) {
		Tasks[ActiveTask].OnSight(o);
	}
	public void OnHit(Hitbox hb) {
		Tasks[ActiveTask].OnHit(hb);
	}
}
