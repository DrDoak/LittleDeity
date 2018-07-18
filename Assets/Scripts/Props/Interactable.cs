using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	
	public Interactor Actor;
	public string InteractionString;
	public bool HoldTrigger;
	public bool PressTrigger;
	public bool oneTime = true;
	public bool TriggerUsed = false;
	[TextArea(3,8)]
	public string value;

	void Start()
	{
		Actor = null;
		//Will become true if the interactor presses/holds the interaction key while in this interactable's area
		HoldTrigger = false;
		PressTrigger = false;
	}

	void Update() {
		destroyAfterUse ();
	}
	protected void destroyAfterUse() {
		if (oneTime && TriggerUsed)
			Destroy (gameObject);
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (Actor = collision.gameObject.GetComponent<Interactor>())
			Actor.PromptedInteraction = this;
	}
}
