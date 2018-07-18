using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PR_Flaming : Property
{

    Vector2 off = new Vector2(0f, 0f);
    float dmg = 5.0f;
    float stun = 0.5f;
    float hd = -0.5f;
    Vector2 kb = new Vector2(0.0f, 0.0f);
    HitboxDoT fireSurround;

    //float time_tracker = 0.0f;
    //float flaming_period = 0.1f;
    //float flaming_damage = 5.0f;
	GameObject fx;

	List<ElementType> fireOnly;


    public override void OnAddProperty()
    {
		Vector3 sc = GetComponent<PropertyHolder> ().BodyScale ();
		sc *= 1.2f;
		fireSurround = GetComponent<HitboxMaker>().CreateHitboxDoT(sc, off, dmg, stun, hd, kb,false, true, ElementType.FIRE);
		fireSurround.GetComponent<Hitbox> ().Faction = FactionType.HOSTILE;
		fx = GetComponent<PropertyHolder> ().AddBodyEffect (FXBody.Instance.FXFlame);
		fireOnly = new List<ElementType> ();
		fireOnly.Add (ElementType.FIRE);
		GetComponent<PropertyHolder> ().AddAmbient (FXBody.Instance.SFXFlaming);
    }

    public override void OnRemoveProperty()
    {
       // GetComponent<HitboxMaker>().ClearHitboxes();
		Destroy(fireSurround);
		GetComponent<PropertyHolder> ().RemoveBodyEffect (fx);
		GetComponent<PropertyHolder> ().RemoveAmbient (FXBody.Instance.SFXFlaming);
    }

    public override void OnUpdate()
    {
		List<ElementType> oldEle = fireSurround.Element;
		fireSurround.Element = fireOnly;
       // if (Time.time > time_tracker)
        //{
          //  time_tracker += flaming_period;
			GetComponent<Attackable>().TakeHit(fireSurround);
			//GetComponent<Attackable>().TakeHit(fireSurround);
        //}
		fireSurround.Element = oldEle;
    }


    public override void OnHitboxCreate(Hitbox hitboxCreated)
    {
		hitboxCreated.AddElement(ElementType.FIRE);
    }
	public override void OnWaterEnter(WaterHitbox waterCollided)  {
		GetComponent<PropertyHolder> ().RequestRemoveProperty ("Flaming");
	}
}
