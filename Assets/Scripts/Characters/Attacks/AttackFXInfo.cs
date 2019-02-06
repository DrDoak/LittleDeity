using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AttackFXInfo
{
	public GameObject EffectPrefab;
	public float RotationZ = 0f;
	public float RotationVariation = 0f;
	public Vector3 Offset = new Vector3(1f,0f,0f);
	public Vector3 OffsetVariation = new Vector3(0f,0f,0f);
	public Vector3 Scale = new Vector3(1f,1f,1f);
	public Vector3 ScaleVariation = new Vector3(0f,0f,0f);
	public bool FlipOnX = true;
	public float Delay = 0f;
	public Color EffectColor = Color.white;
}
