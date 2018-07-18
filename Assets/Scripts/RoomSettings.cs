using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSettings : MonoBehaviour {

	public bool UseCameraLimits = false;
	public bool DisplayLevelInfo = false;
	public string RoomName = "Unknown Room";
	public string Description = "Description not assigned in RoomSettings";

	public bool Silence = false;
	public AudioClip RoomMusic;

	// Use this for initialization
	void Start () {
		if (UseCameraLimits == true) {
			CameraFollow cf = GameManager.Instance.GetComponent<CameraFollow> ();
			cf.UseCameraLimits = true;
			cf.minVertex = new Vector2 (transform.position.x - transform.localScale.x / 2f, transform.position.y - transform.localScale.y / 2f);
			cf.maxVertex = new Vector2 (transform.position.x + transform.localScale.x / 2f, transform.position.y + transform.localScale.y / 2f);
			cf.initFunct ();
		}
		if (DisplayLevelInfo == true) { 
			GameManager.Instance.transform.GetChild(0).GetComponentInChildren<RoomDescription> ().SetNameDescription (RoomName, Description);
		}
		if (Silence)
			AudioManager.instance.StopMusic ();
		if (RoomMusic != null)
			AudioManager.instance.PlayMusic (RoomMusic);
	}
}
