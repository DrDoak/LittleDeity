using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceHolder : MonoBehaviour {
	public int Experience = 0;
	public int VisualExperience = 0;
	public float m_visualExperience;
	public const float MONEY_ADD_SPEED = 0.5f;

	void Start() {
		if (GetComponent<PersistentItem> () != null)
			GetComponent<PersistentItem> ().InitializeSaveLoadFuncs (storeData,loadData);
	}

	public void AddExperience(int value) {
		Experience += value;
        Leveller.UpdateExperience(this);
	}
	void Update() {
		if (VisualExperience < Experience) {
			float diff = Experience - VisualExperience;
			m_visualExperience += Mathf.Max (Time.deltaTime * 10, diff/MONEY_ADD_SPEED * Time.deltaTime);
			VisualExperience = Mathf.RoundToInt (m_visualExperience);
		}
	}

	private void storeData(CharData d) {
		d.PersistentInt ["Experience"] = Experience;
	}

	private void loadData(CharData d) {
		Experience = d.PersistentInt ["Experience"];
	}
}
