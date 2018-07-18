using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PersistentItem : MonoBehaviour {
	public string saveID = "autoID";
	public Vector3 pos = Vector3.zero;
	public float zRot = 0f;
	public string targetID = "none";
	public RoomDirection targetDir = RoomDirection.NEUTRAL;
	public float healthVal = 0;
	public CharData data = new CharData();
	public bool recreated = false;
	public string PrefabPath = "";

	protected bool m_registryChecked = false;
	void Awake() {
		if (data.regID == "") {
			data.regID = SaveObjManager.Instance.GenerateID (gameObject,PrefabPath);
		}
		saveID = data.regID;
	}
	public void registryCheck() {
		m_registryChecked = true;
		if (data.regID == "") {
			data.regID = "Not Assigned";
		}
		if (recreated)
			LoadData ();
		if (SaveObjManager.CheckRegistered(gameObject)) {
			//Debug.Log (gameObject + " Already registered, deleting duplicate ID: " + data.regID);
			Destroy(gameObject);
		}
	}
	void Update () {
		if (!m_registryChecked) {
			registryCheck ();
		}
	}

	public void StoreData() {
		data.name = gameObject.name;
		data.pos = transform.position;
		data.zRot = transform.rotation.eulerAngles.z;
		data.targetID = "";
		if (GetComponent<Interactable>()) {
			data.TriggerUsed = GetComponent<Interactable> ().TriggerUsed;
			data.triggerString = GetComponent<Interactable> ().value;
		}
		if (GetComponent<Attackable> ()) {
			data.health = GetComponent<Attackable> ().Health;
			data.maxHealth = GetComponent<Attackable> ().MaxHealth;
			data.faction = GetComponent<Attackable> ().Faction;
		}
		if (GetComponent<BasicMovement>())
			data.IsCurrentCharacter = GetComponent<BasicMovement> ().IsCurrentPlayer;
		if (GetComponent<PhysicsSS> ()) {
			data.IsFacingLeft = GetComponent<PhysicsSS> ().FacingLeft;
			data.terminalVelocity = GetComponent<PhysicsSS> ().TerminalVelocity;
		}
		if (GetComponent<Turret> ())
			data.TurretDefaultFace = GetComponent<Turret> ().DefaultFaceLeft;
		if (GetComponent<PropertyHolder> ()) {
			Property[] pL = GetComponents<Property> ();
			string[] allPs = new string[pL.Length];
			string[] allDs = new string[pL.Length];
			float[] allVs = new float[pL.Length];
			for (int i = 0; i < pL.Length; i++) {
				allPs [i] = pL [i].GetType ().ToString ();
				allDs [i] = pL [i].Description;
				allVs [i] = pL [i].value;
			}
			data.propertyList = allPs;
			data.propertyDescriptions = allDs;
			data.propertyValues = allVs;
			data.transfers = GetComponent<PropertyHolder> ().NumTransfers;
			data.slots = GetComponent<PropertyHolder> ().MaxSlots;
		}

		if (GetComponent<ExperienceHolder> ())
			data.Experience = GetComponent<ExperienceHolder> ().Experience;
		if (PrefabPath == "")
			data.prefabPath = getProperName ();
		else
			data.prefabPath = PrefabPath;
	}

	public void LoadData() {
		if (GetComponent<Attackable> ()) {
			GetComponent<Attackable> ().MaxHealth = data.maxHealth;
			GetComponent<Attackable> ().SetHealth (data.health);
			GetComponent<Attackable> ().Faction = data.faction;
		}
		Quaternion q = Quaternion.Euler(new Vector3 (0f, 0f, data.zRot));
		transform.localRotation = q;
		if (GetComponent<PhysicsSS> ()) {
			GetComponent<PhysicsSS> ().SetDirection (data.IsFacingLeft);
			GetComponent<PhysicsSS> ().TerminalVelocity = data.terminalVelocity;
		}
		if (GetComponent<Turret> ())
			GetComponent<Turret> ().DefaultFaceLeft = data.TurretDefaultFace;
		if (GetComponent<PropertyHolder> ()) {
			GetComponent<PropertyHolder> ().NumTransfers = data.transfers;
			GetComponent<PropertyHolder> ().MaxSlots = data.slots;
			GetComponent<PropertyHolder> ().ClearProperties ();
			Debug.Log ("Readding properties: " + data.propertyList.Length);
			for (int i = 0; i < data.propertyList.Length; i++) {
				Type t = Type.GetType (data.propertyList [i]);
				GetComponent<PropertyHolder> ().AddProperty (data.propertyList [i]);
				Property p = (Property)gameObject.GetComponent (t);
				p.Description = data.propertyDescriptions [i];
				p.value = data.propertyValues [i];
			}
		}
		if (GetComponent<ExperienceHolder> ()) {
			GetComponent<ExperienceHolder> ().Experience = data.Experience;
		}
		if (GetComponent<Interactable>()) {
			GetComponent<Interactable> ().TriggerUsed = data.TriggerUsed;
			GetComponent<Interactable> ().value = data.triggerString;
		}
		gameObject.name = data.name;
		gameObject.name = getProperName ();
	}
	private string getProperName() {
		string properName = "";
		foreach (char c in gameObject.name) {
			if (!c.Equals ('(') && !c.Equals(' ')) {
				properName += c;
			} else {
				break;
			}
		}
		return properName;
	}
	void OnEnable() {
		SaveObjManager.OnLoaded += LoadData;
		SaveObjManager.OnBeforeSave += StoreData;
		//SaveObjManager.OnBeforeSave += ApplyData;
	}

	void OnDisable() {
		SaveObjManager.OnLoaded -= LoadData;
		SaveObjManager.OnBeforeSave -= StoreData;
		//SaveObjManager.OnBeforeSave -= ApplyData;
	}
}