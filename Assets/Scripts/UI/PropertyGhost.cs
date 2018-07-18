using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PropertyGhost : MonoBehaviour {

	Image m_bkg;
	Image icon;
	TextMeshProUGUI title;
	TextMeshProUGUI description;

	// Use this for initialization
	void Start () {
		m_bkg = GetComponent<Image> ();
		icon = transform.Find ("Image").gameObject.GetComponent<Image> ();
		title = transform.Find ("Title").gameObject.GetComponent<TextMeshProUGUI> ();
		description = transform.Find ("Description").gameObject.GetComponent<TextMeshProUGUI> ();
	}
	
	// Update is called once per frame
	void Update () {
		float alpha = Mathf.Sin (Time.unscaledTime * 2f) * 0.4f + 0.4f;
		m_bkg.color = new Color (0.2f, 0.2f, 0.2f, alpha);
		icon.color = new Color (1f, 1f, 1f, alpha);
		title.color = new Color (1f, 1f, 1f, alpha);
		description.color = new Color (1f, 1f, 1f, alpha);
	}
}
