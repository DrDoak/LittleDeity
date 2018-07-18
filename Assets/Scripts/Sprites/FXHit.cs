using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXHit : MonoBehaviour {
	private static FXHit m_instance;

	public GameObject FXHitPhysical;
	public GameObject FXHitFire;
	public GameObject FXHitLightning;
	public GameObject FXHitBiological;
	public GameObject FXHitPsychic;
	public GameObject FXHitBlock;
	public GameObject FXExplosion;
	public GameObject FXHeal;

	public AudioClip SFXPhysical;
	public AudioClip SFXFire;
	public AudioClip SFXElectric;
	public AudioClip SFXGuard;
	public AudioClip SFXPsychic;
	public AudioClip SFXHeal;
	public AudioClip SFXExplosive;

	public static FXHit Instance
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
