using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Iron : Property {

    Resistence physResist;
    Resistence fireResist;
    Resistence lightningResist;
	GameObject fx;

    public override void OnAddProperty()
    {
        physResist = GetComponent<Attackable>().AddResistence(ElementType.PHYSICAL, 25.0f, false, false, 0.0f, 70.0f, 70.0f);
        fireResist = GetComponent<Attackable>().AddResistence(ElementType.FIRE, 100.0f, false, false,0.0f,0.0f,100.0f);
        lightningResist = GetComponent<Attackable>().AddResistence(ElementType.LIGHTNING, -25.0f, false,false,0.0f,-25.0f,100.0f);
		if (GetComponent<BasicMovement> () != null) {
			GetComponent<BasicMovement> ().SetMoveSpeed (GetComponent<BasicMovement> ().MoveSpeed / 1.25f);
			//GetComponent<BasicMovement> ().SetJumpData (GetComponent<BasicMovement> ().JumpHeight / 1.5f, GetComponent<BasicMovement> ().TimeToJumpApex);
		}
		fx = GetComponent<PropertyHolder> ().AddBodyEffect (FXBody.Instance.FXIron);
     //   GetComponent<PhysicsSS>().SetGravityForce(-2.0f);
    }

	public override void OnUpdate() {
		if (GetComponent<PhysicsSS> () != null) {
			GetComponent<PhysicsSS> ().ReactToWater = false;
			if (GetComponent<PropertyHolder> ().SubmergedHitbox != null) {
				GetComponent<PhysicsSS> ().AddToVelocity (new Vector2 (0f, -20f * Time.deltaTime));
			}
		}
		if (GetComponent<BasicMovement> () != null) {
			GetComponent<BasicMovement> ().Submerged = false;
		}

	}

    public override void OnRemoveProperty()
    {
        GetComponent<Attackable>().RemoveResistence(physResist);
        GetComponent<Attackable>().RemoveResistence(fireResist);
        GetComponent<Attackable>().RemoveResistence(lightningResist);
		if (GetComponent<BasicMovement> () != null) {
			GetComponent<BasicMovement> ().SetMoveSpeed (GetComponent<BasicMovement> ().MoveSpeed * 1.25f);
			//GetComponent<BasicMovement> ().SetJumpData (GetComponent<BasicMovement> ().JumpHeight * 1.5f, GetComponent<BasicMovement> ().TimeToJumpApex);
		}
		GetComponent<PropertyHolder> ().RemoveBodyEffect (fx);
        //GetComponent<PhysicsSS>().SetGravityForce(-1.0f);
    }
	public override void OnWaterEnter(WaterHitbox waterCollided)  {
		//Debug.Log ("Water enter");
		if (GetComponent<BasicMovement>() != null)
			GetComponent<BasicMovement> ().SetMoveSpeed (GetComponent<BasicMovement> ().MoveSpeed * 2f);
	}
	public override void OnWaterExit(WaterHitbox waterCollided) {
		//Debug.Log ("Water exit");
		if (GetComponent<BasicMovement>() != null)
			GetComponent<BasicMovement> ().SetMoveSpeed (GetComponent<BasicMovement> ().MoveSpeed / 2f);
	}

}
