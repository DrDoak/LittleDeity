using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceSS {
	public Vector2 Force;
	public float Duration;

	public ForceSS (Vector2 force, float dur)
	{
		Force = force;
		Duration = dur;
	}

	public ForceSS ()
	{
		Force = new Vector2(0,0);
		Duration = 0;
	}
}
