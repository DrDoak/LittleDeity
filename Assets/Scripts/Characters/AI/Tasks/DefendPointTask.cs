using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendPointTask : FighterTask 
{
	public Vector2 Center;
	public float Radius;
	public int Speed = 3;

	override public void Advance() 
	{
		Vector2 target = DefendPoint(Center, Radius);
		if (Vector2.Distance((Vector2)Fighter.BasicMove.transform.position, target) < .1f)
			return;
		for (int i = 0; i < Speed; i++)
			Fighter.BasicMove.MoveToPoint((Vector3)target);
	}

	Vector2 DefendPoint(Vector2 center, float radius) 
	{
		Vector2 x = Player.transform.position;
		Vector2 d = x-center;
		d.Normalize();
		return center + (d * radius);
	} 
}
