using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponPosition {
	public string name;
	public Vector2 pos;
	public float zRot;
	public float zAngleVariance = 0f;
	public Vector2 scale = new Vector2(1.5f,1.5f);
	public Vector2 scaleVariance = new Vector2();
	public float xRot;
	public bool accel;
	public float timeInPosition = 0.3f;
	public float AnimSpeed = 0f;
}
public class WeaponFloating : MonoBehaviour {

	public float angle = 0f;
	public float base_angle = 0f;

	public float sprite_angle = 0f;
	public float sAngle = 0f;

	public List<WeaponPosition> WeaponPos;

	private Dictionary<string,WeaponPosition> m_weaponDict;

	private SpriteRenderer m_sprite;
	private Animator m_animator;
	private float m_ZAngle;
	private float m_XAngle;

	private ChaseTarget m_chase;
	public GameObject m_target;

	private bool m_slashing = false;
	private float m_timeStopSlash = 0.0f;
	private WeaponPosition m_currentWeaponPos;

	// Use this for initialization
	void Start () {
		m_sprite = GetComponent<SpriteRenderer> ();
		m_animator = GetComponent<Animator> ();
		//m_animator.speed = 0f;
		m_animator.Play ("normal", -1, sAngle);
		m_chase = GetComponent<ChaseTarget> ();
		m_weaponDict = new Dictionary<string, WeaponPosition> ();
		foreach (WeaponPosition wp in WeaponPos) {
			m_weaponDict [wp.name] = wp;
		}
		//m_chase.Active = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!m_slashing) {
			standardSprite ();
			updateStandardSprite ();
		} else {
			ExecuteFX ();
		}
	}

	void standardSprite() {
		PhysicsSS m_physics = m_target.GetComponent<PhysicsSS> ();
		if (!m_physics.OnGround) {
			if (m_physics.TrueVelocity.y > 0f) {
				updateState (new string[]{"jump","air","idle"});
			} else {
				updateState (new string[]{"air","idle"});
			}
		} else {
			if (m_physics.IsAttemptingMovement ())
				updateState (new string[]{"run","idle"});
			else
				updateState (new string[]{"idle"});
		}
	}

	void updateState(string[] sList) {
		foreach (string s in sList) {
			if (m_weaponDict.ContainsKey (s)) {
				updateState (m_weaponDict [s]);
				break;
			}
		}
	}

	void setAnimState(WeaponPosition wp) {
		angle = wp.zRot + Random.Range (-wp.zAngleVariance / 2f, wp.zAngleVariance / 2f);
		float xScale = wp.scale.x + Random.Range(-wp.scaleVariance.x/2f, wp.scaleVariance.x/2f);
		float yScale = wp.scale.y + Random.Range(-wp.scaleVariance.y/2f, wp.scaleVariance.y/2f);
		transform.localScale = new Vector3 (xScale,yScale, 1f);
		sprite_angle = wp.xRot;
		m_animator.speed = wp.AnimSpeed;
		m_currentWeaponPos = wp;
		//Debug.Log("Setting Angle: " + wp.zRot + " to: " + angle);
	}

	void updateState(WeaponPosition wp) {
		if (m_currentWeaponPos == null)
			m_currentWeaponPos = wp;
		if ( m_currentWeaponPos != wp)
			setAnimState (wp);
		Vector2 off = wp.pos;
		float drawAngle = angle;
		if (m_target.GetComponent<PhysicsSS> ().FacingLeft) {
			off.x *= -1f;
			m_sprite.flipX = true;
			drawAngle = - angle;
		} else {
			m_sprite.flipX = false;
		}
		m_chase.SetTargetOffset (m_target.GetComponent<PhysicsSS> (), off);
		m_chase.Accelerate = wp.accel;
		transform.rotation = Quaternion.Euler (new Vector3(0f,0f, drawAngle + base_angle));
	}

	void updateStandardSprite() {
		sAngle = ((sprite_angle + 1) % 360f) / 360f;
		m_animator.Play ("normal", -1, sAngle);
		//transform.position = m_target.transform.position + new Vector3(currentOffset.x,currentOffset.y,0f);
	}

	void ExecuteFX() {
		updateState (m_currentWeaponPos);
		m_animator.Play ("slash",-1);
		if (Time.timeSinceLevelLoad > m_timeStopSlash) {
			StopSlashFX ();
		}
	}

	public void StartSlashFX(WeaponPosition wp) {
		m_animator.Play ("normal", -1, sAngle);
		m_timeStopSlash = Time.timeSinceLevelLoad + wp.timeInPosition;
		m_slashing = true;
		setAnimState (wp);
		Debug.Log ("Anim Speed: " + wp.AnimSpeed);
	}

	void StopSlashFX () {
		m_slashing = false;
		//m_animator.speed = 0f;
	}
}
