using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PropertyText : MonoBehaviour {

	public float PersistTime;
	public Vector3 Velocity;
	public string PropertyName = "PropertyName";
	public string Description = "No Description Provided";

	private float m_time;
	private float m_halfTime;
	private TextMeshPro m_propName;
	private TextMeshPro m_description;

	// Use this for initialization
	void Start () {
		m_halfTime = PersistTime * 0.5f;
		m_propName = GetComponent<TextMeshPro> ();
		m_propName.text = PropertyName;
		m_description = transform.GetChild(0).GetComponent<TextMeshPro> ();
		m_description.text = Description;

	}
	
	// Update is called once per frame
	void Update () {
		m_time += Time.deltaTime;
		if (m_time > m_halfTime) {
			Color c = GetComponent<TextMeshPro> ().color;
			float a = 1f - ((m_time - m_halfTime) / m_halfTime);
			Color newC = new Color (c.r, c.g, c.b, a);
			m_propName.color = newC;
			m_description.color = newC;
			if (m_time > PersistTime)
				Destroy (gameObject);
		}
		transform.Translate (Velocity * Time.deltaTime);
	}
}
