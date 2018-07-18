using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RaycastOrigins
{
	public Vector2 topLeft, topRight, bottomLeft, bottomRight;
}

public struct CollisionInfo
{
	public bool above, below, left, right;
	public bool climbingSlope;
	public float slopeAngle, slopeAngleOld;

	public void Reset() {
		above = below = left = right = false;
		climbingSlope = false;

		slopeAngleOld = slopeAngle;
		slopeAngle = 0;
	}
}
