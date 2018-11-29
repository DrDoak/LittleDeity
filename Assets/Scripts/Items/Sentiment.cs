using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentiment : MonoBehaviour {

	public float AttractDistance;
	public float AbsorbDistance;
	public int Value = 1;

	private List<SentimentHolder> m_holders;
	private bool foundHolders = false;

	private GameObject m_target;
	private ChaseTarget m_chase;

	// Use this for initialization
	void Start () {
		m_chase = GetComponent<ChaseTarget> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (foundHolders) {
			if (m_target != null)
				m_chase.SetTargetOffset (m_target, new Vector2());
			float d = Vector3.Distance (transform.position, m_target.transform.position);
			if (d < AbsorbDistance) {
				m_target.GetComponent<SentimentHolder> ().AddSentiment (Value);
				Destroy (gameObject);
			}
		} else {
			SentimentHolder [] sh = FindObjectsOfType<SentimentHolder> ();
			float minDist = AttractDistance;
			foreach (SentimentHolder s in sh) {
				float d = Vector3.Distance (transform.position, s.transform.position);
				if (d < minDist) {
					minDist = d;
					m_target = s.gameObject;
					transform.position = new Vector3 (transform.position.x, transform.position.y, m_target.transform.position.z);
					foundHolders = true;
				}
			}
		}
	}
}
