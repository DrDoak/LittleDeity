using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskAttack : FighterTask {

	override public void Advance() 
	{
		if (!Fighter.Fighter.IsAttacking())
			NextTask ();
	}
}
