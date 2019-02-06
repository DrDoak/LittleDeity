using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXLand : MonoBehaviour
{
	private ParticleSystem m_sys;
	private PhysicsSS m_phys;
	// Start is called before the first frame update
	void Start()
	{
		m_phys = transform.parent.gameObject.GetComponent<PhysicsSS> ();
		m_sys = GetComponent < ParticleSystem> ();
	}

	// Update is called once per frame
	void Update()
	{
		if ( m_phys.TimeOnGround > 0.0f && m_phys.TimeOnGround < 0.3f) {
			ParticleSystem.EmissionModule e = m_sys.emission;
			e.enabled = true;
		} else {
			ParticleSystem.EmissionModule e = m_sys.emission;
			e.enabled = false;
		}
	}
}
