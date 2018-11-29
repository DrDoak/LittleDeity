using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Luminosity.IO;
using UnityEngine.EventSystems;

[RequireComponent (typeof (PhysicsSS))]
[RequireComponent (typeof (Attackable))]

[System.Serializable]
public class FootstepInfo {
	public bool PlayFootsteps = false;
	public AudioClip FootStepClip = null;
	public float FootStepVolume = 0.3f;
	public float FootStepPitchVariation = 0.25f;
	public float FootStepVolumeVariation = 0.0f;
	public float FootstepInterval = 0.75f;
}

public class BasicMovement : MonoBehaviour
{
	private const float MAX_OFFSET_TOLERANCE = 0.1f;
	private const float SMOOTH_TIME = .1f;
	private const float MIN_JUMP_INTERVAL = .2f;
	private const float NPC_STUCK_JUMP_TIME = .4f;
	private const float NPC_X_DIFF_MOVEMENT_THREASHOLD = 0.4f;
	private const float PIT_JUMP_VERTICAL_THREASHOLD = -0.3f;
	private const float DOUBLE_TAP_DROP_INTERVAL = 0.2f;

	public bool IsCurrentPlayer = false;
	bool m_savedCurrentPlayer = false;

	[SerializeField]
	private float m_jumpHeight = 4.0f;
	public float JumpHeight { get { return m_jumpHeight; } private set { m_jumpHeight = value; } }

	/*[SerializeField]
	private float m_timeToJumpApex = .4f;
	public float TimeToJumpApex { get { return m_timeToJumpApex; } private set { m_timeToJumpApex = value; } }*/

	[SerializeField]
	private bool VariableJumpHeight = false;
	private bool m_variableJumpApplied = false;

	// Physics helpers / configurables
	[SerializeField]
	private float m_moveSpeed = 8.0f;

	public float MoveSpeed { get { return m_moveSpeed; } private set { m_moveSpeed = value; } }
	private PhysicsSS m_physics;
	private Vector2 m_velocity;
	//private float m_accelerationTimeAirborne = .2f;
	private float m_accelerationTimeGrounded = .1f;
	private float m_accelerationFloatingY = .1f;
	private float m_velocityXSmoothing;
	private float jumpVelocity;
	Vector2 velocity;
	public Vector2 m_jumpVector;

	// Movement tracking
	public Vector2 m_inputMove;
	protected bool m_jumpDown;
	protected bool m_jumpHold;
	private Vector3 m_targetPoint;
	private bool m_targetSet = false;
	private bool m_targetObj = false;

	public float m_minDistance = 1.0f;
	public float m_abandonDistance = 10.0f;
	private bool m_submerged = false;
	public bool Submerged { get { return m_submerged; } set { m_submerged = value; } }
	public bool CanJump = true;
	public int CurrentAirJumps = 0;
	public int MidAirJumps = 0;
	private float m_lastJump = 0.0f;
	private float m_stuckTime = 0.0f;
	private float m_jumpThruStuckTime = 0.0f;
	private float m_verticalStuckTime = 0.0f; 

	private PhysicsSS m_followObj;
	private bool m_autonomy = true;
	public bool Autonomy { get { return m_autonomy; } private set { m_autonomy = value; } }

	public FootstepInfo m_FootStepInfo;

	float m_sinceStep = 0f;

	private float m_lastDownTime = 0f;

	internal void Awake()
	{
		m_physics = GetComponent<PhysicsSS>();
		if (CanJump)
			SetJumpHeight (JumpHeight);
		m_savedCurrentPlayer = IsCurrentPlayer;
	}

	internal void Start()
	{
		if (GetComponent<BasicMovement>().IsCurrentPlayer) {
			UIBarInfo ubi = new UIBarInfo ();
			ubi.FillColor = Color.green;
			ubi.UILabel = "Speed";
			ubi.funcUpdate = UpdateSpeedValues;
			ubi.target = gameObject;
			ubi.DisplayMode = UIBarDisplayMode.BASE;
			ubi.useScale = false;
			FindObjectOfType<GUIHandler> ().AddUIBar (ubi);
		}
		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnControllableChange (IsCurrentPlayer));
	}

	void UpdateSpeedValues(UIBarInfo ubi) {
		ubi.element.GetComponent<UIBar> ().UpdateValues (Mathf.Abs(Mathf.Round(ubi.target.GetComponent<PhysicsSS>().TrueVelocity.x * 100f)), 
			20f);
	}
		
		
	internal void Update()
	{
		if (!m_physics.CanMove) {
			m_inputMove = new Vector2(0f, 0f);
		}else if (IsCurrentPlayer && m_autonomy && Time.deltaTime > 0f)
			PlayerMovement();
		else if (m_targetSet)
			NpcMovement();
		if (m_FootStepInfo.PlayFootsteps)
			PlayStepSounds ();
		MoveSmoothly();
		currentPlayerControl ();
		ResetJumps ();
	}

	private void ResetJumps() {
		if (m_physics.OnGround)
			CurrentAirJumps = MidAirJumps;
	}
	private void currentPlayerControl() {
		if (m_savedCurrentPlayer != IsCurrentPlayer) {
			ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnControllableChange (IsCurrentPlayer));
			m_savedCurrentPlayer = IsCurrentPlayer;
		}
	}

	private void PlayStepSounds() {
		if (m_inputMove.x != 0f && m_physics.OnGround) {
			m_sinceStep += Time.deltaTime;
			if (m_sinceStep > m_FootStepInfo.FootstepInterval) {
				m_sinceStep = 0f;
				FindObjectOfType<AudioManager> ().PlayClipAtPos (
					(m_FootStepInfo.FootStepClip == null)?FXBody.Instance.SFXFootstep:m_FootStepInfo.FootStepClip,transform.position,
					m_FootStepInfo.FootStepVolume,m_FootStepInfo.FootStepVolumeVariation,m_FootStepInfo.FootStepPitchVariation);
			}
		} else {
			m_sinceStep = m_FootStepInfo.FootstepInterval;
		}
	}
	protected virtual void PlayerMovement() {
		m_inputMove = new Vector2(0f, 0f);
		if (InputManager.GetButton("Left"))
			m_inputMove.x -= 1f;
		if (InputManager.GetButton("Right"))
			m_inputMove.x += 1f;
		if (InputManager.GetButton("Up"))
			m_inputMove.y += 1f;
		if (InputManager.GetButton ("Down")) {
			m_inputMove.y -= 1f;
			/*if (InputManager.GetButtonDown ("Down")) {
				UIActionText uai = new UIActionText ();
				uai.text = "TEST";
				FindObjectOfType<GUIHandler> ().AddText (uai);
				if (Time.timeSinceLevelLoad - m_lastDownTime < DOUBLE_TAP_DROP_INTERVAL)
					m_physics.setDropTime (0.2f);
				m_lastDownTime = Time.timeSinceLevelLoad;
			}*/
		}

		m_jumpDown = InputManager.GetButtonDown ("Jump");
		m_jumpHold = InputManager.GetButton ("Jump");
		if (CanJump)
			JumpMovement ();
		SetDirectionFromInput();
	}

	protected void JumpMovement() {
		if (m_jumpDown) {
			if (m_inputMove.y < -0f) {
				m_physics.setDropTime(0.2f);
			}
			else {
				AttemptJump ();

			}
		}
		float dt = (Time.timeSinceLevelLoad - m_lastJump);
		if (VariableJumpHeight && dt > 0.1f && dt < 0.2f && m_jumpHold && !m_variableJumpApplied) {
			m_variableJumpApplied = true;
			ApplyJumpVector (new Vector2(1f, 0.35f));
		}
	}
	private void NpcMovement()
	{
		if (m_targetObj)
		{
			if (m_followObj == null)
			{
				EndTarget ();
				return;
			}
			m_targetPoint = m_followObj.transform.position;
		}
		MoveToPoint(m_targetPoint);
		SetDirectionFromInput();
	}


	private void MoveSmoothly() {
		Vector2 targetVel = new Vector2(m_inputMove.x * MoveSpeed, m_inputMove.y * MoveSpeed);
		m_velocity.x = Mathf.SmoothDamp(m_velocity.x, targetVel.x, ref m_accelerationTimeGrounded, SMOOTH_TIME);
		if (m_physics.Floating)
			m_velocity.y = Mathf.SmoothDamp(m_velocity.y, targetVel.y, ref m_accelerationFloatingY, SMOOTH_TIME);
		m_physics.Move(m_velocity, m_inputMove);
	}
		
	protected void SetDirectionFromInput()
	{
		if ( m_inputMove.x != 0f )
			m_physics.SetDirection ( m_inputMove.x < 0.0f );
	}

	private void SetInputAndDirectionFromOffset(Vector2 offset)
	{
		if (Mathf.Abs(offset.x) > MAX_OFFSET_TOLERANCE)
			m_inputMove.x = offset.x < 0 ? -1.0f : 1.0f;
		if (Mathf.Abs(offset.y) > MAX_OFFSET_TOLERANCE)
			m_inputMove.y = offset.y < 0 ? -1.0f : 1.0f;
		SetDirectionFromInput ();
	}

	public bool CanBasicJump() {
		if ((Time.timeSinceLevelLoad - m_lastJump) < MIN_JUMP_INTERVAL)
			return false;

		if (!m_submerged && !m_physics.OnGround && CurrentAirJumps <= 0)
			return false;

		return true;
	}
	private void AttemptJump() {
		ExecuteEvents.Execute<ICustomMessageTarget> (gameObject, null, (x, y) => x.OnJump());
		if (!CanBasicJump ())
			return;

		if (m_submerged) {
			if (m_physics.TrueVelocity.y < 0.05f) 
				ApplyJumpVector (new Vector2 (1f, 0.6f));
		} else if (VariableJumpHeight) {
			ApplyJumpVector (new Vector2 (1f, 0.8f));
		} else {
			ApplyJumpVector (new Vector2 (1f, 1f));
		}

		FindObjectOfType<AudioManager> ().PlayClipAtPos (FXBody.Instance.SFXJump,transform.position,0.3f,0f,0.25f);
		m_lastJump = Time.timeSinceLevelLoad;
		m_variableJumpApplied = false;

		m_physics.AddArtificialVelocity (new Vector2 (0f, -0.75f * m_physics.TrueVelocity.y));
		if (!m_physics.OnGround)
			CurrentAirJumps -= 1;
	}
	
	public void ApplyJumpVector(Vector2 scale) {
		float y = m_jumpVector.y;
		Vector2 jv = new Vector2 (m_jumpVector.x, y ); //- Mathf.Max (0, m_physics.TrueVelocity.y / Time.deltaTime));
		jv.x *= scale.x;
		jv.y *= scale.y;
		m_physics.AddSelfForce (jv, 0f);
	}

	public void FacePoint(Vector3 point) {
		if (point.x > transform.position.x)
			m_physics.SetDirection (false);
		else
			m_physics.SetDirection (true);
	}
	public void MoveToPoint(Vector3 point) {
		m_inputMove = new Vector2(0,0);

		float dist = Vector3.Distance (transform.position, point);
		if (dist > m_abandonDistance || ( dist < m_minDistance && 
			((m_physics.FacingLeft && point.x < transform.position.x) ||
				(!m_physics.FacingLeft && point.x > transform.position.x)))){
			EndTarget ();
		} else {
			if (m_physics.CanMove) {
				if (Mathf.Abs (transform.position.x - point.x) > NPC_X_DIFF_MOVEMENT_THREASHOLD) {
					if (point.x > transform.position.x) {
						if (dist > m_minDistance)
							m_inputMove.x = 1.0f;
						m_physics.SetDirection (false);
					} else {
						if (dist > m_minDistance)
							m_inputMove.x = -1.0f;
						m_physics.SetDirection (true);
					}
				}
				if (Mathf.Abs (point.y - transform.position.y) > m_minDistance) {
					if (point.y > transform.position.y)
						m_inputMove.y = 1.0f;
					else
						m_inputMove.y = -1.0f;
				}
			}
		}
		if (CanJump && m_physics.CanMove) {
			if (Mathf.Abs (m_inputMove.x) >= 0.9f && (m_physics.FallDir == FallDirection.LEFT || m_physics.FallDir == FallDirection.RIGHT ) &&
				(point.y - transform.position.y) > PIT_JUMP_VERTICAL_THREASHOLD) {
				AttemptJump ();
			}

			JumpOverObstacle (point);
			JumpVerticalObstacles (point);
		}
	}

	private void JumpOverObstacle(Vector2 point) {
		if (Mathf.Abs (m_inputMove.x) >= 0.9f && Mathf.Abs (m_physics.TrueVelocity.x) < 0.05f ) {
			m_stuckTime += Time.deltaTime;
			if (m_stuckTime > NPC_STUCK_JUMP_TIME) {
				m_stuckTime = 0f;
				AttemptJump ();
			}
		} else {
			m_stuckTime = 0f;
		}
	}
	private void JumpVerticalObstacles(Vector2 point) {
		if (m_physics.TrueVelocity.x < 0.01f &&
		    (point.y - transform.position.y) > -PIT_JUMP_VERTICAL_THREASHOLD &&
		    (point.y - transform.position.y) < JumpHeight * 1.5f) {
			m_jumpThruStuckTime += Time.deltaTime;
			if (m_jumpThruStuckTime > NPC_STUCK_JUMP_TIME) {
				m_jumpThruStuckTime = 0f;
				AttemptJump ();
			}
		} else {
			m_jumpThruStuckTime = 0f;
		}
		if ((point.y - transform.position.y) < PIT_JUMP_VERTICAL_THREASHOLD) {
			m_verticalStuckTime += Time.deltaTime;
			if (m_verticalStuckTime > NPC_STUCK_JUMP_TIME) {
				m_verticalStuckTime = 0f;
				m_physics.setDropTime(0.2f);
			}
		} else {
			m_verticalStuckTime = 0f;
		}
	}
	public void SetTargetPoint(Vector3 point, float proximity, float max = float.MaxValue)
	{
		m_targetPoint = point;
		m_minDistance = proximity;
		m_abandonDistance = max;
		m_targetSet = true;
	}

	private void SetTarget(PhysicsSS target)
	{
		m_targetObj = true;
		m_targetSet = true;
		m_followObj = target;
	}

	public void EndTarget()
	{
		m_targetSet = false;
		m_targetObj = false;
		m_followObj = null;
	}

	public void SetMoveSpeed(float moveSpeed) {
		MoveSpeed = moveSpeed;
	}

	public void SetAutonomy(bool isAutonomous) {
		m_autonomy = isAutonomous;
		m_inputMove = new Vector2(0f, 0f);
	}

	public void SetJumpHeight(float jumpHeight) {
		m_jumpVector.y = (-m_physics.GravityForce * (8f * Mathf.Sqrt (jumpHeight))) + 9f;
		m_jumpHeight = jumpHeight;
	}

	private void storeData(CharData d) {
		d.PersistentBools ["IsCurrentPlayer"] = IsCurrentPlayer;
	}

	private void loadData(CharData d) {
		IsCurrentPlayer = d.PersistentBools ["IsCurrentPlayer"];
	}
}