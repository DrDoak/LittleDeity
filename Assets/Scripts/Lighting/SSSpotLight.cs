using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SSSpotLight : SSLight {

	public float AngleDown = 0f;
	// Use this for initialization
	void Start () {
		transform.position = new Vector3 (transform.position.x, transform.position.y, -6f);
		init ();
		transform.position = new Vector3 (transform.position.x, transform.position.y + Mathf.Tan(Mathf.Deg2Rad* AngleDown) * 4.5f, -6f );
		m_light.intensity = Brightness;
	}

	// Update is called once per frame
	void Update () {
		calculateOrientation ();
		fluctuateColor ();
		calculateWidth ();
	}

	void calculateOrientation() {
		
		transform.rotation = Quaternion.Euler (new Vector3 (AngleDown, 0f, 0f));
	}

	protected void calculateWidth() {
		float defaultRange = Size;
		if (FluctuateRange)
			defaultRange += (Mathf.Sin (m_startingOffset + (Time.timeSinceLevelLoad / FluctuateRangeRate)) * 0.5f + 0.5f) * FluctuateRangeSize;
		m_light.spotAngle = defaultRange * RANGE_RATIO * Evenness + ((1f - RANGE_RATIO) * defaultRange);
	}
}
