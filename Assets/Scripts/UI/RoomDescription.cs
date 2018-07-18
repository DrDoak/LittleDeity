using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoomDescription : MonoBehaviour {

	string m_name;
	string m_description;
	TextMeshProUGUI roomNamePrefab;
	TextMeshProUGUI descriptionPrefab;
	float m_timeDisplayed = 0.0f;
	float name_alpha = 0f;
	float desc_alpha = 0f;
	const float DISPLAY_TIME = 4.0f;
	bool animating = false;
	void Awake () {
		roomNamePrefab = transform.Find ("Name").gameObject.GetComponent<TextMeshProUGUI> ();
		descriptionPrefab = transform.Find ("Description").gameObject.GetComponent<TextMeshProUGUI> ();
	}
	public void SetNameDescription(string roomName,string roomDescription, bool animate=true) {
		Debug.Log ("Setting name and descp");
		m_name = roomName;
		roomNamePrefab.SetText (m_name);
		descriptionPrefab.SetText (roomDescription);
		if (animate) {
			m_timeDisplayed = 0.0f;
			name_alpha = 0f;
			desc_alpha = 0f;
			animating = true;
		}

	}
	void Update() {
		if (animating) {
			m_timeDisplayed += Time.deltaTime;
			if (m_timeDisplayed > DISPLAY_TIME) {
				if (name_alpha > 0) {
					name_alpha -= Time.deltaTime;
				}
			} else {
				name_alpha = Mathf.Min (1f, 2f * m_timeDisplayed);
			}
			roomNamePrefab.color = new Color (1f, 1f, 1f, name_alpha);

			float descTime = m_timeDisplayed - 2f;
			if ( descTime > DISPLAY_TIME) {
				if (desc_alpha > 0) {
					desc_alpha -= Time.deltaTime;
				} else {
					OnDisappear ();
				}
			} else {
				desc_alpha = Mathf.Min (1f, 2f * descTime);
			}
			descriptionPrefab.color = new Color (1f, 1f, 1f, desc_alpha);				
		}
	}
	private void OnDisappear() {
		name_alpha = 0.0f;
		desc_alpha = 0.0f;
		animating = false;
	}
}
