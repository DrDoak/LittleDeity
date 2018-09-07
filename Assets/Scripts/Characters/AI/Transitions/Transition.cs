using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionType {FROM_THIS_TASK, TO_THIS_TASK}

public class Transition : MonoBehaviour {

	public TransitionType TypeOfTransition;

	[HideInInspector]
	public AIBase MasterAI;

	public TaskType OriginType;
	public Task Origin;
	public TaskType TargetType;
	public Task Target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init() {
	}

	public virtual void OnHit(Hitbox hb) {}

	public virtual void OnSight(Observable o) {}

	public void TriggerTransition() {
		if (TypeOfTransition == TransitionType.FROM_THIS_TASK) {
			if (Target != null)
				MasterAI.TransitionToTask (Target);
			else
				MasterAI.TransitionToTask (TargetType);
		} else {
			MasterAI.TransitionToTask (GetComponent<Task>());
		}
	}


}