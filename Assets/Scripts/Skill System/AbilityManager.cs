using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour {

    public AbilityDescription description;

    public static AbilityTree abilityTree;

	// Use this for initialization
	void Awake () {
        Ability.Manager = this;
        if (abilityTree == null)
        {
            abilityTree = new AbilityTree();
            PopulateTree();
            AbilityTreeUI.tree = abilityTree;
        }
	}

    private void PopulateTree()
    {
        //Transfer Root
        abilityTree.AddRoot(ScriptableObject.CreateInstance<Ab_Transfer>());

        //Melee Abilities
        abilityTree.Add(ScriptableObject.CreateInstance<Ab_Melee>(), Branch.LEFT, 1);
        abilityTree.Add(ScriptableObject.CreateInstance<Ab_NanoSword>(), Branch.LEFT, AbilityType.COMBAT, 2);
        abilityTree.Add(ScriptableObject.CreateInstance<Ab_Passive_AttackRate>(), Branch.LEFT, AbilityType.COMBAT, 4);
        //abilityTree.Add(ScriptableObject.CreateInstance<Ab_Ultimate_Carbon_Copy>(), Branch.LEFT, AbilityType.COMBAT, 8);

        abilityTree.Add(ScriptableObject.CreateInstance<Ab_Passive_Bleed>(), Branch.MIDDLE, AbilityType.COMBAT, 6);
        //abilityTree.Add(ScriptableObject.CreateInstance<Ab_Passive_Heal>(), Branch.MIDDLE, AbilityType.COMBAT, 7);
        //abilityTree.Add(ScriptableObject.CreateInstance<Ab_Ultimate_Siphon>(), Branch.MIDDLE, AbilityType.COMBAT, 9);

       // abilityTree.Add(ScriptableObject.CreateInstance<Ab_CriticalStrike>(), Branch.RIGHT, AbilityType.COMBAT, 3);
        abilityTree.Add(ScriptableObject.CreateInstance<Ab_Passive_AttackDamage>(), Branch.RIGHT, AbilityType.COMBAT, 5);
        //abilityTree.Add(ScriptableObject.CreateInstance<Ab_Ultimate_Overpower>(), Branch.RIGHT, AbilityType.COMBAT, 10);


        //Transfer Abilities


        //Environmental Abilities


        //Sanity Check
        //abilityTree.PrintTree();

    }
}
