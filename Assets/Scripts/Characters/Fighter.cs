using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Luminosity.IO;

[RequireComponent (typeof (HitboxMaker))]
[RequireComponent (typeof (PhysicsSS))]
[RequireComponent (typeof (AnimatorSprite))]
[RequireComponent (typeof (Attackable))]
[RequireComponent (typeof (SpriteOrientation))]
public class Fighter : MonoBehaviour
{
	[HideInInspector]
	public Dictionary<string,AttackInfo> Attacks = new Dictionary<string,AttackInfo>();

	public Dictionary<string, string> Aliases = new Dictionary<string, string>();

	public bool AutoOrientSprite = true;
	public string IdleAnimation = "idle";
	public string WalkAnimation = "walk";
	public string HurtAnimation = "hit";
	public string AirAnimation = "air";
	public string JumpAnimation = "air";
	public string LandAnimation = "land";

	private Dictionary<Attackable,HitInfo> m_hitTargets;
	private PhysicsSS m_physics;
	private AnimatorSprite m_anim;
	private Attackable m_attackable;
	private AttackInfo m_currentAttack = null;
	public Dictionary<Attackable,HitInfo> AttackHistory { get { return m_hitTargets; } private set { m_hitTargets = value; } }

	private float m_animationSpeed = 2f;

	private Dictionary<HitboxInfo, float> m_queuedHitboxes = new Dictionary<HitboxInfo, float> ();
	private bool m_pauseAnim = false;

	private bool m_haveMovedOnGround = false;

	[HideInInspector]
	public AudioClip AttackSound;

	[HideInInspector]
	public float StunTime = 0.0f;

	internal void Awake()
	{
		m_anim = GetComponent<AnimatorSprite>();
		m_physics = GetComponent<PhysicsSS>();
		m_attackable = GetComponent<Attackable>();
		m_hitTargets = new Dictionary<Attackable,HitInfo> ();
	}

	internal void Start()
	{
		AttackInfo[] at1 = GetComponentsInChildren<AttackInfo> ();
		AttackInfo[] at2 =  GetComponents<AttackInfo>();
		var at = new AttackInfo[at1.Length + at2.Length];
		at1.CopyTo(at, 0);
		at2.CopyTo(at, at1.Length); 
		foreach (AttackInfo a in at)
		{
			if (!Attacks.ContainsKey(a.AttackName))
			{
				Attacks.Add(a.AttackName, a);
				a.AddListener(OnAttackProgressed);
			}
		}
		string[] defaultAttacks = new string[]{ "neutral", "up", "down", "side", "air", "air_up", "air_down", "air_side",
			"sneutral", "sup", "sdown", "sside", "sair", "sair_up", "sair_down", "sair_side" };
		foreach (string s in defaultAttacks) {
			Aliases ["i_" + s] = s;
		}
	}
	public void QueueHitbox(HitboxInfo hi, float delay) {
		m_queuedHitboxes.Add (hi, Time.timeSinceLevelLoad + delay);
	}
	private void updateQueueHitboxes() {
		Dictionary<HitboxInfo, float> newQueue = new Dictionary<HitboxInfo, float> ();
		foreach (HitboxInfo hi in m_queuedHitboxes.Keys) {
			if (Time.timeSinceLevelLoad > m_queuedHitboxes [hi])
				GetComponent<HitboxMaker> ().CreateHitbox (hi);
			else
				newQueue.Add (hi,m_queuedHitboxes[hi]);
		}
		m_queuedHitboxes = newQueue;
	}

	internal void Update()
	{
		ActivateStunIfDead ();
		if (ProgressStun())
			return;
		updateQueueHitboxes();
		if (ProgressAttack())
			return;
		if (!m_pauseAnim)
			ProgressWalkOrIdleAnimation();
		if (canAttack ()) {
			if (GetComponent<BasicMovement> ().IsCurrentPlayer)
				CheckInputs ();
		}
	}

	bool canAttack() {
		return (GetComponent<Fighter> ().StunTime <= 0 && GetComponent<BasicMovement> ().Autonomy);
	}
	void CheckInputs() {
		if (InputManager.GetButton ("Ability1")) {
			Fighter f = GetComponent<Fighter> ();
			if (GetComponent<PhysicsSS> ().OnGround) {
				if (InputManager.GetButton("Up"))
					f.TryAttack(new string[]{"i_up","i_neutral"});
				else if (InputManager.GetButton ("Down"))
					f.TryAttack(new string[]{"i_down","i_neutral"});
				else if (InputManager.GetButton("Left") || InputManager.GetButton("Right"))
					f.TryAttack(new string[]{"i_side","i_neutral"});
				else 
					f.TryAttack(new string[]{"i_neutral"});
			} else {
				if (InputManager.GetButton("Up"))
					f.TryAttack(new string[]{"i_air_up","i_air","i_up","i_neutral"});
				else if (InputManager.GetButton ("Down"))
					f.TryAttack(new string[]{"i_air_down","i_air","i_down","i_neutral"});
				else if (InputManager.GetButton("Left") || InputManager.GetButton("Right"))
					f.TryAttack(new string[]{"i_air_side","i_air","i_side","i_neutral"});
				else 
					f.TryAttack(new string[]{"i_air","i_neutral"});
			}
		}

		if (InputManager.GetButton ("Ability2")) {
			Fighter f = GetComponent<Fighter> ();
			if (GetComponent<PhysicsSS> ().OnGround) {
				if (InputManager.GetButton("Up"))
					f.TryAttack(new string[]{"i_sup","i_sneutral"});
				else if (InputManager.GetButton ("Down"))
					f.TryAttack(new string[]{"i_dsown","i_sneutral"});
				else if (InputManager.GetButton("Left") || InputManager.GetButton("Right"))
					f.TryAttack(new string[]{"i_sside","i_sneutral"});
				else 
					f.TryAttack(new string[]{"i_sneutral"});
			} else {
				if (InputManager.GetButton("Up"))
					f.TryAttack(new string[]{"i_sair_up","i_sair","i_sup","i_sneutral"});
				else if (InputManager.GetButton ("Down"))
					f.TryAttack(new string[]{"i_sair_down","i_sair","i_sdown","i_sneutral"});
				else if (InputManager.GetButton("Left") || InputManager.GetButton("Right"))
					f.TryAttack(new string[]{"i_sair_side","i_sair","i_sside","i_sneutral"});
				else 
					f.TryAttack(new string[]{"i_sair","i_sneutral"});
			}
		}
	}

	public void SetBind(string bindName, string attackName) { 
		GetComponent<Fighter>().Aliases [bindName] = attackName;
	}
	
	public void SetPause(bool p) {
		m_pauseAnim = p;
	}
	private bool ProgressStun()
	{
		if (StunTime <= 0.0f)
			return false;
		m_anim.Play(HurtAnimation, AutoOrientSprite);
		StunTime -= Time.deltaTime;
		if (m_currentAttack != null)
			m_currentAttack = null;
		if (StunTime <= 0.0f && m_attackable.Alive)
			EndStun();
		return true;
	}

	private void ActivateStunIfDead()
	{
		if (m_attackable.Alive)
			return;
		StartHitState(3.0f);
	}

	private bool ProgressAttack()
	{
		if (m_currentAttack == default(AttackInfo))
			return false;
		m_currentAttack.Progress();
		return true;
	}

	public void OnAttackProgressed(AttackState state)
	{
		switch (state)
		{
			case AttackState.STARTUP:
				OnAttackStart();
				break;
			case AttackState.RECOVERY:
				OnAttackRecover();
				break;
			case AttackState.INACTIVE:
				EndAttack();
				break;
		}
	}

	private void OnAttackStart()
	{
		/*if (m_currentAttack.m_SoundInfo.AttackFX)
			AddEffect(m_currentAttack.m_SoundInfo.AttackFX, m_currentAttack.m_AttackAnimInfo.RecoveryTime + 0.2f);*/

		m_anim.Play(m_currentAttack.m_AttackAnimInfo.StartUpAnimation, true);
		m_anim.SetSpeed(m_currentAttack.m_AttackAnimInfo.AnimSpeed * m_animationSpeed);
	}

	private void OnAttackRecover()
	{
		m_anim.Play(m_currentAttack.m_AttackAnimInfo.RecoveryAnimation, true);
	}

	public void EndAttack()
	{
		m_physics.CanMove = true;
		m_currentAttack = null;
		m_anim.SetSpeed(1.0f);
	}

	public void ProgressWalkOrIdleAnimation()
	{
		if (!m_physics.OnGround) {
			m_haveMovedOnGround = false;
			if (m_physics.TrueVelocity.y > 0f) {
				m_anim.Play (new string[]{JumpAnimation,AirAnimation});
			} else {
				m_anim.Play (AirAnimation);
			}
		} else {
			if (m_physics.IsAttemptingMovement ()) {
				m_anim.Play (WalkAnimation);
				m_haveMovedOnGround = true;
			} else {
				if ( !m_haveMovedOnGround && m_physics.TimeCollided[Direction.DOWN] < 0.45f) {
					m_anim.Play (new string[]{LandAnimation,IdleAnimation});
				} else {
					m_anim.Play (IdleAnimation);
				}
			}
		}
	}

	void AddEffect(GameObject attackFX, float lifeTime)
	{
		/*
		var fx = GameObject.Instantiate(attackFX, transform);
		fx.GetComponent<disappearing>().duration = m_currentAttack.RecoveryTime;
		fx.GetComponent<disappearing>().toDisappear = true;
		fx.GetComponent<Follow>().followObj = gameObject;
		fx.GetComponent<Follow>().followOffset = new Vector3 (0.0f, 0.0f, -3.0f);
		fx.GetComponent<Follow>().toFollow = true;
		if (m_physics.facingLeft)
			fx.transform.Rotate (new Vector3 (0f, 180f,0f));
		ParticleSystem [] partsys = fx.GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem p in partsys) {
			ParticleSystem.MainModule mainP = p.main;
			mainP.startLifetime = lifeTime; 
		}
		*/
	}

	public bool IsAttacking()
	{
		return m_currentAttack != null && m_currentAttack.CurrentProgress != AttackState.INACTIVE;
	}

	public void RegisterStun(float st, bool defaultStun, HitInfo hi)
	{

		if (m_currentAttack != null) {
			m_currentAttack.OnInterrupt (StunTime, defaultStun, hi);
		}
		if (defaultStun) {
			StartHitState (st);
		}
	}

	void StartHitState(float st)
	{
		//Debug.Log ("Starting Hit State with Stun: "+ st);
		EndAttack();
		StunTime = st;
		m_physics.CanMove = false;
	}

	public void RegisterHit(GameObject otherObj,HitInfo hi, HitResult hr)
	{
		//Debug.Log ("Collision: " + this + " " + otherObj);
		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnHitConfirm (hi, otherObj, hr));
		//Debug.Log ("Registering hit with: " + otherObj);
		if (otherObj.GetComponent<Attackable> () != null) {
			m_hitTargets [otherObj.GetComponent<Attackable> ()] = hi;
		}
		if (m_currentAttack != null)
			m_currentAttack.OnHitConfirm(otherObj,hi,hr);
	}

	public void EndStun()
	{
		if (m_attackable.Alive)
		{
			m_physics.CanMove = true;
			StunTime = 0.0f;
		}
	}

	public AttackInfo TryAttack(string[] attackList)
	{
		foreach (string s in attackList) {
			AttackInfo ai = TryAttack (s);
			if (ai != null)
				return ai;
		}
		return null;
	}

	public AttackInfo TryAttack(string attackName) {
		//Debug.Log ("Trying attacK: " + attackName);
		if (Aliases.ContainsKey (attackName)) {
			//Debug.Log ("Found alias for: " + attackName + " changed to : " + Aliases[attackName]);
			attackName = Aliases [attackName];
		}
		if (IsAttacking () || !Attacks.ContainsKey (attackName) || StunTime > 0.0f)
			return null;
		m_currentAttack = Attacks[attackName];
		m_physics.CanMove = false;
		m_currentAttack.ResetAndProgress();
		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnAttack (m_currentAttack));
		return m_currentAttack;
	}


}