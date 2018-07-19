using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
	[SerializeField]
	private List<TKey> keys = new List<TKey>();

	[SerializeField]
	private List<TValue> values = new List<TValue>();

	// save the dictionary to lists
	public void OnBeforeSerialize()
	{
		keys.Clear();
		values.Clear();
		foreach(KeyValuePair<TKey, TValue> pair in this)
		{
			keys.Add(pair.Key);
			values.Add(pair.Value);
		}
	}

	// load dictionary from lists
	public void OnAfterDeserialize()
	{
		this.Clear();

		if(keys.Count != values.Count)
			throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

		for(int i = 0; i < keys.Count; i++)
			this.Add(keys[i], values[i]);
	}
}


[Serializable]
public class CharData {

	public string regID = "Not Assigned";
	public string prefabPath;
	public RoomDirection targetDir;
	public string name;
	public Vector3 pos;
	public float zRot;
	public bool IsFacingLeft;

	[Serializable] public class DictionaryOfStringAndInt : SerializableDictionary<string, int> {}
	public DictionaryOfStringAndInt PersistentInt;

	[Serializable] public class DictionaryOfStringAndFloat : SerializableDictionary<string, float> {}
	public DictionaryOfStringAndFloat PersistentFloats;

	[Serializable] public class DictionaryOfStringAndString : SerializableDictionary<string, string> {}
	public DictionaryOfStringAndString PersistentStrings;

	public float health = 100f;
	public float maxHealth = 100f;
	public FactionType faction = FactionType.HOSTILE;

	public string targetID;
	public string [] propertyList;
	public string [] propertyDescriptions;
	public float[] propertyValues;
	public int transfers;
	public int slots;

	public bool IsCurrentCharacter;
	public float terminalVelocity;
	public bool TurretDefaultFace;
	public int Experience;

	public bool TriggerUsed;
	public string triggerString;
}