using System;
using System.Collections.Generic;
using UnityEngine;

public enum AttackState { STARTUP, ATTACK, RECOVERY, INACTIVE };
public delegate void AttackProgress(AttackState attackState);

[System.Serializable]
public class AttackAnimInfo {
	public float AnimSpeed = 1f;
	public float StartUpTime = 0.5f;
	public float AttackTime = 0.0f;
	public float RecoveryTime = 0.5f;
	public string StartUpAnimation = "none";
	public string RecoveryAnimation = "none";
}

[System.Serializable]
public class HitboxInfo {
	public Vector2 HitboxScale = new Vector2 (1.0f, 1.0f);
	public Vector2 HitboxOffset = new Vector2(0f,0f);
	public float Damage = 10.0f;
	public float Stun = 0.3f;
	public float HitboxDuration = 0.5f;
	public Vector2 Knockback = new Vector2(10.0f,10.0f);
	public bool FixedKnockback = true;
	public ElementType Element = ElementType.PHYSICAL;
	public bool ApplyProps = true;
	public bool FollowCharacter = true;
	public float Delay = 0.0f;
}

[System.Serializable]
public class SoundInfo {
	//public GameObject AttackFX;
	public AudioClip StartupSoundFX ;
	public AudioClip AttackSoundFX ;
}

[System.Serializable]
public class AIInfo {
	public float Frequency = 0.5f;
	public bool UniqueAIPrediction = false;
	public bool AutoAttack = false;
	public Vector2 AIPredictionHitbox = Vector2.zero;
	public Vector2 AIPredictionOffset = Vector2.zero;
}

public class AttackInfo : MonoBehaviour
{
	private AttackState m_progress;
	private float m_timeSinceStart = 0.0f;
	private Dictionary<AttackState, float> m_progressEndTimes;
	private Dictionary<AttackState, Action> m_progressCalls;

	public AttackState CurrentProgress { get { return m_progress; } }
	private event AttackProgress ProgressEvent = delegate {};

	public string AttackName = "default";

	public List<HitboxInfo> m_HitboxInfo;
	public AttackAnimInfo m_AttackAnimInfo;
	public AIInfo m_AIInfo;
	public SoundInfo m_SoundInfo;

	protected PhysicsSS m_physics;
	protected HitboxMaker m_hitboxMaker;

	internal void Awake()
	{
		m_physics = GetComponent<PhysicsSS>();
		m_hitboxMaker = GetComponent<HitboxMaker>();

		m_progress = AttackState.INACTIVE;
		m_progressEndTimes = new Dictionary<AttackState, float>()
		{
			{ AttackState.STARTUP, m_AttackAnimInfo.StartUpTime },
			{ AttackState.ATTACK,  m_AttackAnimInfo.StartUpTime +  m_AttackAnimInfo.AttackTime },
			{ AttackState.RECOVERY,  m_AttackAnimInfo.StartUpTime +  m_AttackAnimInfo.AttackTime +  m_AttackAnimInfo.RecoveryTime },
			{ AttackState.INACTIVE, 0 }
		};
		m_progressCalls = new Dictionary<AttackState, Action>()
		{
			{ AttackState.STARTUP, OnStartUp},
			{ AttackState.ATTACK, OnAttack },
			{ AttackState.RECOVERY, OnRecovery },
			{ AttackState.INACTIVE, OnConclude }
		};
	}

	public void AddListener(AttackProgress ap)
	{
		ProgressEvent += ap;
	}

	public void Progress()
	{
		m_timeSinceStart += Time.deltaTime;
		if (m_timeSinceStart < m_progressEndTimes [m_progress])
			return;
		m_progressCalls[NextInProgression()]();
		ProgressEvent.Invoke(m_progress);
	}

	private AttackState NextInProgression()
	{
		m_progress = (m_progress == AttackState.INACTIVE) ? AttackState.STARTUP : m_progress + 1;
		return m_progress;
	}

	public void ResetAndProgress()
	{
		m_timeSinceStart = 0;
		m_progress = AttackState.INACTIVE;
		Progress();
	}

	public virtual void OnHitConfirm(GameObject other, Hitbox hb, HitResult hr) {}

	public virtual void OnInterrupt(float stunTime, bool successfulHit, Hitbox hb)
	{
		
	}

	protected virtual void OnStartUp()
	{
		if (m_AIInfo.UniqueAIPrediction == false && m_HitboxInfo.Count > 0){
			m_AIInfo.AIPredictionHitbox = m_HitboxInfo[0].HitboxScale;
			m_AIInfo.AIPredictionOffset = m_HitboxInfo[0].HitboxOffset;
		}
		if (m_SoundInfo.StartupSoundFX != null)
			FindObjectOfType<AudioManager> ().PlayClipAtPos (m_SoundInfo.StartupSoundFX,transform.position,0.5f,0f,0.25f);
	}

	protected virtual void OnAttack()
	{
		if (m_HitboxInfo.Count > 0) {
			createHitboxes ();
		}
		if (m_SoundInfo.AttackSoundFX != null)
			FindObjectOfType<AudioManager> ().PlayClipAtPos (m_SoundInfo.AttackSoundFX,transform.position,0.5f,0f,0.25f);
	}

	protected virtual void OnRecovery()
	{
	}

	protected virtual void OnConclude()
	{
	}

	private void createHitboxes()
	{
		//m_hitboxMaker.AddHitType(HitType);
		foreach (HitboxInfo hi in m_HitboxInfo) {
			if (hi.Delay <= 0f)
				m_hitboxMaker.CreateHitbox (hi);
			else
				GetComponent<Fighter> ().QueueHitbox (hi, hi.Delay);
		}
//		Vector2 offset = m_physics.OrientVectorToDirection(m_HitboxInfo.HitboxOffset);
//		m_hitboxMaker.CreateHitbox(m_HitboxInfo.HitboxScale, offset, m_HitboxInfo.Damage,
//			m_HitboxInfo.Stun, m_HitboxInfo.HitboxDuration, m_HitboxInfo.Knockback, true, true,m_HitboxInfo.Element,m_HitboxInfo.ApplyProps);
	}
}

