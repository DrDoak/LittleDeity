using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentiment : MonoBehaviour {

	public float AttractDistance;
	public float AbsorbDistance;
	public int Value = 1;
	public GameObject DropFXOnDeath;

	private List<PR_SentimentTarget> m_holders;
	private bool foundHolders = false;

	private GameObject m_target;
	private ChaseTarget m_chase;
	private const float CHECK_THREASHOLD = 0.5f;
	private float m_nextCheck = 0f;

	// Use this for initialization
	void Start () {
		m_chase = GetComponent<ChaseTarget> ();
	}

	// Update is called once per frame
	void Update () {
		if (foundHolders) {
			if (m_target != null)
				m_chase.SetTargetOffset (m_target, new Vector2());
			Vector3 targetPos = new Vector3 (m_target.transform.position.x, m_target.transform.position.y, transform.position.z);
			float d = Vector3.Distance (transform.position, targetPos);
			if (d < AbsorbDistance) {
				m_target.GetComponent<PR_SentimentTarget> ().ChangeSentiment (Value);
				if (DropFXOnDeath != null) {
					Instantiate (DropFXOnDeath, transform.position, Quaternion.identity);
				}
				Destroy (gameObject);
			}
		} else {
			if (Time.timeSinceLevelLoad > m_nextCheck) {
				PR_SentimentTarget[] sh = FindObjectsOfType<PR_SentimentTarget> ();
				float minDist = AttractDistance;
				foreach (PR_SentimentTarget s in sh) {
					float d = Vector3.Distance (transform.position, s.transform.position);
					if (d < minDist) {
						minDist = d;
						m_target = s.gameObject;
						transform.position = new Vector3 (transform.position.x, transform.position.y, m_target.transform.position.z);
						foundHolders = true;
					}
				}
				m_nextCheck = Time.timeSinceLevelLoad + CHECK_THREASHOLD;
			}
		}
	}
}
