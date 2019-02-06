using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSettings : MonoBehaviour {

	public bool UseLighting = false;
	public Color AmbientColor;
	public float AmbientIntensity;

	// Use this for initialization
	void Start () {
		if (UseLighting) {
			RenderSettings.ambientLight = AmbientColor;
			RenderSettings.ambientIntensity = AmbientIntensity;
		} else {
			RenderSettings.ambientLight = new Color(1f,1f,1f,1f);
			RenderSettings.ambientIntensity = 1f;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void InitializeSprites() {
	}
}
