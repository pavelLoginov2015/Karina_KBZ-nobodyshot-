using System;
using UnityEngine;

public class WeaponOnTheMapScript : MonoBehaviour
{
	public float flyingHeightAmp = 0.2f;

	public float flyingHeightPeriod = 2f;

	public float flyingRotationSpeed = 1.5f;

	public float height = 0.5f;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.localPosition = new Vector3(0f, height + flyingHeightAmp * Mathf.Sin(Time.time * 2f * (float)Math.PI / flyingHeightPeriod), 0f);
		base.transform.RotateAround(Vector3.up, flyingRotationSpeed * Time.deltaTime);
	}
}
