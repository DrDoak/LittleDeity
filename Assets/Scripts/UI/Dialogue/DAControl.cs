using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAControl : DialogueAction {

	public override bool IsExecutionString(string actionString) {
		return MatchStart (actionString, "!");
	}

	public override void PerformAction(string actionString, Textbox originTextbox) {
		List<string> listChars = ExtractArgs (actionString, "!");
		foreach (string targetChar in listChars) { 
			GameObject target = GameObject.Find (targetChar);
			if (target != null && target.GetComponent<BasicMovement> () != null) {
				originTextbox.FreezeCharacter (target.GetComponent<BasicMovement> (), 
					target.GetComponent<BasicMovement>().Autonomy);
			}	
		}
	}
}