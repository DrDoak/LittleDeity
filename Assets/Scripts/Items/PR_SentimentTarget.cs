using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_SentimentTarget : Property {

	public int MaxSentiment = 1;
	public int ActivateThreashold = 1;
	public int CurrentSentiment = 1;
	public bool DropSentiment = false;
	public bool CanTransmit = true;
	 
	public override void OnAttack(AttackInfo ai) {
		if (ai.m_SentimentInfo.RequiredMinSentiment < CurrentSentiment)
			GetComponent<Fighter> ().EndAttack ();
		if (ai.m_SentimentInfo.DrainOnWhiff)
			CurrentSentiment -= Mathf.Min (CurrentSentiment, ai.m_SentimentInfo.ConsumedSentiment);
	}
	public override void OnHit(HitInfo hi, GameObject attacker) {
		if (hi.HasElement(ElementType.PSYCHIC) && 
			attacker.GetComponent<PropertyHolder> () != null &&
			attacker.GetComponent<PropertyHolder> ().HasProperty (this)) {
			PR_SentimentTarget a = attacker.GetComponent<PR_SentimentTarget>();

		}
	}

	public void TransferSentiment(PR_SentimentTarget doner, PR_SentimentTarget recipiant, int amount) {
		int num = Mathf.Min(doner.CurrentSentiment,
			Mathf.Min (amount, recipiant.MaxSentiment - recipiant.CurrentSentiment));
		if (num > 0 ) {
			recipiant.CurrentSentiment += num;
			doner.CurrentSentiment -= num;
		}
	}
	public virtual void OnDeath() { 
		if (DropSentiment) {
			int sentDropped = 0;
			while (sentDropped < CurrentSentiment) {
				GameObject go = Instantiate (GameManager.Instance.FXExperience, transform.position, Quaternion.identity);
				go.GetComponent<ChaseTarget> ().StartingVel = new Vector2 (Random.Range (-10f, 10f), 10f);
				sentDropped += 1;
			}
		}
	}

	public override void OnSave (CharData d)
	{
		base.OnSave (d);
		d.PersistentInt ["MaxSentiment"] = MaxSentiment;
		d.PersistentInt ["ActivateThreashold"] = ActivateThreashold;
		d.PersistentInt ["CurrentSentiment"] = CurrentSentiment;
		d.PersistentBools ["DropSentiment"] = DropSentiment;
	}

	public override void OnLoad (CharData d)
	{
		MaxSentiment = d.PersistentInt ["MaxSentiment"];
		ActivateThreashold = d.PersistentInt ["ActivateThreashold"];
		CurrentSentiment = d.PersistentInt ["CurrentSentiment"];
		DropSentiment = d.PersistentBools ["DropSentiment"];
		base.OnLoad (d);
	}
}
