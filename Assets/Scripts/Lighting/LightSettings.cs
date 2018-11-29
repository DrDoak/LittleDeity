using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSettings : MonoBehaviour {

	public bool UseLighting = false;
	public Color AmbientColor;
	public float AmbientIntensity;
	public Material lightingMaterial;
	// Use this for initialization
	void Start () {
		if (UseLighting) {
			RenderSettings.ambientLight = AmbientColor;
			RenderSettings.ambientIntensity = AmbientIntensity;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void InitializeSprites() {
	}
}
