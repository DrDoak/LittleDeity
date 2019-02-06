using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXWalk : MonoBehaviour
{
	public bool GroundOnly = true;

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
		if ((GroundOnly && !m_phys.OnGround) ||
			Mathf.Abs(m_phys.TrueVelocity.x) <= 0.01f) {
			ParticleSystem.EmissionModule e = m_sys.emission;
			e.enabled = false;
		} else {
			ParticleSystem.EmissionModule e = m_sys.emission;
			e.enabled = true;
		}
    }
}
