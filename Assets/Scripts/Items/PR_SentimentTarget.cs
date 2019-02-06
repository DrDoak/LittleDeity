using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_SentimentTarget : Property {

	public int MaxSentiment = 30;
	public int ActivateThreashold = 20;
	public int CurrentSentiment = 0;
	public bool DropSentiment = false;
	public bool CanTransmit = true;

	int m_lastRequiredSentiment;
	bool m_DrainOnWhiff;
	HealthDisplay m_display;

	public override void OnAddProperty() {
		if (GetComponent<BasicMovement>() != null && 
				GetComponent<BasicMovement> ().IsCurrentPlayer) {
			UIBarInfo ubi = new UIBarInfo ();
			ubi.FillColor = new Color(0.2f,0.2f,0f);
			ubi.UILabel = "Sentiment";
			ubi.funcUpdate = UpdateSentiment;
			ubi.target = gameObject;
			ubi.DisplayMode = UIBarDisplayMode.BASE;
			ubi.useScale = false;
			FindObjectOfType<GUIHandler>().AddUIBar (ubi);
			Debug.Log ("Adding a GUI bar");
		}
		m_display = Instantiate (UIList.Instance.HealthBarPrefab, this.transform).GetComponent<HealthDisplay>();
		m_display.SetMaxHealth (ActivateThreashold);
		m_display.DisplayAsFraction = true;
		m_display.ValueLabel = "SNT";
		Debug.Log ("Adding for: " + gameObject);
	}

	public override void OnRemoveProperty() { }

	public override void OnAttack(AttackInfo ai) {
		//Debug.Log ("Required minimum sentiment: " + ai.m_SentimentInfo.RequiredMinSentiment + " :current: " + CurrentSentiment);
		m_lastRequiredSentiment = ai.m_SentimentInfo.RequiredMinSentiment;
		m_DrainOnWhiff = ai.m_SentimentInfo.DrainOnWhiff;
		bool b = (CurrentSentiment < ai.m_SentimentInfo.RequiredMinSentiment);
		//Debug.Log (" cond: " + b);
		if (b) {
			GetComponent<Fighter> ().SkipAttackToEnd ();
		}
		if (ai.m_SentimentInfo.DrainOnWhiff)
			CurrentSentiment -= Mathf.Min (CurrentSentiment, ai.m_SentimentInfo.ConsumedSentiment);
	}
	public override void OnHitConfirm(HitInfo myHitbox, GameObject objectHit, HitResult hr) { 
		ChangeSentiment (-m_lastRequiredSentiment);
		bool b = (!m_DrainOnWhiff && 
			CurrentSentiment < m_lastRequiredSentiment);
		if (b) {
			GetComponent<Fighter> ().SkipAttackToEnd ();
		}
	}
	public override void OnHit(HitInfo hi, GameObject attacker) {
		if (hi.HasElement(ElementType.PSYCHIC) && 
			attacker.GetComponent<PropertyHolder> () != null &&
			attacker.GetComponent<PropertyHolder> ().HasProperty (this)) {
			PR_SentimentTarget a = attacker.GetComponent<PR_SentimentTarget>();
			if (a.CurrentSentiment > 0) {
				ChangeSentiment (1);
				Instantiate (FXHit.Instance.FXSentiment, transform.position, Quaternion.identity);
			}
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

	public override void OnDeath() { 
		if (DropSentiment) {
			int sentDropped = 0;
			Debug.Log ("Dropping Sentiment....");
			while (sentDropped < CurrentSentiment) {
				GameObject go = Instantiate (GameManager.Instance.FXExperience, transform.position, Quaternion.identity);
				go.GetComponent<ChaseTarget> ().StartingVel = new Vector2 (Random.Range (-10f, 10f), 10f);
				sentDropped += 1;
			}
		}
	}

	public void ChangeSentiment(int value) {
		CurrentSentiment += value;
		CurrentSentiment = Mathf.Max (0, CurrentSentiment);
		m_display.ChangeValue (value, CurrentSentiment);
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

	void UpdateSentiment(UIBarInfo ubi) {
		ubi.element.GetComponent<UIBar> ().UpdateValues (CurrentSentiment,MaxSentiment);
	}
}
