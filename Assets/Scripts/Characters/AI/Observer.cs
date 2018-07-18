using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Observer : MonoBehaviour {

	PhysicsSS m;
	public float detectionRange = 15.0f;

	public List<Observable> VisibleObjs = new List<Observable>();
	float sinceLastScan;
	float scanInterval = 0.5f;
	float postLineVisibleTime = 3.0f;

	Dictionary<Observable,float> m_lastTimeSeen;
	// Use this for initialization
	void Start () {
		m_lastTimeSeen = new Dictionary<Observable,float> ();
		m = GetComponent<PhysicsSS> ();
		sinceLastScan = UnityEngine.Random.Range (0.0f, scanInterval);
	}

	void Update() {
		if (sinceLastScan > scanInterval) {
			scanForEnemies ();
		}
		sinceLastScan += Time.deltaTime;
	}

	void scanForEnemies() {
		//Debug.Log (gameObject + " is scanning, found " + allObs.Length);
		Observable[] allObs = FindObjectsOfType<Observable> ();
		float lts = Time.realtimeSinceStartup;
		foreach (Observable o in allObs) {
			Vector3 otherPos = o.transform.position;
			Vector3 myPos = transform.position;
			if (o.gameObject != gameObject && otherPos.x < myPos.x && m.FacingLeft ||
				otherPos.x > myPos.x && !m.FacingLeft) {
				float cDist = Vector3.Distance (otherPos, myPos);
				if (cDist < detectionRange) {
					RaycastHit2D[] hits = Physics2D.RaycastAll (myPos, otherPos - myPos, cDist);
					Debug.DrawRay (myPos, otherPos - myPos, Color.green);
					float minDist = float.MaxValue;
					foreach (RaycastHit2D h in hits) {
						GameObject oObj = h.collider.gameObject;
						if (oObj != gameObject && !h.collider.isTrigger && !JumpThruTag(oObj)) {
							minDist = Mathf.Min(minDist,Vector3.Distance (transform.position,h.point));
						}
					}
					float diff = Mathf.Abs (cDist - minDist);
					if (cDist < minDist) {
						if (!VisibleObjs.Contains (o)) {
							OnSight (o);
						}
					}
				}
			}
		}
		if (VisibleObjs.Count > 0) {
			for (int i= VisibleObjs.Count - 1; i >= 0; i --) {
				Observable o = VisibleObjs [i];
				if (o == null) { // c.gameObject == null) {
					VisibleObjs.RemoveAt (i);
				} else if (m_lastTimeSeen.ContainsKey(o)) {
					if (lts - m_lastTimeSeen[o] > postLineVisibleTime) {
						o.removeObserver (this);
						//outOfSight (o, true);
						VisibleObjs.RemoveAt (i);
					} else if (Mathf.Abs(lts - m_lastTimeSeen[o]) > 0.05f){
						//Out of sight thing.
					}	
				}
			}
		}
		sinceLastScan = 0f;
	}

	internal void OnSight(Observable o) {
		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnSight (o));
		if (GetComponent<AIFighter>()) {
			GetComponent<AIFighter> ().OnSight (o);
		}
		o.addObserver (this);
		VisibleObjs.Add (o);
	}
	public bool IsVisible(Observable o) {
		return VisibleObjs.Contains (o);
	}
	void OnDestroy() {
		foreach (Observable o in VisibleObjs) {
			o.removeObserver (this);	
		}
	}

	private bool JumpThruTag( GameObject obj ) {
		return (obj.CompareTag ("JumpThru") || (obj.transform.parent != null &&
			obj.transform.parent.CompareTag ("JumpThru")));
	}
}