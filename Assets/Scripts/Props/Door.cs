using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

	public Sprite OpenedSprite;
	public Sprite ClosedSprite;
	[SerializeField]
	bool IsOpen;

	SpriteRenderer m_renderer;
	BoxCollider2D m_collider;
	// Use this for initialization
	void Start () {
		m_renderer = GetComponent<SpriteRenderer> ();
		m_collider = GetComponent<BoxCollider2D> ();
		SetOpen (IsOpen);
	}
	
	// Update is called once per frame
	void Update () {}

	public void SetOpen(bool open) {
		IsOpen = open;
		if (m_renderer == null)
			return;
		if (open) {
			m_renderer.sprite = OpenedSprite;
			m_collider.isTrigger = true;
			m_renderer.sortingOrder = 0;
		} else {
			m_renderer.sprite = ClosedSprite;
			m_collider.isTrigger = false;
			m_renderer.sortingOrder = 2;
		}
	}
}
