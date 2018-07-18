using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ICustomMessageTarget : IEventSystemHandler
{
	// functions that can be called via the messaging system
	void OnCreation();
	void OnHit(Hitbox hb, GameObject attacker);
	void OnHitConfirm (Hitbox myHitbox, GameObject objectHit, HitResult hr);
	void OnSight(Observable observedObj);
	void OnDeath();
	void OnUpdate();
	void OnCollision();
	void OnAttack();
	void OnHitboxCreate (Hitbox hitboxCreated);

	void OnWaterEnter(WaterHitbox waterCollided);
	void OnWaterExit (WaterHitbox waterCollided);
}