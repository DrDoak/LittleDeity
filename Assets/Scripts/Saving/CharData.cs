using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class CharData {
	public string regID = "Not Assigned";
	public string name;
	public Vector3 pos;
	public float zRot;
	public float health = 100f;
	public float maxHealth = 100f;
	public FactionType faction = FactionType.HOSTILE;
	public string prefabPath;
	public string targetID;
	public string [] propertyList;
	public string [] propertyDescriptions;
	public float[] propertyValues;
	public int transfers;
	public int slots;
	public RoomDirection targetDir;
	public bool IsCurrentCharacter;
	public bool IsFacingLeft;
	public float terminalVelocity;
	public bool TurretDefaultFace;
	public int Experience;

	public bool TriggerUsed;
	public string triggerString;
}