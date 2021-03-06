﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionType {FROM_THIS_TASK, TO_THIS_TASK}

public class Transition : MonoBehaviour {

	public TransitionType TypeOfTransition;

	[HideInInspector]
	public AIBase MasterAI;

	public TaskType OriginType;
	public Task OriginTask;
	public TaskType TargetType;
	public Task TargetTask;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Init() {
	}

	public virtual void OnUpdate() {}

	public virtual void OnHit(HitInfo hb) {}

	public virtual void OnSight(Observable o) {}

	public void TriggerTransition() {
		if (TypeOfTransition == TransitionType.FROM_THIS_TASK) {
			if (TargetTask != null)
				MasterAI.TransitionToTask (TargetTask);
			else
				MasterAI.TransitionToTask (TargetType);
		} else {
			MasterAI.TransitionToTask (GetComponent<Task>());
		}
	}


}