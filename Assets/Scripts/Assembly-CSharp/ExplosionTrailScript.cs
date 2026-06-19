using UnityEngine;

public class ExplosionTrailScript : MonoBehaviour
{
	public float speed = 10f;

	public float timeOfLife = 3f;

	public float gravitation = 1f;

	private float startTime;

	private Vector3 dir;

	private void Start()
	{
		startTime = Time.time;
		dir = Random.onUnitSphere;
	}

	private void Update()
	{
		float num = (Time.time - startTime) / timeOfLife;
		if (num > 0f)
		{
			base.transform.Translate((dir * speed * (1f - num) - Vector3.up * gravitation) * Time.deltaTime, Space.World);
		}
	}
}
