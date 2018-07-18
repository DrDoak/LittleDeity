using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
class ItemInfo {
	public GameObject Item = null;
	public Vector2 ItemCreatePos = new Vector2 (1.0f, 0f);
	public Vector2 ItemAimDirection = new Vector2 (1.0f, 0f);
	public float InitialSpeed = 10.0f;
}

public class AtkItem : AtkDash {

	[SerializeField]
	private ItemInfo m_ItemData;

	protected override void OnAttack()
	{
		base.OnAttack();
		GetComponent<HitboxMaker> ().CreateItem (m_ItemData.Item, m_ItemData.ItemCreatePos, m_ItemData.ItemAimDirection, m_ItemData.InitialSpeed);
	}
}