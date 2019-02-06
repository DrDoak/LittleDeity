using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Focus : Property {

	public float MaxFocus = 100f;
	public float FocusRegainRate = 30.0f;
	public float FocusRegainDelay = 5.0f;
	private float m_focus = 100f;

	private HealthDisplay m_display;
	private string m_displayString = "";

	private float m_sinceLastHit = -10f;

	private float m_last_time_held_direction = 0f;
	private Vector2 LastDirectionHeld = new Vector2();

	private const float PARRY_WINDOW = 0.3f;

	private UIActionText m_msg_parry;
	private UIActionText m_msg_backstab;
	private UIActionText m_msg_perfectguard;

	public override void OnUpdate() {
		if (Time.timeSinceLevelLoad - m_sinceLastHit > FocusRegainDelay)
			damageFocus(-FocusRegainRate * Time.deltaTime);
		Vector2 inputDir = GetComponent < BasicMovement> ().m_inputMove;
		if (inputDir.x != LastDirectionHeld.x || inputDir.y != LastDirectionHeld.y) {
			LastDirectionHeld = inputDir;
			m_last_time_held_direction = Time.timeSinceLevelLoad;
		}
		m_msg_parry = new UIActionText ();
		m_msg_parry.text = "Parry";
		m_msg_parry.textColor = Color.white;
		m_msg_parry.timeToDisplay = 1f;

		m_msg_perfectguard = new UIActionText ();
		m_msg_perfectguard.text = "Perfect Guard";
		m_msg_perfectguard.textColor = Color.yellow;
		m_msg_perfectguard.timeToDisplay = 1f;

		m_msg_backstab = new UIActionText ();
		m_msg_backstab.text = "BackStabbed!!!";
		m_msg_backstab.textColor = Color.red;
		m_msg_backstab.timeToDisplay = 1f;
	}
	public override void OnHit(HitInfo hi, GameObject attacker) {
		if (m_focus <= 0f)
			return;
		if (hi.Damage > 0f || hi.FocusDamage > 0f)
			m_sinceLastHit = Time.timeSinceLevelLoad;
		float f = hi.FocusDamage;
		bool atk = GetComponent<Fighter> ().IsAttacking();
		float timeSinceLastDir = Time.timeSinceLevelLoad - m_last_time_held_direction;
		Vector2 kb = hi.Knockback;
		if (!hi.IsFixedKnockback) {
			Vector3 kb3 = transform.position - hi.mHitbox.transform.position;
			kb = new Vector2 (kb3.x, kb3.y);
		}
		if (AwayAttack (kb,LastDirectionHeld)) {
			if (timeSinceLastDir < PARRY_WINDOW && !atk) {
				FindObjectOfType<GUIHandler> ().AddText (m_msg_parry);
				hi.Stun = 0f;
				f *= 0.1f;
				hi.Knockback *= 0.2f;

			} else {
				GameManager.Instance.GetComponent<GUIHandler>().AddText (m_msg_backstab);
				f *= 2.0f;
			}
		} else if (!FacingTowards (kb)) {
			FindObjectOfType<GUIHandler> ().AddText (m_msg_backstab);
			f *= 2.0f;
		} else if (TowardsAttack (kb,LastDirectionHeld)) {
			if (timeSinceLastDir < PARRY_WINDOW && !atk) {
				FindObjectOfType<GUIHandler> ().AddText (m_msg_perfectguard);
				f *= 0.5f;
				hi.Knockback *= 0.4f;
			} else {
				f *= 1f - Mathf.Max (0.2f, (0.75f * Mathf.Min (2f, timeSinceLastDir) / 3f));
			}
		}
		if (atk)
			f *= 2f;
		float dmg = damageFocus (f);
		if (m_focus > 0f) {
			hi.Damage = hi.Damage * hi.Penetration;
		} else {
			hi.Damage = Mathf.Max (0f, hi.Damage - dmg);
		}
		hi.FocusDamage = f;
	}
	private bool FacingTowards(Vector2 kb) {
		return (kb.x > 0 && GetComponent<PhysicsSS> ().FacingLeft ||
		kb.x < 0 && !GetComponent<PhysicsSS> ().FacingLeft ||
		kb.x == 0);
	}
	private bool AwayAttack(Vector2 kb, Vector2 input) {
		if (Mathf.Abs(kb.y)  > Mathf.Abs(kb.x) * 2f ) {
			if (kb.y > 0 && input.y > 0) {
				return true;
			} else if (kb.y < 0 && input.y < 0) {
				return true;
			} else {
				return false;
			}
		} else {
			if (kb.x > 0 && input.x > 0) {
				return true;
			} else if (kb.x < 0 && input.x < 0) {
				return true;
			} else {
				return false;
			}
		}
	}
	private bool TowardsAttack(Vector2 kb, Vector2 input) {
		if (Mathf.Abs(kb.y) > Mathf.Abs(kb.x) * 2f ) {
			if (kb.y > 0 && input.y < 0) {
				return true;
			} else if (kb.y < 0 && input.y > 0) {
				return true;
			} else {
				return false;
			}
		} else {
			if (kb.x > 0 && input.x < 0) {
				return true;
			} else if (kb.x < 0 && input.x > 0) {
				return true;
			} else {
				return false;
			}
		}
	}
	private float damageFocus(float focus_damage) {
		float focusBefore = m_focus;
		m_focus = Mathf.Max(Mathf.Min(MaxFocus, m_focus - focus_damage), 0);
		float diff =  m_focus - focusBefore;
		return diff;
	}

	public override void OnControllableChange(bool isControllable) {
		//Debug.Log ("On controllable change");
		if (isControllable) {
			//Debug.Log ("adding bar");
			UIBarInfo ubi = new UIBarInfo ();
			ubi.FillColor = Color.blue;
			ubi.UILabel = "Focus";
			ubi.id = "Focus";
			ubi.funcUpdate = UpdateHealthValues;
			ubi.target = gameObject;
			//ubi.DisplayMode = UIBarDisplayMode.PERCENT;
			FindObjectOfType<GUIHandler> ().AddUIBar (ubi);

			m_displayString = ubi.UILabel;
				
			/*
			m_display = Instantiate (UIList.Instance.HealthBarPrefab, this.transform).GetComponent<HealthDisplay>();
			m_display.SetMaxHealth (MaxFocus);
			*/

		} else {
			if (m_displayString.Length > 0)
				FindObjectOfType<GUIHandler> ().RemoveUIBar (m_displayString);
			//Destroy (m_display);
		}
	}

	void UpdateHealthValues(UIBarInfo ubi) {
		ubi.element.GetComponent<UIBar> ().UpdateValues (Mathf.Round(m_focus), Mathf.Round(MaxFocus));
	}

	public override void OnSave (CharData d)
	{
		base.OnSave (d);
		d.PersistentFloats ["MaxFocus"] = MaxFocus;
	}

	public override void OnLoad (CharData d)
	{
		MaxFocus = d.PersistentFloats ["MaxFocus"];
		base.OnLoad (d);
	}
}
