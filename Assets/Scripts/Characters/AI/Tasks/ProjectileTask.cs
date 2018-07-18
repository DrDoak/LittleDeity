using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileTask : FighterTask 
{
	public int Speed = 3;
	public float DistanceTilShoot = 10f;
	override public void Advance() 
	{
		Vector2 target = Player.transform.position;
		if (Vector2.Distance ((Vector2)Fighter.BasicMove.transform.position, target) < DistanceTilShoot)
		{
			print ("Fire!");
		}
		else 
		{
			for (int i = 0; i < Speed; i++)
				Fighter.BasicMove.MoveToPoint ((Vector3)target);
		}
	}
}
