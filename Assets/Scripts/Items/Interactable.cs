using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	
	//public Interactor Actor;
	public string InteractionString;

	public bool autoTrigger = true;
	public float TriggerRefresh = 2.0f;
	float lastTimeTriggered = 0.0f;

	public bool oneTime = true;
	public bool TriggerUsed = false;


	//public bool HoldTrigger;
	//public bool PressTrigger;


	void Start()
	{
		lastTimeTriggered = -TriggerRefresh;
		//Actor = null;
		//Will become true if the interactor presses/holds the interaction key while in this interactable's area
		//HoldTrigger = false;
		//PressTrigger = false;
		if (GetComponent<PersistentItem> () != null)
			GetComponent<PersistentItem> ().InitializeSaveLoadFuncs (storeData,loadData);
	}

	void Update() {
		destroyAfterUse ();
	}
	protected void destroyAfterUse() {
		if (oneTime && TriggerUsed)
			Destroy (gameObject);
	}

	protected virtual void onTrigger(GameObject interactor) { }

	internal void OnTriggerEnter2D(Collider2D other)
	{
		if (autoTrigger && other.gameObject.GetComponent<BasicMovement> () && 
			Time.timeSinceLevelLoad - lastTimeTriggered >= TriggerRefresh) {
			lastTimeTriggered = Time.timeSinceLevelLoad;
			TriggerUsed = true;
			onTrigger (other.gameObject);
		}
		/*
		 * if (Actor = collision.gameObject.GetComponent<Interactor>())
			Actor.PromptedInteraction = this;*/
	}

	private void storeData(CharData d) {
		d.PersistentBools["TriggerUsed"] = TriggerUsed;
	}

	private void loadData(CharData d) {
		TriggerUsed = d.PersistentBools ["TriggerUsed"];
	}
}
