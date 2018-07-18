using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_Boss_Key : MonoBehaviour {
	bool triggered = false;
	internal void OnTriggerEnter2D(Collider2D other)
	{
		if (!triggered && other.gameObject.GetComponent<BasicMovement> () &&
		    other.gameObject.GetComponent<BasicMovement> ().IsCurrentPlayer) {
			PropertyHolder[] pList = FindObjectsOfType<PropertyHolder> ();
			foreach (PropertyHolder h in pList) {
				if (h.HasProperty ("Electric_Door") && !h.HasProperty("Electrical")) {
					h.AddProperty ("PR_Electrical");
				}
			}
			FindObjectOfType<AudioManager> ().PlayClipAtPos (FXHit.Instance.SFXHeal,transform.position,0.5f,0f,0.25f);
			GetComponent<Attackable> ().DamageObj (10000f);
			triggered = true;
		}
	}
}
