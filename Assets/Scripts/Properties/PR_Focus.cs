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

	public override void OnUpdate() {
		if (Time.timeSinceLevelLoad - m_sinceLastHit > FocusRegainDelay)
			damageFocus(-FocusRegainRate * Time.deltaTime);
		Vector2 inputDir = GetComponent < BasicMovement> ().m_inputMove;
		if (inputDir.x != LastDirectionHeld.x || inputDir.y != LastDirectionHeld.y) {
			LastDirectionHeld = inputDir;
			m_last_time_held_direction = Time.timeSinceLevelLoad;
		}
	}
	public override void OnHit(HitInfo hi, GameObject attacker) {
		if (hi.Damage > 0f || hi.FocusDamage > 0f)
			m_sinceLastHit = Time.timeSinceLevelLoad;
		float f = hi.FocusDamage;
		bool atk = GetComponent<Fighter> ().IsAttacking();
		float timeSinceLastDir = Time.timeSinceLevelLoad - m_last_time_held_direction;
		if (AwayAttack (hi.Knockback,LastDirectionHeld)) {
			if (timeSinceLastDir < PARRY_WINDOW && !atk)
				f *= 0.1f;
			else
				f *= 2.0f;
		} else if (!FacingTowards (hi.Knockback)) {
			f *= 2.0f;
		} else if (TowardsAttack (hi.Knockback,LastDirectionHeld)) {
			if (timeSinceLastDir < PARRY_WINDOW && !atk)
				f *= 0.5f;
			else
				f *= 1f - Mathf.Max (0.2f, (0.75f * Mathf.Min (2f, timeSinceLastDir) / 3f));
		}
		if (atk)
			f *= 2f;
		float dmg = damageFocus (f);
		if (m_focus > 0f) {
			hi.Damage = hi.Damage * hi.Penetration;
		} else {
			hi.Damage = Mathf.Max (0f, hi.Damage - dmg);
		}
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
		if (m_display != null) {
			m_display.TakeDamage (diff, m_focus);
		}
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
