using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

[System.Serializable]
public class AttackClingInfo {
	public bool CanFlyToUp;
	public bool JumpOnRelease;
	public float BufferFlyTime;
	public float HalberdAngleOff;
	public Vector2 JumpProportion = new Vector2(1.0f,1.0f);
	public Vector3 ClingPosition;
	public Vector3 FlyPoint;
	public GameObject ClingTarget;
}


public class AtkCling : AtkFlyToPoint {

	public AttackClingInfo m_clingInfo;

	public float m_time_cling;

	// Use this for initialization
	void Start () {
		
	}
	protected override void OnAttack()
	{
		base.OnAttack ();
		ActivateHalberd ();
		if (m_clingInfo.ClingPosition.x < transform.position.x)
			m_physics.SetDirection (true);
		else
			m_physics.SetDirection (false);
	}
	
	protected override void AttackTick ()
	{
		m_time_in_stance += Time.deltaTime;
		GetComponent<AnimatorSprite> ().Play (m_flyInfo.FlyAnim);
		if (m_time_in_stance > m_clingInfo.BufferFlyTime) {
			if (GetComponent<BasicMovement> ().IsCurrentPlayer && InputManager.GetButton ("Jump")) {
				if (m_clingInfo.CanFlyToUp) {
					GetComponent<Fighter>().EndAttack();
					OnInterrupt (0f,true,new HitInfo());
					AtkFlyToPoint ai = (AtkFlyToPoint)GetComponent<Fighter> ().TryAttack ("chase_noatk");
					//Debug.Log (m_clingInfo.ClingPosition);
					ai.m_flyInfo.Target = m_clingInfo.FlyPoint;
					return;
				}
			} else if (m_clingInfo.JumpOnRelease) {
				GetComponent<Fighter>().EndAttack();
				OnInterrupt (0f,true,new HitInfo());

				GetComponent<PhysicsSS> ().CancelVerticalMomentum ();
				GetComponent<BasicMovement> ().ApplyJumpVector (m_clingInfo.JumpProportion);
				return;
			}
		} else {
			DelayCurrentAttack (Time.deltaTime);
		}
	}

	private void ActivateHalberd() {
		GameObject h = GetComponent<HalberdTargetFinder> ().m_halberd;
		WeaponPosition wp = new WeaponPosition ();
		wp.timeInPosition = m_clingInfo.BufferFlyTime;
		wp.Animation = "Normal";
		wp.pos = m_clingInfo.ClingPosition - transform.position;
		wp.pos = new Vector2 (Mathf.Abs (wp.pos.x), wp.pos.y);
		wp.zRot = Mathf.Rad2Deg * Mathf.Atan2 (m_clingInfo.ClingPosition.y - transform.position.y, Mathf.Abs(m_clingInfo.ClingPosition.x - transform.position.x));
		wp.zRot += m_clingInfo.HalberdAngleOff;
		wp.zAngleVariance = 0f;
		h.GetComponent<WeaponFloating> ().StartSlashFX (wp);
	}

}
