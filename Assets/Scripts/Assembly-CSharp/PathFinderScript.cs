using System;
using UnityEngine;

public class PathFinderScript : MonoBehaviour
{
	protected float lastRefindPath;

	public float refindPathPeriod = 3f;

	[NonSerialized]
	public bool isFly;

	public virtual void SetPathFinderParams(float _speed, float _jumpSpeed, int _charSizeY)
	{
	}

	public virtual void WalkingFollowTarget(Vector3 targetPos)
	{
	}

	public virtual bool CanPathTo(Vector3 targetPos)
	{
		return true;
	}
}
