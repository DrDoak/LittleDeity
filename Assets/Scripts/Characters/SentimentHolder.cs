using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentimentHolder : MonoBehaviour {
	public int Sentiment = 0;
	public int VisualSentiment = 0;
	public float m_visualSentiment;
	public const float MONEY_ADD_SPEED = 0.5f;

	void Start() {
		if (GetComponent<PersistentItem> () != null)
			GetComponent<PersistentItem> ().InitializeSaveLoadFuncs (storeData,loadData);
	}

	public void AddSentiment(int value) {
		Sentiment += value;
	}

	void Update() {
		if (VisualSentiment < Sentiment) {
			float diff = Sentiment - VisualSentiment;
			m_visualSentiment += Mathf.Max (Time.deltaTime * 10, diff/MONEY_ADD_SPEED * Time.deltaTime);
			VisualSentiment = Mathf.RoundToInt (m_visualSentiment);
		}
	}

	private void storeData(CharData d) {
		d.PersistentInt ["Sentiment"] = Sentiment;
	}

	private void loadData(CharData d) {
		Sentiment = d.PersistentInt ["Sentiment"];
	}
}
