using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	public float duration = 3.0f;
	public bool toDisappear = true;

	// Use this for initialization
	void Start () {
		if (toDisappear) {
			GameObject.Destroy (gameObject, duration);
		}
	}
}
