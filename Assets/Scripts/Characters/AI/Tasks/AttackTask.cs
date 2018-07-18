using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTask : FighterTask 
{
	public int Speed = 3;
	override public void Advance() 
	{
		Vector2 target = Player.transform.position;
		if (Vector2.Distance ((Vector2)Fighter.BasicMove.transform.position, target) < .1f)
		{
			print ("Attack!");
		}
		else 
		{
			for (int i = 0; i < Speed; i++)
				Fighter.BasicMove.MoveToPoint ((Vector3)target);
		}
	}
}
