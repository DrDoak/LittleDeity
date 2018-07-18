using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityControl : MonoBehaviour {

    private CombatControl _mCombatControl;
    private BasicAbilityControl _mBasicControl;

	// Use this for initialization
	void Start () {
        _mCombatControl = GetComponent<CombatControl>();
        _mBasicControl = GetComponent<BasicAbilityControl>();
	}

    void Update()
    {
        if (Ability.Player == null)
            Ability.Player = gameObject;
    }

    public void AbsorbAbility(Ability a, int i)
    {
        _mCombatControl.SlotAbility(ChooseKeySlot(i), a);
    }

    private string ChooseKeySlot(int i)
    {
		string k = "Ability1";
        int j = 0;
        foreach (string kc in _mCombatControl.keys)
        {
            if (j == i)
            {
                k = kc;
                break;
            }

            j++;
        }

        return k;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<PropertyHolder>())
        {
			foreach (Ability a in _mBasicControl.Abilities) {
				if (a != null && !a.UseAttackHitbox) { // I added a null check here.
					a.SetTarget (collision.gameObject);
				}
			}
        }
    }

}
