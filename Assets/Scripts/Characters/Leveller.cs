using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leveller : MonoBehaviour {

    public static Leveller Instance;

    public int Level;
    public float Scaler = 1.5f;
    public int DataRequirement = 800;
    public int PointAdditions = 1;
    public float HealthAddition = 50f;
	public string levelUpStr;
	public int NextLevel = 200;

    private ExperienceHolder exp;

	void Start()
	{

		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		Level = 1;
		for (int i = 1; i < 10; i++) { 
			//Debug.Log (Instance.DataRequirement * Instance.Scaler * i);
		}
	}

    /// <summary>
    /// Takes in an Experience Holder for correct exp updates, levels up if required
    /// </summary>
    /// <param name="obj"></param>
    public static void UpdateExperience(ExperienceHolder obj)
    {
		if (Instance.exp == null || Instance.exp != obj)
			Instance.exp = obj;
		//Debug.Log ("Requirement: " + Instance.DataRequirement * Instance.Scaler * Instance.Level);
		//Debug.Log ("Current: " + Instance.exp.Experience);

		if (Instance.exp.Experience >= Instance.DataRequirement * (Mathf.Pow(Instance.Level,Instance.Scaler))) {
			EventManager.TriggerEvent (3);
			Instance.NextLevel = (int)(((float)Instance.DataRequirement * (Mathf.Pow ((float)Instance.Level, Instance.Scaler))));
			//Debug.Log ("Event Triggered");
		}
    }

    void IncreaseHealth()
    {
		float oldH = exp.gameObject.GetComponent<Attackable> ().MaxHealth;
        exp.gameObject.GetComponent<Attackable>().MaxHealth += HealthAddition;
		exp.gameObject.GetComponent<Attackable> ().DamageObj (-1000f);
		levelUpStr += "\n~HP: " + oldH + " => " + exp.gameObject.GetComponent<Attackable> ().MaxHealth;
    }

    void DisplayLevelUp()
    {
		TextboxManager.StartSequence ("~RANK UP! LEVEL " + (Instance.Level + 1) + levelUpStr);
		levelUpStr = "";
		Instance.Level++;
    }
   

    void AddAbilityPoints()
    {

        AbilityTree.PointsToSpend += PointAdditions;
        AbilityTreeNode n = AbilityManager.abilityTree.UnlockAbilitiesAutomatic();
        if(n!=null)
        {
            levelUpStr += "\n~ABILITY UPGRADED! " + n.tree.DisplayAbility();
        }
    }

    void AddTransferSlots()
    {
        //Add slots at levels 2, 4, 6, 7
		if (Level == 1 || Level == 3 || Level == 5 || Level == 6 ) {
			int oldMax = exp.gameObject.GetComponent<PropertyHolder> ().MaxSlots;
			exp.gameObject.GetComponent<PropertyHolder> ().MaxSlots += 1;
			levelUpStr += "\n~Property Slots: " + oldMax + " => " + exp.gameObject.GetComponent<PropertyHolder> ().MaxSlots;
		}
    }

	void AddTransfers()
	{
		//Add slots at levels 2,3, 6, 9 TEMPORARY
		if(Level == 2 || Level % 3 == 0)
			exp.gameObject.GetComponent<PropertyHolder>().NumTransfers += 1;
	}

    void OnEnable()
    {

		//Debug.Log ("on enable");
        EventManager.LevelUpEvent += IncreaseHealth;
        EventManager.LevelUpEvent += AddAbilityPoints;
        EventManager.LevelUpEvent += AddTransferSlots;
		EventManager.LevelUpEvent += AddTransfers;
		EventManager.LevelUpEvent += DisplayLevelUp;
    }

    void OnDisable()
    {
		//Debug.Log ("on disable");
        EventManager.LevelUpEvent -= IncreaseHealth;
        EventManager.LevelUpEvent -= AddAbilityPoints;
        EventManager.LevelUpEvent -= AddTransferSlots;
		EventManager.LevelUpEvent -= AddTransfers;
		EventManager.LevelUpEvent -= DisplayLevelUp;
    }
}
