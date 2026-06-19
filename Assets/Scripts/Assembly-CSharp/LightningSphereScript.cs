using UnityEngine;

public class LightningSphereScript : MonoBehaviour
{
	public float rotationSpeed = 360f;

	public float scaleSpeed = 5f;

	public float lifeTime = 2f;

	private float startTime;

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update()
	{
		base.transform.RotateAround(Vector3.up, rotationSpeed * Time.deltaTime);
		Vector3 localScale = base.transform.localScale * (1f + scaleSpeed * Time.deltaTime);
		base.transform.localScale = localScale;
		Color color = GetComponent<Renderer>().material.color;
		color.a = Mathf.Lerp(1f, 0f, (Time.time - startTime) / lifeTime);
	GetComponent<Renderer>().material.color = color;
	}
}
