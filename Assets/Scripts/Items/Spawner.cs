using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

	public GameObject respawnObj;
	public float Interval = 3.0f;
	public int Max_Items = 3;
	private int m_spawnedItems = 0;
	public int SpawnedItems { get { return m_spawnedItems; } set { m_spawnedItems = value; } }

	public bool ResetTimerOnDestroy = true;
	//public bool permanentObject = false;
	//public string groupID = "none";
	float currentTime;
	public bool SinglePointRespawn = true;
	// Use this for initialization
	void Start () {
		MeshRenderer mr = GetComponent<MeshRenderer> ();
		if (mr) {
			Destroy (mr);
		}
		if (Interval > 30.0f) {
			currentTime = Interval;
		}
	}
		
	
	// Update is called once per frame
	void Update () {
		if (respawnObj) {
			
			currentTime += Time.deltaTime;
			if (currentTime > Interval && m_spawnedItems < Max_Items) { 
				float newX = transform.position.x ;
				float newY = transform.position.y ;
				if (!SinglePointRespawn) {
					newX += Random.Range (-transform.localScale.x / 2, transform.localScale.x / 2);
					newY += Random.Range (-transform.localScale.y / 2, transform.localScale.y / 2);
				}
				GameObject obj = GameObject.Instantiate (respawnObj, new Vector3 (newX, newY, 0), Quaternion.identity);
				if (obj.GetComponent<PersistentItem> ())
					Destroy (obj.GetComponent<PersistentItem> ());
				m_spawnedItems += 1;
				//			Debug.Log (spawnedItems);
				currentTime = 0f;
				obj.AddComponent<SpawnedObj> ();
				obj.GetComponent<SpawnedObj> ().mSpawner = this;
				/*if (permanentObject && obj.GetComponent<Disappear> ()) {
					Destroy (obj.GetComponent<Disappear> ());
				}*/
			}
		}
	}
	public void registerDestruction() {
		m_spawnedItems = m_spawnedItems - 1;
		if (ResetTimerOnDestroy) {
			currentTime = 0f;
		}
	}
	void OnDrawGizmos() {
		Gizmos.color = new Color (1, 1, 0, .8f);
		Gizmos.DrawCube (transform.position, transform.localScale);
	}
}
