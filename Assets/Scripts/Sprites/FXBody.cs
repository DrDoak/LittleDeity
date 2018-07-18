using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXBody : MonoBehaviour {
	private static FXBody m_instance;

	public GameObject FXFlame;
	public GameObject FXIron;
	public GameObject FXLightning;
	public GameObject FXFan;
	public GameObject FXBurner;
	public GameObject FXWet;
	public GameObject FXExplosive;

	public AudioClip SFXFootstep;
	public AudioClip SFXJump;
	public AudioClip SFXFlaming;
	public AudioClip SFXFan;

	public static FXBody Instance
	{
		get { return m_instance; }
		set { m_instance = value; }
	}

	void Awake()
	{
		if (m_instance == null)
		{
			m_instance = this;
		}
		else if (m_instance != this)
		{
			Destroy(gameObject);
			return;
		}
	}
}
