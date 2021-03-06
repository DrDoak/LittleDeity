﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.UI;

public class Property : MonoBehaviour, ICustomMessageTarget
{

    public virtual void OnCreation() { }
	public virtual void OnHit(HitInfo hi, GameObject attacker) { }
	public virtual void OnHitConfirm(HitInfo myHitbox, GameObject objectHit, HitResult hr) { }
	public virtual void OnSight(Observable observedObj) { }

	public virtual void OnAttack() { }
    public virtual void OnDeath() { }
    public virtual void OnUpdate() { }
    //public virtual void OnCollision() { }

	public virtual void OnAttack(AttackInfo ai) { }
	public virtual void OnControllableChange(bool isControllable) { }
	public virtual void OnHitboxCreate (Hitbox hitboxCreated) {}

	public virtual void OnAddProperty() { }
	public virtual void OnRemoveProperty() {}

	public virtual void OnJump() {}

	public virtual void OnSave(CharData d) {}
	public virtual void OnLoad(CharData d) {}

	public virtual void OnWaterEnter(WaterHitbox waterCollided) { }
	public virtual void OnWaterExit(WaterHitbox waterCollided) {}

	public bool Stealable = true;
	public bool Viewable = true;
	public bool Stackable = false;
	public bool InstantUse = false;

	public string PropertyName = "";
	public virtual string DefaultPropertyName {get {return "None";}}

	[TextArea(3,5)]
	public string Description = "";

	public virtual string DefaultDescription { get { return "No description provided"; } }
	public Sprite icon;

	public void InitPropertyData() {
		if (PropertyName == "")
			PropertyName = DefaultPropertyName;
		if (Description == "")
			Description = DefaultDescription;
	}

	public void CopyPropInfo(Property p) {
		Stealable = p.Stealable;
		Viewable = p.Viewable;
		Stackable = p.Stackable;
		InstantUse = p.InstantUse;
		if (PropertyName == "")
			PropertyName = p.PropertyName;
		if (Description == "")
			Description = p.Description;
		icon = p.icon;;
	}
}