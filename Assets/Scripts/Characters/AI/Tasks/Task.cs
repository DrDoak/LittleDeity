using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TaskType {NEUTRAL,AGGRESSIVE,ATTACK};

public class Task : MonoBehaviour {

	public AIBase MasterAI;
	public TaskType MyTaskType;
	public bool IsInitialTask = false;

	bool m_active = false;

	public List<Transition> TransitionsTo;
	public List<Transition> TransitionsFrom;

	public GameObject Target;
	public AIBase ParentAI;

	// Use this for initialization
	void Start () {
		
	}

	public void Init() {
		TransitionsTo = new List<Transition> ();
		TransitionsFrom = new List<Transition> ();

		foreach (Transition t in GetComponents<Transition>()) {
			if (t.TypeOfTransition == TransitionType.FROM_THIS_TASK)
				TransitionsFrom.Add (t);
			else
				TransitionsTo.Add (t);
		}
	}
	
	// Update is called once per frame
	public void OnUpdate () {
		
	}

	public void OnSight(Observable o) { 
		foreach (Transition t in TransitionsFrom) {
			t.OnSight (o);
		}
	}

	public void OnHit(HitInfo hb) { 
		foreach (Transition t in TransitionsFrom) {
			t.OnHit (hb);
		}
	}

	public void RequestTransition(Task t) {
	}

	public void RequestTransition(TaskType tt) {
	}

	public void SetActive(bool act) {
		m_active = act;
	}
}
