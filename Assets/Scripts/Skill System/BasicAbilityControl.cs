using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAbilityControl : MonoBehaviour {

    public List<Ability> Abilities;

    // Use this for initialization
    void Start()
    {
        if (Abilities == null)
            Abilities = new List<Ability>();
    }

    public void TakeAbility(Ability a)
    {
        Abilities.Remove(a);
    } 
}
