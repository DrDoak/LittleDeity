using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : MonoBehaviour {

	Task m_currentTask;

	Dictionary<TaskType, List<Task>> MyTasks;

	Dictionary<TaskType, List<Transition>> GenericTransitions;

	// Use this for initialization
	void Awake () {
		GenericTransitions = new Dictionary<TaskType, List<Transition>> ();
		ReloadTasks ();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_currentTask != null)
			m_currentTask.OnUpdate ();
	}

	void ReloadTasks() {
		Task[] tList = GetComponentsInChildren<Task> ();
		MyTasks = new Dictionary<TaskType, List<Task>>();
		foreach (Task t in tList) {
			t.MasterAI = this;
			if (m_currentTask == null || t.IsInitialTask)
				TransitionToTask (t);
			AddTask (t);
		}
	}

	public void OnHit(Hitbox hb) { 
		m_currentTask.OnHit (hb);
		foreach (Transition t in GenericTransitions[m_currentTask.MyTaskType]) {
			t.OnHit (hb);
		}
	}

	public void OnSight(Observable o) { 
		if (m_currentTask != null) {
			m_currentTask.OnSight (o);
			foreach (Transition t in GenericTransitions[m_currentTask.MyTaskType]) {
				t.OnSight (o);
			}
		}
	}

	public void TransitionToTask(Task t) {
		Debug.Log ("Transitioning to task: " + t.name);
		if (m_currentTask != null)
			m_currentTask.SetActive (false);
		m_currentTask = t;
		m_currentTask.SetActive (true);
		if (!GenericTransitions.ContainsKey (m_currentTask.MyTaskType))
			GenericTransitions [m_currentTask.MyTaskType] = new List<Transition> ();
	}

	public void TransitionToTask(TaskType tt) {
		if (MyTasks.ContainsKey(tt))
			TransitionToTask (MyTasks [tt] [0]);
	}

	public void AddTask(Task t) {
		t.Init ();
		if (!MyTasks.ContainsKey(t.MyTaskType))
			MyTasks[t.MyTaskType] = new List<Task>();
		
		if (!MyTasks [t.MyTaskType].Contains (t)) {
			MyTasks [t.MyTaskType].Add (t);
			addTransitions (t.TransitionsTo);
			foreach(Transition tt in t.TransitionsFrom) {
				tt.MasterAI = this;
			}
		}
	}

	public void RemoveTask(Task t) {
		if (!MyTasks.ContainsKey(t.MyTaskType))
			MyTasks[t.MyTaskType] = new List<Task>();
		if (MyTasks [t.MyTaskType].Contains (t))
			MyTasks [t.MyTaskType].Remove (t);
		removeTransitions (t.TransitionsTo);
	}

	void addTransitions(List<Transition> lt) {
		Debug.Log ("Adding generics: " + lt.Count);
		foreach (Transition t in lt) {
			t.MasterAI = this;
			if (!GenericTransitions [t.OriginType].Contains (t))
				GenericTransitions [t.OriginType].Add (t);
		}
	}

	void removeTransitions (List<Transition> lt) {
		foreach (Transition t in lt) {
			if (GenericTransitions [t.OriginType].Contains (t))
				GenericTransitions [t.OriginType].Remove (t);
		}
	}

}
