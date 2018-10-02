using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSLight : MonoBehaviour {

	public bool Fluctuate = false;
	public bool RandomizeInit = false;

	public bool FluctuateRange = false;
	public float FluctuateRangeSize = 1f;
	public float FluctuateRangeRate = 1f;

	public Color LowColor;
	public Color HighColor;
	public float FluctuateRate = 0.1f;

	public float Size = 10f;
	public float Brightness = 6f;
	public float Evenness = 1f;

	private Color CurrentColor;

	protected const float RANGE_RATIO = 0.25f;
	protected const float INTENSITY_RATIO = 0.15f;

	protected Light m_light;

	protected float m_startingOffset = 0f;

	// Use this for initialization
	void Start () {
		init ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (transform.position.x, transform.position.y, -3f * Evenness);
		calculateIntensity ();
		calculateRange ();
		fluctuateColor ();
	}

	protected void init() {
		m_light = GetComponent<Light> ();
		if (RandomizeInit)
			m_startingOffset = Random.Range (0f, 100f);		
	}
	protected void calculateIntensity() {
		m_light.intensity = Brightness * INTENSITY_RATIO * Evenness + ((1f - INTENSITY_RATIO) * Brightness);
	}
	protected void calculateRange() {
		float defaultRange = Size;
		if (FluctuateRange)
			defaultRange += (Mathf.Sin (m_startingOffset + (Time.timeSinceLevelLoad / FluctuateRangeRate)) * 0.5f + 0.5f) * FluctuateRangeSize;
		m_light.range = defaultRange * RANGE_RATIO * Evenness + ((1f - RANGE_RATIO) * defaultRange);
	}
	protected void fluctuateColor() {
		if (Fluctuate) {
			CurrentColor = Color.Lerp (LowColor, HighColor, Mathf.Sin (m_startingOffset + (Time.timeSinceLevelLoad / FluctuateRate)) * 0.5f + 0.5f);
			m_light.color = CurrentColor;
		}
	}
}
