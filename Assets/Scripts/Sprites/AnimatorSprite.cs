using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (SpriteRenderer))]
public class AnimatorSprite : MonoBehaviour
{
	private Animator m_anim;
	private List<string> m_states;
	private string m_currentAnim = "";

	internal void Awake()
	{
		m_states = new List<string>();
		m_anim = GetComponent<Animator>();
		if (FindObjectOfType<LightSettings> () != null &&
		    FindObjectOfType<LightSettings> ().UseLighting) {
			GetComponent<SpriteRenderer> ().material.shader = FindObjectOfType<LightSettings> ().lightingShader;
			transform.position = new Vector3 (transform.position.x, transform.position.y,  -1f * (GetComponent<Renderer> ().sortingOrder) / 32);
		}
	}

	public void Play(string[] stateNames)
	{
		foreach (string s in stateNames)
		{
			if (Play(s)) 
				break;
		}
	}

	public bool Play(string stateName, bool autoAlign = false)
	{
		if (m_currentAnim == stateName || m_currentAnim == "none")
			return true;
		if (m_states.Contains(stateName))
			return SetAndPlay(stateName);
		
		if (m_anim.HasState(0, Animator.StringToHash(stateName))) {
			m_states.Add(stateName);
			return SetAndPlay(stateName);
		}
		Debug.Log ("Does not have anim: " + stateName);
		return false;
	}

	private bool SetAndPlay(string stateName)
	{
		m_anim.Play(stateName);
		m_currentAnim = stateName;
		return true;
	}

	public void SetSpeed(float speed)
	{
		m_anim.speed = speed;
	}
}
