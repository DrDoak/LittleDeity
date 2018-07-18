using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{

	public Interactable PromptedInteraction;
	private Collider2D m_col;
	private Text m_promptUI;
	public string InteractionKey;

	void Start()
	{
		m_col = gameObject.GetComponent<Collider2D>();
		m_promptUI = GameObject.Find("Interaction_prompt").GetComponentInChildren<Text>();
	}

	void Update()
	{
		if (PromptedInteraction)
		{
			if (m_col.IsTouching(PromptedInteraction.gameObject.GetComponent<Collider2D>()))
			{
				m_promptUI.text = "Press '" + InteractionKey + "' " + PromptedInteraction.InteractionString;
				if (Input.GetKey(InteractionKey))
				{
					PromptedInteraction.HoldTrigger = true;
					m_promptUI.text = "TRIGGERED!";
				}
				else
					PromptedInteraction.HoldTrigger = false;

				if (Input.GetKeyUp(InteractionKey))
				{
					PromptedInteraction.PressTrigger = !PromptedInteraction.PressTrigger;
				}
			}
			else
			{
				PromptedInteraction.HoldTrigger = false;
				PromptedInteraction = null;
				m_promptUI.text = "";
			}
		}
	}
}
