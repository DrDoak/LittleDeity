using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RoomDirection {
	LEFT,
	RIGHT,
	UP,
	DOWN,
	NEUTRAL
}

public class SceneTrigger : Interactable {

	public Scene TestScene;
	public bool onContact = true;
	public string sceneName;
	public Vector3 newPos = Vector2.zero;
	public string TriggerID = "none";
	public string TargetTriggerID = "none";
	public RoomDirection dir;

	void Update () {}
	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 1, 0, .5f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}
	internal void OnTriggerEnter2D(Collider2D other) {
		if (onContact && other.gameObject.GetComponent<BasicMovement> () ) {
			changeRoom (other.gameObject);
		}
	}
	protected virtual void changeRoom(GameObject go) {
		if (go.GetComponent<BasicMovement> ()) {
			if (go.GetComponent<Attackable> ().Alive == false)
				return;
			TriggerUsed = true;
			if (TriggerID != "none") {
				RoomDirection realDir = dir;
				string realTarget = TargetTriggerID;
				if (TargetTriggerID == "none") {
					realTarget = TriggerID;
				}
				if (realDir == RoomDirection.NEUTRAL) {
					float diffX = transform.position.x - go.transform.position.x;
					float diffY = transform.position.y - go.transform.position.y;
					if (Mathf.Abs (diffX) > Mathf.Abs (diffY)) {
						if (diffX < 0f) {
							realDir = RoomDirection.LEFT;
						} else {
							realDir = RoomDirection.RIGHT;
						}
					} else {
						if (diffY > 0f) {
							realDir = RoomDirection.UP;
						} else {
							realDir = RoomDirection.DOWN;
						}
					}
				}
				if (go.GetComponent<PersistentItem>() != null)
					SaveObjManager.MoveItem (go, sceneName, realTarget,realDir);
			} else if (Vector2.Equals(Vector2.zero,newPos) && (go.GetComponent<PersistentItem>() != null)){
				SaveObjManager.MoveItem (go, sceneName, go.gameObject.transform.position);
			} else if (go.GetComponent<PersistentItem>() != null) {
				SaveObjManager.MoveItem (go, sceneName, newPos);
			}
			if (go.GetComponent<BasicMovement> ().IsCurrentPlayer) {
				//GameManager.Instance.LoadRoom (sceneName);
				Initiate.Fade (sceneName, Color.black, 5.0f);
			}
			Destroy (go);
		} else {
		}
	}
}