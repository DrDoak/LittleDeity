using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tiled2Unity;
public class LightSettings : MonoBehaviour {

	public bool UseLighting = false;
	public Color AmbientColor;
	public float AmbientIntensity;
	public Shader Tiled2UnityLightingShader;
	public Shader lightingShader;
	// Use this for initialization
	void Start () {
		if (UseLighting) {
			InitializeBackgrounds ();
			RenderSettings.ambientLight = AmbientColor;
			RenderSettings.ambientIntensity = AmbientIntensity;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void InitializeBackgrounds() {
		SortingLayerExposed [] sList  = FindObjectsOfType<SortingLayerExposed>();

		foreach (SortingLayerExposed sl in sList) {
			sl.transform.position = new Vector3 (sl.transform.position.x, sl.transform.position.y,  -1f * (sl.GetComponent<Renderer> ().sortingOrder) / 32);
			sl.GetComponent<MeshRenderer> ().material.shader = Tiled2UnityLightingShader;
		}
	}

	void InitializeSprites() {
	}
}
